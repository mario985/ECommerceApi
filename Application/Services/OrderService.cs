using AutoMapper;

public class OrderService : IOrderService
{
    private readonly IorderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICartService _cartService;
    private readonly IInventoryService _inventoryService;
    private readonly IStripePaymentService _stripePaymentService;
    private readonly IMapper _mapper;

    public OrderService(
        IorderRepository orderRepository,
        IUnitOfWork unitOfWork,
        ICartService cartService,
        IInventoryService inventoryService,
        IStripePaymentService stripePaymentService,
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _cartService = cartService;
        _inventoryService = inventoryService;
        _stripePaymentService = stripePaymentService;
        _mapper = mapper;
    }

    public async Task<PlaceOrderResult> PlaceOrderAsync(string userId)
    {
        var cart = await _cartService.GetCartAsync(userId);
        if (cart == null || !cart.CartItems.Any())
            throw new InvalidOperationException("cart is empty");

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            // 1) Reserve stock (NOT reduce permanently)
            foreach (var item in cart.CartItems)
            {
                await _inventoryService.ReserveStockAsync(item.ProductId, item.Quantity);
            }

            // 2) Create order in DB with PendingPayment
            var order = new Order
            {
                UserId = userId,
                OrderItems = cart.CartItems.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    Price = ci.Price
                }).ToList(),
                TotalPrice = cart.CartItems.Sum(p => p.Price * p.Quantity),
                Status = OrderStatus.PendingPayment,
                Currency = "egp" // store this on order; better than hardcoding
            };

            await _orderRepository.AddAsync(order);

            // 3) Create PaymentIntent for THIS order (after we have order.Id)
            var (paymentIntentId, clientSecret) =
                await _stripePaymentService.CreatePaymentIntentForOrderAsync(order);

            // 4) Save Stripe IDs on the order
            order.StripePaymentIntentId = paymentIntentId;
            await _orderRepository.UpdateAsync(order);

            // 5) Option: do NOT clear cart until payment success
            // In many shops, cart remains until paid OR you create "OrderCartSnapshot".
            // For now, keep it simple: clear cart now because order is created.
            await _cartService.ClearCartAsync(userId);

            await _unitOfWork.CommitAsync();

            return new PlaceOrderResult(order.Id , order.TotalPrice , order.Currency , clientSecret);
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<OrderDto> GetAsync(int id)
    {
        var order= await _orderRepository.GetAsync(id);
        return _mapper.Map<OrderDto>(order);
    }

    public async Task<List<OrderDto>> GetByUserIdAsync(string userId)
    {
        var orders = await _orderRepository.GetAllAsync(userId);
        return _mapper.Map<List<OrderDto>>(orders);
    }
}