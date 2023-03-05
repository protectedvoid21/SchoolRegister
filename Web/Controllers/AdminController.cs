using AutoMapper;
using Data.Models;
using Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.SchoolClasses;
using Services.Students;
using Services.Subjects;
using Services.Teachers;

namespace SchoolRegister.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller {
    private readonly UserManager<AppUser> userManager;

    private readonly ITeachersService teachersService;
    private readonly IStudentsService studentsService;
    private readonly ISchoolClassesService schoolClassesService;
    private readonly ISubjectsService subjectsService;
    private readonly IMapper mapper;

    public AdminController(
        UserManager<AppUser> userManager,
        ITeachersService teachersService,
        IStudentsService studentsService,
        ISubjectsService subjectsService,
        ISchoolClassesService schoolClassesService,
        IMapper mapper) {
        this.userManager = userManager;
        this.teachersService = teachersService;
        this.studentsService = studentsService;
        this.schoolClassesService = schoolClassesService;
        this.subjectsService = subjectsService;
        this.mapper = mapper;
    }

    public async Task<ViewResult> Panel() {
        var adminViewModel = new AdminPanelViewModel {
            ClassCount = await schoolClassesService.GetCountAsync(),
            StudentCount = await studentsService.GetCountAsync(),
            TeacherCount = await teachersService.GetCountAsync(),
            SubjectCount = await subjectsService.GetCountAsync()
        };
        return View(adminViewModel);
    }

    [HttpGet]
    public async Task<ViewResult> CreateSchoolSubject(int id) {
        Teacher teacher = await teachersService.GetById(id);

        var schoolSubjectModel = mapper.Map<SchoolSubjectViewModel>(teacher);

        schoolSubjectModel.SubjectList = await subjectsService.GetAllSubjects();
        IEnumerable<SchoolClass> schoolClassList = await schoolClassesService.GetAllAsync();
        schoolSubjectModel.SchoolClassList = schoolClassList;

        return View(schoolSubjectModel);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSchoolSubject(SchoolSubjectViewModel schoolSubjectModel) {
        if (!ModelState.IsValid) {
            return View(schoolSubjectModel);
        }

        await subjectsService.AddSchoolSubjectsAsync(
            schoolSubjectModel.SubjectId, schoolSubjectModel.SelectedClassIds, schoolSubjectModel.TeacherId);

        return RedirectToAction("ViewAll", "Teacher");
    }

    public async Task<IActionResult> DeleteSchoolSubject(int id) {
        await subjectsService.DeleteSchoolSubjectAsync(id);
        return RedirectToAction("ViewAll", "Teacher");
    }
}