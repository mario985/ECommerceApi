using MediatR;

public record ProductCreated(string ProductId , int quantity):INotification;