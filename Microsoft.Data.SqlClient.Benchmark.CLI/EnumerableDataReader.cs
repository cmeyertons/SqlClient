// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Microsoft.Data.SqlClient.Benchmark.CLI
{
    public sealed class EnumerableDataReader<T> : IDataReader
    {
        private readonly IEnumerator<T> _source;
        private readonly EnumerableDataReaderFactory<T> _context;
        private bool _disposedValue;

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

            if (g != null)
                return g(_source.Current);

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

            if (g != null)
                return g(_source.Current);

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

            if (g != null)
                return g(_source.Current);

            return _context.NullableIntGetters[i](_source.Current).Value;
        }

        public long GetInt64(int i) => _context.LongGetters[i](_source.Current);

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

        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Close();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
