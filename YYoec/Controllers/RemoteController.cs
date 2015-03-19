using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YYoec.Models;


namespace YYoec.Controllers
{
    //this controller is for validatting the province code  
    public class RemoteController : Controller
    {

       private OECContext db = new OECContext();

       public JsonResult ValidateProvinceCode(string provinceCode)
       {
           string pCode;
           
           try 
           {
               //if passed province code is null or empty, return json true
               if (provinceCode == null || provinceCode.ToString() == "")
               {
                   return Json(true, JsonRequestBehavior.AllowGet);
               }

               pCode = provinceCode.ToString().ToUpper();
               pCode = pCode.Trim();

               //this step only check the string length
               if (pCode.Length != 2)
               {
                   return Json("the province code can only be 2 letters",
                   JsonRequestBehavior.AllowGet);

               }
               // if the length equals 2, then check the chars in this string, if the char is not "Letter", then return the validation message
               else if (pCode.ElementAt(0) < 'A' || pCode.ElementAt(0) > 'Z' || pCode.ElementAt(1) < 'A' || pCode.ElementAt(1) > 'Z')
               {
                   return Json("the province code can only be 2 letters",
                   JsonRequestBehavior.AllowGet);
               }
               // if the passed province code can't be found in the database, then return "this province code is not on file"
               else if (null == db.provinces.Find(pCode))
               {
                   return Json("this province code is not on file",
                   JsonRequestBehavior.AllowGet);
               }

               return Json(true, JsonRequestBehavior.AllowGet);
               
           }

            
           catch(Exception ex)
           {
               string sEx = "error validating province code " + ex.GetBaseException().Message; 
               return Json( sEx, JsonRequestBehavior.AllowGet);
           }
           
         

       }
	}
}