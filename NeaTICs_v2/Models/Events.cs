using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NeaTICs_v2.Models
{
    public class Events
    {
        public int ID { get; set; }
        public Users Owner { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string Place { get; set; }
        public byte[] Image { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageToUpload { get; set; }
        [NotMapped]
        public string ImageToRead { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        [Required]
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public string Url { get; set; }
    }
}