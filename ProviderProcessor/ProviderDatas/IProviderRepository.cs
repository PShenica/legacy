using System;
using ProviderProcessing.ProviderDatas;

namespace ProviderProcessor.ProviderDatas
{
    public interface IProviderRepository
    {
        public ProviderData FindByProviderId(Guid providerId);
        public void RemoveById(Guid id);
        public void Save(ProviderData data);
        public void Update(ProviderData existingData);
    }
}