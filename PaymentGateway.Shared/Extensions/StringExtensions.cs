namespace PaymentGateway.Shared.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Masks a credit card number to only return the first and last 4 digits
        /// </summary>
        /// <param name="input">The credit card number.</param>
        /// <returns>The masked credit card number.</returns>
        public static string MaskCreditCard(this string input)
        {
            input = input.Replace("-", string.Empty);
            string first = input.Substring(0, 4);

            int inputLength = input.Length;
            string last = input.Substring(inputLength - 4, 4);
            
            return $"{first}-XXXX-XXXX-{last}";
        }
    }
}
