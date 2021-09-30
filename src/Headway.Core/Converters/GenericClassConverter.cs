using Headway.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.Json;

namespace Headway.Core.Converters
{
    public class GenericClassConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                var args = JsonSerializer.Deserialize(value.ToString(), typeof(List<Arg>)) as List<Arg>;
                if(args != null)
                {
                    var typeName = args.First(a => a.Name.Equals("Type")).Value;
                    var json = args.First(a => a.Name.Equals("Value")).Value;
                    var type = Type.GetType(typeName);
                    return JsonSerializer.Deserialize(json, type);
                }
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return destinationType; 
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
