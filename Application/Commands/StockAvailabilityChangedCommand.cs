using MediatR;

public record StockAvailabilityChangedCommand(string productId):INotification;