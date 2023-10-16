using ibge.Dtos;
using ibge.Models;
using ibge.Providers.interfaces;
using ibge.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniExcelLibs;

namespace ibge.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class BulkController : ControllerBase
{
	private readonly ILocationRepository _locationService;
	private readonly IBlobProvider _blobProvider;
	public BulkController(ILocationRepository locationService, IBlobProvider blobProvider)
	{
		_locationService = locationService;
		_blobProvider = blobProvider;
	}

	[Authorize]
	[HttpPost(Name = "Import from excel")]
	public async Task<ActionResult<List<Dictionary<string, object>>>> Create(
            [FromForm] IFormFile? file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            if (!IsValidExcelFile(file.ContentType))
            {
                return BadRequest("The uploaded file is not in XLSX format.");
            }

            var values = new List<Dictionary<string, object>>();
            var stream = file.OpenReadStream();
            var path = Path.GetTempFileName();
	        var i = 0;
	        
	        await using (var fileStream = new FileStream(path, FileMode.OpenOrCreate))
            {
                await stream.CopyToAsync(fileStream);
               
                foreach(BulkRow row in stream.Query<BulkRow>())
                {
                    var reference = i + 1;
                    i++;
                    
                    if (row.Id == 0 || row.State == "" || row.City == "")
                    {
	                    values.Add(
		                    new Dictionary<string, object> { { "Reference", reference }, { "ID", row.Id }, { "State", row.State }, { "City", row.City }, { "Error", "Invalid row" } }
	                    );
	                    continue;
                    }
                    
                    try
                    {
                        await ProcessExcelRow(row);
                    }
                    catch (Exception ex)
                    {
                        values.Add(
                            new Dictionary<string, object> { { "Reference", reference }, { "ID", row.Id }, { "State", row.State }, { "City", row.City }, { "Error", ex.Message } }
                        );
                    }
                }
            }
	        
	        string errorFile = null!;
	        
	        if (values.Count > 0)
	        {
		        var filename = $"{Guid.NewGuid()}.xlsx".ToLower();
		        var tempPath = Path.GetTempPath() + filename;
		        await MiniExcel.SaveAsAsync(tempPath, values);
		        await using (var tempStream = new FileStream(tempPath, FileMode.Open))
		        {
			        await _blobProvider.UploadFile(filename, tempStream);
		        };
	        
		        errorFile = _blobProvider.GetDownloadUrl(filename);
	        }
	        
			var successed = i - values.Count;
			var processedLabel = $"{successed} processed, {values.Count} errors";
			
			return Ok(
	            new BulkResponse()
	            {
		            ProcessedLabel = processedLabel,
		            Errors = errorFile,
	            });
        }

	private async Task ProcessExcelRow(BulkRow row)
	{
		Location location = new()
		{
			Id = row.Id,
			State = row.State,
			City = row.City
		};

		await _locationService.Create(location);
	}


	private static bool IsValidExcelFile(string contentType)
	{
		return contentType.Equals("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", StringComparison.OrdinalIgnoreCase);
	}
}