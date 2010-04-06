using System;
using System.Collections;
using System.Collections.Generic;
using xstream;
using Xstream.Core.Mappers;

namespace Xstream.Core.Converters.Collections {
    internal class DictionaryConverter : BaseDictionaryConverter<Dictionary<object, object>> {
        public DictionaryConverter(IMapper mapper) : base(mapper) {
        }

        protected override IDictionary EmptyDictionary(XStreamReader reader) {
            return (IDictionary) DynamicInstanceBuilder.CreateInstance(Type.GetType(reader.GetAttribute(XsAttribute.classType)));
        }
    }
}