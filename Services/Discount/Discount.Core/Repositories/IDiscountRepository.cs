using Discount.Core.Entities;

namespace Discount.Core.Repositories
{
    public interface IDiscountRepository
    {
        Task<Coupon> GetDiscount(string productName);
        Task<bool> CreateCouponAsync(Coupon coupon);
        Task<bool> UpdateCouponAsync(Coupon coupon);
        Task<bool> DeleteCouponAsync(string productName);
    }
}
