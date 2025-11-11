namespace InputLayer.Common.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNotNullOrWhiteSpace(this string value)
            => !string.IsNullOrWhiteSpace(value);
    }
}