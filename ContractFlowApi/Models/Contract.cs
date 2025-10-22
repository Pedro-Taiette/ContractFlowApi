using System;
using System.Collections.Generic;

namespace ContractsMvc.Models;

/// <summary>
/// The central entity representing a contract. A contract has an official
/// number, relates to a supplier and an organisational unit, and includes
/// details about its type, modality, status, term and value. A contract
/// aggregates a collection of obligations and attachments. Business
/// operations for creating obligations and deliverables are implemented in
/// services to keep the entity simple.
/// </summary>
public class Contract : AggregateRoot
{
    public string OfficialNumber { get; set; } = null!;
    public string? AdministrativeProcess { get; set; }
    public Guid SupplierId { get; set; }
    public Supplier Supplier { get; set; } = null!;
    public Guid OrgUnitId { get; set; }
    public OrgUnit OrgUnit { get; set; } = null!;
    public ContractType Type { get; set; }
    public ContractModality Modality { get; set; }
    public ContractStatus Status { get; set; } = ContractStatus.Active;
    public Period Term { get; set; } = null!;
    public Money TotalValue { get; set; } = null!;

    public List<Obligation> Obligations { get; set; } = new();
    public List<Attachment> Attachments { get; set; } = new();
}