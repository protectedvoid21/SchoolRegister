using Data.Models;
using Microsoft.EntityFrameworkCore;
using Services.Subjects;
using Xunit;

namespace Tests;

public class SubjectsServiceTests {
    private readonly SubjectsService subjectsService;

    public SubjectsServiceTests() {
        var options = new DbContextOptionsBuilder<SchoolContext>().UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var dbContext = new SchoolContext(options);
        subjectsService = new SubjectsService(dbContext);
    }

    [Fact]
    public async Task Add_NewSubject_ShouldBeExisting() {
        await subjectsService.AddAsync("SampleSubject1");
        await subjectsService.AddAsync("SampleSubject2");
        await subjectsService.AddAsync("SampleSubject3");

        int subjectCount = (await subjectsService.GetAllSubjects()).Count;

        Assert.Equal(3, subjectCount);
    }

    [Fact]
    public async Task Get_ById_ReturnsSubject() {
        await subjectsService.AddAsync("SampleSubject");

        var subjectList = await subjectsService.GetAllSubjects();
        var currentSubject = subjectList.First();

        Assert.True(currentSubject == await subjectsService.GetById(currentSubject.Id));
    }

    //[Fact]
    //public async Task Get_SchoolSubjects_ReturnsSchoolSubjectsInClass() {
    //    await subjectsService.AddSchoolSubjectAsync(subjectId: 1, schoolClassId: 1, teacherId: 1);
    //    await subjectsService.AddSchoolSubjectAsync(2, 1, 2);
    //    await subjectsService.AddSchoolSubjectAsync(3, 1, 2);
    //    await subjectsService.AddSchoolSubjectAsync(4, 1, 3);
    //    await subjectsService.AddSchoolSubjectAsync(1, 2, 1);
    //    await subjectsService.AddSchoolSubjectAsync(2, 2, 2);

    //    var schoolSubjectList = await subjectsService.GetSchoolSubjectsByClass(1);

    //    Assert.Equal(4, schoolSubjectList.Count());
    //}
}