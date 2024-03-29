﻿using System;

namespace PaymentGateway.BankService.Contracts
{
    public class BankPaymentResponse
    {
        public BankPaymentResponse()
        {
            Id = Guid.Empty;
        }

        public Guid Id { get; set; }
        public bool IsSuccessful { get; set; }
    }
}