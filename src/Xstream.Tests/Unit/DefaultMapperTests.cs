using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Xstream.Core;
using Xstream.Core.Mappers;

namespace Xstream.Tests.Unit {
    
    [TestFixture]
    public class DefaultMapperTests {
        private DefaultMapper mapper;

        [SetUp]
        public void BeforeTest() {
            mapper = new DefaultMapper();
        }

        [Test]
        public void CanMapToSerializeValueForAType() {

            
            var serializedValue = mapper.GetSerializedClassFor(GetType());
            Assert.That(serializedValue, Is.EqualTo(new SerializedValue("Xstream.Tests.Unit.DefaultMapperTests", attribute("class", GetType().AssemblyQualifiedName))));
           
        }

        [Test]
        public void CanMapToSerializeValueForGenericType() {
            var genericObjType = typeof(GenericObject<int>);
            var expectedValue = serializedValue("Xstream.Tests.GenericObject", attribute("class", genericObjType.AssemblyQualifiedName));
            Assert.AreEqual(expectedValue, mapper.GetSerializedClassFor(genericObjType));
        }

        private SerializedValue serializedValue(string value, params XsAttribute[] xsAttributes) {
            return new SerializedValue(value, xsAttributes);
        }


        private XsAttribute attribute(string name, string value) {
            return new XsAttribute(name, value);
        }
    }
}