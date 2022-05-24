namespace SchoolRegister.Models; 

public interface INoteService {
    public IEnumerable<int> GetStudentNotes(string studentId, Subject subject);
    public int GetStudentSubjectAverage(string studentId, Subject subject);
}