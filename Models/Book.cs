using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TesteFoa.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Author Author { get; set; }
        [ForeignKey("AuthorId")]
        public Guid AuthorId { get; set; }
        public string Publisher { get; set; }
        public string ISBN { get; set; }
        public DateTime PublishDate { get; private set; }
    }
}
