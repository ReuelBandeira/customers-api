namespace Api.Shared.Helpers;

public class FilterParams
{
    /// <summary>
    /// Nome para filtragem parcial (case-insensitive).
    /// </summary>
    public string? name { get; set; }

    /// <summary>
    /// Data mínima de criação para filtragem.
    /// </summary>
    public DateTime? createdAfter { get; set; }

    /// <summary>
    /// Data máxima de criação para filtragem.
    /// </summary>
    public DateTime? createdBefore { get; set; }
    public string? Status { get; set; }
}

