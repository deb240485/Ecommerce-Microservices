using Basket.Application.Responses;
using MediatR;

namespace Basket.Application.Queries
{
    public class GetBasketByUserNameQuery : IRequest<ShoppingCartResponse>
    {
        public readonly string UserName;
        public GetBasketByUserNameQuery(string userName)
        {
            UserName = userName;
        }
    }
}
