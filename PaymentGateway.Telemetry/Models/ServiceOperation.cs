namespace PaymentGateway.Telemetry.Models
{
    public class ServiceOperation : TimedOperationBase
    {
        public ServiceOperation(string serviceName, string operationName)
        {
            ServiceName = serviceName;
            OperationName = operationName;
        }

        public string ServiceName { get; set; }
        public string OperationName { get; set; }

    }
}