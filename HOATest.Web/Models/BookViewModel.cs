using FluentValidation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace HOATest.Web.Models
{
    public class BookViewModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("publishingCompany")]
        [DisplayName("Publisher")]
        public string PublishingCompany { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }
    }
    public class BookValidator : AbstractValidator<BookViewModel>
    {
        public BookValidator()
        {
            RuleFor(login => login.Title).NotEmpty().WithMessage(login => string.Format("FirstName is empty"));
            RuleFor(login => login.Title).Length(0, 20); 
            RuleFor(login => login.Author).NotEmpty().WithMessage(login => string.Format("Email Id is empty"));
            RuleFor(login => login.PublishingCompany).NotEmpty().WithMessage(login => string.Format("Username is empty"));
            RuleFor(x => x.Price).InclusiveBetween(1, 1000);
        }
    }
}
