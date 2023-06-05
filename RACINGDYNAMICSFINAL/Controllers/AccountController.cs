using RACINGDYNAMICSFINAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;


namespace RACINGDYNAMICSFINAL.Controllers
{
    public class AccountController : Controller
    {

        ProiectRacingDynamicsDataBase db = new ProiectRacingDynamicsDataBase();
        ProiectRacingDynamicsCars cars1 = new ProiectRacingDynamicsCars();
        // GET: Account
        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(CreateAccount account)
        {
            if (db.CreateAccounts.Any(x => x.UserName == account.UserName))
            {
                ModelState.AddModelError("UserName", "This username already exists!");
            }
            if (db.CreateAccounts.Any(x => x.Email == account.Email))
            {
                ModelState.AddModelError("Email", "This email already exists!");
            }

            if (!ModelState.IsValid)
            {
                return View(account);
            }

            // Generate a unique ID in a thread-safe manner
            int newId;
            lock (db)
            {
                newId = db.CreateAccounts.Max(x => (int?)x.Id) ?? 0;
                newId++;
            }

            // Set the new ID for the account
            account.Id = newId;

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.CreateAccounts.Add(account);
                    db.SaveChanges();

                    transaction.Commit();

                    Session["Id"] = account.Id.ToString();
                    Session["Username"] = account.UserName.ToString();
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    ModelState.AddModelError("", "An error occurred while creating the account.");
                    // Log the exception or handle it as needed

                    return View(account);
                }
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Signin()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signin(CreateAccount account)
        {
            var checkLogin = db.CreateAccounts.Where(x => x.UserName.Equals(account.UserName) && x.Password.Equals(account.Password)).FirstOrDefault();
            
            if(account.UserName == "eduard" && account.Password == "admin")
            {
                Session["Id"] = account.Id.ToString();
                Session["Username"] = account.UserName.ToString();
                return RedirectToAction("Dashboard", "Account");
            }

            if(checkLogin != null)
            {
                Session["Id"] = account.Id.ToString();
                Session["Username"] = account.UserName.ToString();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Wrong Username or Password!");
            }
            return View();
        }


        public ActionResult DashBoard()
        {
            List<CarsTable> carModels = new List<CarsTable>();
            

            if (Session["Username"] != null)
            {
                using (var dbContext = new ProiectRacingDynamicsCars())
                {
                    var cars = dbContext.CarsTable.ToList();

                    foreach (var car in cars)
                    {
                        CarsTable carModel = new CarsTable
                        {
                            car_id = car.car_id,
                            car_name = car.car_name,
                            car_image = car.car_image,
                            car_type = car.car_type,
                            car_engine = car.car_engine,
                            car_description = car.car_description,
                            car_year = car.car_year
                        };

                        carModels.Add(carModel);
                    }
                }
                return View(carModels);
            }
            else
            {
                return RedirectToAction("Signin", "Account");
            }
        }

        public ActionResult Delete(int id)
        {
            if (Session["Username"] != null)
            {
                var cartoDelete = cars1.CarsTable.Find(id);

                if (cartoDelete != null)
                {
                    cars1.CarsTable.Remove(cartoDelete);
                    cars1.SaveChanges();
                    return RedirectToAction("DashBoard", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "An error occured while deleting the car");
                    return RedirectToAction("DashBoard", "Account");
                }
            }
            else
            {
                return RedirectToAction("Signin", "Account");
            }
        }

        public ActionResult Edit(int id)
        {
            if (Session["Username"] != null)
            {
                var car = cars1.CarsTable.Find(id);

                if (car != null)
                {
                    var carViewModel = new CarsTable
                    {
                        car_id = car.car_id,
                        car_name = car.car_name,
                        car_type = car.car_type,
                        car_description = car.car_description,
                        car_year = car.car_year,
                        car_engine = car.car_engine,
                        car_image = car.car_image
                    };
                    return View(carViewModel); 
                }
                else
                {
                    
                    return RedirectToAction("DashBoard", "Account"); 
                }
            }
            else
            {
                return RedirectToAction("Signin", "Account"); 
            }
        }

