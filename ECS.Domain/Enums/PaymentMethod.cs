namespace ECS.Domain.Enums;

/// <summary>
/// Defines supported payment methods
/// </summary>
public enum PaymentMethod
{
    /// <summary>
    /// Credit or debit card payment
    /// </summary>
    CreditCard = 0,

    /// <summary>
    /// PayPal payment
    /// </summary>
    PayPal = 1,

    /// <summary>
    /// Bank transfer
    /// </summary>
    BankTransfer = 2,

    /// <summary>
    /// Cash on delivery
    /// </summary>
    CashOnDelivery = 3,

    /// <summary>
    /// Digital wallet (Apple Pay, Google Pay, etc.)
    /// </summary>
    DigitalWallet = 4
}