namespace Microservices.MVC.Models.Basket
{
    public class BasketItemViewModel
    {
        public int Quantity { get; set; }
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public decimal Price { get; set; }


        //İNDİRİM UYGULANMIŞSA İNDİRİMLİ FİYATI
        //HER BİR ÜRÜNÜN İNDİRİMLİ FİYATINI GÖREBİLMEK İÇİN OLUŞTURULDU
        private decimal? DiscountAppliedPrice { get; set; }


        public void AppliedDiscount(decimal discountPrice) => DiscountAppliedPrice = discountPrice;

        public decimal GetCurrentPrice
        {
            get => DiscountAppliedPrice != null ? DiscountAppliedPrice.Value : Price;
        }
    }
} 