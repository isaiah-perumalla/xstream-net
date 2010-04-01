using System;
using System.Collections;
using System.Collections.Generic;
using xstream;
using xstream.Converters.Collections;

namespace Xstream.Core.Converters.Collections {
    internal class DictionaryConverter : BaseDictionaryConverter<Dictionary<object, object>> {
        protected override IDictionary EmptyDictionary(XStreamReader reader) {
            return (IDictionary) DynamicInstanceBuilder.CreateInstance(Type.GetType(reader.GetAttribute(XsAttribute.classType)));
        }
    }
}