// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Microsoft.Data.SqlClient.Benchmark.CLI
{
    public static class SqlBulkCopyBenchmark
    {
        private const string _database = "SqlBulkCopyBenchmark";
        private const int _count = (int)1e5;
        private const int _iterationCount = 20;

        private static readonly IEnumerable<ItemToCopy> _items;
        private static readonly IDataReader _reader;


        private static readonly string _connString = new SqlConnectionStringBuilder()
        {
            DataSource = "localhost",
            InitialCatalog = _database,
            IntegratedSecurity = true
        }.ToString();

        static SqlBulkCopyBenchmark()
        {
            var item = new ItemToCopy();

            using var cmaster = new SqlConnection(new SqlConnectionStringBuilder(_connString) { InitialCatalog = "master" }.ToString());
            cmaster.Open();

            cmaster.Execute($@"
                ALTER DATABASE [{_database}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;

                DROP DATABASE IF EXISTS [{_database}];

                CREATE DATABASE [{_database}];
            ");

            using var c = new SqlConnection(_connString);

            c.Execute(ItemToCopy.TableSql);

            _items = Enumerable.Range(0, _count).Select(x => item).ToArray();

            _reader = ItemToCopy.CreateReader(_items);
        }

        public static void RunBenchmark()
        {
            BenchmarkRunner.Run<InternalBenchmark>(BenchmarkConfig.Instance);
        }

        public class InternalBenchmark
        {
            [Benchmark]
            public void BulkCopy()
            {
                for (int i = 1; i <= _iterationCount; i++)
                {
                    Console.WriteLine($"{DateTime.Now} - Iteration #{i}");
                    _reader.Close(); // this resets the reader

                    using (var bc = new SqlBulkCopy(_connString, SqlBulkCopyOptions.TableLock))
                    {
                        bc.BatchSize = _count;
                        bc.DestinationTableName = ItemToCopy.TableName;
                        bc.BulkCopyTimeout = 60;

                        bc.WriteToServer(_reader);
                    }
                }

                Console.WriteLine($"{DateTime.Now} - Finished");
            }
        }
    }
}
