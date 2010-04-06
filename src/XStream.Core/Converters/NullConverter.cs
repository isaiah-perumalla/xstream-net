using System;
using xstream;
using Xstream.Core.Mappers;

namespace Xstream.Core.Converters {
    internal class NullConverter : Converter {
        public bool CanConvert(Type type) {
            return typeof(XNull) == type;
        }

        public void Marshall(object value, XStreamWriter writer, MarshallingContext context) {
            writer.WriteAttribute(XsAttribute.Null, true.ToString());
        }

        public object UnMarshall(XStreamReader reader, UnmarshallingContext context, Type type) {
            return null;
        }
    }
}