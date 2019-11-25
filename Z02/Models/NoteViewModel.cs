using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Z02.Models{
    public class NoteViewModel{
        public int Id { get; set; }
        
        public byte[] RowVersion { get; set; }

        [Required (ErrorMessage = "Title is required")]
        public String Title { get; set; }

        public List<String> Categories { get; set; } = new List<string> ();
        [DataType (DataType.Date)] public DateTime Date { get; set; }
        public String Content { get; set; }
        public Boolean IsMarkdown { get; set; }

        public NoteViewModel (){
            Id = -1;
            Date = DateTime.Now;
        }
    }
}