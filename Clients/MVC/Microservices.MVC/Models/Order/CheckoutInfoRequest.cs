using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.MVC.Models.Order
{
    public class CheckoutInfoRequest
    {
        [Display(Name = "İl")]
        public string Province { get; set; }

        [Display(Name = "İlçe")]
        public string District { get; set; }

        [Display(Name = "Cadde")]
        public string Street { get; set; }

        [Display(Name = "Posta Kodu")]
        public string ZipCode { get; set; }

        [Display(Name = "Adres")]
        public string Line { get; set; }


        [Display(Name = "Kartın Üzerindeki İsim")]
        public string CardName { get; set; }

        [Display(Name = "Kart Numarası")]
        public string CardNumber { get; set; }

        [Display(Name = "Son Kullanma Tarihi (Ay/Yıl)")]
        public string Expiration { get; set; }

        [Display(Name = "CVV Numarası")]
        public string CVV { get; set; }
        public string TotalPrice { get; set; }
    }
}
