namespace ibge.Dtos;

public class CityRow
{
	public int Codigo_Municipio { get; set; } = 0;
	public int Codigo_UF { get; set; } = 0;
	public string Nome_Municipio { get; set; } = "";
}

public class StatesRow
{
	public int Code { get; set; } = 0;
	public string Abbreviation { get; set; } = "";
	public string Name { get; set; } = "";
}

public class BulkResponse {
	public string ProcessedLabel { get; set; } = "";
	public string? Errors { get; set; } = null;
}