using System.Collections;
using AnswerCube.BL.Domain.Slide;
using Domain;

namespace AnswerCube.UI.MVC.Models;

public class SlideModel
{
    //SlideType slideType, string text, ICollection<Answer> answers, int slide_id, int slideListId
    public SlideType SlideType { get; set; }
    public string Text { get; set; }
    public ICollection<Answer> Answers { get; set; }
    public int slideId { get; set; }
    public int slideListId { get; set; }
}