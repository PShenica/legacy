using System;
using ApprovalTests;
using ApprovalTests.Reporters;
using FakeItEasy;
using Newtonsoft.Json;
using NUnit.Framework;
using ProviderProcessing.ProcessReports;
using ProviderProcessing.ProviderDatas;
using ProviderProcessor.ProviderDatas;

namespace ProviderProcessor
{
    [UseReporter(typeof(DiffReporter))]
    public class ProviderProcessorTests
    {
        private IProviderRepository provider;
        private IProductValidator productValidator;
        private ProviderProcessing.ProviderProcessor processor;

        [SetUp]
        public void SetUp()
        {
            provider = A.Fake<IProviderRepository>();
            productValidator = A.Fake<IProductValidator>();
            processor = new ProviderProcessing.ProviderProcessor(provider, productValidator);
        }

        [Test]
        public void ProcessProviderData_ProviderUpdateMustHaveHappened_WhenCorrectData()
        {
            var providerData = new ProviderData {Products = new[] {new ProductData()}};
            var message = JsonConvert.SerializeObject(providerData);

            A.CallTo(() => provider.FindByProviderId(A<Guid>._)).Returns(providerData);
            processor.ProcessProviderData(message);

            A.CallTo(() => provider.Update(A<ProviderData>._)).MustHaveHappened();
        }

        [Test]
        public void ProcessProviderData_ReportFail_WhenOutdatedTimestamp()
        {
            var providerData1 = new ProviderData {Timestamp = new DateTime(2020, 12, 15)};
            var providerData2 = new ProviderData {Timestamp = new DateTime(2020, 12, 25)};
            var message = JsonConvert.SerializeObject(providerData1);
            A.CallTo(() => provider.FindByProviderId(A<Guid>._)).Returns(providerData2);

            var result = processor.ProcessProviderData(message);
            Approvals.Verify(result);
        }

        [Test]
        public void ProcessProviderData_ReportFail_WhenIncorrectProduct()
        {
            var providerData1 = new ProviderData {Products = new[] {new ProductData()}};
            var validationResult = new ProductValidationResult();

            var message = JsonConvert.SerializeObject(providerData1);
            A.CallTo(() => productValidator.ValidateProductData(new [] {A<ProductData>._})).Returns(validationResult);

            var result = processor.ProcessProviderData(message);
            Approvals.Verify(result);
        }
    }
}