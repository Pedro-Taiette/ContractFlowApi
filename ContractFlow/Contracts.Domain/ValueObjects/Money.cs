namespace Contracts.Domain.ValueObjects;

public class Money
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = "BRL";

    private Money() { } // EF

    public Money(decimal amount, string currency = "BRL")
    {
        if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
        if (string.IsNullOrWhiteSpace(currency)) currency = "BRL";
        Amount = amount;
        Currency = currency;
    }

    public override string ToString() => $"{Currency} {Amount:0.00}";
}
