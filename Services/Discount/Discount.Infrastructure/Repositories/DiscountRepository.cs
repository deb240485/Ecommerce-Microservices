using Dapper;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Discount.Infrastructure.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;
        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<Coupon> GetDiscount(string productName)
        {
            await using var conn = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var coupon = await conn.QueryFirstOrDefaultAsync<Coupon>(
                "SELECT * FROM Coupon WHERE ProductName = @ProductName",
                new { ProductName = productName });
            if (coupon == null) { 
                return new Coupon
                {
                    ProductName = "No Discount",
                    Description = "No Discount Available",
                    Amount = 0
                };
            }
            return coupon;
        }

        public async Task<bool> CreateCouponAsync(Coupon coupon)
        {
            await using var conn = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var result = await conn.ExecuteAsync(
                "INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                new { coupon.ProductName, coupon.Description, coupon.Amount });
            return result > 0 ? true : false;
        }

        public async Task<bool> UpdateCouponAsync(Coupon coupon)
        {
            await using var conn = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var result = await conn.ExecuteAsync(
                "UPDATE Coupon SET Description = @Description, Amount = @Amount WHERE ProductName = @ProductName",
                new { coupon.Description, coupon.Amount, coupon.ProductName, coupon.Id });
            return result > 0 ? true : false;
        }

        public async Task<bool> DeleteCouponAsync(string productName)
        {
            await using var conn = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var result = await conn.ExecuteAsync(
                "DELETE FROM Coupon WHERE ProductName = @ProductName",
                new { ProductName = productName });
            return result > 0 ? true : false;
        }       

        
    }
}
