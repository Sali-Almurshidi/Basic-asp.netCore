using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;
using WebApplication.ViewModels;

namespace WebApplication.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        [Obsolete]
        private readonly IHostingEnvironment _hostingEnvironment;

        [Obsolete]
        public HomeController(IEmployeeRepository employeeRepository, IHostingEnvironment hostingEnvironment)
        {
            _employeeRepository = employeeRepository;
            _hostingEnvironment = hostingEnvironment;
        }
        [Route("~/Home")]
        [Route("~/")]
        public ViewResult Index()
        {
            // to return just one element
            //return _employeeRepository.GetEmployee(1).Name;

            var model = _employeeRepository.GetEmployees();

            // add the title here or in the view page as detalis example
            ViewBag.Title = "Employee Index Bag";

            // IEnumerable<Employee> model
            return View(model);
            // by default it will call index view but we can change that
            // return View("~/Views/Home/index.cshtml")
        }

        [Route("{id?}")]
        public ViewResult Detalis(int id)
        {
            HomeDetalisViewModels homeDetalisViewModels = new HomeDetalisViewModels()
            {
                // if id= null so use thz default value 1  id ?? 1
                Employee = _employeeRepository.GetEmployee(id),

                PageTitleView = "Employee Details"
            };
            Employee model = _employeeRepository.GetEmployee(id);
            // it go to show Details by default but we can cheane the view 

            //return View("Detals", model);
            // to send the model to the view
            ViewData["Employee"] = model;
            ViewBag.EmployeeBag = model;
            // send the title to view by pageTitle key
            ViewData["PageTitle"] = "Employee Details";
            ViewBag.PageTitleBag = "Employee Details Bag";

            // add model as return value is how strongly typed view 
            // return View(model);

            // return HomeDetalisViewModels this is a view models way
            return View(homeDetalisViewModels);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }
        [HttpPost]
        [Obsolete]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            // to check to model is valid
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                if (model.Photo != null)
                {

                    // to save the photo in images file
                    string uploadsFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot/images");
                    // give the photo unique name
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;

                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                // create new Employee object to send the data to data base
                Employee newEmployee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    Photo = uniqueFileName

                };
                _employeeRepository.Add(newEmployee);

                // Employee newEmployee = _employeeRepository.Add(employee);
                // to move to detalis page
                return RedirectToAction("Detalis", new { id = newEmployee.Id });
            }
            return View();



        }
    }
}
