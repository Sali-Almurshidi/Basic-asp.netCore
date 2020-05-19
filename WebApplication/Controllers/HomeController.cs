using Microsoft.AspNetCore.Http;
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
            throw new Exception("Error in Detailes view");

            Employee employee = _employeeRepository.GetEmployee(id);

            if (employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id);
            }

            HomeDetalisViewModels homeDetalisViewModels = new HomeDetalisViewModels()
            {
                // if id= null so use thz default value 1  id ?? 1
                //Employee = _employeeRepository.GetEmployee(id),
                Employee = employee,

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
                string uniqueFileName = processUploadedFile(model);

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

        [HttpGet]
        public ViewResult Edit(int id)
        {
            Employee employee = _employeeRepository.GetEmployee(id);

            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                ExistingPhotoPath = employee.Photo
            };

            return View(employeeEditViewModel);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel model)
        {

            // to check to model is valid
            if (ModelState.IsValid)
            {
                Employee employee = _employeeRepository.GetEmployee(model.Id);
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = model.Department;

                if (model.Photos != null)
                {
                    if (model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot/images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    employee.Photo = processUploadedFile(model);
                }

                _employeeRepository.Update(employee);

                // Employee newEmployee = _employeeRepository.Add(employee);
                // to move to detalis page
                return RedirectToAction("index");
            }
            return View();



        }

        private string processUploadedFile(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;

            if (model.Photos != null && model.Photos.Count > 0)
            {

                // can select more than one photo
                foreach (IFormFile photo in model.Photos)
                {
                    // to save the photo in images file
                    string uploadsFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot/images");
                    // give the photo unique name
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        photo.CopyTo(fileStream);
                    }
                    // photo.CopyTo(new FileStream(filePath, FileMode.Create));
                }

            }

            return uniqueFileName;
        }
    }
}
