namespace Data.ViewModels.Teachers;

public class TeacherViewModel {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public int SubjectCount { get; set; }
    public int ClassCount { get; set; }
    public int SchoolSubjectCount { get; set; }
    public List<TeachingClassModel> TeachingClassList { get; set; } = new();
}

public struct TeachingClassModel {
    public string ClassName { get; set; }
    public string SubjectName { get; set; }
}