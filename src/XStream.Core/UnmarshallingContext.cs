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

            var serializedValue = ReadSerializedValue();
            var type = mapper.ResolveTypeFor(serializedValue);
            Converter converter = converterLookup.GetConverter(type);
            if (converter == null) return Start();
            return converter.UnMarshall(reader, this);
        }

        public object Start() {
            var serializedValue = ReadSerializedValue();
            var type = mapper.ResolveTypeFor(serializedValue);
             
            var converter = converterLookup.GetConverter(type);
            if (converter != null) return converter.UnMarshall(reader, this);
            return new Unmarshaller(reader, this, converterLookup, mapper).Unmarshal(type);
        }

        private SerializedValue ReadSerializedValue() {
            var tagName = reader.GetNodeName();
            string attributeValue = reader.GetAttribute(XsAttribute.classType);
            string nullValue = reader.GetAttribute(XsAttribute.Null);
            
            var classAtrribute = new XsAttribute(XsAttribute.classType, attributeValue);
            var nullAtrribute = new XsAttribute(XsAttribute.Null, nullValue);
            return new SerializedValue(tagName, classAtrribute, nullAtrribute);
        }

        public void StackObject(object value) {
            try {
                alreadyDeserialised.Add(reader.CurrentPath, value);
            }
            catch (ArgumentException e) {
                throw new ConversionException(string.Format("Couldn't add path:{0}, value: {1}", reader.CurrentPath, value), e);
            }
        }

        public object FindReferenceFromCurrentNode() {
            string referencesAttribute = reader.GetAttribute(XsAttribute.references);
            if (!string.IsNullOrEmpty(referencesAttribute)) return alreadyDeserialised[referencesAttribute];
            return null;
        }
    }
}