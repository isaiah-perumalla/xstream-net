using System.Xml.Linq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Xstream.Core;
using Xstream.Core.Mappers;

namespace Xstream.Tests.Unit {
    
    [TestFixture]
    public class DefaultMapperTests {
    
        [Test]
        public void GetsSerializeNodeValueForAType() {

            var mapper = new DefaultMapper();
            var serializedValue = mapper.GetSerializedClassFor(GetType());
            Assert.That(serializedValue, Is.EqualTo(new SerializedValue("DefaultMapperTests", attribute("class", GetType().AssemblyQualifiedName))));
           
        }

        private XsAttribute attribute(string name, string value) {
            return new XsAttribute(name, value);
        }
    }
}