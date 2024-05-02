using Microsoft.AspNetCore.Mvc;
using Demo.BLL.Repositories;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        //private IDepartmentRepository _departmentRepository; // NULL
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(/*IDepartmentRepository departmentRepository*/ IUnitOfWork unitOfWork) // Ask CLR Create Object From DepartmentRepository
        {
            //_departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
        }

        // Get ALL
        // /Department/Index
        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAll();
            return View(departments);
        }

        // HTTP Get
        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Department model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
               await _unitOfWork.DepartmentRepository.Add(model);
                int count = await _unitOfWork.Complete();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }

            }

            return View(model);
        }

        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {

            if (id is null)
                return BadRequest(); // 400

            var department = await _unitOfWork.DepartmentRepository.Get(id.Value);

            if (department is null)
                return NotFound(); // 404

            return View(ViewName, department);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {

            return await  Details(id, "Edit");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, Department model)
        {
            if (id != model.Id)
                return BadRequest(); //400

            _unitOfWork.DepartmentRepository.Update(model);
            int count = await  _unitOfWork.Complete();

            if (count > 0)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await  Details(id, "Delete");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete([FromRoute] int id, Department model)
        {
            if (id != model.Id)
                return BadRequest(); //400

            _unitOfWork.DepartmentRepository.Delete(model);
            int count = await _unitOfWork.Complete();

            if (count > 0)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


    }
}
