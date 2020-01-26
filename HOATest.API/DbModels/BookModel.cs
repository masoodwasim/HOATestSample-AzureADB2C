using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HOATest.API.DbModels
{
    public class BookModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string PublishingCompany { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
    }
}
