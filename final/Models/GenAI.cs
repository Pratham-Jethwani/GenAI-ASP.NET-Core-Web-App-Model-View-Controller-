
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
namespace final.Models
{
    public class GenAI
    {

        public int Id { get; set; }
        public string GenAIName { get; set; }
        public string Summary { get; set; }


       
        [Display(Name = "Image File")]

        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public string ImageFileName { get; set; }
        public string AnchorLink { get; set; }
        public int Like { get; set; }
    }
}
