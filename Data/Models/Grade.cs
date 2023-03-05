using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public class Grade {
    public int Id { get; set; }

    public int StudentSubjectId { get; set; }

    public StudentSubject StudentSubject { get; set; }

    public int SubjectId { get; set; }

    public Subject Subject { get; set; }

    [Range(GlobalConstants.MinGrade, GlobalConstants.MaxGrade)] public int GradeType { get; set; }

    public GradeAdditionalInfo GradeInfo { get; set; }

    public DateTime DateAdd { get; set; }

    public string? Comment { get; set; }


    public string GetGradeName() {
        return GradeInfo switch {
            GradeAdditionalInfo.Plus => $"{GradeType}+",
            GradeAdditionalInfo.Minus => $"-{GradeType}",
            _ => GradeType.ToString()
        };
    }

    public float GetValue() {
        return GradeInfo switch {
            GradeAdditionalInfo.Plus => GradeType + 0.5f,
            GradeAdditionalInfo.Minus => GradeType - 0.25f,
            _ => GradeType
        };
    }
}