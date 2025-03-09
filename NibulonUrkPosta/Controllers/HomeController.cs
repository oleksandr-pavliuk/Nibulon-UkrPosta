namespace NibulonUrkPosta.Controllers;

public class HomeController(ILogger<HomeController> logger, IAupService aupService) : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ImportExcel(IFormFile file)
    {
        try
        {
            if (await aupService.ImportDataFromExcelAsync(file))
            {
                var viewModel = new ImportResultViewModel()
                {
                    Message = $"Дані успішно завантажені з файлу: {file.FileName}",
                    IsSuccess = true
                };
                return View("FileOperationResult", viewModel);
            }
            
            return View("FileOperationResult", new ImportResultViewModel() {Message = "Помилка у виконанні коду ..."});
        }
        catch (NoFileSelectedException)
        {
            return View("FileOperationResult", new ImportResultViewModel() {Message = "Помилка: файл не було вибрано"});
        }
        catch(Exception ex)
        {
            logger.LogError("Error: " + ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace);
            return View("FileOperationResult", new ImportResultViewModel() {Message = "Помилка у виконанні коду ..."});
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> ExportExcel()
    {
        try
        {
            var result = await aupService.ExportDataToExcelAsync();
            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "post-codes.xlsx");
        }
        catch(Exception ex)
        {
            logger.LogError("Error: " + ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace);
            return View("ImportResult", new ImportResultViewModel() {Message = "Помилка у виконанні коду ..."});
        }
    }
}