using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;
using xstream;

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

        public  SerializedValue GetSerializedClassFor(Type getType) {
            
            var serializedClassName = S.RemoveFrom(getType.Name.Replace("[]", serializedArraySymbol), "`");
            return new SerializedValue(serializedClassName, new XsAttribute(XsAttribute.classType, getType.AssemblyQualifiedName));
        }
    }
}