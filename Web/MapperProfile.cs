using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SchoolRegister.Models;
using SchoolRegister.Models.ViewModels;
using SchoolRegister.Models.ViewModels.Grades;
using SchoolRegister.Models.ViewModels.Students;
using SchoolRegister.Models.ViewModels.Teachers;
using SchoolRegister.Models.ViewModels.User;

namespace SchoolRegister; 

public class MapperProfile : Profile {
    public MapperProfile() {
        CreateMap<IdentityRole, RoleViewModel>();
        CreateMap<Student, StudentViewModel>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.User.Surname));
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

        CreateMap<SchoolSubject, TeachingClassModel>()
            .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.SchoolClass.Name))
            .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.Name));
        CreateMap<Teacher, SchoolSubjectViewModel>()
            .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.TeacherSurname, opt => opt.MapFrom(src => src.User.Surname))
            .ForMember(dest => dest.TeacherId, opt => opt.MapFrom(src => src.Id));
        CreateMap<Teacher, TeacherViewModel>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.User.Surname))
            .ForMember(dest => dest.ClassCount, opt => opt.MapFrom(src => src.SchoolSubjects.Select(s => s.SchoolClass.Id).Distinct().Count()))
            .ForMember(dest => dest.SubjectCount, opt => opt.MapFrom(src => src.SchoolSubjects.Select(s => s.Subject).Distinct().Count()))
            .ForMember(dest => dest.SchoolSubjectCount, opt => opt.MapFrom(src => src.SchoolSubjects.Count))
            .ForMember(dest => dest.TeachingClassList,
                opt => opt.MapFrom(src => src.SchoolSubjects.Where(s => s.TeacherId == src.Id).OrderBy(c => c.SchoolClass.Name)));

        CreateMap<Announcement, AnnouncementViewModel>()
            .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.Teacher.User.Name))
            .ForMember(dest => dest.TeacherSurname, opt => opt.MapFrom(src => src.Teacher.User.Surname));
        CreateMap<Announcement, AnnouncementCreateViewModel>();
    }
}