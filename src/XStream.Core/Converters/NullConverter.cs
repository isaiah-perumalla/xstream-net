using System;
using Xstream.Core;
using Xstream.Core.Converters;

namespace xstream.Converters {
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