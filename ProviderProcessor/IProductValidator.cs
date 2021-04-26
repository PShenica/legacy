using System.Collections.Generic;
using ProviderProcessing.ProcessReports;
using ProviderProcessing.ProviderDatas;

namespace ProviderProcessor
{
    public interface IProductValidator
    {
        IEnumerable<ProductValidationResult> ValidateProductData(ProductData[] data);
    }
}