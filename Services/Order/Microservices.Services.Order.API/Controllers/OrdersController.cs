using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microservices.Services.Order.Application.Commands;
using Microservices.Services.Order.Application.Queries;
using Microservices.Shared.ControllerBase;
using Microservices.Shared.Services;

namespace Microservices.Services.Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : CustomControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ISharedIdentityService _sharedIdentityService;


        public OrdersController(IMediator mediator, ISharedIdentityService sharedIdentityService)
        {
            _mediator = mediator;
            _sharedIdentityService = sharedIdentityService;
        }



        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var response = await _mediator.Send(new GetOrdersByUserIdQuery(){UserId = _sharedIdentityService.GetCurrentUserId()});
            return CreateActionResultInstance(response);
        }


        [HttpPost]
        public async Task<IActionResult> SaveOrder(CreateOrderCommand request)
        {
            var response = await _mediator.Send(request);
            return CreateActionResultInstance(response);
        }
    }
}
