using System;
using System.Linq;

namespace Microsoft.Data.SqlClient.Benchmark.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Any(a => a == "--profile"))
            {
                var b = new SqlBulkCopyBenchmark.InternalBenchmark();
                b.BulkCopy();
            }
            else
            {
                SqlBulkCopyBenchmark.RunBenchmark();
            }
        }
    }
}
