using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PwC.Data.SqlClient.Tests
{
    public class ValueTypeConverterTests
    {
        [Fact]
        public void ObjectConversionSuccessful()
        {
            object test = 0m;

            decimal converted = ValueTypeConverter<object, decimal>.Convert(test);

            Assert.Equal(test, converted);
        }

        [Fact]
        public void SelfConversionSuccessful()
        {
            decimal test = 0m;

            decimal converted = ValueTypeConverter<decimal, decimal>.Convert(test);

            Assert.Equal(test, converted);
        }

        [Fact]
        public void AssignableConversionSuccessful()
        {
            SqlDecimal test = 0m;

            decimal converted = ValueTypeConverter<SqlDecimal, decimal>.Convert(test);

            Assert.Equal(test, converted);
        }
    }
}
