﻿using System;
using System.Reflection;

namespace Xstream.Core.Mappers {
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
    }
}