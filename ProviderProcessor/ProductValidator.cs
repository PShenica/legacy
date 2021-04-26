using System.Collections.Generic;
using ProviderProcessing.ProcessReports;
using ProviderProcessing.ProviderDatas;
using ProviderProcessing.References;

namespace ProviderProcessor
{
    public class ProductValidator : IProductValidator
    {
        public IEnumerable<ProductValidationResult> ValidateProductData(ProductData[] products)
        {
            foreach (var product in products)
            {
                if (!NameIsValid(product))
                    yield return new ProductValidationResult(product, $"Unknown product name: {product.Name}",
                        ProductValidationSeverity.Error);

                if (!PriceIsValid(product))
                    yield return new ProductValidationResult(product, $"Price should not be negative, but was: {product.Price}",
                        ProductValidationSeverity.Warning);

                if (!MeasureUnitCodeIsValid(product))
                    yield return new ProductValidationResult(product, $"Bad units of measure: {product.MeasureUnitCode}",
                        ProductValidationSeverity.Warning);

                yield return new ProductValidationResult(product, "Correct product values", ProductValidationSeverity.Ok);
            }
        }

        private bool NameIsValid(ProductData product)
        {
            var reference = ProductsReference.GetInstance();
            return reference.FindCodeByName(product.Name).HasValue;
        }

        private bool PriceIsValid(ProductData product)
        {
            return product.Price <= 0;
        }

        private bool MeasureUnitCodeIsValid(ProductData product)
        {
            var reference = MeasureUnitsReference.GetInstance();
            return reference.FindByCode(product.MeasureUnitCode) != null;
        }
    }
}