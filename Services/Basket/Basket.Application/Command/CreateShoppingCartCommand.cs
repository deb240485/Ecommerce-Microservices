using Basket.Application.Responses;
using Basket.Core.Entities;
using MediatR;

namespace Basket.Application.Command
{
    public class CreateShoppingCartCommand : IRequest<ShoppingCartResponse>
    {
        public string UserName { get; set; }
        public List<ShoppingCartItem> Items { get; set; }
        public CreateShoppingCartCommand(string username, List<ShoppingCartItem> items)
        {
            UserName = username;
            Items = items;
        }
    }
}
