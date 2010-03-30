using System.Reflection;
using NUnit.Framework;
using Xstream.Tests.Converters;

namespace xstream.Converters {
    [TestFixture]
    public class EnumConverterTest : ConverterTestCase {
        [Test]
        public void ConvertsEnums() {
            SerialiseAndDeserialise(BindingFlags.Public);
        }
    }
}