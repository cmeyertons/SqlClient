using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "--direct")
            {
                var b = new DataReaderGithubBenchmark();

                b.Setup();

                for (int i = 0; i < 10; i++)
                {
                    var sw = Stopwatch.StartNew();

                    Console.WriteLine($"Starting Batch {i}");
                    b.Decimal();

                    sw.Stop();

                    Console.WriteLine($"Finished batch {i} in {sw.ElapsedMilliseconds} ms.");
                }

                return;
            }

            var config = ManualConfig.Create(DefaultConfig.Instance);
            config.Options = ConfigOptions.DisableOptimizationsValidator;
            config.Add(new ConsoleLogger());

            BenchmarkDotNet.Running.BenchmarkRunner.Run(typeof(DataReaderGithubBenchmark), config);
        }
    }

    [SimpleJob(launchCount: 1)]
    [MarkdownExporter, RankColumn, MemoryDiagnoser]
    public class DataReaderGithubBenchmark
    {
        private const string DB = "cmeyertons_benchmark";
        private static readonly Random _sR;
        private static readonly IEnumerable<ItemToCopy> _items;
        private IDataReader _decimalReader;
        private IDataReader _stringReader;
        private IDataReader _intReader;
        private IDataReader _boolReader;
        private static readonly string _decimalTable = "dbo._decimalTable";
        private static readonly string _stringTable = "dbo._stringTable";
        private static readonly string _intTable = "dbo._intTable";
        private static readonly string _boolTable = "dbo._boolTable";
        private static readonly string _connString;
        private static readonly int _count = 100000;

        private class ItemToCopy
        {
            public decimal DecimalColumn { get; } = Convert.ToDecimal(_sR.NextDouble());
            public string StringColumn { get; } = _sR.Next().ToString();
            public int IntColumn { get; } = _sR.Next();
            public bool BoolColumn { get; } = true;
        }

        static DataReaderGithubBenchmark()
        {
            var csb = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder()
            {
                DataSource = ".",
                InitialCatalog = "master", 
                IntegratedSecurity = true,
            };

            var masterCs = csb.ToString();
            _connString = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(masterCs)
            {
                InitialCatalog = DB,
            }.ToString();

            using var masterc = new Microsoft.Data.SqlClient.SqlConnection(masterCs);
            using var c = new Microsoft.Data.SqlClient.SqlConnection(_connString);

            masterc.Execute(@$"
                DROP DATABASE IF EXISTS {DB};
                CREATE DATABASE {DB};
            ");

            c.Execute($@"
                USE {DB};

                CREATE TABLE {_decimalTable} (
                    DecimalColumn DECIMAL NOT NULL,
                )

                CREATE TABLE {_stringTable} (
                    StringColumn NVARCHAR(50) NULL,
                )

                CREATE TABLE {_intTable} (
                    IntColumn INT NOT NULL,
                )

                CREATE TABLE {_boolTable} (
                    BoolColumn BIT NOT NULL
                )
            ");

            _sR = new Random();

            var item = new ItemToCopy();

            _items = Enumerable.Range(0, _count).Select(x => item).ToArray();
        }

        [GlobalSetup]
        public void Setup()
        {
            _decimalReader = new EnumerableDataReaderFactoryBuilder<ItemToCopy>(_decimalTable)
                .Add("DecimalColumn", i => i.DecimalColumn)
                .BuildFactory()
                .CreateReader(_items)
            ;

            _stringReader = new EnumerableDataReaderFactoryBuilder<ItemToCopy>(_stringTable)
                .Add("StringColumn", i => i.StringColumn)
                .BuildFactory()
                .CreateReader(_items)
            ;

            _intReader = new EnumerableDataReaderFactoryBuilder<ItemToCopy>(_intTable)
                .Add("IntColumn", i => i.IntColumn)
                .BuildFactory()
                .CreateReader(_items)
            ;

            _boolReader = new EnumerableDataReaderFactoryBuilder<ItemToCopy>(_boolTable)
                .Add("BoolColumn", i => i.BoolColumn)
                .BuildFactory()
                .CreateReader(_items)
            ;
        }

        [Benchmark]
        public void Decimal() => BulkCopy(_decimalReader, _decimalTable);

        [Benchmark]
        public void String() => BulkCopy(_stringReader, _stringTable);

        [Benchmark]
        public void Int() => BulkCopy(_intReader, _intTable);

        [Benchmark]
        public void Bool() => BulkCopy(_boolReader, _boolTable);

        private static void BulkCopy(IDataReader reader,  string tableName)
        {
            reader.Close(); // this resets the reader

            using var bc = new Microsoft.Data.SqlClient.SqlBulkCopy(_connString, Microsoft.Data.SqlClient.SqlBulkCopyOptions.TableLock);

            bc.BatchSize = _count;
            bc.DestinationTableName = tableName;
            bc.BulkCopyTimeout = 60;

            bc.WriteToServer(reader);
        }
    }
}
