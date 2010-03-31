using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using xstream;

namespace Xstream.Core.Mappers {
    internal class DefaultMapper : IMapper
    {
        public IEnumerable<Field> GetSerializableFieldsIn(Type type)
        {
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            foreach (var field in fields)
            {
                var serializeFieldName = field.Name;
                if (field.GetCustomAttributes(typeof(DontSerialiseAttribute), true).Length != 0) continue;
                if (field.GetCustomAttributes(typeof(XmlIgnoreAttribute), true).Length != 0) continue;
                if (typeof(MulticastDelegate).IsAssignableFrom(field.FieldType)) continue;
                Match match = Constants.AutoPropertyNamePattern.Match(field.Name);
                if (match.Success)
                    serializeFieldName = match.Result("$1");
                yield return new Field(field, serializeFieldName);

            }

        }
    }
}