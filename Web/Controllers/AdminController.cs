using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolRegister.Models;
using SchoolRegister.Models.ViewModels;
using SchoolRegister.Services.SchoolClasses;
using SchoolRegister.Services.Students;
using SchoolRegister.Services.Subjects;
using SchoolRegister.Services.Teachers;

namespace SchoolRegister.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller {
    private readonly UserManager<AppUser> userManager;

    private readonly ITeachersService teachersService;
    private readonly IStudentsService studentsService;
    private readonly ISchoolClassesService schoolClassesService;
    private readonly ISubjectsService subjectsService;

    public AdminController(
        UserManager<AppUser> userManager,
        ITeachersService teachersService,
        IStudentsService studentsService,
        ISubjectsService subjectsService,
        ISchoolClassesService schoolClassesService) {
        this.userManager = userManager;
        this.teachersService = teachersService;
        this.studentsService = studentsService;
        this.schoolClassesService = schoolClassesService;
        this.subjectsService = subjectsService;
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

    #region SchoolClass

    [HttpGet]
    public ViewResult CreateSchoolClass() {
        var schoolClassModel = new SchoolClassViewModel();
        return View(schoolClassModel);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSchoolClass(SchoolClassViewModel schoolClassModel) {
        if(!ModelState.IsValid || await schoolClassesService.IsSchoolClassExisting(schoolClassModel.Name)) {
            return View(schoolClassModel);
        }

        SchoolClass schoolClass = new() {
            Name = schoolClassModel.Name,
            StudentsList = new()
        };

        await schoolClassesService.AddAsync(schoolClass);
        return RedirectToAction("SchoolClassList");
    }

    [HttpGet]
    public async Task<IActionResult> EditSchoolClass(int schoolClassId) {
        SchoolClass schoolClass = await schoolClassesService.GetById(schoolClassId);
        return View(schoolClass);
    }

    [HttpPost]
    public async Task<IActionResult> EditSchoolClass(SchoolClass schoolClass) {
        if(!ModelState.IsValid) {
            return View(schoolClass);
        }

        if(await schoolClassesService.IsSchoolClassExisting(schoolClass.Name)) {
            ModelState.AddModelError("", "Class with this name already exists");
            return View(schoolClass);
        }

        await schoolClassesService.UpdateAsync(schoolClass);
        return RedirectToAction("SchoolClassList");
    }

    public async Task<IActionResult> DeleteSchoolClass(int schoolClassId) {
        await schoolClassesService.DeleteAsync(schoolClassId);
        return RedirectToAction("SchoolClassList");
    }

    public async Task<IActionResult> SchoolClassList() {
        IEnumerable<SchoolClass> schoolClassList = await schoolClassesService.GetAllAsync();
        return View(schoolClassList);
    }

    public async Task<IActionResult> SchoolClassView(int schoolClassId) {
        SchoolClass schoolClass = await schoolClassesService.GetById(schoolClassId);
        return View(schoolClass);
    }

    public async Task<IActionResult> ClassSubjectView(int schoolClassId) {
        ViewBag.ClassName = (await schoolClassesService.GetById(schoolClassId)).Name;

        IEnumerable<Subject> subjects = (await subjectsService.GetAllSchoolSubjects())
            .Where(s => s.SchoolClass.Id == schoolClassId)
            .Select(s => s.Subject);
        return View(subjects);
    }

    #endregion

    #region Subject

    [HttpGet]
    public async Task<ViewResult> CreateSchoolSubject(int teacherId) {
        Teacher teacher = await teachersService.GetById(teacherId);

        var schoolSubjectModel = new SchoolSubjectViewModel {
            SubjectList = await subjectsService.GetAllSubjects(),
            TeacherId = teacherId,
            TeacherName = teacher.User.Name,
            TeacherSurname = teacher.User.Surname,
        };
        var schoolClassList = await schoolClassesService.GetAllAsync();
        schoolSubjectModel.SchoolClassList = schoolClassList.ToArray();
        schoolSubjectModel.ClassChoiceId = new List<ClassChoiceModel>();

        foreach(var schoolClass in schoolSubjectModel.SchoolClassList) {
            schoolSubjectModel.ClassChoiceId.Add(new ClassChoiceModel {
                Id = schoolClass.Id,
                IsPicked = false
            });
        }

        return View(schoolSubjectModel);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSchoolSubject(SchoolSubjectViewModel schoolSubjectModel) {
        if(!ModelState.IsValid) {
            return RedirectToAction("CreateSchoolSubject", schoolSubjectModel.TeacherId);
        }

        List<SchoolSubject> schoolSubjects = new();
        IEnumerable<int> pickList = schoolSubjectModel.ClassChoiceId.Where(c => c.IsPicked).Select(c => c.Id);
        Subject subject = await subjectsService.GetById(schoolSubjectModel.SubjectId);
        List<int> schoolClassIdList = new();

        foreach(var schoolClassId in pickList) {
            if((await subjectsService.GetSchoolSubjectsByClass(schoolClassId)).Any(s => s.SubjectId == schoolSubjectModel.SubjectId)) {
                continue;
            }

            schoolClassIdList.Add(schoolClassId);

            schoolSubjects.Add(new SchoolSubject {
                TeacherId = schoolSubjectModel.TeacherId,
                SubjectId = schoolSubjectModel.SubjectId,
                SchoolClassId = schoolClassId
            });
        }
        await subjectsService.AddSchoolSubjectRangeAsync(schoolSubjects);

        foreach(var schoolClassId in schoolClassIdList) {
            SchoolSubject schoolSubject = (await subjectsService.GetSchoolSubjectsByClass(schoolClassId)).First(s => s.Subject == subject);
            await subjectsService.UpdateStudentSubjectsInClass(schoolSubject);
        }

        return RedirectToAction("ViewAll", "Teacher");
    }

    public async Task<IActionResult> DeleteSchoolSubject(int schoolSubjectId) {
        SchoolSubject schoolSubject = await subjectsService.GetSchoolSubjectById(schoolSubjectId);
        await subjectsService.DeleteSchoolSubjectAsync(schoolSubject);
        return RedirectToAction("ViewAll", "Teacher");
    }

    #endregion
}