using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Xstream.Core;
using Xstream.Core.Mappers;

namespace Xstream.Tests.Unit {
    
    [TestFixture]
    public class DefaultMapperTests {
        private DefaultMapper mapper;
        private static Type innerClassType = typeof(InnerClass);
        private string innerClassName = innerClassType.AssemblyQualifiedName;

        [SetUp]
        public void BeforeTest() {
            mapper = new DefaultMapper();
        }

        [Test]
        public void CanMapToSerializeValueForAType() {

            
            var serializedValue = mapper.SerializedTypeFor(GetType());
            Assert.That(serializedValue, Is.EqualTo(new SerializedValue("DefaultMapperTests", attribute("class", GetType().AssemblyQualifiedName))));
           
        }

        [Test]
        public void CanMapToSerializeValueForGenericType() {
            var genericObjType = typeof(GenericObject<int>);
            var expectedValue = serializedValue("GenericObject", attribute("class", genericObjType.AssemblyQualifiedName));
            Assert.AreEqual(expectedValue, mapper.SerializedTypeFor(genericObjType));
        }
        
        [Test]
        public void CanMapToSerializeValueForArray() {
            var arrayType = typeof(int[]);
            var expectedValue = serializedValue("Int32-array", attribute("class", arrayType.AssemblyQualifiedName));
            Assert.AreEqual(expectedValue, mapper.SerializedTypeFor(arrayType));
        }
        
        [Test]
        public void CanMapToSerializeValueForInnerClass() {
            var expectedValue = serializedValue("InnerClass", attribute("class", innerClassName));
            Assert.AreEqual(expectedValue, mapper.SerializedTypeFor(innerClassType));
        }
        
        [Test]
        public void CanResolveTypeWhenExplicitClassAttribute() {
            var serializedType = serializedValue("SomeClass", attribute("class", innerClassName));
            var expectedType = innerClassType;
            Assert.AreEqual(expectedType, mapper.ResolveClassTypeFor(serializedType));
        }
        
        [Test]
        public void CanResolveTypeForNull() {
            var serializedType = serializedValue("SomeClass", attribute(XsAttribute.Null, true.ToString()));
            var expectedType = typeof(XNull);
            Assert.AreEqual(expectedType, mapper.ResolveClassTypeFor(serializedType));
        }



        internal class InnerClass {
        }

        private SerializedValue serializedValue(string value, params XsAttribute[] xsAttributes) {
            return new SerializedValue(value, xsAttributes);
        }


        private XsAttribute attribute(string name, string value) {
            return new XsAttribute(name, value);
        }
    }
}