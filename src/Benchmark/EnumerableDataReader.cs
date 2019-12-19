using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark
{
    public interface IEnumerableDataReaderFactoryBuilder<T>
    {
        string Name { get; }

        EnumerableDataReaderFactoryBuilder<T> Add<TColumn>(string column, Expression<Func<T, TColumn>> expression);
        EnumerableDataReaderFactory<T> BuildFactory();
    }

    public class EnumerableDataReaderFactoryBuilder<T> : IEnumerableDataReaderFactoryBuilder<T>
    {
        private readonly List<LambdaExpression> _expressions = new List<LambdaExpression>();
        private readonly List<Func<T, object>> _objExpressions = new List<Func<T, object>>();
        private readonly DataTable _schemaTable;

        public EnumerableDataReaderFactoryBuilder(string tableName)
        {
            this.Name = tableName;
            _schemaTable = new DataTable();
        }

        private static readonly HashSet<Type> _validTypes = new[]
        {
                typeof(decimal),
                typeof(decimal?),
                typeof(string),
                typeof(int),
                typeof(int?),
                typeof(double),
                typeof(bool),
                typeof(bool?),
                typeof(Guid),
                typeof(DateTime),
            }.ToHashSet();

        private static readonly object _boxedTrue = (object)true;
        private static readonly object _boxedFalse = (object) false;

        public EnumerableDataReaderFactoryBuilder<T> Add<TColumn>(string column, Expression<Func<T, TColumn>> expression)
        {
            var t = typeof(TColumn);

            var func = expression.Compile();

            Expression<Func<T, object>> objExpression;
            
            if (typeof(T) == typeof(bool))
            {
                //use a pre-boxed value -- the JIT should optimize out the (bool)(object) boxing
                objExpression = o => ((bool)(object)func(o) ? _boxedTrue : _boxedFalse);
            }
            else
            {
                objExpression = o => func(o);
            }

            _objExpressions.Add(objExpression.Compile());

            if (_validTypes.Contains(t))
            {
                t = Nullable.GetUnderlyingType(t) ?? t; // data table doesn't accept nullable.
                _schemaTable.Columns.Add(column, t);
                _expressions.Add(expression);
            }
            else
            {
                Console.WriteLine($"Could not matching return type for {this.Name}.{column} of: {t.Name}");
                _schemaTable.Columns.Add(column); //add w/o type to force using GetValue

                _expressions.Add(objExpression);
            }

            return this;
        }

        public EnumerableDataReaderFactory<T> BuildFactory() => new EnumerableDataReaderFactory<T>(_schemaTable, _expressions, _objExpressions);

        public string Name { get; }
    }

    public class EnumerableDataReaderFactory<T>
    {
        public DataTable SchemaTable { get; }
        public Func<T, object>[] ObjectGetters { get; }
        public Func<T, decimal>[] DecimalGetters { get; }
        public Func<T, decimal?>[] NullableDecimalGetters { get; }
        public Func<T, string>[] StringGetters { get; }
        public Func<T, double>[] DoubleGetters { get; }
        public Func<T, int>[] IntGetters { get; }
        public Func<T, int?>[] NullableIntGetters { get; }
        public Func<T, bool>[] BoolGetters { get; }

        public Func<T, bool?>[] NullableBoolGetters { get; }

        public Func<T, Guid>[] GuidGetters { get; }
        public Func<T, DateTime>[] DateTimeGetters { get; }
        public bool[] NullableIndexes { get; }

        public EnumerableDataReaderFactory(DataTable schemaTable, List<LambdaExpression> expressions, List<Func<T, object>> objectGetters)
        {
            SchemaTable = schemaTable;
            DecimalGetters = new Func<T, decimal>[expressions.Count];
            NullableDecimalGetters = new Func<T, decimal?>[expressions.Count];
            StringGetters = new Func<T, string>[expressions.Count];
            DoubleGetters = new Func<T, double>[expressions.Count];
            IntGetters = new Func<T, int>[expressions.Count];
            NullableIntGetters = new Func<T, int?>[expressions.Count];
            BoolGetters = new Func<T, bool>[expressions.Count];
            NullableBoolGetters = new Func<T, bool?>[expressions.Count];
            GuidGetters = new Func<T, Guid>[expressions.Count];
            DateTimeGetters = new Func<T, DateTime>[expressions.Count];
            NullableIndexes = new bool[expressions.Count];

            ObjectGetters = objectGetters.ToArray();

            for (int i = 0; i < expressions.Count; i++)
            {
                var expression = expressions[i];

                NullableIndexes[i] = !expression.ReturnType.IsValueType || Nullable.GetUnderlyingType(expression.ReturnType) != null;

                switch (expression)
                {
                    case Expression<Func<T, object>> e: break; // do nothing
                    case Expression<Func<T, decimal>> e: DecimalGetters[i] = e.Compile(); break;
                    case Expression<Func<T, decimal?>> e: NullableDecimalGetters[i] = e.Compile(); break;
                    case Expression<Func<T, string>> e: StringGetters[i] = e.Compile(); break;
                    case Expression<Func<T, double>> e: DoubleGetters[i] = e.Compile(); break;
                    case Expression<Func<T, int>> e: IntGetters[i] = e.Compile(); break;
                    case Expression<Func<T, int?>> e: NullableIntGetters[i] = e.Compile(); break;
                    case Expression<Func<T, bool>> e: BoolGetters[i] = e.Compile(); break;
                    case Expression<Func<T, bool?>> e: NullableBoolGetters[i] = e.Compile(); break;
                    case Expression<Func<T, Guid>> e: GuidGetters[i] = e.Compile(); break;
                    case Expression<Func<T, DateTime>> e: DateTimeGetters[i] = e.Compile(); break;
                    default:
                        throw new Exception($"Type missing: {expression.GetType().FullName}");
                }
            }
        }

        public IDataReader CreateReader(IEnumerable<T> items) => new EnumerableDataReader<T>(this, items.GetEnumerator());
    }

    public class EnumerableDataReader<T> : IDataReader
    {
        private readonly IEnumerator<T> _source;
        private readonly EnumerableDataReaderFactory<T> _context;

        public EnumerableDataReader(EnumerableDataReaderFactory<T> context, IEnumerator<T> source)
        {
            _source = source;
            _context = context;
        }

        public object GetValue(int i)
        {
            var v = _context.ObjectGetters[i](_source.Current);
            return v;
        }

        public int FieldCount => _context.ObjectGetters.Length;

        public bool Read() => _source.MoveNext();

        public void Close() => _source.Reset();

        public void Dispose() => this.Close();

        public bool NextResult() => throw new NotImplementedException();

        public int Depth => 0;

        public bool IsClosed => false;

        public int RecordsAffected => -1;

        public DataTable GetSchemaTable() => _context.SchemaTable;

        public object this[string name] => throw new NotImplementedException();

        public object this[int i] => GetValue(i);

        public bool GetBoolean(int i)
        {
            var g = _context.BoolGetters[i];

            if (g != null) return g(_source.Current);

            return _context.NullableBoolGetters[i](_source.Current).Value;
        }

        public byte GetByte(int i) => throw new NotImplementedException();

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length) => throw new NotImplementedException();

        public char GetChar(int i) => throw new NotImplementedException();
        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length) => -1;

        public IDataReader GetData(int i) => throw new NotImplementedException();

        public string GetDataTypeName(int i) => throw new NotImplementedException();

        public DateTime GetDateTime(int i) => _context.DateTimeGetters[i](_source.Current);

        public decimal GetDecimal(int i)
        {
            var g = _context.DecimalGetters[i];

            if (g != null) return g(_source.Current);

            return _context.NullableDecimalGetters[i](_source.Current).Value;
        }

        public double GetDouble(int i) => _context.DoubleGetters[i](_source.Current);

        public Type GetFieldType(int i) => _context.SchemaTable.Columns[i].DataType;

        public float GetFloat(int i) => throw new NotImplementedException();

        public Guid GetGuid(int i) => _context.GuidGetters[i](_source.Current);

        public short GetInt16(int i) => throw new NotImplementedException();

        public int GetInt32(int i)
        {
            var g = _context.IntGetters[i];

            if (g != null) return g(_source.Current);

            return _context.NullableIntGetters[i](_source.Current).Value;
        }

        public long GetInt64(int i) => throw new NotImplementedException();

        public string GetName(int i)
        {
            if (_context.SchemaTable.Columns.Count > i)
            {
                return _context.SchemaTable.Columns[i].ColumnName;
            }
            throw new IndexOutOfRangeException($"No column for index {i}");
        }

        public int GetOrdinal(string name)
        {
            if (_context.SchemaTable.Columns.Count == 0)
            {
                throw new Exception("Schema table is empty");
            }
            return _context.SchemaTable.Columns.IndexOf(name);
        }

        public string GetString(int i) => _context.StringGetters[i](_source.Current);

        public int GetValues(object[] values) => throw new NotImplementedException();

        public bool IsDBNull(int i)
        {
            // short circuit for non-nullable types
            if (!_context.NullableIndexes[i])
            {
                return false;
            }

            // otherwise find the first one -- starting w/ most occurring to least

            var ig = _context.NullableIntGetters[i];
            if (ig != null)
            {
                return ig(_source.Current) == null;
            }

            var sg = _context.StringGetters[i];
            if (sg != null)
            {
                return sg(_source.Current) == null;
            }

            var bg = _context.NullableBoolGetters[i];
            if (bg != null)
            {
                return bg(_source.Current) == null;
            }

            var dg = _context.NullableDecimalGetters[i];
            if (dg != null)
            {
                return dg(_source.Current) == null;
            }

            return false;
        }
    }
}
