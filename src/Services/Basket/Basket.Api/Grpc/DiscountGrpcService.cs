using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discount.Grpc.Protos;

namespace Basket.Api.Grpc
{
    public class DiscountGrpcService: IDiscountGrpcService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;

        public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient )
        {
            _discountProtoServiceClient = discountProtoServiceClient;
        }

        public async Task<CouponModel> GetDiscount(string productName)
        {
            var discountRequest = await _discountProtoServiceClient.GetDiscountAsync(new GetDiscountRequest
            {
                ProductName = productName
            });

            return discountRequest;
        }
    }

    public interface IDiscountGrpcService
    {
        Task<CouponModel> GetDiscount(string productName);
    }
}
