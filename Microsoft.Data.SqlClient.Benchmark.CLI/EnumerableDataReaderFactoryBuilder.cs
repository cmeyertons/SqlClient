// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace Microsoft.Data.SqlClient.Benchmark.CLI
{
    public sealed class EnumerableDataReaderFactoryBuilder<T> : IDisposable
    {
        private readonly List<LambdaExpression> _expressions = new List<LambdaExpression>();
        private readonly List<Func<T, object>> _objExpressions = new List<Func<T, object>>();
        private readonly DataTable _schemaTable;

        public EnumerableDataReaderFactoryBuilder(string tableName)
        {
            Name = tableName;
            _schemaTable = new DataTable();
        }

        private static readonly HashSet<Type> _validTypes = new HashSet<Type>
            {
                typeof(decimal),
                typeof(decimal?),
                typeof(string),
                typeof(int),
                typeof(long),
                typeof(int?),
                typeof(double),
                typeof(bool),
                typeof(bool?),
                typeof(Guid),
                typeof(DateTime),
            };
        private bool _disposedValue;

        public EnumerableDataReaderFactoryBuilder<T> Add<TColumn>(string column, Expression<Func<T, TColumn>> expression)
        {
            var t = typeof(TColumn);

            var func = expression.Compile();

            // don't do any optimizations for boxing bools here to detect boxing occurring properly.
            Expression<Func<T, object>> objExpression = o => func(o);

            _objExpressions.Add(objExpression.Compile());

            if (_validTypes.Contains(t))
            {
                t = Nullable.GetUnderlyingType(t) ?? t; // data table doesn't accept nullable.
                _schemaTable.Columns.Add(column, t);
                _expressions.Add(expression);
            }
            else
            {
                Console.WriteLine($"Could not matching return type for {Name}.{column} of: {t.Name}");
                _schemaTable.Columns.Add(column); //add w/o type to force using GetValue

                _expressions.Add(objExpression);
            }

            return this;
        }

        public EnumerableDataReaderFactory<T> BuildFactory() => new EnumerableDataReaderFactory<T>(_schemaTable, _expressions, _objExpressions);

        public string Name { get; }

        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _schemaTable.Dispose();
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
