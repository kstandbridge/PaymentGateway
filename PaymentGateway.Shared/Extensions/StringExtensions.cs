namespace PaymentGateway.Shared.Extensions
{
    public static class StringExtensions
    {
        public static string Mask(this string input)
        {
            input = input.Replace("-", string.Empty);
            string first = input.Substring(0, 4);

            int inputLength = input.Length;
            string last = input.Substring(inputLength - 4, 4);
            
            return $"{first}-XXXX-XXXX-{last}";
        }
    }
}
