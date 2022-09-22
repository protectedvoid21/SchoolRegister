using System.Runtime.CompilerServices;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolRegister.Models;
using SchoolRegister.Models.ViewModels.Grades;
using SchoolRegister.Models.ViewModels.Students;
using SchoolRegister.Services.Grades;
using SchoolRegister.Services.SchoolClasses;
using SchoolRegister.Services.Students;
using SchoolRegister.Services.Subjects;
using SchoolRegister.Services.Teachers;

namespace SchoolRegister.Controllers;

[Authorize(Roles = GlobalConstants.AdministratorRoleName)]
public class StudentController : Controller {
    private readonly IStudentsService studentsService;
    private readonly IGradesService gradesService;
    private readonly ISubjectsService subjectsService;
    private readonly ITeachersService teachersService;
    private readonly ISchoolClassesService schoolClassesService;
    private readonly UserManager<AppUser> userManager;
    private readonly IMapper mapper;

    public StudentController(IStudentsService studentsService,
        IGradesService gradesService,
        ISubjectsService subjectsService,
        ITeachersService teachersService,
        ISchoolClassesService schoolClassesService,
        UserManager<AppUser> userManager,
        IMapper mapper) {
        this.studentsService = studentsService;
        this.gradesService = gradesService;
        this.subjectsService = subjectsService;
        this.teachersService = teachersService;
        this.schoolClassesService = schoolClassesService;
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

    [HttpGet]
    public async Task<ViewResult> Add(int schoolClassId) {
        SchoolClass schoolClass = await schoolClassesService.GetById(schoolClassId);
        CreateStudentViewModel studentModel = new() {
            SchoolClassId = schoolClassId,
            SchoolClassName = schoolClass.Name,
        };
        return View(studentModel);
    }

    [HttpPost]
    public async Task<IActionResult> Add(CreateStudentViewModel studentModel) {
        if(!ModelState.IsValid) {
            return View(studentModel);
        }

        await studentsService.AddAsync(studentModel.Name, studentModel.Surname, studentModel.SchoolClassId);

        return RedirectToAction("SchoolClassList", "Admin");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id) {
        Student student = await studentsService.GetById(id);
        StudentViewModel studentModel = mapper.Map<StudentViewModel>(student);

        return View(studentModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(StudentViewModel studentModel) {
        if(!ModelState.IsValid) {
            return View(studentModel);
        }

        await studentsService.UpdateAsync(studentModel.Id, studentModel.Name, studentModel.Surname);

        return RedirectToAction("SchoolClassList", "Admin");
    }

    public async Task<IActionResult> Delete(int id) {
        await studentsService.DeleteAsync(id);
        return RedirectToAction("SchoolClassList", "Admin");
    }
}