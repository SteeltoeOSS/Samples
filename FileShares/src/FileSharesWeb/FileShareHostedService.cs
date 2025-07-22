using System.Text;
using Steeltoe.Common.Net;

namespace Steeltoe.Samples.FileSharesWeb;

public sealed class FileShareHostedService(FileShareConfiguration fileShareConfiguration) : IHostedService, IDisposable
{
    private WindowsNetworkFileShare? _fileShare;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _fileShare = new WindowsNetworkFileShare(fileShareConfiguration.Location, fileShareConfiguration.Credential);

        // Perform a quick write and delete to confirm the path and credentials are valid.
        // Unhandled exceptions will occur if credentials or permissions are incorrect.
        string testFilePath = Path.Combine(fileShareConfiguration.Location, "TestWriteAccess.txt");
        await using var stream = new FileStream(testFilePath, FileMode.Create);
        await stream.WriteAsync(Encoding.Unicode.GetBytes("Write access is permitted."), cancellationToken);
        stream.Close();
        File.Delete(Path.Combine(fileShareConfiguration.Location, testFilePath));
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Dispose();
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _fileShare?.Dispose();
        _fileShare = null;
    }
}
