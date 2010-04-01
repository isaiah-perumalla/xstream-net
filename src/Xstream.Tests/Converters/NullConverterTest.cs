using NUnit.Framework;
using Xstream.Tests.Accepatance;

namespace Xstream.Tests.Converters {
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