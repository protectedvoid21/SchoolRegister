using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolRegister.Models;
using SchoolRegister.Models.ViewModels;
using SchoolRegister.Services.Grades;
using SchoolRegister.Services.Persons;

namespace SchoolRegister.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller {
    private readonly UserManager<IdentityUser> userManager;
    private readonly SignInManager<IdentityUser> signInManager;
    private readonly IPersonsService personsService;
    private readonly ISubjectsService subjectsService;

    public AdminController(
        UserManager<IdentityUser> userManager, 
        SignInManager<IdentityUser> signInManager,
        IPersonsService personsService,
        ISubjectsService subjectsService) {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.personsService = personsService;
        this.subjectsService = subjectsService;
    }

    public async Task<IActionResult> Panel() {
        var adminViewModel = new AdminPanelViewModel {
            ClassCount = await personsService.GetClassCount(),
            StudentCount = await personsService.GetStudentCount(),
            TeacherCount = await personsService.GetTeacherCount(),
            SubjectCount = await subjectsService.GetSubjectCount()
        };
        return View(adminViewModel);
    }

    [HttpGet]
    public ViewResult CreateSchoolClass() {
        var schoolClassModel = new SchoolClassViewModel();
        return View(schoolClassModel);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSchoolClass(SchoolClassViewModel schoolClassModel) {
        if (!ModelState.IsValid) {
            return View();
        }

        SchoolClass schoolClass = new() {
            Name = schoolClassModel.Name,
            SubjectList = new(),
            StudentsList = new()
        };

        await personsService.AddSchoolClass(schoolClass);
        return RedirectToAction("Panel");
    }

    [HttpGet]
    public ViewResult CreateSubject() {
        var subjectModel = new SubjectViewModel();
        return View(subjectModel);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSubject(SubjectViewModel subjectModel) {
        if (!ModelState.IsValid) {
            return View(subjectModel);
        }

        if (await subjectsService.IsSubjectExisting(subjectModel.Name)) {
            ModelState.AddModelError("", "Subject with this name already exists");
            return View(subjectModel);
        }

        Subject subject = new() {
            Name = subjectModel.Name,
        };

        await subjectsService.AddSubject(subject);
        return RedirectToAction("Panel");
    }

    [HttpGet]
    public async Task<ViewResult> CreateStudent() {
        var studentModel = new CreateStudentViewModel {
            SchoolClassList = await personsService.GetAllSchoolClasses(),
        };
        return View(studentModel);
    }

    [HttpPost]
    public async Task<IActionResult> CreateStudent(CreateStudentViewModel studentModel) {
        if (!ModelState.IsValid) {
            return RedirectToAction("CreateStudent");
        }

        Student student = new() {
            Name = studentModel.Name,
            Surname = studentModel.Surname,
            Class = await personsService.GetSchoolClassById(studentModel.SchoolClassId),
        };

        await personsService.AddStudent(student);
        return RedirectToAction("Panel");
    }

    [HttpGet]
    public async Task<ViewResult> CreateSchoolSubject() {
        var schoolSubjectModel = new SchoolSubjectViewModel {
            SchoolClassList = await personsService.GetAllSchoolClasses(),
            SubjectList = await subjectsService.GetAllSubjects(),
            TeacherList = await personsService.GetAllTeachers()
        };
        return View(schoolSubjectModel);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSchoolSubject(SchoolSubjectViewModel schoolSubjectModel) {
        if (!ModelState.IsValid) {
            return RedirectToAction("CreateSchoolSubject");
        }

        SchoolSubject subject = new() {
            Subject = await subjectsService.GetSubject(schoolSubjectModel.SubjectId),
            SchoolClass = await personsService.GetSchoolClassById(schoolSubjectModel.SchoolClassId),
            Teacher = await personsService.GetTeacherById(schoolSubjectModel.TeacherId),
        };

        await subjectsService.AddSchoolSubject(subject);
        return RedirectToAction("Panel");
    }

    [HttpGet]
    public ViewResult CreateTeacher() {
        var teacherModel = new TeacherViewModel();
        return View(teacherModel);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTeacher(TeacherViewModel teacherModel) {
        if (!ModelState.IsValid) {
            return View();
        }

        Teacher teacher = new() {
            Name = teacherModel.Name,
            Surname = teacherModel.Surname,
        };

        await personsService.AddTeacher(teacher);
        return RedirectToAction("Panel");
    }
}