using ibge.Dtos;
using ibge.Exceptions;
using ibge.Models;
using ibge.Providers.interfaces;
using ibge.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniExcelLibs;

namespace ibge.Controllers;

[ApiController]
[Route("v1/locations/[controller]")]
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
        IFormFile file)
    {
        if (file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        if (!IsValidExcelFile(file.ContentType))
        {
            return BadRequest("The uploaded file is not in XLSX format.");
        }

        var stream = file.OpenReadStream();
        var path = Path.GetTempFileName();
        BulkProcessResults results;

        await using (var fileStream = new FileStream(path, FileMode.OpenOrCreate))
        {
            await stream.CopyToAsync(fileStream);

            try
            {
                var states = ReadStates(stream);
                results = await ProcessExcel(stream, states);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        string errorFile = null!;

        if (results.errors > 0)
        {
            var filename = $"{Guid.NewGuid()}.xlsx".ToLower();
            var tempPath = Path.GetTempPath() + filename;
            await MiniExcel.SaveAsAsync(tempPath, results.errorsResult);
            await using (var tempStream = new FileStream(tempPath, FileMode.Open))
            {
                await _blobProvider.UploadFile(filename, tempStream);
            };

            errorFile = _blobProvider.GetDownloadUrl(filename);
        }

        var processedLabel = $"{results.processed} processed, {results.errors} errors";

        return Ok(
            new BulkResponse()
            {
                ProcessedLabel = processedLabel,
                Errors = errorFile,
            });
    }

    private async Task<BulkProcessResults> ProcessExcel(Stream stream, List<StatesRow> states)
    {
        var i = 0;
        var values = new List<Dictionary<string, object>>();

        foreach (CityRow row in stream.Query<CityRow>(sheetName: "MUNICIPIOS"))
        {
            i++;
            StatesRow? state = states.Find(s => s.Code == row.Codigo_UF);
            if (state == null) continue;

            try
            {
                var abbreviation = state.Abbreviation.ToUpper() ?? "";
                await ProcessExcelRow(row.Codigo_Municipio, abbreviation, row.Nome_Municipio);
            }
            catch (Exception ex)
            {
                if (ex is ConflictException)
                {
                    values.Add(
                        new Dictionary<string, object> { { "Reference", i }, { "ID", row.Codigo_Municipio }, { "State", state.Name }, { "City", row.Nome_Municipio }, { "Error", "Localização já existe" } }
                    );
                    continue;
                }
                values.Add(
                    new Dictionary<string, object> { { "Reference", i }, { "ID", row.Codigo_Municipio }, { "State", state.Name }, { "City", row.Nome_Municipio }, { "Error", "Linha inválida" } }
                );
            }
        }

        BulkProcessResults results = new()
        {
            processed = i,
            errors = values.Count,
            errorsResult = values
        };
        return results;
    }

    private List<StatesRow> ReadStates(Stream stream)
    {
        try
        {
            return stream
                .Query(useHeaderRow: true)
                .Select(rowData => new StatesRow()
                {
                    Name = rowData.Nome_UF,
                    Code = (int)
                        rowData.Codigo_UF,
                    Abbreviation = rowData.Sigla_UF,
                }).ToList();
        }
        catch
        {
            const string link = "https://github.com/andrebaltieri/ibge/blob/main/SQL%20INSERTS%20-%20API%20de%20localidades%20IBGE.xlsx";
            throw new Exception($"O arquivo não está no formato correto. Baixe o arquivo correto em {link}");
        }
    }
    private async Task ProcessExcelRow(int id, string? state, string city)
    {
        Location location = new()
        {
            Id = id,
            State = state,
            City = city
        };

        await _locationService.Create(location);
    }


    private static bool IsValidExcelFile(string contentType)
    {
        return contentType.Equals("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", StringComparison.OrdinalIgnoreCase);
    }
}