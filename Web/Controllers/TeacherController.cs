using System.ComponentModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolRegister.Models;
using SchoolRegister.Models.ViewModels.Grades;
using SchoolRegister.Models.ViewModels.Teacher;
using SchoolRegister.Models.ViewModels.Teachers;
using SchoolRegister.Services.Grades;
using SchoolRegister.Services.SchoolClasses;
using SchoolRegister.Services.Subjects;
using SchoolRegister.Services.Teachers;

namespace SchoolRegister.Controllers;

[Authorize(Roles = "Teacher")]
public class TeacherController : Controller {
    private readonly ITeachersService teachersService;
    private readonly ISubjectsService subjectsService;
    private readonly ISchoolClassesService schoolClassesService;
    private readonly IGradesService gradesService;
    private readonly UserManager<AppUser> userManager;
    private readonly IMapper mapper;

    public TeacherController(ITeachersService teachersService, 
        ISubjectsService subjectsService, 
        ISchoolClassesService schoolClassesService,
        IGradesService gradesService,
        UserManager<AppUser> userManager,
        IMapper mapper) {
        this.teachersService = teachersService;
        this.subjectsService = subjectsService;
        this.schoolClassesService = schoolClassesService;
        this.gradesService = gradesService;
        this.userManager = userManager;
        this.mapper = mapper;
    }

    public async Task<IActionResult> Index() {
        AppUser user = await userManager.GetUserAsync(User);
        Teacher teacher = await teachersService.GetByUser(user);

        IEnumerable<SchoolSubject> schoolSubjects = await teachersService.GetTaughtSubjects(teacher);
        var groupedSubjects = schoolSubjects.GroupBy(s => s.SchoolClass).AsEnumerable();

        List<TeacherSubjectViewModel> teacherSubjectsModelList = new();

        foreach(var group in groupedSubjects) {
            teacherSubjectsModelList.Add(new TeacherSubjectViewModel {
                SchoolClass = group.Key,
                SubjectList = group.Select(s => s.Subject).ToList()
            });
        }

        return View(teacherSubjectsModelList);
    }

    public async Task<IActionResult> ViewClassSubject(int subjectId, int classId) {
        AppUser user = await userManager.GetUserAsync(User);
        Teacher teacher = await teachersService.GetByUser(user);
        var studentSubjectModel = new TeacherStudentSubjectViewModel();

        IEnumerable<StudentSubject> studentSubjects = await subjectsService.GetStudentSubjectsForTeacher(teacher.Id);
        var schoolClass = await schoolClassesService.GetById(classId);

        studentSubjectModel.StudentSubjects = studentSubjects.Where(s => s.SchoolSubject.Subject.Id == subjectId && s.Student.SchoolClass == schoolClass);
        studentSubjectModel.SubjectName = (await subjectsService.GetById(subjectId)).Name;
        studentSubjectModel.ClassName = schoolClass.Name;

        return View(studentSubjectModel);
    }
}