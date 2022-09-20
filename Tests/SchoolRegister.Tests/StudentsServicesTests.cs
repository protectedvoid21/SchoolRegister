using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using SchoolRegister.Models;
using SchoolRegister.Services.Students;
using SchoolRegister.Services.Subjects;
using SchoolRegister.Tests.Utils;
using Xunit;

namespace SchoolRegister.Tests;

public class StudentsServiceTests {
    [Theory]
    [InlineData("Joe", "Doe")]
    [InlineData("Adam", "Żółć")]
    [InlineData("Michael", "Gray")]
    public async Task Add_Students_ExistsInDb(string name, string surname) {
        var options = new DbContextOptionsBuilder<SchoolContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        var context = new SchoolContext(options);

        var userManagerMock = new Mock<FakeUserManager>();
        var subjectsServiceMock = new Mock<ISubjectsService>();
        var studentsService = new StudentsService(context, userManagerMock.Object, subjectsServiceMock.Object);

        await studentsService.AddAsync(name, surname, 0);
        
        var students = await studentsService.GetAllAsync();

        Assert.Single(students);
    }
}