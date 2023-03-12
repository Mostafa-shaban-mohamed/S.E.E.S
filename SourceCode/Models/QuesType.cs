using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SourceCode.Models;

[NotMapped]
public class QuesType
{
    public SelectListItem[] list { get; set;}

    public QuesType()
    {
        list = new SelectListItem[]
        {
            new SelectListItem ("MultiChoice", "MultiChoice"),
            new SelectListItem ("Written", "Written")
        };
    }
}