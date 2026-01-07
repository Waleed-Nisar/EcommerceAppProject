namespace ECS.Domain.Enums;

/// <summary>
/// Defines the various states a payment can be in
/// </summary>
public enum PaymentStatus
{
    /// <summary>
    /// Payment initiated, awaiting confirmation
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Payment successfully processed
    /// </summary>
    Completed = 1,

    /// <summary>
    /// Payment failed or declined
    /// </summary>
    Failed = 2,

    /// <summary>
    /// Payment refunded to customer
    /// </summary>
    Refunded = 3,

    /// <summary>
    /// Payment cancelled before processing
    /// </summary>
    Cancelled = 4
}