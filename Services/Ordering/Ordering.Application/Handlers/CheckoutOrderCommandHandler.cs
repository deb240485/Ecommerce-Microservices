using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using System.Drawing;

namespace Ordering.Application.Handlers
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;
        public CheckoutOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<CheckoutOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
        
            //_logger.LogInformation("Checkout order started for user {UserName}", request.UserName);
            var orderEntity = _mapper.Map<Order>(request);
            var createdOrder = await _orderRepository.AddAsync(orderEntity);
            _logger.LogInformation($"Order with Id {createdOrder.Id} is successfully created.");
            return createdOrder.Id;

        }
    }
}
