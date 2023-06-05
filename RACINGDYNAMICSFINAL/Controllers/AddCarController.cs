using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using RACINGDYNAMICSFINAL.Models;

namespace RACINGDYNAMICSFINAL.Controllers
{
    public class AddCarController : Controller
    {

        public ActionResult AddCar()
        {
            
            if (Session["Username"] != null)
            {
                if (Session["Username"].ToString() == "eduard")
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("MyAccount", "Account");
                }       
            }
            else
            {
                return RedirectToAction("Signin", "Account");
            }
                
            
        }

        ProiectRacingDynamicsCars db = new ProiectRacingDynamicsCars(); 
        ProiectRacingDynamicsRequests requests = new ProiectRacingDynamicsRequests();
        // GET: AddCar
        [HttpPost]
        public ActionResult AddCar(CarsTable cars, HttpPostedFileBase car_image)
         {
            if (db.CarsTable.Any(x => x.car_name == cars.car_name))
            {
                ModelState.AddModelError("car_name", "This car already exists!");
            }

            if (!ModelState.IsValid)
            {
                return View(cars);
            }

            int newId;
            lock (db)
            {
                newId = db.CarsTable.Max(x => (int?)x.car_id) ?? 0;
                newId++;
            }

            cars.car_id = newId;

            if (car_image != null && car_image.ContentLength > 0)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(car_image.FileName);
                string imagePath = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                car_image.SaveAs(imagePath);
                cars.car_image = "/Uploads/" + fileName;
            }
            try
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    db.CarsTable.Add(cars);
                    db.SaveChanges();

                    transaction.Commit();
                }
                if (Session["Username"].ToString() == "eduard")
                {
                    return RedirectToAction("DashBoard", "Account");
                }
                else
                {
                    return RedirectToAction("MyAccount", "Account");
                }
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

                var fullErrorMessage = string.Join("; ", errorMessages);
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                throw new Exception(exceptionMessage, ex);
            }
            catch (DbUpdateException ex)
            {
                var innerExceptionMessage = (ex.InnerException != null) ? ex.InnerException.Message : string.Empty;
                var exceptionMessage = string.Concat(ex.Message, " Inner Exception: ", innerExceptionMessage);

                throw new Exception(exceptionMessage, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while adding the car.", ex);
            }
        }

        public ActionResult AddRequest()
        {
            if (Session["Username"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Signin", "Account");
            }
        }

        [HttpPost]
        public ActionResult AddRequest(UserRequestCars requestCar)
        {
            if (requests.UserRequestCars.Any(x => x.CarName == requestCar.CarName))
            {
                ModelState.AddModelError("CarName", "This car already exists!");
            }

            if (!ModelState.IsValid)
            {
                return View(requestCar);
            }

            // Generate a unique ID in a thread-safe manner
            int newId;
            lock (requests)
            {
                newId = requests.UserRequestCars.Max(x => (int?)x.RequestId) ?? 0;
                newId++;
            }

            // Set the new ID for the car
            requestCar.RequestId = newId;

            // Commented out image handling code for brevity

            try
            {
                using (var transaction = requests.Database.BeginTransaction())
                {
                    requestCar.RequestUsername = Session["Username"].ToString();
                    requestCar.RequestStatus = "Pending";
                    requests.UserRequestCars.Add(requestCar);
                    requests.SaveChanges();
                    transaction.Commit();
                }
                    return RedirectToAction("MyAccount", "Account");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "There was a problem submitting the request!");
                return View(requestCar);
            }
        }

        public ActionResult MyRequests()
        {
            if (Session["Username"] != null)
            {
                string username = Session["Username"].ToString();

                using (var dbContext = new ProiectRacingDynamicsRequests())
                {
 
                    var userRequests = dbContext.UserRequestCars.Where(r => r.RequestUsername == username).ToList();

                    var requestViewModels = userRequests.Select(r => new UserRequestCars
                    {
                        RequestId = r.RequestId,
                        CarName = r.CarName,
                        CarImage = r.CarImage,
                        CarType = r.CarType,
                        CarEngine = r.CarEngine,
                        CarDescription = r.CarDescription,
                        CarYear = r.CarYear,
                        RequestStatus = r.RequestStatus
                    }).ToList();

                    return View(requestViewModels);
                }
            }
            else
            {
                return RedirectToAction("Signin", "Account");
            }
        }

    }
}