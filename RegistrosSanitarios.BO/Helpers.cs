using System;
using System.ComponentModel;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RegistrosSanitarios.BO
{
    public static class Helpers
    {
        public static Attribute GetMetadataAttribute(this IHtmlHelper html, Type baseClass, string property, Type attribute)
        {
            var metadataAttribute = baseClass.GetCustomAttribute(typeof(ModelMetadataTypeAttribute)) as ModelMetadataTypeAttribute;
            if (metadataAttribute == null)
                return null;

            var metadataType = metadataAttribute.MetadataType;
            var attrs = metadataType.GetProperty(property)?.GetCustomAttributes(attribute, false);
            if (attrs != null && attrs.Length > 0)
            {
                return (Attribute)attrs[0];
            }

            return null;
        }

        public static object GetModelProperty(this IHtmlHelper html, object o, string property)
        {
            var ret = o.GetType().GetProperty(property)?.GetValue(o);
            return ret;
        }
    }
}