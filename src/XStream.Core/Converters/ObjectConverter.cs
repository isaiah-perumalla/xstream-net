﻿using System;
using xstream;
using Xstream.Core.Mappers;

namespace Xstream.Core.Converters {
    internal class ObjectConverter : Converter
    {
        private readonly IMapper mapper;

        public ObjectConverter(IMapper mapper) {
            this.mapper = mapper;
        }

        public bool CanConvert(Type type)
        {
            return true;
        }

        public void Marshall(object value, XStreamWriter writer, MarshallingContext context) {
            MarshalAs(value, value.GetType(), writer, context);
        }

        private void MarshalAs(object containingObject, Type type, XStreamWriter writer, MarshallingContext context)
        {
            if (type.Equals(typeof(object))) return;
            foreach (var field in mapper.GetSerializableFieldsIn(type))
            {
                field.WriteValueOn(writer, containingObject);
                context.ConvertAnother(field.GetObjectFrom(containingObject));
                writer.EndNode();
            }
            MarshalAs(containingObject, type.BaseType, writer, context);
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