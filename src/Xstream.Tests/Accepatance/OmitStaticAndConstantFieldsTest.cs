using NUnit.Framework;
using Xstream.Tests.Converters;

namespace Xstream.Tests.Accepatance {
    [TestFixture]
    public class OmitStaticAndConstantFieldsTest : ConverterTestCase {
        [Test]
        public void DoesntSerialiseConstants() {
            Assert.AreEqual(false, xstream.ToXml(new ObjectWithConstantAndStatic()).Contains(ObjectWithConstantAndStatic.constant));
        }

        [Test]
        public void DoesntSerialiseStatics() {
            Assert.AreEqual(false, xstream.ToXml(new ObjectWithConstantAndStatic()).Contains(ObjectWithConstantAndStatic.stat));
        }

        private class ObjectWithConstantAndStatic {
            public const string constant = "Some stupid constant";
            public static readonly string stat = "Some stupid static";
        }
    }
}