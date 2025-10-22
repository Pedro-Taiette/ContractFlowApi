namespace ContractsMvc.Models;

/// <summary>
/// Enumerates the high‑level categories of contracts. Mirroring the original
/// domain model, the names reflect common public procurement modalities. By
/// storing these as strings in the database we avoid magic numbers and
/// simplify JSON serialization.
/// </summary>
public enum ContractType
{
    /// <summary>Prestação de serviço.</summary>
    Servico = 1,
    /// <summary>Obras de construção.</summary>
    Obra = 2,
    /// <summary>Fornecimento de bens ou insumos.</summary>
    Fornecimento = 3,
    /// <summary>Locação de bens móveis ou imóveis.</summary>
    Locacao = 4,
    /// <summary>Outro tipo de contrato não classificado.</summary>
    Outro = 99
}