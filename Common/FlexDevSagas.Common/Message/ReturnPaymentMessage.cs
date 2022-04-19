using MassTransit;

namespace FlexDevSagas.Common.Message
{
    public record ReturnPaymentMessage(Guid CorrelationId) : CorrelatedBy<Guid>;
}
