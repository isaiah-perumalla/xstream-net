using System;
using xstream;
using Xstream.Core.Mappers;
using xstream.Utilities;

namespace Xstream.Core
{
    internal class Unmarshaller
    {
        private readonly XStreamReader reader;
        private readonly UnmarshallingContext context;
        private readonly ConverterLookup converterLookup;
        private readonly IMapper mapper;


        public Unmarshaller(XStreamReader reader, UnmarshallingContext context, ConverterLookup converterLookup, IMapper mapper)
        {
            this.reader = reader;
            this.mapper = mapper;
            this.context = context;
            this.converterLookup = converterLookup;
        }

        internal object Unmarshal(Type type)
        {
            var result = context.FindReferenceFromCurrentNode();
            if (result != null) return result;
            if (reader.GetAttribute(XsAttribute.Null) == true.ToString())
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
            //ToDo: use mapper to resolve type names
            //var type = mapper.RealTypeFor(serializeValue)
            var classAttribute = reader.GetAttribute(XsAttribute.classType);
            if (!string.IsNullOrEmpty(classAttribute)) fieldType = Type.GetType(Xmlifier.UnXmlify(classAttribute));
            var converter = converterLookup.GetConverter(fieldType);
            return converter != null ? converter.UnMarshall(reader, context) : Unmarshal(fieldType);
        }

    }
}
