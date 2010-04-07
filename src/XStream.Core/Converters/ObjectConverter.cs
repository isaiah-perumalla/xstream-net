using System;
using xstream;
using Xstream.Core.Mappers;
using xstream.Utilities;

namespace Xstream.Core.Converters {
    internal class ObjectConverter : Converter
    {
        private readonly IMapper mapper;
        private readonly ConverterLookup converterLookup;

        public ObjectConverter(IMapper mapper, ConverterLookup converterLookup) {
            this.mapper = mapper;
            this.converterLookup = converterLookup;
        }

        public bool CanConvert(Type type)
        {
            return true;
        }

        public void Marshall(object value, XStreamWriter writer, MarshallingContext context) {
            Type type = value.GetType();
            if (type.Equals(typeof(object))) return;
            foreach (var field in mapper.GetSerializableFieldsIn(type))
            {
                writer.StartNode(field.SerializedName);
                WriteClassNameIfNeedBe(value, field, writer);
                context.ConvertAnother(field.GetObjectFrom(value));
                writer.EndNode();
            }
            MarshalAs(value, type.BaseType, writer, context);
        }

        private void MarshalAs(object value, Type type, XStreamWriter writer, MarshallingContext context)
        {
            if (type.Equals(typeof(object))) return;
            foreach (var field in mapper.GetSerializableFieldsIn(type))
            {
                writer.StartNode(field.SerializedName);
                WriteClassNameIfNeedBe(value, field, writer);
                context.ConvertAnother(field.GetObjectFrom(value));
                writer.EndNode();
            }
            var baseType = type.BaseType;
            if (baseType.Equals(typeof(object))) return;
            foreach (var field in mapper.GetSerializableFieldsIn(baseType))
            {
                writer.StartNode(field.SerializedName);
                WriteClassNameIfNeedBe(value, field, writer);
                context.ConvertAnother(field.GetObjectFrom(value));
                writer.EndNode();
            }
            MarshalAs(value, baseType.BaseType, writer, context);
        }

        private void WriteClassNameIfNeedBe(object value, Field field, XStreamWriter writer)
        {
            object fieldValue = field.GetObjectFrom(value);
            if (fieldValue == null) return;
            Type actualType = fieldValue.GetType();
            if (!field.FieldType.Equals(actualType))
                writer.WriteAttribute(XsAttribute.classType, actualType.AssemblyQualifiedName);
        }
        public object UnMarshall(XStreamReader reader, UnmarshallingContext context, Type type)
        {
            var result = context.FindReferenceFromCurrentNode();
            if (result != null) return result;

            if (reader.GetAttribute(XsAttribute.Null) == true.ToString())
                return null;
            result = DynamicInstanceBuilder.CreateInstance(type);
            context.StackObject(result);
            UnmarshalAs(result, type, reader, context);
            return result;
        }

        private void UnmarshalAs(object result, Type type, XStreamReader reader, UnmarshallingContext context)
        {
            if (type.Equals(typeof(object))) return;

            foreach (var field in mapper.GetSerializableFieldsIn(type))
            {
                reader.MoveDown(field.SerializedName);
                
                var serializedField = ReadSerializedValue(reader);
                var fieldType = mapper.ResolveFieldTypeFor(field, serializedField);
                object fieldValue = context.ConvertAnother(result, fieldType);
                field.SetValue(result, fieldValue);
                reader.MoveUp();
            }
            UnmarshalAs(result, type.BaseType, reader, context);
        }

        //Todo: remove this, duplicated in many places
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