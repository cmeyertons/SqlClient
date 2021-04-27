using System;

namespace Microsoft.Data.SqlClient.Benchmark.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlBulkCopyBenchmark.RunBenchmark();
        }
    }
}
