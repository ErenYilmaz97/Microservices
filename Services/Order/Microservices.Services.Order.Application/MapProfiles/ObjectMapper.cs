using System;
using AutoMapper;

namespace Microservices.Services.Order.Application.MapProfiles
{
    public static class ObjectMapper
    {
        private static readonly Lazy<IMapper> mapper = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CustomMapperProfile>();
            });

            return config.CreateMapper();
        });



        //BU PROPU ÇAĞIRDIĞIMIZDA, VALUE PROPU TETİKLENEREK, YUKARIDAKİ MAPPER FİELDINA DEĞER ATANMASI SAĞLANACAK.
        //PROGRAM AYAĞA KALKTIĞINDA, STATİC OLMASINA RAĞMEN YUKARI KISIM ÇALIŞMAYACAK, ÇÜNKÜ LAZY OLARAK TANIMLANDI.
        public static IMapper Mapper
        {
            get => mapper.Value;
        }
    }
}