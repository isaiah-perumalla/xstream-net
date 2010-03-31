using System.Collections.Generic;
using System.Reflection;
using System.Text;
using xstream;
using Xstream.Core;
using Xstream.Core.Converters;
using Xstream.Core.Mappers;

namespace Xstream.Core {
    public class XStream {
        private readonly ConverterLookup converterLookup = new ConverterLookup();
        
        private readonly List<Assembly> assemblies = new List<Assembly>();
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
            UnmarshallingContext context = new UnmarshallingContext(reader, converterLookup, mapper, assemblies);
            return context.ConvertOriginal();
        }

      

        public void Load(params Assembly[] externalAssemblies) {
            if (externalAssemblies == null) return;
            assemblies.AddRange(externalAssemblies);
        }

        public void Unload(params Assembly[] externalAssemblies) {
            if (externalAssemblies == null) return;
            foreach (Assembly externalAssembly in externalAssemblies) assemblies.Remove(externalAssembly);
        }

        public void Alias<T>(string typeAlias) {
            
        }
    }
}