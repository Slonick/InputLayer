using System.Text;

namespace InputLayer.Common.Extensions
{
    public static class StringBuilderExtensions
    {
        public static void Append2DigitsZeroPadded(this StringBuilder builder, int number)
        {
            builder.Append((char)(number / 10 + 48));
            builder.Append((char)(number % 10 + 48));
        }

        public static void Append4DigitsZeroPadded(this StringBuilder builder, int number)
        {
            builder.Append((char)(number / 1000 % 10 + 48));
            builder.Append((char)(number / 100 % 10 + 48));
            builder.Append((char)(number / 10 % 10 + 48));
            builder.Append((char)(number / 1 % 10 + 48));
        }
    }
}