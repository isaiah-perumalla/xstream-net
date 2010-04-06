using System.Collections;
using xstream;
using Xstream.Core.Mappers;

namespace Xstream.Core.Converters.Collections {
    internal class HashtableConverter : BaseDictionaryConverter<Hashtable> {
        public HashtableConverter(IMapper mapper) : base(mapper) {
        }

        protected override IDictionary EmptyDictionary(XStreamReader reader) {
            return new Hashtable();
        }
    }
}