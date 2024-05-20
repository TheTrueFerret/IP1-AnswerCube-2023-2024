using System.Collections;
using AnswerCube.BL.Domain.Slide;
using Domain;

namespace AnswerCube.UI.MVC.Models;

public class SlideModel
{
    public int Id { get; set; }
    public SlideType SlideType { get; set; }
    public string Text { get; set; }
    public ICollection<Answer> Answers { get; set; }
    public int slideListId { get; set; }
}