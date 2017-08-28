using NUnit.Framework;
using FluentAssertions;

namespace TemporalExpressions.Tests
{
    [TestFixture]
    public class SerializerTests
    {
        [TestCase("{dayinmonth(count:1,day:sunday)}")]
        [TestCase("{dayinmonth(count:5,day:monday)}")]
        [TestCase("{dayinmonth(count:-1,day:tuesday)}")]
        [TestCase("{dayinmonth(count:2,day:wednesday)}")]
        [TestCase("{dayinmonth(count:1,day:thursday)}")]
        [TestCase("{dayinmonth(count:1,day:friday)}")]
        [TestCase("{dayinmonth(count:1,day:saturday)}")]
        [TestCase("{rangeeachyear(month:1)}")]
        [TestCase("{rangeeachyear(startmonth:1,endmonth:2)}")]
        [TestCase("{rangeeachyear(startmonth:1,endmonth:2,startday:3,endday:4)}")]
        [TestCase("{difference(included:{rangeeachyear(month:1)},excluded:{rangeeachyear(month:2)})}")]
        [TestCase("{intersection(elements:{rangeeachyear(month:1)};{rangeeachyear(month:1)})}")]
        [TestCase("{union(elements:{rangeeachyear(month:1)};{rangeeachyear(month:1)})}")]
        [TestCase("{regularinterval(year:2017,month:1,day:1,count:2,unit:day)}", "01/01/17")]
        [TestCase("{true}")]
        [TestCase("{false}")]
        [TestCase("{not(expression:{true})}")]
        public void ShouldSerializeCorrectly(string input)
        {
            var compiled = Compiler.Compiler.Compile(input);

            var serialized = Serializer.Serializer.Serialize(compiled);

            serialized.Should().Be(input);
        }
    }
}
