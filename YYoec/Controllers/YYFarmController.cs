using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YYoec.Models;

namespace YYoec.Controllers
{

    //this controller is used to mainipulate the CRUD functions for farm and render particular views depending on the  action.
    public class YYFarmController : Controller
    {
        private OECContext db = new OECContext();

        //before showing the records of farms, using a inner join to retrieve province infos from province table
        //then order the data by province name, farm name respectively. at last, send the data to index view.
        public ActionResult Index()
        {
            var farms = from f in db.farms
                        join p in db.provinces
                        on  f.provinceCode equals p.provinceCode 
                        orderby p.name, f.name
                        select f;
            return View(farms.ToList());
        }

        //if the passed id is null, then display a message to user, and redirect to index view, otherwise, pass the object to detail view.
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["messageType"] = "danger";
                TempData["message"] = "Please select a valid farm record first";
                return RedirectToAction("index");
            }
            farm farm = db.farms.Find(id);
            if (farm == null)
            {
                return HttpNotFound();
            }
            return View(farm);
        }

        //to render the create view to create a farm record.
        public ActionResult Create()
        {
            return View(); 
        }

        // get the passed farm object, if the modelstate is valid, then persist the farm into database, otherwise go back the create view. if there is any exception thrown,  
        // add the error message into modelstate.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="farmId,name,address,town,county,provinceCode,postalCode,homePhone,cellPhone,directions,dateJoined,lastContactDate")] farm farm)
        {
            try 
            {
                if (ModelState.IsValid)
                {
                    db.farms.Add(farm);
                    db.SaveChanges();
                    TempData["messageType"] = "success";
                    TempData["message"] = "One farm record has been added successfully!";
                    return RedirectToAction("Index");
                }
                
            }
            catch (Exception ex) 
            {
                TempData["messageType"] = "danger";
                TempData["message"] = ex.GetBaseException().Message;
                ModelState.AddModelError("", ex.GetBaseException().Message);
            }
            
            return View(farm);
        }

        //if the passed id is null, then navigate to the index view and display a message, if the id is valid and the farm exists in the database, 
        //then retrieve the object and pass the it to the edit view, meanwhile get the province info collection and pass it into the viewbag.
        public ActionResult Edit(int? id)
        {        
            if (id == null)
            {
                TempData["messageType"] = "danger";
                TempData["message"] = "Please select a valid farm record first";
                return RedirectToAction("index");
            }
            farm farm = db.farms.Find(id);
            if (farm == null)
            {
                return HttpNotFound();
            }

            //order the province collection by province name
            var provinceCodes = db.provinces.OrderBy(p => p.name);

            ViewBag.provinceCode = new SelectList(provinceCodes, "provinceCode", "name", farm.provinceCode);
            return View(farm);
        }

        // POST: /YYFarm/Edit/5
        // get the data from the edit submission, if the modelstate is valid, then persist the change into the database, otherwise stay at the edit view and display the error message.
        // if there is any exception has been trown, catch the exception and add the info of the exception into the modelstate.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="farmId,name,address,town,county,provinceCode,postalCode,homePhone,cellPhone,directions,dateJoined,lastContactDate")] farm farm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(farm).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["messageType"] = "success";
                    TempData["message"] = "One farm record has been updated successfully!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.GetBaseException().Message);
            }
                     
            return Edit(farm.farmId);
        }

        //if the passed id is null, then navigate to the index view and display a message, if the id is valid and the farm exists in the database, 
        //then retrieve the farm object and pass it into the delete view
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["messageType"] = "danger";
                TempData["message"] = "Please select a valid farm record first";
                return RedirectToAction("index");
            }
            farm farm = db.farms.Find(id);
            if (farm == null)
            {
                return HttpNotFound();
            }
            return View(farm);
        }

        //get the data submitted from the delete view, if the farm object can be found in the database by the passed id, delete it and navigate to the index view.
        // if there is any exception has been trown, catch the exception and add the info of the exception into the modelstate.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try 
            {
                farm farm = db.farms.Find(id);
                db.farms.Remove(farm);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.GetBaseException().Message);
            }

            return Delete(id);
        }

        //release all the data in this controller 
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
