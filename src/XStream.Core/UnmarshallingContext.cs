using System;
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
            Converter converter = converterLookup.GetConverter(reader.GetNodeName());
            if (converter == null) return ConvertOriginal();
            return converter.FromXml(reader, this);
        }

        public object ConvertOriginal() {
            Type type = TypeToUse();
            Converter converter = converterLookup.GetConverter(type);
            if (converter != null) return converter.FromXml(reader, this);
            return new Unmarshaller(reader, this, converterLookup, mapper).Unmarshal(type);
        }

        private Type TypeToUse() {
            
            var typeName = reader.GetAttribute(XsAttribute.classType);
            return Type.GetType(typeName);
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