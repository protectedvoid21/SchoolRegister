using System.ComponentModel;
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

        IEnumerable<StudentSubject> studentSubjects = await subjectsService.GetStudentSubjectsForTeacher(teacher.Id);
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
            StudentName = student.User.Name,
            StudentSurname = student.User.Surname,
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

    [HttpGet]
    public async Task<IActionResult> EditGrade(int gradeId) {
        Grade grade = await gradesService.GetById(gradeId);
        Student student = grade.StudentSubject.Student;

        GradeEditViewModel gradeModel = new() {
            Id = gradeId,
            GradeType = grade.GradeType,
            GradeInfo = grade.GradeInfo,
            StudentName = student.User.Name,
            StudentSurname = student.User.Surname,
            SubjectName = grade.Subject.Name,
            Comment = grade.Comment,
        };

        return View(gradeModel);
    }

    [HttpPost]
    public async Task<IActionResult> EditGrade(GradeEditViewModel gradeModel) {
        if (!ModelState.IsValid) {
            return View(gradeModel);
        }

        Grade grade = await gradesService.GetById(gradeModel.Id);
        grade.GradeType = gradeModel.GradeType;
        grade.GradeInfo = gradeModel.GradeInfo;
        grade.Comment = grade.Comment;

        await gradesService.UpdateAsync(grade);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> DeleteGrade(int gradeId) {
        Grade grade = await gradesService.GetById(gradeId);
        await gradesService.DeleteAsync(grade);
        return RedirectToAction("Index");
    }
}