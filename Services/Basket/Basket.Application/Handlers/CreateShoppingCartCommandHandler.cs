using Basket.Application.Command;
using Basket.Application.Mappers;
using Basket.Application.Responses;
using Basket.Core.Repositories;
using MediatR;
using Basket.Core.Entities;
using Basket.Application.GrpcService;

namespace Basket.Application.Handlers
{
    public class CreateShoppingCartCommandHandler : IRequestHandler<CreateShoppingCartCommand, ShoppingCartResponse>
    {
        private readonly IBasketRepository _basketRepository;
        private readonly DiscountGrpcService _discountGrpcService;
        public CreateShoppingCartCommandHandler(IBasketRepository basketRepository, DiscountGrpcService discountGrpcService)
        {
            _basketRepository = basketRepository;
            _discountGrpcService = discountGrpcService;
        }
        public async Task<ShoppingCartResponse> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
        {
            foreach (var item in request.Items)
            {
                // Fetch discount for each item and apply it
                var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
                if (coupon != null && coupon.Amount > 0)
                {
                    item.Price -= coupon.Amount; // Apply discount to the item price
                }
            }

            var shoppingCart = await _basketRepository.UpdateBasket(new ShoppingCart {
                UserName = request.UserName,
                Items = request.Items            
            });
            var shoppingCartResponse = BasketMapper.Mapper.Map<ShoppingCartResponse>(shoppingCart);
            return shoppingCartResponse;
        }
    }
}
