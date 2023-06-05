using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RACINGDYNAMICSFINAL.Models;

namespace RACINGDYNAMICSFINAL.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult JDM()
        {
            List<CarsTable> carModels = new List<CarsTable>();

            using (var dbContext = new ProiectRacingDynamicsCars())
            {
                var cars = dbContext.CarsTable.Where(c => c.car_type == "JDM").ToList();

                foreach (var car in cars)
                {
                    CarsTable carModel = new CarsTable
                    {
                        car_id = car.car_id,
                        car_name = car.car_name,
                        car_image = car.car_image
                    };

                    carModels.Add(carModel);
                }
            }

            return View(carModels);
        }

        public ActionResult EuropeanMarket()
        {
            List<CarsTable> carModels = new List<CarsTable>();

            using (var dbContext = new ProiectRacingDynamicsCars())
            {
                var cars = dbContext.CarsTable.Where(c => c.car_type == "EuropeanMarket").ToList();

                foreach (var car in cars)
                {
                    CarsTable carModel = new CarsTable
                    {
                        car_id = car.car_id,
                        car_name = car.car_name,
                        car_image = car.car_image
                    };

                    carModels.Add(carModel);
                }
            }

            return View(carModels);
        }

        public ActionResult AmericanMarket()
        {
            List<CarsTable> carModels = new List<CarsTable>();

            using (var dbContext = new ProiectRacingDynamicsCars())
            {
                var cars = dbContext.CarsTable.Where(c => c.car_type == "AmericanMarket").ToList();

                foreach (var car in cars)
                {
                    CarsTable carModel = new CarsTable
                    {
                        car_id = car.car_id,
                        car_name = car.car_name,
                        car_image = car.car_image
                    };

                    carModels.Add(carModel);
                }
            }

            return View(carModels);
        }
    }
}