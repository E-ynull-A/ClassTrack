namespace ClassTrack.MVC.ViewModels
{
    public record RegisterVM(string Name,
        string Surname,
        string UserName,
        string Email,
        string Password,
        string ConfirmPassword,
        byte Age,
        DateOnly BirthDate);

}
