using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSwag.Annotations;
using PaymentGateway.Contracts;
using PaymentGateway.Domain;
using PaymentGateway.Telemetry.Extensions;
using PaymentGateway.Telemetry.Models;
using PaymentGateway.Telemetry.Submitters;

namespace PaymentGateway.Controllers
{
    /// <summary>
    /// The Payments controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentManager _paymentManager;
        private readonly IValidator<CreatePayment> _createPaymentValidator;
        private readonly ITelemetrySubmitter _telemetrySubmitter;
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(
            IPaymentManager paymentManager,
            IValidator<CreatePayment> createPaymentValidator,
            ITelemetrySubmitter telemetrySubmitter,
            ILogger<PaymentsController> logger)
        {
            _paymentManager = paymentManager;
            _createPaymentValidator = createPaymentValidator;
            _telemetrySubmitter = telemetrySubmitter;
            _logger = logger;
        }

        /// <summary>
        /// Gets a payment.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <returns>The payment.</returns>
        [HttpGet("{id}")]
        [SwaggerResponse(StatusCodes.Status200OK, typeof(GetPayment))]
        [SwaggerResponse(StatusCodes.Status404NotFound, typeof(void))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, typeof(void))]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            GetPayment result;

            using (var operation = _telemetrySubmitter.BeginTimedOperation(new ServiceOperation(nameof(PaymentsController),nameof(Get))))
            {
                try
                {
                    result = await _paymentManager.GetByIdAsync(id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to Get payment Id: {0} due to {1}", id, ex.Message);
                    operation.SetFaulted(ex);
                    throw;
                }
            }

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Creates a payment.
        /// </summary>
        /// <param name="createPayment">The create payment request.</param>
        /// <returns>The created payment.</returns>
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status201Created, typeof(GetPayment))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, typeof(void))]
        public async Task<IActionResult> Create([FromBody] CreatePayment createPayment)
        {
            var validate = await _createPaymentValidator.ValidateAsync(createPayment);
            if (!validate.IsValid)
            {
                return BadRequest(validate.Errors.Select(e => e.ErrorMessage));
            }

            GetPayment result;

            using (var operation = _telemetrySubmitter.BeginTimedOperation(new ServiceOperation(nameof(PaymentsController),nameof(Create))))
            {
                try
                {
                    result = await _paymentManager.CreateAsync(createPayment);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to Create payment due to (0)", ex.Message);
                    operation.SetFaulted(ex);
                    throw;
                }
            }
            return CreatedAtAction(nameof(Get), new {id = result.Id}, result);
        }

    }
}
