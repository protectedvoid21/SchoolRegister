﻿using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.Models.ViewModels; 

public class CreateStudentViewModel {
    [Required]
    public string Name { get; set; }
    [Required]
    public string Surname { get; set; }
    [Required]
    public int SchoolClassId { get; set; }

    public IEnumerable<SchoolClass> SchoolClassList { get; set; }
}