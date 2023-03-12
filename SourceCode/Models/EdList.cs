using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using SourceCode.Models;

namespace SourceCode.Models;

[NotMapped]
public class EdList
{
    public SelectListItem[] list { get; set;}

    public EdList()
    {
        list = new SelectListItem[]
        {
            new SelectListItem ("1st Pri", "1st Pri"),
            new SelectListItem ("2nd Pri", "2nd Pri"),
            new SelectListItem ("3rd Pri", "3rd Pri"),
            new SelectListItem ("4th Pri", "4th Pri"),
            new SelectListItem ("5th Pri", "5th Pri"),
            new SelectListItem ("6th Pri", "6th Pri"),
            new SelectListItem ("1st Pre", "1st Pre"),
            new SelectListItem ("2nd Pre", "2nd Pre"),
            new SelectListItem ("3rd Pre", "3rd Pre"),
            new SelectListItem ("1st Sec", "1st Sec"),
            new SelectListItem ("2nd Sec", "2nd Sec"),
            new SelectListItem ("3rd Sec", "3rd Sec")
        };
    }

    public EdList(string email)
    {
        var db = new LmsdbContext();
        var lec = db.LecturerTbls.First(m => m.Email == email);
        var courses = from c in db.CourseTbls
                    where c.Teacher == lec.Id
                    select c;

        var listList = new List<SelectListItem>();
        foreach(var c in courses){
            if(c != null)
                listList.Add(new SelectListItem(c.EdLevel != null? c.EdLevel : "", c.EdLevel != null? c.EdLevel : ""));
        }
        list = listList != null? listList.ToArray() : 
        new SelectListItem[]
        {
            new SelectListItem ("1st Pri", "1st Pri"),
            new SelectListItem ("2nd Pri", "2nd Pri"),
            new SelectListItem ("3rd Pri", "3rd Pri"),
            new SelectListItem ("4th Pri", "4th Pri"),
            new SelectListItem ("5th Pri", "5th Pri"),
            new SelectListItem ("6th Pri", "6th Pri"),
            new SelectListItem ("1st Pre", "1st Pre"),
            new SelectListItem ("2nd Pre", "2nd Pre"),
            new SelectListItem ("3rd Pre", "3rd Pre"),
            new SelectListItem ("1st Sec", "1st Sec"),
            new SelectListItem ("2nd Sec", "2nd Sec"),
            new SelectListItem ("3rd Sec", "3rd Sec")
        };
    }
}