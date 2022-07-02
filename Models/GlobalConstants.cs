namespace SchoolRegister.Models; 

public static class GlobalConstants {
    public const string AdministratorName = "Administrator";
    public const string AdministratorPassword = "Admin123";
    public const string AdministratorRoleName = "Admin";

    public static readonly string[] RequiredRoles = { "Admin", "Teacher", "Student" };

    public const int MinGrade = 0;
    public const int MaxGrade = 6;
}