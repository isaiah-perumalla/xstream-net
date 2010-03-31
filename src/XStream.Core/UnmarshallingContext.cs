using System;
using System.Collections.Generic;
using System.Reflection;
using Xstream.Core;
using Xstream.Core.Converters;
using Xstream.Core.Mappers;


namespace xstream {
    public class UnmarshallingContext {
        private readonly Dictionary<string, object> alreadyDeserialised = new Dictionary<string, object>();
        private readonly XStreamReader reader;
        private readonly ConverterLookup converterLookup;
        private readonly IMapper _mapper;
        private readonly List<Assembly> assemblies;

        internal UnmarshallingContext(XStreamReader reader, ConverterLookup converterLookup, IMapper mapper, List<Assembly> assemblies) {
            this.reader = reader;
            this.converterLookup = converterLookup;
            _mapper = mapper;
            this.assemblies = assemblies;
        }

        public object ConvertAnother() {
            string nullAttribute = reader.GetAttribute(Attributes.Null);
            if (nullAttribute != null && nullAttribute == "true") return null;
            object result = Find();
            if (result != null) return result;
            Converter converter = converterLookup.GetConverter(reader.GetNodeName());
            if (converter == null) return ConvertOriginal();
            return converter.FromXml(reader, this);
        }

        public object ConvertOriginal() {
            string nodeName = reader.GetNodeName();
            Type type = TypeToUse(nodeName);
            Converter converter = converterLookup.GetConverter(type);
            if (converter != null) return converter.FromXml(reader, this);
            return new Unmarshaller(reader, this, converterLookup, _mapper).Unmarshal(type);
        }

        private Type TypeToUse(string nodeName) {
            
            string typeName = reader.GetAttribute(Attributes.classType);
            return GetTypeFromOtherAssemblies(typeName);
        }

        internal Type GetTypeFromOtherAssemblies(string typeName) {
            Type type = Type.GetType(typeName);
            int indexOfComma = typeName.IndexOf(',');
            if (type == null) {
                string assemblyName = typeName.Substring(indexOfComma + 2);
                string actualTypeName = typeName.Substring(0, indexOfComma);
                foreach (Assembly assembly in assemblies) {
                    if (assemblyName.Equals(assembly.FullName)) type = assembly.GetType(actualTypeName);
                    if (type != null) break;
                }
                if (type == null) throw new ConversionException("Couldn't deserialise from " + typeName);
            }
            return type;
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
            string referencesAttribute = reader.GetAttribute(Attributes.references);
            if (!string.IsNullOrEmpty(referencesAttribute)) return alreadyDeserialised[referencesAttribute];
            return null;
        }
    }
}