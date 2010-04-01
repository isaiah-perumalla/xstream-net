using System;
using Xstream.Core.Mappers;
using xstream.Utilities;

namespace Xstream.Core {
    public class MarshallingContext {
        private readonly AlreadySerialisedDictionary alreadySerialised = new AlreadySerialisedDictionary();
        private readonly XStreamWriter writer;
        private readonly ConverterLookup converterLookup;
        private readonly IMapper mapper;

        internal MarshallingContext(XStreamWriter writer, ConverterLookup converterLookup, IMapper mapper) {
            this.writer = writer;
            this.converterLookup = converterLookup;
            this.mapper = mapper;
        }

        internal void ConvertAnother(object value) {
            var converter = converterLookup.GetConverter(value);
            if (converter != null) converter.ToXml(value, writer, this);
            else ConvertObject(value);
        }

        private void ConvertObject(object value) {
            if (alreadySerialised.ContainsKey(value))
                writer.WriteAttribute(XsAttribute.references, alreadySerialised[value]);
            else {
                alreadySerialised.Add(value, writer.CurrentPath);
                new Marshaller(writer, this, mapper).Marshal(value);
            }
        }

        public void ConvertOriginal(object value) {
            StartNode(value);
            ConvertAnother(value);
            writer.EndNode();
        }

        private void StartNode(object value) {
            //ToDo: use domain specific null types
            var type = value != null ? value.GetType() : typeof (object);
            var serializedValue = mapper.SerializedTypeFor(type);
            serializedValue.WriteOn(writer);
           
        }
    }
}