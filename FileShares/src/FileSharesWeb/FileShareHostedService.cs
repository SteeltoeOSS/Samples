using Steeltoe.Common.Net;

namespace Steeltoe.Samples.FileSharesWeb;

public sealed class FileShareHostedService(FileShareConfiguration fileShareConfiguration) : IHostedService, IDisposable
{
    private WindowsNetworkFileShare? _fileShare;

    private bool _disposed;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _fileShare = new WindowsNetworkFileShare(fileShareConfiguration.Location, fileShareConfiguration.Credential);

        if (!Directory.Exists(fileShareConfiguration.Location))
        {
            throw new IOException($"File share path '{fileShareConfiguration.Location}' does not exist. Review the contents of README.md.");
        }

        // Perform a quick write and delete to confirm the path and credentials are valid.
        // Unhandled exceptions will occur if credentials or permissions are incorrect.
        const string testFileName = "TestWriteAccess.txt";

        await using (var outputFile = new StreamWriter(Path.Combine(fileShareConfiguration.Location, testFileName)))
        {
            await outputFile.WriteAsync("Write access is permitted.");
        }

        File.Delete(Path.Combine(fileShareConfiguration.Location, testFileName));
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Dispose();
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _fileShare?.Dispose();
        _fileShare = null;
        _disposed = true;
    }
}
