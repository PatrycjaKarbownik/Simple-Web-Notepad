using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Z02.Repositories;

namespace Z02.Models{
    public class NoteViewModel{
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public String Title { get; set; }
        public List<String> Categories { get; set; } = new List<string> ();
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public String Content { get; set; }
        public Boolean IsMarkdown { get; set; }
        public Boolean IsEdit { get; set; }

        public NoteViewModel (){
            Id = -1;
            Date = DateTime.Now;
        }
    }
}