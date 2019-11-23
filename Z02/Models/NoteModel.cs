using System;
using System.Collections.Generic;

namespace Z01.Models{
    public class NoteModel{
        public NoteModel (String title, List<String> categories, DateTime date, String content = "",
                     Boolean isMarkdown = false){
            this.Title = title;
            this.Categories = categories;
            this.Date = date;
            this.Content = content;
            this.isMarkdown = isMarkdown;
        }

        private String Title { get; set; }
        private List<String> Categories { get; set; }
        private DateTime Date { get; set; }
        private String Content { get; set; }
        private Boolean isMarkdown { get; set; }
    }
}