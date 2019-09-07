using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace PaymentGateway.Tests.Controllers.PaymentsControllerTests
{
    public static class ActionResultExtensions
    {
        public static void AssertStatusCode(this IActionResult result, HttpStatusCode code)
        {
            AssertStatusCode(result, (int) code);
        }

        public static void AssertStatusCode(this IActionResult result, int statusCode)
        {
            var statusCodeResult = result as StatusCodeResult;
            var resultStatusCode = statusCodeResult?.StatusCode;

            var objectResult = result as ObjectResult;
            var objectCode = objectResult?.StatusCode;

            var actualStatusCode = resultStatusCode ?? objectCode;

            actualStatusCode.Should().Be(statusCode);
        }

        public static T GetObject<T>(this IActionResult result)
        {
            var objectResult = result as ObjectResult;
            return (T) objectResult?.Value;
        }
    }
}