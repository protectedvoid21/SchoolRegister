﻿using Data.Models;

namespace Data.ViewModels.Teachers;

public class TeacherStudentSubjectViewModel {
    public string ClassName { get; set; }
    public string SubjectName { get; set; }
    public IEnumerable<StudentSubject> StudentSubjects { get; set; }
}