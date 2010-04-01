using System;
using Xstream.Core;
using Xstream.Core.Converters;

namespace xstream.Converters {
    internal class SingleValueConverter<T> : Converter {
        private readonly Parse<T> parse;

        public SingleValueConverter(Parse<T> parse) {
            this.parse = parse;
        }

        public virtual bool CanConvert(Type type) {
            return type.Equals(typeof (T));
        }

        public void Marshall(object value, XStreamWriter writer, MarshallingContext context) {
            writer.SetValue(value.ToString());
        }

        public object UnMarshall(XStreamReader reader, UnmarshallingContext context) {
            return parse(reader.GetValue());
        }
    }

    internal delegate T Parse<T>(string s);
}