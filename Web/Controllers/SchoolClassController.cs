using Data.Models;
using Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Services.SchoolClasses;
using Services.Subjects;

namespace SchoolRegister.Controllers;

public class SchoolClassController : Controller {
    private readonly ISchoolClassesService schoolClassesService;
    private readonly ISubjectsService subjectsService;

    public SchoolClassController(ISchoolClassesService schoolClassesService, ISubjectsService subjectsService) {
        this.schoolClassesService = schoolClassesService;
        this.subjectsService = subjectsService;
    }

    [HttpGet]
    public ViewResult Add() {
        var schoolClassModel = new SchoolClassViewModel();
        return View(schoolClassModel);
    }

    [HttpPost]
    public async Task<IActionResult> Add(SchoolClassViewModel schoolClassModel) {
        if (!ModelState.IsValid || await schoolClassesService.IsSchoolClassExisting(schoolClassModel.Name)) {
            return View(schoolClassModel);
        }

        SchoolClass schoolClass = new() {
            Name = schoolClassModel.Name,
            StudentsList = new()
        };

        await schoolClassesService.AddAsync(schoolClass);
        return RedirectToAction("ViewAll");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id) {
        SchoolClass schoolClass = await schoolClassesService.GetById(id);
        return View(schoolClass);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(SchoolClass schoolClass) {
        if (!ModelState.IsValid) {
            return View(schoolClass);
        }

        if (await schoolClassesService.IsSchoolClassExisting(schoolClass.Name)) {
            ModelState.AddModelError("", "Class with this name already exists");
            return View(schoolClass);
        }

        await schoolClassesService.UpdateAsync(schoolClass);
        return RedirectToAction("ViewAll");
    }

    public async Task<IActionResult> Delete(int id) {
        await schoolClassesService.DeleteAsync(id);
        return RedirectToAction("ViewAll");
    }

    public async Task<IActionResult> ViewAll() {
        IEnumerable<SchoolClass> schoolClassList = await schoolClassesService.GetAllAsync();
        return View(schoolClassList);
    }

    public async Task<IActionResult> View(int id) {
        SchoolClass schoolClass = await schoolClassesService.GetById(id);
        return View(schoolClass);
    }

    public async Task<IActionResult> ClassSubjectView(int schoolClassId) {
        ViewBag.ClassName = (await schoolClassesService.GetById(schoolClassId)).Name;

        IEnumerable<Subject> subjects = (await subjectsService.GetAllSchoolSubjects())
            .Where(s => s.SchoolClass.Id == schoolClassId)
            .Select(s => s.Subject);
        return View(subjects);
    }
}