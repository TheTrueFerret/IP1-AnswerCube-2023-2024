namespace AnswerCube.UI.MVC.Controllers.DTO_s;

public class ContactInfoDto
{
    public string Name { get; set; }
    public string Email { get; set; }

    public ContactInfoDto()
    {
    }

    public ContactInfoDto(string name, string email)
    {
        Name = name;
        Email = email;
    }
}