        [HttpPost]
        public ActionResult Edit(CarsTable car) 
        {
            var existingCar = cars1.CarsTable.Find(car.car_id);
            if (cars1.CarsTable.Any(x => x.car_name == car.car_name))
            {
                ModelState.AddModelError("car_name", "This car already exists!");

            }
            if (ModelState.IsValid)
            {     
                if (existingCar != null)
                {   
                    existingCar.car_id = car.car_id;
                    existingCar.car_name = car.car_name;
                    existingCar.car_description = car.car_description;
                    existingCar.car_type = car.car_type;
                    existingCar.car_engine = car.car_engine;
                    existingCar.car_image = car.car_image;
                    existingCar.car_year = car.car_year;
                    cars1.SaveChanges();
                    return RedirectToAction("DashBoard", "Account");
                }
                else
                {
                    return View(car);
                }
            }
            else
            {
                return View(car);
            }
        }

        public ActionResult Details(int id)
        {
            if (Session["Username"] != null)
            {
                var car = cars1.CarsTable.Find(id);

                if (car != null)
                {
                    return View(car);
                }
                else
                {
                    return RedirectToAction("DashBoard", "Account");
                }
            }
            else
            {
                return RedirectToAction("Signin", "Account");
            }
        }

        public ActionResult PendingRequests()
        {
            List<UserRequestCars> carrequests = new List<UserRequestCars>();

            if (Session["Username"] != null)
            {
                using (var dbContext = new ProiectRacingDynamicsRequests())
                {
                    var cars = dbContext.UserRequestCars.Where(c => c.RequestStatus == "Pending").ToList();

                    foreach (var car in cars)
                    {
                        UserRequestCars carrequest = new UserRequestCars
                        {
                            RequestId = car.RequestId,
                            CarName = car.CarName,
                            CarImage = car.CarImage,
                            CarType = car.CarType,
                            CarEngine = car.CarEngine,
                            CarDescription = car.CarDescription,
                            CarYear = car.CarYear
                        };

                        carrequests.Add(carrequest);
                    }
                }
                return View(carrequests);
            }
            else
            {
                return RedirectToAction("Signin", "Account");
            }
        }

        public ActionResult Accept(int requestId, CarsTable cars)
        {
            using (var requests = new ProiectRacingDynamicsRequests())
            using (var cars1 = new ProiectRacingDynamicsCars())
            {
                var acceptedRequest = requests.UserRequestCars.Find(requestId);

                if (acceptedRequest != null)
                {
                    acceptedRequest.RequestStatus = "Accepted";

                    int newId;
                    lock (cars1)
                    {
                        newId = cars1.CarsTable.Max(x => (int?)x.car_id) ?? 0;
                        newId++;
                    }
                    cars.car_id = newId;

                    using (var transaction = cars1.Database.BeginTransaction())
                    {
                        try
                        {
                            var newCar = new CarsTable
                            {
                                car_id = newId,
                                car_name = acceptedRequest.CarName,
                                car_type = acceptedRequest.CarType,
                                car_description = acceptedRequest.CarDescription,
                                car_engine = acceptedRequest.CarEngine,
                                car_year = acceptedRequest.CarYear,
                                car_image = acceptedRequest.CarImage
                            };
                            cars1.CarsTable.Add(newCar);
                            cars1.SaveChanges();
                            requests.SaveChanges();
                            transaction.Commit();

                            return RedirectToAction("PendingRequests", "Account"); 
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
                }
            }
            return RedirectToAction("PendingRequests", "Account");
        }

        public ActionResult Deny(int requestId)
        {
            using(var requests = new ProiectRacingDynamicsRequests())
            {
                var deniedRequest = requests.UserRequestCars.Find(requestId);

                if(deniedRequest != null) {
                    deniedRequest.RequestStatus = "Denied";
                    requests.SaveChanges();
                    return RedirectToAction("PendingRequests", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Couldn't find the request!");
                    return RedirectToAction("PendingRequests", "Account");
                }
            }
        }


        public ActionResult DetailsRequest(int requestId)
        {
            if (Session["Username"] != null)
            {
                var car = cars1.CarsTable.Find(requestId);

                if (car != null)
                {
                    return View(car);
                }
                else
                {
                    return RedirectToAction("PendingRequests", "Account");
                }
            }
            else
            {
                return RedirectToAction("Signin", "Account");
            }
        }

        public ActionResult MyAccount()
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
    }
}