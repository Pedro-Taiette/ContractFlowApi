using System;

namespace ContractsMvc.Models;

/// <summary>
/// Represents a monetary amount and its currency. This class mirrors the
/// value object in the original domain model but exposes public setters to
/// simplify mapping via EF Core. The constructor validates that the amount
/// is non‑negative and uses BRL as the default currency.
/// </summary>
public class Money
{
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "BRL";

    public Money() { }

    public Money(decimal amount, string currency = "BRL")
    {
        if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
        if (string.IsNullOrWhiteSpace(currency)) currency = "BRL";
        Amount = amount;
        Currency = currency;
    }

    public override string ToString() => $"{Currency} {Amount:0.00}";
}