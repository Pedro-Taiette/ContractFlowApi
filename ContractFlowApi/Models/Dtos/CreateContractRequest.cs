using System;

namespace ContractsMvc.Models.Dtos;

/// <summary>
/// Represents the data required to create a new contract. Used as the input
/// model for the ContractsController POST endpoint. All fields are
/// mandatory except AdministrativeProcess.
/// </summary>
public class CreateContractRequest
{
    public string OfficialNumber { get; set; } = null!;
    public Guid SupplierId { get; set; }
    public Guid OrgUnitId { get; set; }
    public ContractType Type { get; set; }
    public ContractModality Modality { get; set; }
    public DateTime TermStart { get; set; }
    public DateTime TermEnd { get; set; }
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; } = "BRL";
    public string? AdministrativeProcess { get; set; }
}