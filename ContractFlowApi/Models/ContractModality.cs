namespace ContractsMvc.Models;

/// <summary>
/// Enumerates the procurement modalities used to award a contract. These
/// modalities correspond to Brazilian public procurement law. The values
/// match the original domain model but are exposed as an enum for clarity.
/// </summary>
public enum ContractModality
{
    Pregao = 1,
    Concorrencia = 2,
    TomadaPreco = 3,
    Convite = 4,
    Dispensa = 5,
    Inexigibilidade = 6,
    RDC = 7,
    Credenciamento = 8
}