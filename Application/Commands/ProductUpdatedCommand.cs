using MediatR;

public record ProductUpdatedCommand(string productId , int quantity) : INotification;