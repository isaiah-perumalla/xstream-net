using NUnit.Framework;
using Xstream.Tests.Accepatance;
using Xstream.Tests.Converters;

namespace xstream.Converters {
    [TestFixture]
    public class NullConverterTest : ConverterTestCase {
        [Test]
        public void InternalNulls() {
            SerialiseAndDeserialise(new Person("name"));
        }

        [Test]
        public void OriginalValueNull() {
            SerialiseAndDeserialise(null);
        }
    }
}