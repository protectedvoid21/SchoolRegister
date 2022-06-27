using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.Models;

public class Grade {
    public int Id { get; set; }
    
    public StudentSubject StudentSubject { get; set; }
    
    public int SubjectId { get; set; }
    public Subject Subject { get; set; }

    [Range(0, 6)]
    public int GradeType { get; set; }
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
}