namespace ibge.Dtos;

public class BulkRow
{
	public int Id { get; set; } = 0;
	public string City { get; set; } = "";
	public string State { get; set; } = "";
}


public class BulkResponse {
	public string ProcessedLabel { get; set; } = "";
	public string? Errors { get; set; } = null;
}