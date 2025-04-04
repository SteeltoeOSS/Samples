using System.Web;
using Microsoft.AspNetCore.Mvc;
using Steeltoe.Samples.FileSharesWeb.Models;
using SystemFile = System.IO.File;

namespace Steeltoe.Samples.FileSharesWeb.Controllers;

public sealed class FilesController(FileShareConfiguration fileShareConfiguration, TimeProvider timeProvider) : Controller
{
    [HttpGet]
    public IActionResult Upload()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Upload(List<IFormFile>? files)
    {
        UploadViewModel model = new();

        if (files is null)
        {
            model.Error = "No files were uploaded, this could happen if the file is too large.";
        }
        else
        {
            model.Files = [];

            foreach (IFormFile file in files)
            {
                string sanitizedFileName = string.Join('_', file.FileName.Split(Path.GetInvalidFileNameChars()));
                string saveFileAs = $"UPLOADED_{timeProvider.GetUtcNow():yyyyMMdd-hhmmss}_{sanitizedFileName}";
                await using var stream = new FileStream(Path.Combine(fileShareConfiguration.Location, saveFileAs), FileMode.Create);
                await file.CopyToAsync(stream, HttpContext.RequestAborted);
                model.Files.Add(file.FileName, saveFileAs);
            }
        }

        return View(model);
    }

    [HttpGet]
    public ActionResult List()
    {
        return View(Directory.EnumerateFiles(fileShareConfiguration.Location));
    }

    [HttpDelete]
    public JsonResult Delete(string fileToDelete)
    {
        string actualFileName = HttpUtility.UrlDecode(fileToDelete);
        SystemFile.Delete(actualFileName);
        return Json($"Successfully deleted {actualFileName}");
    }
}
