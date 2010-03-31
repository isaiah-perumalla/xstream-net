using System;
using System.Reflection;

namespace Xstream.Core.Mappers
{
    internal interface IMapper
    {
        void ProcessFieldsIn(Type type, IFieldProcessor fieldProcessor);
    }

    internal interface IFieldProcessor {
        void ProcessField(FieldInfo field, string serializeFieldName);
    }
}