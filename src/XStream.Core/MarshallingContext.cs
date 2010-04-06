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
            if (alreadySerialised.ContainsKey(value))
                writer.WriteAttribute(XsAttribute.references, alreadySerialised[value]);
            else{
                alreadySerialised.Add(value, writer.CurrentPath);
            var converter = converterLookup.GetConverter(value);
            converter.Marshall(value, writer, this);   
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