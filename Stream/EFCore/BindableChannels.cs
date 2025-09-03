using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Steeltoe.Stream.Attributes;
using Steeltoe.Stream.Messaging;

namespace EFCore
{
    [EnableBinding(typeof(ISink))]
    public class BindableChannels
    {
        private DbContext _db;
        private ILogger<BindableChannels> _logger;

        public BindableChannels(FooContext db, ILogger<BindableChannels> logger)
        {
            _db = db;
            _logger = logger;
        }

        [StreamListener("input")]
        public void HandleInputMessage(Foo foo)
        {
            _logger.LogInformation("Received foo named '{name}' with tag '{tag}'", foo.name, foo.tag);
            _db.Database.EnsureCreated();
            _db.Add(foo);
            _db.SaveChanges();
            _logger.LogInformation("Foo was assigned id '{id}' after saving to the database.", foo.id);
        }
    }
}