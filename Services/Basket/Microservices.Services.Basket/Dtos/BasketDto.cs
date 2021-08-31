using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;

namespace Microservices.Services.Basket.Dtos
{
    public class BasketDto
    {
        public string UserId { get; set; }  //SEPET KİME AİT?
        public string DiscountCode { get; set; }
        public int? DiscountRate { get; set; }

        //SET EDİLEMEZ. OTOMATIK SEPETTEKİ ÜRÜNLERİN TOPLAMI
        public decimal TotalPrice { get => BasketItems.Sum(x=>x.Price * x.Quantity);}

        public List<BasketItemDto> BasketItems { get; set; } 
    }
}