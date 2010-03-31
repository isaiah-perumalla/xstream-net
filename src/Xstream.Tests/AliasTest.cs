using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Xstream.Core;

namespace Xstream.Tests {
    
    [TestFixture]
    public class AliasTest  {
    
        [Test, Ignore("work in progress")]
        public void ShouldAllowAliasingOfAClass(){

            var xStream = new XStream();
            xStream.Alias<Software>("software");

            const string expectedXml = "<software><name>apache</name></software>";

            Assert.That(xStream.ToXml(new Software("apache")), Is.EqualTo(expectedXml));
            Assert.That(xStream.FromXml(expectedXml), Is.EqualTo(new Software("apache")));
        }
    }

    internal class Software {
        private readonly string name;

        public Software(string name) {
            this.name = name;
        }

        public bool Equals(Software other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.name, name);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Software)) return false;
            return Equals((Software) obj);
        }

        public override int GetHashCode() {
            return (name != null ? name.GetHashCode() : 0);
        }
    }
}