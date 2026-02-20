public interface IOrderExpirationService
{
    Task ExpirePendingOrdersAsync(CancellationToken ct);
}
