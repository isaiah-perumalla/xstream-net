using System;
using System.Reflection;
using xstream;
using Xstream.Core.Mappers;

namespace Xstream.Core
{
    internal class Marshaller
    {
        private readonly XStreamWriter writer;
        private readonly MarshallingContext context;
        private readonly IMapper mapper ;

        public Marshaller(XStreamWriter writer, MarshallingContext context, IMapper mapper)
        {
            this.writer = writer;
            this.mapper = mapper;
            this.context = context;
        }

        public void Marshal(object value)
        {
            MarshalAs(value, value.GetType());
        }

        private void MarshalAs(object value, Type type)
        {
            if (type.Equals(typeof (object))) return;
            foreach (var field in mapper.GetSerializableFieldsIn(type))
            {
                writer.StartNode(field.SerializedName);
                WriteClassNameIfNeedBe(value, field);
                context.ConvertAnother(field.GetObjectFrom(value));
                writer.EndNode();
            }
            MarshalAs(value, type.BaseType);
        }

        private void WriteClassNameIfNeedBe(object value, Field field)
        {
            object fieldValue = field.GetObjectFrom(value);
            if (fieldValue == null) return;
            Type actualType = fieldValue.GetType();
            if (!field.FieldType.Equals(actualType))
                writer.WriteAttribute(XsAttribute.classType, actualType.AssemblyQualifiedName);
        }
    }
}