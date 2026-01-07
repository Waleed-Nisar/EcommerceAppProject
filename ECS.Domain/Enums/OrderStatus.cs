namespace ECS.Domain.Enums;

/// <summary>
/// Defines the various states an order can be in
/// </summary>
public enum OrderStatus
{
    /// <summary>
    /// Order created, awaiting payment
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Payment confirmed, order being prepared
    /// </summary>
    Processing = 1,

    /// <summary>
    /// Order has been shipped
    /// </summary>
    Shipped = 2,

    /// <summary>
    /// Order delivered to customer
    /// </summary>
    Delivered = 3,

    /// <summary>
    /// Order cancelled by customer or admin
    /// </summary>
    Cancelled = 4,

    /// <summary>
    /// Order refunded
    /// </summary>
    Refunded = 5
}