namespace NibulonUrkPosta.Controllers;

public class AupController(ILogger<AupController> logger, IAupService aupService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Filtering(AupFilterViewModel model, int page = 1)
    {
        try
        {
            var result = await aupService.GetAupFilterAsync(model, page);
            return View(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

}