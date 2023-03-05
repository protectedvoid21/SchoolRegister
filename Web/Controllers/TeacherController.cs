using AutoMapper;
using Data.Models;
using Data.ViewModels.Teachers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Grades;
using Services.SchoolClasses;
using Services.Subjects;
using Services.Teachers;

namespace SchoolRegister.Controllers;

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

        foreach (var group in groupedSubjects) {
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

        studentSubjectModel.StudentSubjects = studentSubjects.Where(s =>
            s.SchoolSubject.Subject.Id == subjectId && s.Student.SchoolClass == schoolClass);
        studentSubjectModel.SubjectName = (await subjectsService.GetById(subjectId)).Name;
        studentSubjectModel.ClassName = schoolClass.Name;

        return View(studentSubjectModel);
    }

    [HttpGet, Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public ViewResult Add() {
        var teacherModel = new CreateTeacherViewModel();
        return View(teacherModel);
    }

    [HttpPost, Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public async Task<IActionResult> Add(CreateTeacherViewModel createTeacherModel) {
        if (!ModelState.IsValid) {
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
        return RedirectToAction("ViewAll");
    }

    [HttpGet, Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public async Task<IActionResult> Edit(int id) {
        return View(await teachersService.GetById(id));
    }

    [HttpPost, Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public async Task<IActionResult> Edit(Teacher teacher) {
        if (!ModelState.IsValid) {
            return View(teacher);
        }

        teacher.User.UserName = Utils.GenerateUserName(teacher.User.Name, teacher.User.Surname);

        await teachersService.UpdateAsync(teacher);
        return RedirectToAction("ViewAll");
    }

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public async Task<IActionResult> Delete(int id) {
        Teacher teacher = await teachersService.GetById(id);
        await teachersService.DeleteAsync(teacher);

        return RedirectToAction("ViewAll");
    }

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public async Task<IActionResult> ViewAll() {
        IEnumerable<Teacher> teacherList = await teachersService.GetAllAsync();
        List<TeacherViewModel> teacherModelList = new();

        foreach (var teacher in teacherList) {
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

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public async Task<IActionResult> View(int id) {
        Teacher teacher = await teachersService.GetById(id);
        TeacherViewModel teacherModel = mapper.Map<TeacherViewModel>(teacher);
        return View(teacherModel);
    }
}