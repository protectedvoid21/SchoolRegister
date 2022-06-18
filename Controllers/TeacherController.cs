using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolRegister.Models;
using SchoolRegister.Models.ViewModels;
using SchoolRegister.Services.Subjects;
using SchoolRegister.Services.Teachers;

namespace SchoolRegister.Controllers;

[Authorize(Roles = "Teacher")]
public class TeacherController : Controller {
    private readonly ITeachersService teachersService;
    private readonly ISubjectsService subjectsService;
    private readonly UserManager<AppUser> userManager;

    public TeacherController(ITeachersService teachersService, ISubjectsService subjectsService, UserManager<AppUser> userManager) {
        this.teachersService = teachersService;
        this.subjectsService = subjectsService;
        this.userManager = userManager;
    }

    public async Task<IActionResult> Index() {
        AppUser user = await userManager.GetUserAsync(User);
        Teacher teacher = await teachersService.GetByUser(user);
        return View(teacher);
    }

    public async Task<IActionResult> TeachingSubjectsView() {
        AppUser user = await userManager.GetUserAsync(User);
        Teacher teacher = await teachersService.GetByUser(user); 

        IEnumerable<SchoolSubject> schoolSubjects = await teachersService.GetTaughtSubjects(teacher);
        var groupedSubjects = schoolSubjects.GroupBy(s => s.SchoolClass).AsEnumerable();

        List<TeacherSubjectViewModel> teacherSubjectsModelList = new();

        foreach (var group in groupedSubjects) {
            teacherSubjectsModelList.Add(new TeacherSubjectViewModel {
                SchoolClass = group.Key,
                Subject = group.Select(s => s.Subject).ToList()
            });
        }

        return View(teacherSubjectsModelList);
    }

    public async Task<IActionResult> ViewSubject(int subjectId) {
        AppUser user = await userManager.GetUserAsync(User);
        Teacher teacher = await teachersService.GetByUser(user);
        IEnumerable<StudentSubject> studentSubjects = await subjectsService.GetSchoolSubjectsByTeacher(teacher);
        //todo: change it to certain class view
        return View(studentSubjects);
    }
}