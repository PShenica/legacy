using ProviderProcessing.ProviderDatas;

namespace ProviderProcessor
{
    public static class ProviderDataExtensions
    {
        public static string ToString(this ProviderData data)
        {
            return data != null
                ? data.Id + " for " + data.ProviderId + " products count " + data.Products.Length
                : "null";
        }
    }
}