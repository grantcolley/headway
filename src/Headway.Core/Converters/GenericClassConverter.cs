using Headway.Core.Args;
using Headway.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
                if (JsonSerializer.Deserialize(value.ToString(), typeof(List<Arg>)) is List<Arg> args)
                {
                    var typeName = args.FirstArgValue(Constants.Args.TYPE);
                    var json = args.FirstArgValue(Constants.Args.VALUE);
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
