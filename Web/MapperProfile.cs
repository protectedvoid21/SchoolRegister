using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SchoolRegister.Models;
using SchoolRegister.Models.ViewModels.Grades;
using SchoolRegister.Models.ViewModels.Students;
using SchoolRegister.Models.ViewModels.User;

namespace SchoolRegister; 

public class MapperProfile : Profile {
    public MapperProfile() {
        CreateMap<IdentityRole, RoleViewModel>();
        CreateMap<Student, StudentPanelViewModel>();
        CreateMap<StudentSubject, GradeCreateViewModel>()
            .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.User.Name))
            .ForMember(dest => dest.StudentSurname, opt => opt.MapFrom(src => src.Student.User.Surname))
            .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.SchoolSubject.Subject.Name))
            .ForMember(dest => dest.SubjectId, opt => opt.MapFrom(src => src.SchoolSubject.SubjectId));
        CreateMap<Grade, GradeViewModel>()
            .ForMember(dest => dest.GradeName, opt => opt.MapFrom(src => src.GetGradeName()))
            .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.Name));
        CreateMap<Grade, GradeEditViewModel>()
            .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.Name))
            .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.StudentSubject.Student.User.Name))
            .ForMember(dest => dest.StudentSurname, opt => opt.MapFrom(src => src.StudentSubject.Student.User.Surname));
    }
}