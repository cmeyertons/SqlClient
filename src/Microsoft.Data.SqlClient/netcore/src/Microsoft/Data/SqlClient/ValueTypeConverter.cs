using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Data.SqlClient
{
    /// <summary>
    /// Converts value types from a generic to the desired, underlying type without boxing.
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    public static class ValueTypeConverter<TIn, TOut>
    {
        /// <summary>
        /// Converts the value to the TOut type
        /// </summary>
        public static Func<TIn, TOut> Convert { get; }

        static ValueTypeConverter()
        {
            var paramExpr = Expression.Parameter(typeof(TIn));

            var label = Expression.Label();

            var lambda = typeof(TIn) != typeof(TOut)
                ? Expression.Lambda<Func<TIn, TOut>>(Expression.Convert(paramExpr, typeof(TOut)), paramExpr)
                : Expression.Lambda<Func<TIn, TOut>>(paramExpr, paramExpr)
            ;

            Convert = lambda.Compile();
        }
    }
}
