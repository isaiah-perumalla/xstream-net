using System.Collections;
using xstream;

namespace Xstream.Core.Converters.Collections {
    internal class HashtableConverter : BaseDictionaryConverter<Hashtable> {
        protected override IDictionary EmptyDictionary(XStreamReader reader) {
            return new Hashtable();
        }
    }
}