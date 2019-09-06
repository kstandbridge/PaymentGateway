using System;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using PaymentGateway.Contracts;
using PaymentGateway.Domain;

namespace PaymentGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentManager _paymentManager;
        private readonly IValidator<CreatePayment> _createPaymentValidator;

        public PaymentsController(
            IPaymentManager paymentManager,
            IValidator<CreatePayment> createPaymentValidator)
        {
            _paymentManager = paymentManager;
            _createPaymentValidator = createPaymentValidator;
        }

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

            var result = await _paymentManager.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [SwaggerResponse(StatusCodes.Status201Created, typeof(GetPayment))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, typeof(void))]
        public async Task<IActionResult> Create([FromBody] CreatePayment createPayment)
        {
            if (createPayment == null)
            {
                return BadRequest();
            }

            await _createPaymentValidator.ValidateAndThrowAsync(createPayment);

            var result = await _paymentManager.CreateAsync(createPayment);
            return CreatedAtAction(nameof(Get), new {id = result.Id}, result);
        }

    }
}
