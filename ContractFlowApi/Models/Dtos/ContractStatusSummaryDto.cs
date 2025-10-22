namespace ContractsMvc.Models.Dtos;

/// <summary>
/// Simple DTO used to summarise the number of contracts per status. Returned
/// by the reports endpoint. Serialised status name as a string rather than
/// the enum value.
/// </summary>
public class ContractStatusSummaryDto
{
    public string Status { get; set; } = null!;
    public int Count { get; set; }
}