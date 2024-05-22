using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.Slide;
using Domain;

namespace AnswerCube.UI.MVC.Models;

public class SlideModel
{
    public int Id { get; set; }
    public SlideType SlideType { get; set; }
    public string Text { get; set; } 
    public List<string>? AnswerList { get; set; }
    public string? MediaUrl { get; set; }
    public SlideList SlideList { get; set; }
    public SubTheme SubTheme { get; set; }
}