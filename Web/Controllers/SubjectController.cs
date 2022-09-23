using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolRegister.Models;
using SchoolRegister.Models.ViewModels;
using SchoolRegister.Services.Subjects;

namespace SchoolRegister.Controllers;

[Authorize(Roles = GlobalConstants.AdministratorRoleName)]
public class SubjectController : Controller {
    private readonly ISubjectsService subjectsService;

    public SubjectController(ISubjectsService subjectsService) {
        this.subjectsService = subjectsService;
    }

    public async Task<IActionResult> ViewAll() {
        List<Subject> subjectList = await subjectsService.GetAllSubjects();
        List<SubjectElementViewModel> subjectListModel = new();
        foreach(var subject in subjectList) {
            subjectListModel.Add(new SubjectElementViewModel {
                Subject = subject,
                StudentCount = await subjectsService.GetCountByStudents(subject.Id)
            });
        }
        return View(subjectListModel);
    }

    [HttpGet]
    public ViewResult Add() {
        var subjectModel = new SubjectViewModel();
        return View(subjectModel);
    }

    [HttpPost]
    public async Task<IActionResult> Add(SubjectViewModel subjectModel) {
        if(!ModelState.IsValid) {
            return View(subjectModel);
        }

        if(await subjectsService.IsSubjectExisting(subjectModel.Name)) {
            ModelState.AddModelError("", "Subject with this name already exists");
            return View(subjectModel);
        }

        await subjectsService.AddAsync(subjectModel.Name);
        return RedirectToAction("ViewAll");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id) {
        Subject subject = await subjectsService.GetById(id);
        return View(subject);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Subject subject) {
        if(!ModelState.IsValid) {
            return View(subject);
        }
        await subjectsService.UpdateAsync(subject.Id, subject.Name);
        return RedirectToAction("ViewAll");
    }

    public async Task<IActionResult> Delete(int id) {
        await subjectsService.DeleteAsync(id);
        return RedirectToAction("ViewAll");
    }
}