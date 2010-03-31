using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Xstream.Core.Mappers
{
    internal interface IMapper
    {
        
        IEnumerable<Field> GetSerializableFieldsIn(Type type);
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
    }

   
}