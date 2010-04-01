using System;
using xstream;

namespace Xstream.Core.Converters {
    public interface Converter {
        bool CanConvert(Type type);
        void Marshall(object value, XStreamWriter writer, MarshallingContext context);
        object UnMarshall(XStreamReader reader, UnmarshallingContext context);
    }
}