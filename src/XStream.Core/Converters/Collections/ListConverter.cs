using System;
using System.Collections;
using xstream;
using Xstream.Core.Mappers;

namespace Xstream.Core.Converters.Collections {
    internal class ListConverter : Converter {
        private IMapper mapper = new DefaultMapper();
        private const string LIST_TYPE = "list-type";

        public bool CanConvert(Type type) {
            return typeof (ArrayList).Equals(type);
        }

        public void Marshall(object value, XStreamWriter writer, MarshallingContext context) {
            IList list = (IList) value;
            writer.WriteAttribute(LIST_TYPE, value.GetType().FullName);
            foreach (object o in list)
                context.ConvertOriginal(o);
        }

        public object UnMarshall(XStreamReader reader, UnmarshallingContext context, Type type) {
            IList result = (IList) DynamicInstanceBuilder.CreateInstance(Type.GetType(reader.GetAttribute(LIST_TYPE)));
            int count = reader.NoOfChildren();
            reader.MoveDown();
            for (int i = 0; i < count; i++) {
                object item = ReadItem(reader, context, result);
                result.Add(item);
                reader.MoveNext();
            }
            reader.MoveUp();
            return result;
        }

        private object ReadItem(XStreamReader reader, UnmarshallingContext context, IList result) {
            var serializedValue = ReadSerializedValue(reader);
            var type = mapper.ResolveClassTypeFor(serializedValue);
            return context.ConvertAnother(result, type);
        }

        //Todo: duplicated!! refactor
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