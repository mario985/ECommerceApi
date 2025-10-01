
using AutoMapper;
using MediatR;

public class OrderService : IOrderService
{
    private readonly IorderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICartService _cartService;
    private readonly IInventoryService _inventoryService;
    private readonly IMapper _mapper;
    public OrderService(IorderRepository orderRepository,
                        IUnitOfWork unitOfWork,
                        ICartService cartService,
                        IInventoryService inventoryService,
                        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _cartService = cartService;
        _inventoryService = inventoryService;
        _mapper = mapper;
    }
    public async Task<OrderDto> PlaceOrderAsync(string userId)
    {
        var cart = await _cartService.GetCartAsync(userId);
        if (cart == null || !cart.CartItems.Any())
            throw new InvalidOperationException("cart is empty");
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            foreach (var item in cart.CartItems)
            {
                var inventory = await _inventoryService.GetInventoryAsync(item.ProductId);
                if (inventory == null || inventory.Quantity < item.Quantity)
                    throw new InvalidOperationException("out of stock");
                await _inventoryService.ReduceQuantityAsync(inventory.ProductId, item.Quantity);
            }
            var order = new Order
            {
                UserId = userId,
                OrderItems = cart.CartItems.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    Price = ci.Price

                }).ToList(),
                TotalPrice = cart.CartItems.Sum(p => p.Price),
                Status = OrderStatus.Pending
            };
            await _orderRepository.AddAsync(order);
            await _cartService.ClearCartAsync(userId);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<OrderDto>(order);
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;

        }
    }
}