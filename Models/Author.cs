using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TesteFoa.Models
{
    public class Author
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime CreateAt { get; private set; }
        public IEnumerable<Book> Books { get; set; }

        public Author()
        {
            CreateAt = DateTime.Now.AddDays(1);
        }
    }
}
