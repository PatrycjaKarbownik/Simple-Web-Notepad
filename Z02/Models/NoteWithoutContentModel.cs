using System;
using System.Collections.Generic;

namespace Z01.Models{
    public class NoteWithoutContentModel{
        public DateTime Date { get; set; }
        public String Title { get; set; }
        public List<String> Categories { get; set; }
    }
}