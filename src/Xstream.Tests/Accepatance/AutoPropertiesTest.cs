using NUnit.Framework;
using Xstream.Tests.Converters;

namespace Xstream.Tests.Accepatance {
    [TestFixture]
    public class AutoPropertiesTest : ConverterTestCase {
        [Test]
        public void HandlesAutoProperties() {
            var objectWithAnAutoProperty = new ClassWithAnAutoProperty {AutoProperty = 10};
            const string serializedXml = @"<ClassWithAnAutoProperty class=" +
                                        "\"Xstream.Tests.Accepatance.AutoPropertiesTest+ClassWithAnAutoProperty, XStream.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\">" +
                                        "<AutoProperty>10</AutoProperty></ClassWithAnAutoProperty>";

            AssertXmlEquals(serializedXml, xstream.ToXml(objectWithAnAutoProperty));
            Assert.AreEqual(objectWithAnAutoProperty, xstream.FromXml(serializedXml));
            
        }

        internal class ClassWithAnAutoProperty {
            public int AutoProperty { get; set; }

            private bool Equals(ClassWithAnAutoProperty other) {
                return other.AutoProperty == AutoProperty;
            }

            public override bool Equals(object obj) {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != typeof (ClassWithAnAutoProperty)) return false;
                return Equals((ClassWithAnAutoProperty) obj);
            }

            public override int GetHashCode() {
                return AutoProperty;
            }
        }
    }
}