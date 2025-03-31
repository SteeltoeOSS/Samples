using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Steeltoe.Common.Net;
using Steeltoe.Configuration.CloudFoundry;
using System.Net;

namespace Steeltoe.Samples.FileSharesWeb.Controllers;

public class FilesController : Controller
{
    private readonly string _sharePath;
    private NetworkCredential ShareCredentials { get; }

    public FilesController(IOptions<CloudFoundryServicesOptions> serviceOptions, ILogger<FilesController> logger)
    {
        if (!serviceOptions.Value.Services.TryGetValue("credhub", out IList<CloudFoundryService>? value))
        {
            throw new InvalidOperationException();
        }
        var credHubEntry = value.FirstOrDefault(service => service.Name!.Equals("sampleNetworkShare"));
        _sharePath = credHubEntry?.Credentials["location"].Value ?? throw new InvalidOperationException("Network share path is required.");
        var userName = credHubEntry.Credentials["username"].Value ?? throw new InvalidOperationException("Network share username is required.");
        var password = credHubEntry.Credentials["password"].Value ?? throw new InvalidOperationException("Network share password is required.");
        ShareCredentials = new NetworkCredential(userName, password);
        logger.LogDebug("File share path found in configuration: {path}", _sharePath);
    }

    public async Task<IActionResult> Upload(List<IFormFile>? files)
    {
        Dictionary<string, string> filesUploaded = new();
        if (files is { Count: > 0 })
        {
            using var networkPath = new WindowsNetworkFileShare(_sharePath, ShareCredentials);
            foreach (var file in files.Where(file => file.Length > 0))
            {
                var fileExtension = Path.GetExtension(file.FileName);
                var saveFileAs = Guid.NewGuid() + fileExtension;
                await using var stream = new FileStream(Path.Combine(_sharePath, saveFileAs), FileMode.Create);
                await file.CopyToAsync(stream);
                filesUploaded.Add(file.FileName, saveFileAs);
            }
        }

        return View(filesUploaded);
    }

    public ActionResult List()
    {
        using var networkPath = new WindowsNetworkFileShare(_sharePath, ShareCredentials);
        return View(Directory.EnumerateFiles(_sharePath));
    }

    public ActionResult<string> Delete(string fileToDelete)
    {
        using var networkPath = new WindowsNetworkFileShare(_sharePath, ShareCredentials);
        System.IO.File.Delete(Path.Combine(_sharePath, fileToDelete));

        return RedirectToAction("List");
    }
}
