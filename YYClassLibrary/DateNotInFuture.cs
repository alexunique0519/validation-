using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YYClassLibrary
{
    //this class is for validating the date not being in future
    public class DateNotInFuture : ValidationAttribute
    {
        protected override System.ComponentModel.DataAnnotations.ValidationResult IsValid(object value, System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            //the passed value can be null
            if(value == null)
            {
                return ValidationResult.Success;
            }

            
            DateTime valueDT = (DateTime)value;
            //if the valueDT is greater then current datetime value, that means the passed value is in future.
            if(valueDT > DateTime.Now)
            {
                return new ValidationResult(string.Format("{0} cannot be in the future", validationContext.DisplayName));
            }
            else
            {
                return ValidationResult.Success;
            }
        } 
    }
}
