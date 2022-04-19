namespace FlexDevSagas.Services.Orders.Entities
{
    public enum OrderState
    {
        Created = 0,
        Reserved = 1,
        Paid  = 2,
        Finished = 3,
        Failed = 4,
        Cancelled = 5
    }
}
