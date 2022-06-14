using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TesteFoa.ViewModels
{
    public class BookViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string Publisher { get; set; }
        public string ISBN { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
