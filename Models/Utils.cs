namespace SchoolRegister.Models;

public static class Utils {
    public static string GeneratePassword(int length, string availableCharacters = "abcdefghijklmnoprstxuwyzABCDEFGHIJKLMNOPRSTXUWYZ0123456789") {
        if (length <= 0) {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        string password = string.Empty;
        Random rng = new();
        for (int i = 0; i < length; i++) {
            password += availableCharacters[rng.Next(availableCharacters.Length)];
        }

        password = char.ToUpper(password[0]) + password.Substring(1);

        return password;
    }

    public static string GenerateUserName(string name, string surname) {
        return (name + surname).ToLower() + GenerateRandomNumberText(4);
    }

    public static string GenerateRandomNumberText(int length) {
        if (length <= 0) {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        Random rng = new();
        string text = string.Empty;
        for (int i = 0; i < length; i++) {
            text += rng.Next(10).ToString();
        }

        return text;
    }
}