using System;
using System.Collections;
using System.Collections.Generic;
using xstream;
using Xstream.Core.Converters;
using Xstream.Core.Mappers;

namespace Xstream.Core {
    public class UnmarshallingContext {
        private readonly Dictionary<string, object> alreadyDeserialised = new Dictionary<string, object>();
        private readonly XStreamReader reader;
        private readonly ConverterLookup converterLookup;
        private readonly IMapper mapper;

        internal UnmarshallingContext(XStreamReader reader, ConverterLookup converterLookup, IMapper mapper) {
            this.reader = reader;
            this.converterLookup = converterLookup;
            this.mapper = mapper;
        }

        public object ConvertAnother() {
            string nullAttribute = reader.GetAttribute(XsAttribute.Null);
            if (nullAttribute != null && nullAttribute == "true") return null;
            object result = Find();
            if (result != null) return result;
           
            //Todo: use mapper here to resolve type
            //Todo: move into mapper, should be give real type
            string typeName = reader.GetNodeName();
            var type = Type.GetType(typeName);
            if (typeName.EndsWith("-array")) type = typeof(Array);
            if (typeName.EndsWith("-list")) type = typeof(ArrayList);
            Converter converter = converterLookup.GetConverter(type);
           
           
            if (converter == null) return Start();
            return converter.UnMarshall(reader, this);
        }

        public object Start() {
            var tagName = reader.GetNodeName();
            string attributeValue = reader.GetAttribute(XsAttribute.classType);
            var classAtrribute = new XsAttribute(XsAttribute.classType, attributeValue);
            var nullAtrribute = new XsAttribute(XsAttribute.Null, attributeValue);
            var serializedValue = new SerializedValue(tagName, classAtrribute, nullAtrribute);
            Type type = mapper.ResolveTypeFor(serializedValue);
             
            Converter converter = converterLookup.GetConverter(type);
            if (converter != null) return converter.UnMarshall(reader, this);
            return new Unmarshaller(reader, this, converterLookup, mapper).Unmarshal(type);
        }

        public void StackObject(object value) {
            try {
                alreadyDeserialised.Add(reader.CurrentPath, value);
            }
            catch (ArgumentException e) {
                throw new ConversionException(string.Format("Couldn't add path:{0}, value: {1}", reader.CurrentPath, value), e);
            }
        }

        public object Find() {
            string referencesAttribute = reader.GetAttribute(XsAttribute.references);
            if (!string.IsNullOrEmpty(referencesAttribute)) return alreadyDeserialised[referencesAttribute];
            return null;
        }
    }
}