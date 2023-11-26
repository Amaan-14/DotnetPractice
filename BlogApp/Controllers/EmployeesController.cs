using BlogApp.Data;
using BlogApp.Models;
using BlogApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.ObjectModelRemoting;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Controllers
{
    public class EmployeesController : Controller
    {
        private BlogDbContext _DbContext;
        public EmployeesController(BlogDbContext DbContext)
        {
                _DbContext = DbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await _DbContext.Employees.ToListAsync();

            return View(employees);

          
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(EmployeeViewModel EmployeeModelRequest)
        {
            var Employee = new Employee();
            {
                Employee.Id = Guid.NewGuid();
                Employee.Name = EmployeeModelRequest.Name;
                Employee.Salary = EmployeeModelRequest.Salary;
                Employee.DateOFBirth = EmployeeModelRequest.DateOFBirth;
                Employee.Department = EmployeeModelRequest.Department;
                Employee.Email = EmployeeModelRequest.Email;
            }

           await  _DbContext.Employees.AddAsync(Employee);
           await   _DbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid? Id)
        {
            var employee = await _DbContext.Employees.FirstOrDefaultAsync(x => x.Id == Id);

            if(employee != null)
            {
                var updateemployee = new UpdateEmployeeViewModel()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Department = employee.Department,
                    DateOFBirth = employee.DateOFBirth,
                    Salary = employee.Salary

                };

                return await Task.Run( () => View("View",updateemployee));
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View( UpdateEmployeeViewModel updateEmployee)
        {
            var employee = await _DbContext.Employees.FindAsync(updateEmployee.Id);

            if(employee != null)
            {
                employee.Salary = updateEmployee.Salary;
                employee.DateOFBirth = updateEmployee.DateOFBirth;
                employee.Email = updateEmployee.Email;
                employee.Department = updateEmployee.Department;
                employee.Name = updateEmployee.Name;

                await _DbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");


        }


        public async Task<IActionResult> Delete(UpdateEmployeeViewModel DeleteEmployee)
        {
            var employee = await _DbContext.Employees.FindAsync(DeleteEmployee.Id);

            if(employee != null)
            {
                 _DbContext.Employees.Remove(employee);
                await _DbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

    }
}
