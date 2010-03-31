using System.Text;
using xstream;
using Xstream.Core.Converters;
using Xstream.Core.Mappers;

namespace Xstream.Core {
    public class XStream {
        private readonly ConverterLookup converterLookup = new ConverterLookup();

        private readonly IMapper mapper = new DefaultMapper();

        public string ToXml(object value) {
            var stringBuilder = new StringBuilder();
            var writer = new XWriter(stringBuilder);
            var context = new MarshallingContext(writer, converterLookup, mapper);
            context.ConvertOriginal(value);
            return stringBuilder.ToString();
        }

        public void AddConverter(Converter converter) {
            converterLookup.AddConverter(converter);
        }

        public object FromXml(string s) {
            var reader = new XReader(s);
            var context = new UnmarshallingContext(reader, converterLookup, mapper);
            return context.ConvertOriginal();
        }


        public void Alias<T>(string typeAlias) {
            
        }
    }
}