﻿using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.Models.ViewModels; 

public class StudentViewModel {
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Surname { get; set; }
}