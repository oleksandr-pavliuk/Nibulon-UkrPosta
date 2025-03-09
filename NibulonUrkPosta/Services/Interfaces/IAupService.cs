namespace NibulonUrkPosta.Services.Interfaces;

public interface IAupService
{
    Task<bool> ImportDataFromExcelAsync(IFormFile file);
    Task<byte[]> ExportDataToExcelAsync();
    Task<AupFilterViewModel> GetAupFilterAsync(AupFilterViewModel model, int page);
}