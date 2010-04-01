using System;
using NUnit.Framework;
using xstream.Utilities;

namespace Xstream.Tests.Unit {
    [TestFixture]
    public class XmlifierTest {
        [Test]
        public void HandlesGenerics() {
            Assert.AreEqual("Xstream.Tests.GenericObject", Xmlifier.UnXmlify(Xmlifier.Xmlify(typeof(GenericObject<int>))));
        }

        [Test]
        public void XmlifiesWithGenerics() {
            string actual = Xmlifier.Xmlify(typeof(GenericObject<int>));
         
            Assert.AreEqual("Xstream.Tests.GenericObject", actual);
        }
    }
}