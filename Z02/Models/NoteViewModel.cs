using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Z01.Repositories;

namespace Z01.Models{
    public class NoteViewModel{
        [Required(ErrorMessage = "Title is required")]
        public String Title { get; set; }
        public List<String> Categories { get; set; } = new List<string> ();
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public String Content { get; set; }
        public Boolean IsMarkdown { get; set; }
        public Boolean IsEdit { get; set; }

        public NoteViewModel (){ Date = DateTime.Now; }
    }
}