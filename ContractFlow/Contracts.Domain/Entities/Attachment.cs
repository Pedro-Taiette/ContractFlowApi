using Contracts.Domain.Abstractions;

namespace Contracts.Domain.Entities;
public class Attachment : Entity
{
    public Guid ContractId { get; private set; }
    public Contract Contract { get; private set; } = null!;
    public string FileName { get; private set; } = null!;
    public string MimeType { get; private set; } = null!;
    public string StoragePath { get; private set; } = null!;

    private Attachment() { }
    public Attachment(Contract contract, string fileName, string mimeType, string storagePath)
    {
        Contract = contract;
        ContractId = contract.Id;
        FileName = fileName;
        MimeType = mimeType;
        StoragePath = storagePath;
    }
}
