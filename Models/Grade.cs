using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.Models; 

public class Grade {
    public int Id { get; set; }
    public int StudentId { get; set; }
    public Student Student { get; set; }

    [Range(0, 6)]
    public int GradeType { get; set; }
    public GradeAdditionalInfo GradeInfo { get; set; }
    public DateTime DateAdd { get; set; }
    public Subject Subject { get; set; }

    public string GetGradeName() {
        if (GradeInfo == GradeAdditionalInfo.Plus) {
            return $"{GradeType}+";
        }
        return $"-{GradeType}";
    }
}