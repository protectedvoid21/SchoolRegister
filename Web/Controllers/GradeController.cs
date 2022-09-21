using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolRegister.Models;
using SchoolRegister.Models.ViewModels.Grades;
using SchoolRegister.Services.Grades;
using SchoolRegister.Services.Students;
using SchoolRegister.Services.Teachers;

namespace SchoolRegister.Controllers;

[Authorize(Roles = "Teacher")]
public class GradeController : Controller {
    private readonly IGradesService gradesService;
    private readonly ITeachersService teachersService;
    private readonly IStudentsService studentsService;
    private readonly UserManager<AppUser> userManager;

    public GradeController(IGradesService gradesService, ITeachersService teachersService, IStudentsService studentsService, UserManager<AppUser> userManager) {
        this.gradesService = gradesService;
        this.teachersService = teachersService;
        this.studentsService = studentsService;
        this.userManager = userManager; 
    }

    [Authorize(Roles = "Student,Teacher")]
    public async Task<IActionResult> View(int gradeId) {
        var user = await userManager.GetUserAsync(User);
        if(User.IsInRole("Teacher")) {
            Teacher teacher = await teachersService.GetByUser(user);
            if(!await teachersService.IsTeacherGradeAuthor(teacher.Id, gradeId)) {
                return Forbid();
            }
        }
        else if(User.IsInRole("Student")) {
            Student student = await studentsService.GetByUser(user);
            if(!await gradesService.IsOwner(gradeId, student.Id)) {
                return Forbid();
            }
        }
        else {
            return BadRequest();
        }

        GradeViewModel gradeModel = await gradesService.GetById<GradeViewModel>(gradeId);

        return View(gradeModel);
    }

    [HttpGet]
    public async Task<IActionResult> Add(int studentSubjectId) {
        GradeCreateViewModel gradeModel = await gradesService.GetById<GradeCreateViewModel>(studentSubjectId);
        return View(gradeModel);
    }

    [HttpPost]
    public async Task<IActionResult> Add(GradeCreateViewModel gradeModel) {
        if(!ModelState.IsValid) {
            return View(gradeModel);
        }

        await gradesService.AddAsync(gradeModel.SubjectId, gradeModel.StudentSubjectId, gradeModel.GradeType,
            gradeModel.GradeInfo, gradeModel.Comment);
        //todo: Return to class view
        return RedirectToAction("Index", "Teacher");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id) {
        GradeEditViewModel gradeModel = await gradesService.GetById<GradeEditViewModel>(id);

        return View(gradeModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(GradeEditViewModel gradeModel) {
        if(!ModelState.IsValid) {
            return View(gradeModel);
        }

        await gradesService.UpdateAsync(gradeModel.Id, gradeModel.GradeType, gradeModel.GradeInfo, gradeModel.Comment);
        return RedirectToAction("Index", "Teacher");
    }

    public async Task<IActionResult> Delete(int id) {
        await gradesService.DeleteAsync(id);
        return RedirectToAction("Index", "Teacher");
    }
}