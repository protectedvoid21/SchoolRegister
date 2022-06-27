using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolRegister.Models;
using SchoolRegister.Models.ViewModels;
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

    public TeacherController(ITeachersService teachersService, 
        ISubjectsService subjectsService, 
        ISchoolClassesService schoolClassesService,
        IGradesService gradesService,
        UserManager<AppUser> userManager) {
        this.teachersService = teachersService;
        this.subjectsService = subjectsService;
        this.schoolClassesService = schoolClassesService;
        this.gradesService = gradesService;
        this.userManager = userManager;
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

        IEnumerable<StudentSubject> studentSubjects = await subjectsService.GetSchoolSubjectsByTeacher(teacher);
        var schoolClass = await schoolClassesService.GetById(classId);

        studentSubjectModel.StudentSubjects = studentSubjects.Where(s => s.SchoolSubject.Subject.Id == subjectId && s.Student.SchoolClass == schoolClass);
        studentSubjectModel.SubjectName = (await subjectsService.GetById(subjectId)).Name;
        studentSubjectModel.ClassName = schoolClass.Name;

        return View(studentSubjectModel);
    }

    [HttpGet]
    public async Task<IActionResult> AddGrade(int studentSubjectId) {
        StudentSubject studentSubject = await subjectsService.GetStudentSubjectById(studentSubjectId);
        Student student = studentSubject.Student;

        GradeCreateViewModel gradeModel = new() {
            StudentName = student.Name,
            StudentSurname = student.Surname,
            SubjectName = studentSubject.SchoolSubject.Subject.Name,
            SubjectId = studentSubject.SchoolSubject.Subject.Id
        };
        return View(gradeModel);
    }

    [HttpPost]
    public async Task<IActionResult> AddGrade(GradeCreateViewModel gradeModel) {
        if (!ModelState.IsValid) {
            return View(gradeModel);
        }

        Grade grade = new() {
            StudentSubject = await subjectsService.GetStudentSubjectById(gradeModel.StudentSubjectId),
            Subject = await subjectsService.GetById(gradeModel.SubjectId),
            SubjectId = gradeModel.SubjectId,
            DateAdd = DateTime.Now,
            GradeType = gradeModel.GradeType,
            GradeInfo = gradeModel.GradeInfo,
            Comment = gradeModel.Comment,
        };

        await gradesService.AddAsync(grade);
        //todo: Return to class view
        return RedirectToAction("Index");
    }
}