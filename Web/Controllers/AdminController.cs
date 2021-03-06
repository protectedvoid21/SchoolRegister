using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolRegister.Models;
using SchoolRegister.Models.ViewModels;
using SchoolRegister.Models.ViewModels.Students;
using SchoolRegister.Models.ViewModels.Teachers;
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

        var user = new AppUser {
            UserName = Utils.GenerateUserName(createTeacherModel.Name, createTeacherModel.Surname),
            Name = createTeacherModel.Name,
            Surname = createTeacherModel.Surname,
        };

        await userManager.CreateAsync(user, Utils.GeneratePassword(10));
        await userManager.AddToRoleAsync(user, "Teacher");

        Teacher teacher = new() {
            User = user,
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

        teacher.User.UserName = Utils.GenerateUserName(teacher.User.Name, teacher.User.Surname);

        await teachersService.UpdateAsync(teacher);
        return RedirectToAction("TeacherList");
    }

    public async Task<IActionResult> DeleteTeacher(int teacherId) {
        Teacher teacher = await teachersService.GetById(teacherId);
        //await userManager.DeleteAsync(teacher.User);
        await teachersService.DeleteAsync(teacher);
        return RedirectToAction("TeacherList");
    }

    public async Task<IActionResult> TeacherList() {
        IEnumerable<Teacher> teacherList = await teachersService.GetAllAsync();
        List<TeacherViewModel> teacherModelList = new();

        foreach(var teacher in teacherList) {
            teacherModelList.Add(new TeacherViewModel {
                Id = teacher.Id,
                Name = teacher.User.Name,
                Surname = teacher.User.Surname,
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
            Name = teacher.User.Name,
            Surname = teacher.User.Surname,
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

        await studentsService.AddAsync(studentModel.Name, studentModel.Surname, studentModel.SchoolClassId);

        return RedirectToAction("SchoolClassList");
    }

    [HttpGet]
    public async Task<IActionResult> EditStudent(int studentId) {
        Student student = await studentsService.GetById(studentId);
        StudentViewModel studentModel = new() {
            Id = studentId,
            Name = student.User.Name,
            Surname = student.User.Surname,
        };
        return View(studentModel);
    }

    [HttpPost]
    public async Task<IActionResult> EditStudent(StudentViewModel studentModel) {
        if(!ModelState.IsValid) {
            return View(studentModel);
        }

        await studentsService.UpdateAsync(studentModel.Id, studentModel.Name, studentModel.Surname);

        return RedirectToAction("SchoolClassList");
    }

    public async Task<IActionResult> DeleteStudent(int studentId) {
        await studentsService.DeleteAsync(studentId);
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

        return RedirectToAction("TeacherList");
    }

    public async Task<IActionResult> DeleteSchoolSubject(int schoolSubjectId) {
        SchoolSubject schoolSubject = await subjectsService.GetSchoolSubjectById(schoolSubjectId);
        await subjectsService.DeleteSchoolSubjectAsync(schoolSubject);
        return RedirectToAction("TeacherList");
    }

    public async Task<IActionResult> SubjectList() {
        List<Subject> subjectList = await subjectsService.GetAllSubjects();
        List<SubjectElementViewModel> subjectListModel = new();
        foreach(var subject in subjectList) {
            subjectListModel.Add(new SubjectElementViewModel {
                Subject = subject,
                StudentCount = await subjectsService.GetCountByStudents(subject)
            });
        }
        return View(subjectListModel);
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

        await subjectsService.AddAsync(subjectModel.Name);
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
        await subjectsService.UpdateAsync(subject.Id, subject.Name);
        return RedirectToAction("SubjectList");
    }

    public async Task<IActionResult> DeleteSubject(int subjectId) {
        await subjectsService.DeleteAsync(subjectId);
        return RedirectToAction("SubjectList");
    }

    #endregion
}