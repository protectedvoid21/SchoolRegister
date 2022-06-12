namespace SchoolRegister.Models.ViewModels; 

public class TeacherViewModel {
    public string Name { get; set; }
    public string Surname { get; set; }
    public int SubjectCount { get; set; }
    public int ClassCount { get; set; }
    public List<TeachingClassModel> TeachingClassList { get; set; } = new();
}

public struct TeachingClassModel {
    public string ClassName { get; set; }
    public string SubjectName { get; set; }
}