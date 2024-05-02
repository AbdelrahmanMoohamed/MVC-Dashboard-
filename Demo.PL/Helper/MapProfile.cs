using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.ViewModels;

namespace Demo.PL.Helper
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Employee, EmployeeViewModel>().ReverseMap();
            //CreateMap<EmployeeViewModel, Employee>();
        }
    }
}
