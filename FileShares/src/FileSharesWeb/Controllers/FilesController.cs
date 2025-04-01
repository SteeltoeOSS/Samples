using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Steeltoe.Common.Net;
using Steeltoe.Samples.FileSharesWeb.Models;
using SystemFile = System.IO.File;

namespace Steeltoe.Samples.FileSharesWeb.Controllers;

public sealed class FilesController(IOptionsMonitor<FileShareOptions> serviceOptionsMonitor, TimeProvider timeProvider) : Controller
{
    public async Task<IActionResult> Upload(List<IFormFile>? files)
    {
        FileShareOptions fileShareOptions = serviceOptionsMonitor.CurrentValue;
        Dictionary<string, string> filesUploaded = [];

        if (Request.Method == HttpMethod.Post.ToString() && files is null)
        {
            throw new InvalidOperationException("No files were uploaded, this could happen if the file is too large.");
        }

        if (files is { Count: > 0 })
        {
            using var fileShare = new WindowsNetworkFileShare(fileShareOptions.Location, fileShareOptions.Credential);

            foreach (IFormFile file in files)
            {
                string fileExtension = Path.GetExtension(file.FileName);
                string saveFileAs = $"UPLOADED_{timeProvider.GetUtcNow():yyyyMMdd-hhmmss.fff}{fileExtension}";
                await using var stream = new FileStream(Path.Combine(fileShareOptions.Location, saveFileAs), FileMode.Create);
                await file.CopyToAsync(stream);
                filesUploaded.Add(file.FileName, saveFileAs);
            }
        }

        return View(filesUploaded);
    }

    public ActionResult List()
    {
        FileShareOptions fileShareOptions = serviceOptionsMonitor.CurrentValue;
        using var fileShare = new WindowsNetworkFileShare(fileShareOptions.Location, fileShareOptions.Credential);
        return View(Directory.EnumerateFiles(fileShareOptions.Location));
    }

    public ActionResult<string> Delete(string fileToDelete)
    {
        FileShareOptions fileShareOptions = serviceOptionsMonitor.CurrentValue;
        using var fileShare = new WindowsNetworkFileShare(fileShareOptions.Location, fileShareOptions.Credential);
        SystemFile.Delete(Path.Combine(fileShareOptions.Location, fileToDelete));

        return RedirectToAction("List");
    }
}
