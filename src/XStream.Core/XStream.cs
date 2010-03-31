using System.Text;
using xstream;
using Xstream.Core.Converters;
using Xstream.Core.Mappers;

namespace Xstream.Core {
    public class XStream {
        private readonly ConverterLookup converterLookup = new ConverterLookup();

        private IMapper mapper = new DefaultMapper();

        public string ToXml(object value) {
            StringBuilder stringBuilder = new StringBuilder();
            XWriter writer = new XWriter(stringBuilder);
            MarshallingContext context = new MarshallingContext(writer, converterLookup, mapper);
            context.ConvertOriginal(value);
            return stringBuilder.ToString();
        }

        public void AddConverter(Converter converter) {
            converterLookup.AddConverter(converter);
        }

        public object FromXml(string s) {
            XReader reader = new XReader(s);
            UnmarshallingContext context = new UnmarshallingContext(reader, converterLookup, mapper);
            return context.ConvertOriginal();
        }


        public void Alias<T>(string typeAlias) {
            
        }
    }
}