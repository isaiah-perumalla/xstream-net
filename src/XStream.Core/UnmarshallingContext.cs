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

        public object ConvertAnother(object parent, Type type) {

            Converter converter = converterLookup.GetConverter(type);
            if (converter == null) return Start();
            return converter.UnMarshall(reader, this);
        }

        public object Start() {
            var serializedValue = ReadSerializedValue(reader);
            var type = mapper.ResolveTypeFor(serializedValue);
             
            var converter = converterLookup.GetConverter(type);
            if (converter != null) return converter.UnMarshall(reader, this);
            
            //ToDo: duplicated in unmarshaller
            var result = FindReferenceFromCurrentNode();
            if (result != null) return result;

           /* converter = new ObjectConverter(mapper);
            converter.UnMarshall(reader, this);*/

            return new Unmarshaller(reader, this, converterLookup, mapper).Unmarshal(type);
        }

        private static SerializedValue ReadSerializedValue(XStreamReader xStreamReader) {
            var tagName = xStreamReader.GetNodeName();
            string attributeValue = xStreamReader.GetAttribute(XsAttribute.classType);
            string nullValue = xStreamReader.GetAttribute(XsAttribute.Null);
            
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

    internal class ObjectConverter : Converter {
        private readonly IMapper mapper;

        public ObjectConverter(IMapper mapper) {
            this.mapper = mapper;
        }

        public bool CanConvert(Type type) {
            throw new NotImplementedException();
        }

        public void Marshall(object value, XStreamWriter writer, MarshallingContext context) {
            throw new NotImplementedException();
        }

        public object UnMarshall(XStreamReader reader, UnmarshallingContext context) {
            throw new NotImplementedException();
        }
    }
}