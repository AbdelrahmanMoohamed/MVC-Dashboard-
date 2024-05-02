using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL
{
	public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        private readonly IEmployeeRepository employeeRepository;
        private readonly IDepartmentRepository departmentRepository;

        public IEmployeeRepository EmployeeRepository => employeeRepository; // NULL
        public IDepartmentRepository DepartmentRepository => departmentRepository; // NULL


        public UnitOfWork(AppDbContext context) // Ask CLR Create Object From AppDbContext
        {
            _context = context;
            employeeRepository = new EmployeeRepository(_context);
            departmentRepository = new DepartmentRepository(_context);
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

		public async ValueTask DisposeAsync()
		{
            await _context.DisposeAsync();
		}
	}
}
