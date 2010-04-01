using System;
using xstream;

namespace Xstream.Core.Converters {
    internal class NullConverter : Converter {
        public bool CanConvert(Type type) {
            return false;
        }

        public void Marshall(object value, XStreamWriter writer, MarshallingContext context) {
            writer.WriteAttribute(XsAttribute.Null, true.ToString());
        }

        public object UnMarshall(XStreamReader reader, UnmarshallingContext context) {
            return null;
        }
    }
}