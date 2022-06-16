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
    private readonly UserManager<IdentityUser> userManager;
    private readonly SignInManager<IdentityUser> signInManager;
    private readonly ITeachersService teachersService;
    private readonly IStudentsService studentsService;
    private readonly ISchoolClassesService schoolClassesService;
    private readonly ISubjectsService subjectsService;

    public AdminController(
        UserManager<IdentityUser> userManager, 
        SignInManager<IdentityUser> signInManager,
        ITeachersService teachersService,
        IStudentsService studentsService,
        ISubjectsService subjectsService,
        ISchoolClassesService schoolClassesService) {
        this.userManager = userManager;
        this.signInManager = signInManager;
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
        if (!ModelState.IsValid) {
            return View(schoolClass);
        }

        if (await schoolClassesService.IsSchoolClassExisting(schoolClass.Name)) {
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

    #endregion

    #region Teacher

    [HttpGet]
    public ViewResult AddTeacher() {
        var teacherModel = new CreateTeacherViewModel();
        return View(teacherModel);
    }

    [HttpPost]
    public async Task<IActionResult> AddTeacher(CreateTeacherViewModel createTeacherModel) {
        if(!ModelState.IsValid) {
            return View(createTeacherModel);
        }

        Teacher teacher = new() {
            Name = createTeacherModel.Name,
            Surname = createTeacherModel.Surname,
        };

        await teachersService.AddAsync(teacher);
        return RedirectToAction("TeacherList");
    }

    [HttpGet]
    public async Task<IActionResult> EditTeacher(int teacherId) {
        return View(await teachersService.GetById(teacherId));
    }

    [HttpPost]
    public async Task<IActionResult> EditTeacher(Teacher teacher) {
        if(!ModelState.IsValid) {
            return View(teacher);
        }

        await teachersService.UpdateAsync(teacher);
        return RedirectToAction("TeacherList");
    }

    public async Task<IActionResult> DeleteTeacher(int teacherId) {
        Teacher teacher = await teachersService.GetById(teacherId);
        await teachersService.DeleteAsync(teacher);
        return RedirectToAction("TeacherList");
    }

    public async Task<IActionResult> TeacherList() {
        IEnumerable<Teacher> teacherList = await teachersService.GetAllAsync();
        List<TeacherViewModel> teacherModelList = new();

        foreach (var teacher in teacherList) {
            teacherModelList.Add(new TeacherViewModel {
                Id = teacher.Id,
                Name = teacher.Name,
                Surname = teacher.Surname,
                ClassCount = teacher.SchoolSubjects.Select(s => s.SchoolClass.Id).Distinct().Count(),
                SubjectCount = teacher.SchoolSubjects.Select(s => s.Subject).Distinct().Count(),
                SchoolSubjectCount = teacher.SchoolSubjects.Count,
            });
        }

        return View(teacherModelList);
    }

    public async Task<IActionResult> TeacherView(int teacherId) {
        Teacher teacher = await teachersService.GetById(teacherId);
        TeacherViewModel teacherModel = new() {
            Id = teacherId,
            Name = teacher.Name,
            Surname = teacher.Surname,
            ClassCount = teacher.SchoolSubjects.Select(s => s.SchoolClass.Id).Distinct().Count(),
            SubjectCount = teacher.SchoolSubjects.Select(s => s.Subject).Distinct().Count(),
            SchoolSubjectCount = teacher.SchoolSubjects.Count
        };

        var schoolSubjects = await subjectsService.GetAllSchoolSubjects();
        schoolSubjects = schoolSubjects.Where(s => s.Teacher == teacher).ToList();

        foreach(var schoolSubject in schoolSubjects) {
            teacherModel.TeachingClassList.Add(new TeachingClassModel {
                ClassName = schoolSubject.SchoolClass.Name,
                SubjectName = schoolSubject.Subject.Name,
            });
        }
        teacherModel.TeachingClassList = teacherModel.TeachingClassList.OrderBy(c => c.ClassName).ToList();

        return View(teacherModel);
    }

    #endregion

    #region Student

    [HttpGet]
    public async Task<ViewResult> CreateStudent(int schoolClassId) {
        var schoolClass = await schoolClassesService.GetById(schoolClassId);
        var studentModel = new CreateStudentViewModel {
            SchoolClassId = schoolClassId,
            SchoolClassName = schoolClass.Name,
        };
        return View(studentModel);
    }

    [HttpPost]
    public async Task<IActionResult> CreateStudent(CreateStudentViewModel studentModel) {
        if(!ModelState.IsValid) {
            return View("CreateStudent", studentModel);
        }

        Student student = new() {
            Name = studentModel.Name,
            Surname = studentModel.Surname,
            Class = await schoolClassesService.GetById(studentModel.SchoolClassId),
        };

        await studentsService.AddAsync(student);
        return RedirectToAction("SchoolClassList");
    }

    [HttpGet]
    public async Task<IActionResult> EditStudent(int studentId) {
        Student student = await studentsService.GetById(studentId);
        StudentViewModel studentModel = new StudentViewModel {
            Id = studentId,
            Name = student.Name,
            Surname = student.Surname,
        };
        return View(studentModel);
    }

    [HttpPost]
    public async Task<IActionResult> EditStudent(StudentViewModel studentModel) {
        if (!ModelState.IsValid) {
            return View(studentModel);
        }
        Student student = await studentsService.GetById(studentModel.Id);
        student.Name = studentModel.Name;
        student.Surname = studentModel.Surname;

        await studentsService.UpdateAsync(student);

        return RedirectToAction("SchoolClassList");
    }

    public async Task<IActionResult> DeleteStudent(int studentId) {
        Student student = await studentsService.GetById(studentId);
        await studentsService.DeleteAsync(student);
        return RedirectToAction("SchoolClassList");
    }

    #endregion

    #region Subject

    [HttpGet]
    public async Task<ViewResult> CreateSchoolSubject(int teacherId) {
        Teacher teacher = await teachersService.GetById(teacherId);

        var schoolSubjectModel = new SchoolSubjectViewModel {
            SubjectList = await subjectsService.GetAllSubjects(),
            TeacherId = teacherId,
            TeacherName = teacher.Name,
            TeacherSurname = teacher.Surname,
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
        var pickList = schoolSubjectModel.ClassChoiceId.Where(c => c.IsPicked).Select(c => c.Id);

        foreach(var schoolClassId in pickList) {
            schoolSubjects.Add(new SchoolSubject {
                Teacher = await teachersService.GetById(schoolSubjectModel.TeacherId),
                Subject = await subjectsService.GetById(schoolSubjectModel.SubjectId),
                SchoolClass = await schoolClassesService.GetById(schoolClassId)
            });
        }

        await subjectsService.AddSchoolSubjectRangeAsync(schoolSubjects);
        return RedirectToAction("TeacherList");
    }

    public async Task<IActionResult> DeleteSchoolSubject(int schoolSubjectId) {
        SchoolSubject schoolSubject = await subjectsService.GetSchoolSubjectById(schoolSubjectId);
        await subjectsService.DeleteSchoolSubjectAsync(schoolSubject);
        return RedirectToAction("TeacherList");
    }

    public async Task<IActionResult> SubjectList() {
        List<Subject> subjectList = await subjectsService.GetAllSubjects();
        return View(subjectList);
    }

    [HttpGet]
    public ViewResult CreateSubject() {
        var subjectModel = new SubjectViewModel();
        return View(subjectModel);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSubject(SubjectViewModel subjectModel) {
        if(!ModelState.IsValid) {
            return View(subjectModel);
        }

        if(await subjectsService.IsSubjectExisting(subjectModel.Name)) {
            ModelState.AddModelError("", "Subject with this name already exists");
            return View(subjectModel);
        }

        Subject subject = new() {
            Name = subjectModel.Name,
        };

        await subjectsService.AddAsync(subject);
        return RedirectToAction("SubjectList");
    }

    [HttpGet]
    public async Task<IActionResult> EditSubject(int subjectId) {
        return View(await subjectsService.GetById(subjectId));
    }

    [HttpPost]
    public async Task<IActionResult> EditSubject(Subject subject) {
        if(!ModelState.IsValid) {
            return View(subject);
        }
        await subjectsService.UpdateAsync(subject);
        return RedirectToAction("SubjectList");
    }

    public async Task<IActionResult> DeleteSubject(int subjectId) {
        Subject subject = await subjectsService.GetById(subjectId);
        await subjectsService.DeleteAsync(subject);
        return RedirectToAction("SubjectList");
    }

    #endregion
}