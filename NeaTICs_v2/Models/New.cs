using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Web;

namespace NeaTICs_v2.Models
{
    public class New
    {
        public int ID { get; set; }
        
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        
        public byte[] Image { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageToUpload { get; set; }
        [NotMapped]
        public string ImageToRead { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }
        public Users Owner { get; set; }
        public List<Tag> Tags { get; set; }
        
        //De donde vienen las noticias
        public string Url { get; set; }
    }
}