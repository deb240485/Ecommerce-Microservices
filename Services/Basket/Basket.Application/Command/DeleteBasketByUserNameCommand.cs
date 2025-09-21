using MediatR;

namespace Basket.Application.Command
{
    public class DeleteBasketByUserNameCommand : IRequest<Unit>
    {
        public string UserName { get; set; }

        public DeleteBasketByUserNameCommand(string userName)
        {
            UserName = userName;
        }
    }
}
