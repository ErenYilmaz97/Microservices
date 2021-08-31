using System.ComponentModel.DataAnnotations;

namespace Microservices.MVC.Models.Catalog
{
    public class FeatureViewModel
    {
        [Display(Name = "Kurs Süresi")]
        public int Duration { get; set; }
    }
}