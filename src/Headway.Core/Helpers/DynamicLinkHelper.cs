using Headway.Core.Args;
using Headway.Core.Dynamic;
using Headway.Core.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Headway.Core.Helpers
{
    public static class DynamicLinkHelper
    {
        public static void LinkSearchItems(DynamicSearchItem dynamicSearchItem, List<DynamicSearchItem> dynamicSearchItems, string parameterName)
        {
            if (dynamicSearchItems == null
                || string.IsNullOrWhiteSpace(parameterName))
            {
                return;
            }

            var sourceSearchItem = dynamicSearchItems.FirstOrDefault(f => f.ParameterName.Equals(parameterName));

            dynamicSearchItem.LinkSource = sourceSearchItem;
            sourceSearchItem.HasLinkDependents = true;
        }

        public static void LinkFields(DynamicField dynamicField, List<DynamicField> dynamicFields, string propertyName)
        {
            if (dynamicFields == null
                || string.IsNullOrWhiteSpace(propertyName))
            {
                return;
            }

            var sourceField = dynamicFields.FirstOrDefault(f => f.PropertyName.Equals(propertyName));
            LinkFields(dynamicField, sourceField);
        }

        public static void LinkFields(DynamicField dynamicField, List<DynamicArg> sourceArgs, string propertyName)
        {
            if(sourceArgs == null
               || string.IsNullOrWhiteSpace(propertyName))
            {
                return;
            }

            var sourceFieldArg = sourceArgs.FirstDynamicArgOrDefault(propertyName);
            if (sourceFieldArg == null
                || sourceFieldArg.Value == null)
            {
                return;
            }

            if (sourceFieldArg.Value is DynamicField sourceField)
            {
                LinkFields(dynamicField, sourceField);
            }
        }

        public static void LinkFields(DynamicField dynamicField, DynamicField sourceField)
        {
            if (sourceField == null
                || dynamicField == null)
            {
                return;
            }

            dynamicField.LinkSource = sourceField;
            sourceField.HasLinkDependents = true;
        }
    }
}