using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using xstream;
using Xstream.Core.Converters;
using Xstream.Core.Mappers;
using xstream.Utilities;

namespace Xstream.Core
{
    internal class Unmarshaller
    {
        private readonly XStreamReader reader;
        private readonly UnmarshallingContext context;
        private readonly ConverterLookup converterLookup;
        private readonly IMapper mapper = new DefaultMapper();


        public Unmarshaller(XStreamReader reader, UnmarshallingContext context, ConverterLookup converterLookup)
        {
            this.reader = reader;
            this.context = context;
            this.converterLookup = converterLookup;
        }

        internal object Unmarshal(Type type)
        {
            var result = context.Find();
            if (result != null) return result;
            if (reader.GetAttribute(Attributes.Null) == true.ToString())
                return null;
            result = DynamicInstanceBuilder.CreateInstance(type);
            context.StackObject(result);
            UnmarshalAs(result, type);
            return result;
        }

        private void UnmarshalAs(object result, Type type)
        {

            if (type.Equals(typeof(object))) return;


            foreach (var field in mapper.GetSerializableFieldsIn(type))
            {
                reader.MoveDown(field.SerializedName);
                field.SetValue(result, ConvertField(field.FieldType));
                reader.MoveUp();
            }

            UnmarshalAs(result, type.BaseType);
        }



        private object ConvertField(Type fieldType)
        {
            string classAttribute = reader.GetAttribute(Attributes.classType);
            if (!string.IsNullOrEmpty(classAttribute)) fieldType = Type.GetType(Xmlifier.UnXmlify(classAttribute));
            Converter converter = converterLookup.GetConverter(fieldType);
            if (converter != null)
                return converter.FromXml(reader, context);
            else
                return Unmarshal(fieldType);
        }

        public void ProcessField(FieldInfo field, string serializeFieldName)
        {
            reader.MoveDown(serializeFieldName);
            //            field.SetValue(currentObject, ConvertField(field.FieldType));
            reader.MoveUp();

        }
    }


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
