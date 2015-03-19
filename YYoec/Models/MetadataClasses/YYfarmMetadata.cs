using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YYClassLibrary;

namespace YYoec.Models
{
    [MetadataType(typeof(YYfarmMetadata))]
    public partial class farm : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //if neither the town nor the county is null or empty return a error message 
            if((town == null || town.Trim() == "") && (county == null || county.Trim() == ""))
            {
                yield return new ValidationResult("you need to provide at least one name between town and county!", new[] { "town", "county" });                    
            }

            
            provinceCode = provinceCode.ToUpper();

            if(postalCode != null)
            {
                //postalCode to Upper case and make sure there is a space in the middle
                int len = postalCode.Length;
                if (len == 6)
                    postalCode = postalCode.Insert(3, " ");
                postalCode = postalCode.ToUpper();
            }
           
            //validate the homephone number 
            string number = "";
            if(!validatePhoneNumber(homePhone, ref number))
            {
                yield return new ValidationResult("you need to input 10 digits of numbers for you homephone number", new[] { "Homephone" });        
            }
            homePhone = number.Length == 0 ? null : number;

            number = "";
            if(!validatePhoneNumber(cellPhone, ref number))
            {
                yield return new ValidationResult("you need to input 10 digits of numbers for you homephone number", new[] { "Cellphone" });        
            }
            cellPhone = number.Length == 0 ? null : number; 

            //validate the joined date and contact date
            yield return ValidateContactDateAndDateJoinedCorrect(lastContactDate, dateJoined);          


            yield return ValidationResult.Success;
        }

        //this function is for validating homephone number and cellphone number
        private bool validatePhoneNumber(string phoneNumber, ref string pureNumber)
        {
            int nNumberCount = 0;
            if(phoneNumber == null)
            {
                return true;
            }

            int len = phoneNumber.Length;
            if(len < 10)
            {
                return false;
            }

            //get all the number from user input, and ignore the characters which are not numbers
            for (int i = 0; i < len; i++)
            {
                char temp = phoneNumber.ElementAt(i);
                if(temp >= '0' && temp <= '9')
                {
                    nNumberCount++;
                    pureNumber += temp;
                }
            }

            //if the length of the whole number string doesn't equal 10, the validation failed. 
            if(nNumberCount != 10)
            {
                return false;
            }

            //if the length of the whole number string equals 10, and '-' to reformat the number string to required formation.
            else
            {
                pureNumber = pureNumber.Insert(3, "-");
                pureNumber = pureNumber.Insert(7, "-");
            }
            
            return true;
        }

        //this function is for validating the contact data and joined data
        private ValidationResult ValidateContactDateAndDateJoinedCorrect(DateTime? lastContactDate, DateTime? JoinedDate)
        {
            //if last contact date doesn't equal null, then check the whether the joined date equals null 
            if(lastContactDate != null)
            {
                //if the joined date equals null, which means the joined date field in error.
                if(JoinedDate == null)
                {
                    return new ValidationResult("you need to input the Joined Date", new[] { "dateJoined" });
                }
                //if the JoinedDate is greater than lastContactDate, that means last contact date is  earlier than joined date, which is also an error
                else if (JoinedDate > lastContactDate)
                {
                    return new ValidationResult("the Last Contact date cannont be earlier than Joined Date", new[] { "dateJoined" });
                }
            }

            return ValidationResult.Success; 
        }

    }

    public class YYfarmMetadata
    {
    
        public int farmId { get; set; }
        
        [Required]
        [Display(Name="Farm Name")]
        public string name { get; set; }

        [Display(Name="Address")]
        public string address { get; set; }


        [Display(Name = "Town")]
        public string town { get; set; }

        [Required]
        [Remote("ValidateProvinceCode", "Remote")]
        [Display(Name= "Province")]
        public string provinceCode { get; set; }

        [Display(Name = "County")]
        public string county { get; set; }

        [Required]
        [PostalCodeValidation]
        [Display(Name="Postal Code")]
        public string postalCode { get; set; }
        
        [Display(Name="Home Phone")]
        public string homePhone { get; set; }

        [Display(Name="Cell Phone")]
        public string cellPhone { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name="Directions")]
        public string directions { get; set; }

        [DateNotInFuture]
        [Display(Name = "Date Joined")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        public Nullable<System.DateTime> dateJoined { get; set; }

        [DateNotInFuture]
        [Display(Name = "Last Contact Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        public Nullable<System.DateTime> lastContactDate { get; set; }

    }
}