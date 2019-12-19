using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
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
        private IDataReader _reader;
        private static readonly string _tableName = "dbo.DataReaderBenchmark";
        private static readonly string _connString;
        private static readonly int _count = 100000;

        private class ItemToCopy
        {
            public decimal Col1 { get; } = Convert.ToDecimal(_sR.NextDouble());
            public string Col2 { get; } = _sR.Next().ToString();
            public int Col3 { get; } = _sR.Next();
            public bool Col4 { get; } = true;
        }

        private static int _runId = 0;

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

                CREATE TABLE {_tableName} (
                    RunId INT NOT NULL,
                    Col1 DECIMAL NOT NULL,
                    Col2 NVARCHAR(50) NULL,
                    Col3 INT NOT NULL,
                    Col4 BIT NOT NULL
                )
            ");

            _sR = new Random();

            var item = new ItemToCopy();

            _items = Enumerable.Range(0, _count).Select(x => item).ToArray();
        }

        [GlobalSetup]
        public void Setup()
        {
            _reader = new EnumerableDataReaderFactoryBuilder<ItemToCopy>("test")
                .Add("RunId", i => _runId)
                .Add("Col1", i => i.Col1)
                .Add("Col2", i => i.Col2)
                .Add("Col3", i => i.Col3)
                .Add("Col4", i => i.Col4)
                .BuildFactory()
                .CreateReader(_items)
            ;
        }

        [Benchmark]
        public void NewBulkCopy_NewReader()
        {
            _runId = 1;
            _reader.Close(); // this resets the reader
            OldBulkCopy(_reader);
        }

        // When I was doing the compare -- I changed all the namespaces around so I could load both assemblies.
        //[Benchmark]
        //public void OldBulkCopy_NewReader()
        //{
        //    _runId = 4;
        //    _newReader.Close();
        //    OldBulkCopy(_reader);
        //}

        private static void OldBulkCopy(IDataReader reader)
        {
            using var bc = new Microsoft.Data.SqlClient.SqlBulkCopy(_connString, Microsoft.Data.SqlClient.SqlBulkCopyOptions.TableLock);

            bc.BatchSize = _count;
            bc.DestinationTableName = _tableName;
            bc.BulkCopyTimeout = 60;

            bc.WriteToServer(reader);
        }

        private static void NewBulkCopy(IDataReader reader)
        {
            //using var bc = new cmeyertons.Data.SqlClient.SqlBulkCopy(_connString, PwC.Data.SqlClient.SqlBulkCopyOptions.TableLock);

            //bc.BatchSize = _count;
            //bc.DestinationTableName = _tableName;
            //bc.BulkCopyTimeout = 60;

            //bc.WriteToServer(reader);
        }
    }
}
