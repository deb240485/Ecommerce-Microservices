using Asp.Versioning;
using Basket.Application.Command;
using Basket.Application.Mappers;
using Basket.Application.Queries;
using Basket.Core.Entities;
using EventBus.Messages.Common;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers.V2
{
    [ApiVersion("2")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class BasketController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<BasketController> _logger;
        public BasketController(IMediator mediator, IPublishEndpoint publishEndpoint, ILogger<BasketController> logger)
        {
           _mediator = mediator;
           _publishEndpoint = publishEndpoint;
           _logger = logger; 
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckoutV2 basketCheckout)
        {
            //1.Get the Existing basket with username.

            var query = new GetBasketByUserNameQuery(basketCheckout.UserName);
            var basket = await _mediator.Send(query);
            if (basket == null)
            {
                return BadRequest();
            }
            //2.Event Message Creation.
            var eventMsg = BasketMapper.Mapper.Map<BasketCheckoutEventV2>(basketCheckout);
            eventMsg.TotalPrice = basket.TotalPrice;
            //eventMsg.CorrelationId = 
            await _publishEndpoint.Publish(eventMsg);
            _logger.LogInformation($"Basket published for {basket.UserName} with V2 endpoint.");

            //3.Remove the basket once the order is created.
            var deleteCmd = new DeleteBasketByUserNameCommand(basketCheckout.UserName);
            await _mediator.Send(deleteCmd);
            return Accepted();
        }
    }
}
