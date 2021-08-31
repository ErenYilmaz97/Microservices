using FluentValidation;
using Microservices.MVC.Models.Discount;

namespace Microservices.MVC.Validators
{
    public class DiscountApplyRequestValidator : AbstractValidator<DiscountApplyRequest>
    {
        public DiscountApplyRequestValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithMessage("İndirim Kodu Boş Olamaz.");
        }   
    }
}