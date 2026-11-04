using WMS.Application.Interfaces;
using WMS.Core.Common;
using WMS.Core.Entities;

namespace WMS.Application.common.Helper
{
    public static class VerifyStockHelper
    {
        private static IUnitOfWorks _unitOfWorks;
        private static async Task<bool> LocationExistsAsync(int locationId, CancellationToken cancellationToken)
        {
            var result = await _unitOfWorks.Location.GetByIdAsync(locationId, cancellationToken);
            return result is not null;
        }

        private static async Task<bool> ProductExistsAsync(int productId, CancellationToken cancellationToken)
        {
            var result = await _unitOfWorks.Product.GetByIdAsync(productId, cancellationToken);
            return result is not null;
        }

        public static async Task<Error?> VerifyAsync(
            IUnitOfWorks unitOfWorks,
            int locationId,
            int productId,
            CancellationToken ct)
        {
            _unitOfWorks = unitOfWorks;

            if (!await ProductExistsAsync(productId, ct))
            {
                return Errors<Product>.ObjectNotExists(productId);
            }
            if (!await LocationExistsAsync(locationId, ct))
            {
                return Errors<Location>.ObjectNotExists(locationId);
            }
            return null;
        }
    }
}