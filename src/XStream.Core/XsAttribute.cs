using System;
using System.Text;
using System.Linq;

namespace Xstream.Core {
    internal class XsAttribute {
        private readonly string name;
        private readonly string value;

        public XsAttribute(string name, string value) {
            this.name = name;
            this.value = value;
        }

        public bool Equals(XsAttribute other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.name, name) && Equals(other.value, value);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (XsAttribute)) return false;
            return Equals((XsAttribute) obj);
        }

        public override int GetHashCode() {
            unchecked
            {
                return ((name != null ? name.GetHashCode() : 0)*397) ^ (value != null ? value.GetHashCode() : 0);
            }
        }

        public override string ToString() {
            return string.Format("Attribute Name: {0}, Value: {1}", name, value);
        }

        internal const string AttributeType = "attributeType";
        internal const string references = "references";
        internal const string Null = "null";
        internal const string classType = "class";

    }


    internal class SerializedValue {
        private readonly string serializedClassName;
        private readonly XsAttribute[] attributes;

        internal SerializedValue(string serializedClassName, params XsAttribute[] attributes) {
            this.serializedClassName = serializedClassName;
            this.attributes = attributes;
        }

        public bool Equals(SerializedValue other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.serializedClassName, serializedClassName) && other.attributes.SequenceEqual( attributes);
        }

        public override string ToString() {
            var builder = new StringBuilder( string.Format("SerializedClassName: {0} ", serializedClassName));
            
            foreach (var attr in attributes)
            {
                builder.Append(attr);
            }
            return builder.ToString();
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (SerializedValue)) return false;
            return Equals((SerializedValue) obj);
        }

        public override int GetHashCode() {
            unchecked
            {
                return ((serializedClassName != null ? serializedClassName.GetHashCode() : 0)*397) ^ (attributes != null ? attributes.GetHashCode() : 0);
            }
        }

        public void WriteOn(XStreamWriter writer) {
            
        }
    }
}
