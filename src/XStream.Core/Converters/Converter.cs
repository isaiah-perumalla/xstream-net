using System;
using xstream;

namespace Xstream.Core.Converters {
    public interface Converter {
        bool CanConvert(Type type);
        void ToXml(object value, XStreamWriter writer, MarshallingContext context);
        object FromXml(XStreamReader reader, UnmarshallingContext context);
    }
}