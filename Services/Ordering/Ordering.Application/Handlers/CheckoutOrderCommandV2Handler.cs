using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers
{
    public class CheckoutOrderCommandV2Handler : IRequestHandler<CheckoutOrderCommandV2, int>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CheckoutOrderCommandV2Handler> _logger;
        public CheckoutOrderCommandV2Handler(IOrderRepository orderRepository, IMapper mapper, ILogger<CheckoutOrderCommandV2Handler> logger)
        {
           _logger = logger;
           _orderRepository = orderRepository;
           _mapper = mapper;
        }

        public async Task<int> Handle(CheckoutOrderCommandV2 request, CancellationToken cancellationToken)
        {
            //_logger.LogInformation("Checkout order started for user {UserName}", request.UserName);
            var orderEntity = _mapper.Map<Order>(request);
            var createdOrder = await _orderRepository.AddAsync(orderEntity);
            _logger.LogInformation($"Order with Id {createdOrder.Id} is successfully created.");
            return createdOrder.Id;
        }
    }
}
