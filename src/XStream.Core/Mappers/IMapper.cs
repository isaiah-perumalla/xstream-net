using System;
using System.Collections.Generic;
using System.Reflection;

namespace Xstream.Core.Mappers
{
    internal interface IMapper
    {
        
        IEnumerable<Field> GetSerializableFieldsIn(Type type);
        SerializedValue SerializedTypeFor(Type type);
        Type ResolveClassTypeFor(SerializedValue serializedValue);
        Type ResolveFieldTypeFor(Field field, SerializedValue serializedField);
    }

    internal class Field {
        private string serializedName;
        private FieldInfo fieldInfo;

        public Field(FieldInfo field, string serializeFieldName) {
            this.serializedName = serializeFieldName;
            this.fieldInfo = field;
        }

        public string SerializedName {
            get {
                return serializedName;
            }
        }

        public Type FieldType
        {
            get { return fieldInfo.FieldType; }
        }

        public void SetValue(object result, object value) {
            fieldInfo.SetValue(result, value);
            
        }

        public object GetObjectFrom(object containingObject) {
            return fieldInfo.GetValue(containingObject);
        }

        public void WriteOn(XStreamWriter writer, object value) {
            writer.StartNode(SerializedName);
            object fieldValue = GetObjectFrom(value);
            if (fieldValue == null) return;
            Type actualType = fieldValue.GetType();
            if (!FieldType.Equals(actualType))
                writer.WriteAttribute(XsAttribute.classType, actualType.AssemblyQualifiedName);
        }
    }

   
}