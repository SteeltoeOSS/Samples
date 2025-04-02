using Microsoft.AspNetCore.Mvc;
using SystemFile = System.IO.File;

namespace Steeltoe.Samples.FileSharesWeb.Controllers;

public sealed class FilesController(TimeProvider timeProvider) : Controller
{
    [HttpGet]
    public IActionResult Upload()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Upload(List<IFormFile>? files)
    {
        Dictionary<string, string> filesUploaded = [];

        if (files is null)
        {
            filesUploaded.Add("error", "No files were uploaded, this could happen if the file is too large.");
        }
        else
        {
            foreach (IFormFile file in files)
            {
                string sanitizedFileName = string.Join("_", file.FileName.Split(Path.GetInvalidFileNameChars()));
                string saveFileAs = $"UPLOADED_{timeProvider.GetUtcNow():yyyyMMdd-hhmmss}_{sanitizedFileName}";
                await using var stream = new FileStream(Path.Combine(FileShareHostedService.Location!, saveFileAs), FileMode.Create);
                await file.CopyToAsync(stream);
                filesUploaded.Add(file.FileName, saveFileAs);
            }
        }

        return View(filesUploaded);
    }

    [HttpGet]
    public ActionResult List()
    {
        return View(Directory.EnumerateFiles(FileShareHostedService.Location!));
    }

    [HttpGet]
    public ActionResult<string> Delete(string fileToDelete)
    {
        SystemFile.Delete(Path.Combine(FileShareHostedService.Location!, fileToDelete));

        return RedirectToAction("List");
    }
}
