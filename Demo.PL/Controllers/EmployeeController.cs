using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.Helper;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private IEmployeeRepository _employeeRepository; // NULL
        private readonly IMapper mapper;


        //private IDepartmentRepository _departmentRepository;
        public EmployeeController(IUnitOfWork unitOfWork /*IEmployeeRepository employeeRepository,*/ /*IDepartmentRepository departmentRepository*/, IMapper mapper) // Ask CLR Create Object From DepartmentRepository
        {
            _unitOfWork = unitOfWork;
            //_employeeRepository = employeeRepository;
            this.mapper = mapper;
            //_departmentRepository = departmentRepository;
        }

        // Get ALL
        // /Department/Index
        public async Task<IActionResult> Index(string searchInput)
        {
            var employees = Enumerable.Empty<Employee>();


            if (string.IsNullOrEmpty(searchInput))
            {
                //employees = _employeeRepository.GetAll();
                employees = await _unitOfWork.EmployeeRepository.GetAll();
            }
            else
            {
                //employees = _employeeRepository.GetByName(searchInput);
                employees = await _unitOfWork.EmployeeRepository.GetByName(searchInput);

            }

            // 1. Add
            // 2. Update
            // 3. Delete


            var result = mapper.Map<IEnumerable<EmployeeViewModel>>(employees);

            // View's Dictionary: Transfer Extra Informations


            // 1. ViewData -> Property Inherited From Class Contoller : Dictionary

            ViewData["Message"] = "Hello ViewData";

            // 2. ViewBag -> Property Inherited From Class Contoller : dynamic

            ViewBag.Message = "Hello ViewBag";


            // 3. TempData -> Transfer Information From Request To Another

            return View(result);
        }

		public async Task<IActionResult> Search(string searchInput)
		{
			var employees = Enumerable.Empty<Employee>();


			if (string.IsNullOrEmpty(searchInput))
			{
				//employees = _employeeRepository.GetAll();
				employees = await _unitOfWork.EmployeeRepository.GetAll();
			}
			else
			{
				//employees = _employeeRepository.GetByName(searchInput);
				employees = await _unitOfWork.EmployeeRepository.GetByName(searchInput);

			}

			// 1. Add
			// 2. Update
			// 3. Delete


			var result = mapper.Map<IEnumerable<EmployeeViewModel>>(employees);

			// View's Dictionary: Transfer Extra Informations


			// 1. ViewData -> Property Inherited From Class Contoller : Dictionary

			ViewData["Message"] = "Hello ViewData";

			// 2. ViewBag -> Property Inherited From Class Contoller : dynamic

			ViewBag.Message = "Hello ViewBag";


			// 3. TempData -> Transfer Information From Request To Another

			return PartialView("EmployeeTablePartialView", result);
		}

		// HTTP Get
		[HttpGet]
        public IActionResult Create()
        {
            //ViewData["Departments"] = _departmentRepository.GetAll();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {

                model.ImageName = DocumentSettings.UploadFile(model.Image, "images");

                var employee = mapper.Map<Employee>(model);

                //Employee employee = new Employee()
                //{
                //    Id = model.Id,
                //    Name = model.Name,
                //    HireDate = model.HireDate,
                //    Salary = model.Salary,
                //    Address = model.Address,
                //    Phone = model.Phone,
                //    DateOfCreate = model.DateOfCreate,
                //    DepartmentId = model.DepartmentId,
                //    Department = model.Department,
                //    IsDeleted = model.IsDeleted
                //};

                //int count = _employeeRepository.Add(employee);
                await _unitOfWork.EmployeeRepository.Add(employee);
                int count =  await _unitOfWork.Complete();
                if (count > 0)
                {
                    TempData["Message"] = "Employee Added!";

                }
                else
                {
                    TempData["Message"] = "Employee Dose not Added!";
                }

                return RedirectToAction(nameof(Index));


            }

            return View(model);
        }

        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {

            if (id is null)
                return BadRequest(); // 400

            //var employee = _employeeRepository.Get(id.Value);
            var employee = await _unitOfWork.EmployeeRepository.Get(id.Value);
            //EmployeeViewModel employeeViewModel = (EmployeeViewModel) employee;

            var result = mapper.Map<EmployeeViewModel>(employee);

            if (employee is null)
                return NotFound(); // 404

            return View(ViewName, result);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            //ViewData["Departments"] = _departmentRepository.GetAll();

            return await Details(id, "Edit");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, EmployeeViewModel model)
        {
            if (id != model.Id)
                return BadRequest(); //400

            //int count = _employeeRepository.Update(model);

            if (model.ImageName is not null)
            {
                DocumentSettings.DeleteFile(model.ImageName, "images");
            }
            model.ImageName = DocumentSettings.UploadFile(model.Image, "images");

            var result = mapper.Map<Employee>(model);


            _unitOfWork.EmployeeRepository.Update(result);
            int count = await _unitOfWork.Complete();

            if (count > 0)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete([FromRoute] int id, EmployeeViewModel model)
        {
            if (id != model.Id)
                return BadRequest(); //400

            //var count = _employeeRepository.Delete(model);

            var result = mapper.Map<Employee>(model);

            _unitOfWork.EmployeeRepository.Delete(result);
            int count = await _unitOfWork.Complete();

            if (count > 0)
            {
                DocumentSettings.DeleteFile(model.ImageName, "images");

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}
