using System;
using NUnit.Framework;
using xstream;

namespace Xstream.Tests.Converters {
    public abstract class ConverterTestCase {
        protected Core.XStream xstream = new Core.XStream();

        internal void SerialiseAssertAndDeserialise(object value, string expectedSerialisedObject, AssertEqualsDelegate equalsDelegate) {
            AssertXmlEquals(expectedSerialisedObject, SerialiseAndDeserialise(value, equalsDelegate));
        }

        internal void SerialiseAssertAndDeserialise(object value, string expectedSerialisedObject) {
            SerialiseAssertAndDeserialise(value, expectedSerialisedObject, XStreamAssert.AreEqual);
        }

        internal string SerialiseAndDeserialise(object value) {
            return SerialiseAndDeserialise(value, XStreamAssert.AreEqual);
        }

        internal string SerialiseAndDeserialise(object value, AssertEqualsDelegate equalsDelegate) {
            string actualSerialisedObject = xstream.ToXml(value);
            Console.WriteLine(actualSerialisedObject);
            equalsDelegate(value, xstream.FromXml(actualSerialisedObject));
            return actualSerialisedObject;
        }

        protected static void AssertXmlEquals(string expected, string actual) {
            Assert.AreEqual(RemoveWhitespace(expected), RemoveWhitespace(actual));
        }

        private static string RemoveWhitespace(string s) {
            return s.Replace(" ", "").Replace("\r", "").Replace("\n", "").Replace("\t", "");
        }
    }

    internal delegate void AssertEqualsDelegate(object first, object second);
}