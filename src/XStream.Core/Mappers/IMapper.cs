using System;

namespace Xstream.Core.Mappers
{
    internal interface IMapper
    {
        void ProcessFieldsIn(Type type, IFieldProcessor fieldProcessor);
    }

    internal interface IFieldProcessor {
    }
}