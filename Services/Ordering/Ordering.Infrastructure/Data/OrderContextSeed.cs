using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation($"Ordering Database:{typeof(OrderContext).Name} seeded.");
            }
        }

        //private static Order[] GetOrders()
        //{
        //    throw new NotImplementedException();
        //}

        private static IEnumerable<Order> GetOrders()
        {
            return new List<Order>
            {
                new()
                {
                    UserName = "benspark",
                    FirstName = "ben",
                    LastName = "spark",
                    EmailAddress = "deb240485rana@gmail.com",
                    AddressLine = "Bangalore",
                    Country = "India",
                    TotalPrice = 750,
                    State = "KA",
                    ZipCode = "560001",

                    CardName = "Visa",
                    CardNumber = "1234567890123456",
                    CreatedBy = "benspark",
                    Expiration = "12/25",
                    Cvv = "123",
                    PaymentMethod = 1,
                    LastModifiedBy = "benspark",
                    LastModifiedDate = new DateTime(),
                }
            };
        }
    }
}
