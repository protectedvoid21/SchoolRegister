using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolRegister.Models;
using SchoolRegister.Models.ViewModels.Grades;
using SchoolRegister.Models.ViewModels.Students;
using SchoolRegister.Services.Grades;
using SchoolRegister.Services.Students;
using SchoolRegister.Services.Subjects;
using SchoolRegister.Services.Teachers;

namespace SchoolRegister.Controllers;

/*[Authorize(Roles = "Student")]*/
public class StudentController : Controller {
    private readonly IStudentsService studentsService;
    private readonly IGradesService gradesService;
    private readonly ISubjectsService subjectsService;
    private readonly ITeachersService teachersService;
    private readonly UserManager<AppUser> userManager;
    private readonly IMapper mapper;

    public StudentController(IStudentsService studentsService,
        IGradesService gradesService,
        ISubjectsService subjectsService,
        ITeachersService teachersService,
        UserManager<AppUser> userManager,
        IMapper mapper) {
        this.studentsService = studentsService;
        this.gradesService = gradesService;
        this.subjectsService = subjectsService;
        this.teachersService = teachersService;
        this.userManager = userManager;
        this.mapper = mapper;
    }

    [Authorize(Roles = "Student")]
    public async Task<IActionResult> Index() {
        AppUser user = await userManager.GetUserAsync(User);
        Student student = await studentsService.GetByUser(user);

        //IEnumerable<StudentSubject> studentSubjects = await subjectsService.GetStudentSubjectsForStudent(student);
        StudentPanelViewModel studentPanel = mapper.Map<StudentPanelViewModel>(student);
        return View(studentPanel);
    }
}