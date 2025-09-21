using Asp.Versioning;
using Basket.Application.Command;
using Basket.Application.GrpcService;
using Basket.Application.Mappers;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Entities;
using EventBus.Messages.Common;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    [ApiVersion("1")]
    [Authorize]
    public class BasketController : ApiController
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

        [HttpGet]
        [Route("[action]/{userName}", Name = "GetBasketByUserName")]
        [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCartResponse>> GetBasket(string userName)
        {
            _logger.LogInformation($"GetBasket called for user {userName}");
            var query = new GetBasketByUserNameQuery(userName);
            var basketResponse = await _mediator.Send(query);
            return Ok(basketResponse);
        }

        [HttpPost("CreateBasket")]
        [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCartResponse>> UpdateBasket([FromBody] CreateShoppingCartCommand createShoppingCartCommand)
        {
            _logger.LogInformation($"CreateBasket called for user {createShoppingCartCommand.UserName}");
            var basket = await _mediator.Send(createShoppingCartCommand);
            return Ok(basket);
        }

        [HttpDelete]
        [Route("[action]/{userName}", Name = "DeleteBasketByUserName")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteBasket(string userName)
        {
            _logger.LogInformation($"DeleteBasket called for user {userName}");
            var command = new DeleteBasketByUserNameCommand(userName);
            await _mediator.Send(command);
            return Ok();
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            //1.Get the Existing basket with username.

            var query = new GetBasketByUserNameQuery(basketCheckout.UserName);
            var basket = await _mediator.Send(query);
            if(basket == null)
            {
                return BadRequest();
            }
            //2.Event Message Creation.
            var eventMsg = BasketMapper.Mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMsg.TotalPrice = basket.TotalPrice;
            //eventMsg.CorrelationId = 
            await _publishEndpoint.Publish(eventMsg);
            _logger.LogInformation($"Basket published for {basket.UserName}");

            //3.Remove the basket once the order is created.
            var deleteCmd = new DeleteBasketByUserNameCommand(basketCheckout.UserName);
            await _mediator.Send(deleteCmd);
            return Accepted();
        }

    }
}
