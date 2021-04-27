// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace Microsoft.Data.SqlClient.Benchmark.CLI
{
    public class EnumerableDataReaderFactory<T>
    {
        public DataTable SchemaTable { get; }
        public Func<T, object>[] ObjectGetters { get; }
        public Func<T, decimal>[] DecimalGetters { get; }
        public Func<T, decimal?>[] NullableDecimalGetters { get; }
        public Func<T, string>[] StringGetters { get; }
        public Func<T, double>[] DoubleGetters { get; }
        public Func<T, int>[] IntGetters { get; }
        public Func<T, long>[] LongGetters { get; }
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
            LongGetters = new Func<T, long>[expressions.Count];
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
                    case Expression<Func<T, object>> e:
                        break; // do nothing
                    case Expression<Func<T, decimal>> e:
                        DecimalGetters[i] = e.Compile();
                        break;
                    case Expression<Func<T, decimal?>> e:
                        NullableDecimalGetters[i] = e.Compile();
                        break;
                    case Expression<Func<T, string>> e:
                        StringGetters[i] = e.Compile();
                        break;
                    case Expression<Func<T, double>> e:
                        DoubleGetters[i] = e.Compile();
                        break;
                    case Expression<Func<T, int>> e:
                        IntGetters[i] = e.Compile();
                        break;
                    case Expression<Func<T, long>> e:
                        LongGetters[i] = e.Compile();
                        break;
                    case Expression<Func<T, int?>> e:
                        NullableIntGetters[i] = e.Compile();
                        break;
                    case Expression<Func<T, bool>> e:
                        BoolGetters[i] = e.Compile();
                        break;
                    case Expression<Func<T, bool?>> e:
                        NullableBoolGetters[i] = e.Compile();
                        break;
                    case Expression<Func<T, Guid>> e:
                        GuidGetters[i] = e.Compile();
                        break;
                    case Expression<Func<T, DateTime>> e:
                        DateTimeGetters[i] = e.Compile();
                        break;
                    default:
                        throw new Exception($"Type missing: {expression.GetType().FullName}");
                }
            }
        }

        public IDataReader CreateReader(IEnumerable<T> items) => new EnumerableDataReader<T>(this, items.GetEnumerator());
    }
}
