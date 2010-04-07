using System;
using xstream;
using Xstream.Core.Mappers;

namespace Xstream.Core.Converters.Collections {
    internal class ArrayConverter : Converter {
        private IMapper mapper;
        private const string ARRAY_TYPE = "array-type";

        public ArrayConverter(IMapper mapper) {
            this.mapper = mapper;
        }

        public bool CanConvert(Type type) {
            return type.IsArray;
        }

        public void Marshall(object value, XStreamWriter writer, MarshallingContext context) {
            Array array = (Array) value;
            string typeName = value.GetType().AssemblyQualifiedName;
            int lastIndexOfBrackets = typeName.LastIndexOf("[]");
            string arrayType = string.Concat(typeName.Substring(0, lastIndexOfBrackets), typeName.Substring(lastIndexOfBrackets + 2));
            writer.WriteAttribute(ARRAY_TYPE, arrayType);
            foreach (object o in array)
                context.ConvertOriginal(o);
        }

        public object UnMarshall(XStreamReader reader, UnmarshallingContext context, Type type) {
            int count = reader.NoOfChildren();
            Array result = Array.CreateInstance(Type.GetType(reader.GetAttribute(ARRAY_TYPE)), count);
            if (count != 0) {
                reader.MoveDown();
                for (int i = 0; i < count; i++) {
                    var serializedValue = ReadSerializedValue(reader);
                    var elementType = mapper.ResolveClassTypeFor(serializedValue);
                    result.SetValue(context.ConvertAnother(result, elementType), i);
                    reader.MoveNext();
                }
                reader.MoveUp();
            }
            return result;
        }
        private static SerializedValue ReadSerializedValue(XStreamReader xStreamReader)
        {
            var tagName = xStreamReader.GetNodeName();
            string attributeValue = xStreamReader.GetAttribute(XsAttribute.classType);
            string nullValue = xStreamReader.GetAttribute(XsAttribute.Null);

            var classAtrribute = new XsAttribute(XsAttribute.classType, attributeValue);
            var nullAtrribute = new XsAttribute(XsAttribute.Null, nullValue);
            return new SerializedValue(tagName, classAtrribute, nullAtrribute);
        }
    }

}