using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolRegister.Models;
using SchoolRegister.Models.ViewModels;
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

    public StudentController(IStudentsService studentsService,
        IGradesService gradesService,
        ISubjectsService subjectsService,
        ITeachersService teachersService,
        UserManager<AppUser> userManager) {
        this.studentsService = studentsService;
        this.gradesService = gradesService;
        this.subjectsService = subjectsService;
        this.teachersService = teachersService;
        this.userManager = userManager;
    }

    [Authorize(Roles = "Student")]
    public async Task<IActionResult> Index() {
        AppUser user = await userManager.GetUserAsync(User);
        Student student = await studentsService.GetByUser(user);

        IEnumerable<StudentSubject> studentSubjects = await subjectsService.GetStudentSubjectForStudent(student);
        StudentPanelViewModel studentPanelModel = new() {
            Student = student,
            StudentSubjects = studentSubjects
        };
        return View(studentPanelModel);
    }

    [Authorize(Roles = "Student,Teacher")]
    public async Task<IActionResult> GradeView(int gradeId) {
        var user = await userManager.GetUserAsync(User);
        if (User.IsInRole("Teacher")) {
            Teacher teacher = await teachersService.GetByUser(user);

        }
        else if (User.IsInRole("Student")) {
            Student student = await studentsService.GetByUser(user);
            if (!await gradesService.IsOwner(gradeId, student.Id)) {
                return Forbid();
            }
        }
        else {
            return BadRequest();
        }

        Grade grade = await gradesService.GetById(gradeId);

        GradeViewModel gradeModel = new() {
            Id = gradeId,
            DateAdd = grade.DateAdd,
            GradeName = grade.GetGradeName(),
            SubjectName = grade.Subject.Name,
            Comment = grade.Comment,
        };

        return View(gradeModel);
    }
}