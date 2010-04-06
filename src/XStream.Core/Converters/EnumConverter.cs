using System;
using Xstream.Core;
using Xstream.Core.Converters;

namespace xstream.Converters {
    internal class EnumConverter : Converter {
        public bool CanConvert(Type type) {
            return type.IsEnum;
        }

        public void Marshall(object value, XStreamWriter writer, MarshallingContext context) {
            writer.WriteAttribute(XsAttribute.AttributeType, value.GetType().AssemblyQualifiedName);
            writer.SetValue(value.ToString());
        }

        public object UnMarshall(XStreamReader reader, UnmarshallingContext context, Type type) {
            return Enum.Parse(Type.GetType(reader.GetAttribute(XsAttribute.AttributeType)), reader.GetValue());
        }
    }
}