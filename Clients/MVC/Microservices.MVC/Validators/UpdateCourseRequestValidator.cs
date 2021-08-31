using FluentValidation;
using Microservices.MVC.Models.Catalog;

namespace Microservices.MVC.Validators
{
    public class UpdateCourseRequestValidator : AbstractValidator<UpdateCourseRequest>
    {
        public UpdateCourseRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Kurs Numarası Boş Olamaz.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("İsim Alanı Boş Olamaz.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama Alanı Boş Olamaz.");
            RuleFor(x => x.Feature.Duration).InclusiveBetween(1, int.MaxValue).WithMessage("Süre Alanı Boş Olamaz.");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Fiyat Alanı Boş Olamaz").ScalePrecision(2, 6).WithMessage("Hatalı Para Formatı");
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Kategori Alanını Seçiniz");
        }
    }
}