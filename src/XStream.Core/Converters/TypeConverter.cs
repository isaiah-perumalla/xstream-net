using System;
using xstream.Converters;

namespace Xstream.Core.Converters {
    internal class TypeConverter : SingleValueConverter<Type> {
        public TypeConverter() : base(Type.GetType) {}

        public override bool CanConvert(Type type) {
            return typeof (Type).IsAssignableFrom(type);
        }
    }
}