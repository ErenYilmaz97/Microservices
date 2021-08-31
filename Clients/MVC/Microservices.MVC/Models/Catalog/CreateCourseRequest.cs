using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Microservices.MVC.Models.Catalog
{
    public class CreateCourseRequest
    {
        [Display(Name="Kurs Adı")]
        public string Name { get; set; }

        [Display(Name = "Kurs Açıklaması")]
        public string Description { get; set; }

        [Display(Name = "Kurs Fiyatı")]
        public decimal Price { get; set; }
        public IFormFile Photo { get; set; }
        public string Picture { get; set; }
        public string UserId { get; set; }
        public FeatureViewModel Feature { get; set; }

        [Display(Name = "Kategori")]
        public string CategoryId { get; set; }
    }
}