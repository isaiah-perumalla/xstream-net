﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;
using xstream;
using xstream.Utilities;

namespace Xstream.Core.Mappers {
    internal class DefaultMapper : IMapper
    {
        private const string plusSymbol = "-plus";
        private const string serializedArraySymbol = "-array";
        private const BindingFlags DefaultBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy;

        public IEnumerable<Field> GetSerializableFieldsIn(Type type)
        {
            var fields = type.GetFields(DefaultBindingFlags);
            foreach (var field in fields)
            {
                var serializeFieldName = field.Name;
                if (field.GetCustomAttributes(typeof(DontSerialiseAttribute), true).Length != 0) continue;
                if (field.GetCustomAttributes(typeof(XmlIgnoreAttribute), true).Length != 0) continue;
                if (typeof(MulticastDelegate).IsAssignableFrom(field.FieldType)) continue;
                var match = Constants.AutoPropertyNamePattern.Match(field.Name);
                if (match.Success)
                    serializeFieldName = match.Result("$1");
                yield return new Field(field, serializeFieldName);

            }

        }

        public  SerializedValue SerializedTypeFor(Type getType) {
            
            var serializedClassName = S.RemoveFrom(getType.Name.Replace("[]", serializedArraySymbol), "`");
            return new SerializedValue(serializedClassName, new XsAttribute(XsAttribute.classType, getType.AssemblyQualifiedName));
        }

        public Type ResolveClassTypeFor(SerializedValue serializedValue) {
             if("true".Equals(serializedValue.ValueOfAtrributeNamed(XsAttribute.Null), StringComparison.InvariantCultureIgnoreCase))
                    return typeof (XNull);
            var classType = serializedValue.ValueOfAtrributeNamed(XsAttribute.classType);
            return Type.GetType(classType);
        }

        public Type ResolveFieldTypeFor(Field field, SerializedValue serializedField) {
            
            var classAttribute = serializedField.ValueOfAtrributeNamed(XsAttribute.classType);
            Type fieldType = field.FieldType;
            if (!string.IsNullOrEmpty(classAttribute)) fieldType = Type.GetType(resolveTypeFrom(classAttribute));
            return fieldType;
        }

        private static string resolveTypeFrom(string classAttribute) {
            return classAttribute.Replace("-array", "[]").Replace("-plus", "+");
        }
    }
}