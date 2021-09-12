using System.Threading.Tasks;
using Dapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Settings;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Discount.Grpc.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly DiscountSettings settings;
        public DiscountRepository(IOptions<DiscountSettings> options)
        {
            settings = options.Value;
        }
        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(settings.ConnectionString);
            var affected =
                await connection.ExecuteAsync
                    ("INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                            new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(settings.ConnectionString);

            var affected = await connection.ExecuteAsync("DELETE FROM Coupon WHERE ProductName = @ProductName",
                new { ProductName = productName });

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(settings.ConnectionString);
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
                ("select * from Coupon where ProductName = @ProductName", new { ProductName = productName});
            if(coupon == null)
            {
                return new Coupon { Amount = 0, Description = "No Discount", ProductName = "No Discount" };
            }
            return coupon;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(settings.ConnectionString);
            var affected = await connection.ExecuteAsync
                    ("UPDATE Coupon SET ProductName=@ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",
                            new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, Id = coupon.Id });

            if (affected == 0)
                return false;

            return true;
        }
    }
}
