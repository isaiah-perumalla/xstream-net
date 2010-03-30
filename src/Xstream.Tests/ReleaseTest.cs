using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using xstream;
using Xstream.Core;

namespace Xstream.Tests {
    [TestFixture]
    public class ReleaseTest {
        private static readonly List<Type> publicTypes = new List<Type>();

        public ReleaseTest() {
            publicTypes.Add(typeof (Core.XStream));
            publicTypes.Add(typeof (MarshallingContext));
            publicTypes.Add(typeof (UnmarshallingContext));
            publicTypes.Add(typeof (DontSerialiseAttribute));
        }

        [Test]
        public void AllClassesAreInternal() {
            Type[] allTypes = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type type in allTypes) {
                if (type.IsVisible && !publicTypes.Contains(type) && !type.IsInterface && !type.IsAbstract && IsNotTest(type) &&
                    !typeof(Exception).IsAssignableFrom(type))
                    Assert.Fail(type + " is visible outside");
            }
        }

        private static bool IsNotTest(Type type) {
            object[] attributes = type.GetCustomAttributes(false);
            foreach (object attribute in attributes) {
                if (attribute.GetType().IsAssignableFrom(typeof (TestFixtureAttribute)))
                    return false;
            }
            return true;
        }
    }
}