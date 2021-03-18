using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using System.Globalization;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RegistrosSanitarios.EntityModel.Models;

namespace RegistrosSanitarios.EntityModel.Models
{
    public class ValidateDateRange : ValidationAttribute
    {

        private string From { get; set; }
        private string To { get; set; }

        public ValidateDateRange(string from, string to)
        {
            From = from;
            To = to;
        }

        public ValidateDateRange()
        {
            From = "01/01/2000";
            To = "01/01/2100";
        }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime FirstDate = Convert.ToDateTime(From);
            DateTime SecondDate = Convert.ToDateTime(To);
            var nombreCampo = validationContext.DisplayName;

            // Se permite que un null pertenezca al rango, si no es nulo tiene que 
            // agarrarlo el validador de requerido
            if (value == null || (Convert.ToDateTime(value) >= FirstDate && Convert.ToDateTime(value) <= SecondDate))
            {
                return ValidationResult.Success;
            }
            else
            {


                return new ValidationResult("Ingrese una fecha válida en " + nombreCampo);
            }
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "daterange",
            };

            yield return rule;
        }
    }
    
    public class DateGreaterThanAttribute : ValidationAttribute, IClientValidatable
    {
        public string otherPropertyName;
        public DateGreaterThanAttribute() { }
        public DateGreaterThanAttribute(string otherPropertyName, string errorMessage)
            : base(errorMessage)
        {
            this.otherPropertyName = otherPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult validationResult = ValidationResult.Success;
            try
            {
                // Using reflection we can get a reference to the other date property, in this example the project start date
                var containerType = validationContext.ObjectInstance.GetType();
                var field = containerType.GetProperty(this.otherPropertyName);
                var extensionValue = field.GetValue(validationContext.ObjectInstance, null);
                if (extensionValue == null)
                {
                    //validationResult = new ValidationResult("Start Date is empty");
                    return validationResult;
                }
                var datatype = extensionValue.GetType();

                //var otherPropertyInfo = validationContext.ObjectInstance.GetType().GetProperty(this.otherPropertyName);
                if (field == null)
                    return new ValidationResult(String.Format("Unknown property: {0}.", otherPropertyName));
                // Let's check that otherProperty is of type DateTime as we expect it to be
                if ((field.PropertyType == typeof(DateTime) || (field.PropertyType.IsGenericType && field.PropertyType == typeof(Nullable<DateTime>))))
                {
                    if (value == null)
                        validationResult = ValidationResult.Success;
                    else
                    {
                        DateTime toValidate = (DateTime)value;
                        DateTime referenceProperty = (DateTime)field.GetValue(validationContext.ObjectInstance, null);
                        // if the end date is lower than the start date, than the validationResult will be set to false and return
                        // a properly formatted error message
                        if (toValidate.CompareTo(referenceProperty) < 1)
                        {
                            validationResult = new ValidationResult(ErrorMessageString);
                        }
                    }
                }
                else
                {
                    validationResult = new ValidationResult("Ocurrió un error validando la propiedad. La propiedad OtherProperty no es una fecha");
                }
            }
            catch (Exception ex)
            {
                // Do stuff, i.e. log the exception
                // Let it go through the upper levels, something bad happened
                throw ex;
            }

            return validationResult;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "isgreater",
            };
            rule.ValidationParameters.Add("otherproperty", otherPropertyName);
            yield return rule;
        }
    }

    public class DateGreaterThanOrEqualAttribute : ValidationAttribute, IClientValidatable
    {
        public string otherPropertyName;
        public DateGreaterThanOrEqualAttribute() { }
        public DateGreaterThanOrEqualAttribute(string otherPropertyName, string errorMessage)
            : base(errorMessage)
        {
            this.otherPropertyName = otherPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult validationResult = ValidationResult.Success;
            try
            {
                // Using reflection we can get a reference to the other date property, in this example the project start date
                var containerType = validationContext.ObjectInstance.GetType();
                var field = containerType.GetProperty(this.otherPropertyName);
                var extensionValue = field.GetValue(validationContext.ObjectInstance, null);
                if (extensionValue == null)
                {
                    //validationResult = new ValidationResult("Start Date is empty");
                    return validationResult;
                }
                var datatype = extensionValue.GetType();

                //var otherPropertyInfo = validationContext.ObjectInstance.GetType().GetProperty(this.otherPropertyName);
                if (field == null)
                    return new ValidationResult(String.Format("Unknown property: {0}.", otherPropertyName));
                // Let's check that otherProperty is of type DateTime as we expect it to be
                if ((field.PropertyType == typeof(DateTime) || (field.PropertyType.IsGenericType && field.PropertyType == typeof(Nullable<DateTime>))))
                {
                    if (value == null)
                        validationResult = ValidationResult.Success;
                    else
                    {
                        DateTime toValidate = (DateTime)value;
                        DateTime referenceProperty = (DateTime)field.GetValue(validationContext.ObjectInstance, null);
                        // if the end date is lower than the start date, than the validationResult will be set to false and return
                        // a properly formatted error message
                        if (toValidate.CompareTo(referenceProperty) < 0)
                        {
                            validationResult = new ValidationResult(ErrorMessageString);
                        }
                    }

                }
                else
                {
                    validationResult = new ValidationResult("Ocurrió un error validando la propiedad. La propiedad OtherProperty no es una fecha");
                }
            }
            catch (Exception ex)
            {
                // Do stuff, i.e. log the exception
                // Let it go through the upper levels, something bad happened
                throw ex;
            }

            return validationResult;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "isgreaterthanorequal",
            };
            rule.ValidationParameters.Add("otherproperty", otherPropertyName);
            yield return rule;
        }
    }

    public class DynamicDropDownAttribute : Attribute
    {
        public bool IsMultiple { get; }

        public DynamicDropDownAttribute(bool isMultiple = false)
        {
            IsMultiple = isMultiple;
        }
    }

    public class ValidCUIT : ValidationAttribute, IClientModelValidator

    {
        public override bool IsValid(object value)
        {
            return true;
        }
        
        public void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            var errorMessage = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
            MergeAttribute(context.Attributes, "data-val-cuitvalido", errorMessage);
        }
        
        private bool MergeAttribute(
            IDictionary<string, string> attributes,
            string key,
            string value)
        {
            if (attributes.ContainsKey(key))
            {
                return false;
            }
            attributes.Add(key, value);
            return true;
        }
    }
   

    public class ADGroups
    {
        public string Name { get; set; }
    }

    public class Funciones
    {
        public int IdPermiso { get; set; }
        public string Seccion { get; set; }
        public string GrupoAD { get; set; }
        public string Tipo { get; set; }

    }

    public class RelacionHijoPadre
    {
        public string IdPosicionHijo { get; set; }
        public string NombreCompletoHijo { get; set; }

        public string UsuarioADHijo { get; set; }
        public string CategoriaHijo { get; set; }
        public string EmailHijo { get; set; }
        public string SectorHijo { get; set; }
        public string LegajoHijo { get; set; }
        public string TelefonoHijo { get; set; }
        public string IdPosicionPadre { get; set; }
        public string NombreCompletoPadre { get; set; }

        public string UsuarioADPadre { get; set; }
        public string CategoriaPadre { get; set; }
        public string EmailPadre { get; set; }
        public string SectorPadre { get; set; }
    }


         

}
