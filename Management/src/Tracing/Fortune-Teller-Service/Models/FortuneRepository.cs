using Microsoft.Extensions.Logging;
using OpenTelemetry.Trace;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace FortuneTeller.Service.Models
{
    public class FortuneRepository : IFortuneRepository
    {
        private const string SPAN_NAME_RANDOM = "FortuneRepository-Random";
        private const string SPAN_NAME_ALL = "FortuneRepository-All";
        private const string SPAN_NAME_RANDOM_INDEX_ATTRIBUTE = SPAN_NAME_RANDOM + "-Index";
        private const string SPAN_NAME_RANDOM_FORTUNEID_ATTRIBUTE = SPAN_NAME_RANDOM + "-FortuneId";
        private const string SPAN_NAME_RANDOM_FORTUNETEXT_ATTRIBUTE = SPAN_NAME_RANDOM + "-FortuneText";

        internal static readonly AssemblyName AssemblyName = typeof(FortuneRepository).Assembly.GetName();
        internal static readonly string ActivitySourceName = AssemblyName.Name;
        internal static readonly Version Version = AssemblyName.Version;
        internal static readonly ActivitySource ActivitySource = new (ActivitySourceName, Version.ToString());

        private readonly FortuneContext _db;
        private readonly ILogger<FortuneRepository> _logger;
        private readonly Random _random = new ();

        public FortuneRepository(FortuneContext db, ILogger<FortuneRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public IEnumerable<Fortune> GetAll()
        {
            _logger.LogDebug("Starting GetAll()");

            var result = _db.Fortunes.AsEnumerable();

            _logger.LogDebug("Finished GetAll()");
            return result;
        }

        public Fortune RandomFortune()
        {
            _logger.LogDebug("Starting RandomFortune() ");

            // Start a scoped span.  This will create a new span with the parent equal to whatever is the current span.
            // When Dispose() called on the returned scope the span will end and the parent span will become the current span
            using var activity = ActivitySource.StartActivity(SPAN_NAME_RANDOM)?.Start();
            var count = _db.Fortunes.Count();
            var index = _random.Next() % count;
            var result = GetAll().ElementAt(index);

            _logger.LogDebug("RandomFortune() ->" + result.Text);

            // Obtain the current span and add some attributes which will be captured along with the span itself
            activity.SetTag(SPAN_NAME_RANDOM_INDEX_ATTRIBUTE, index);
            activity.SetTag(SPAN_NAME_RANDOM_FORTUNEID_ATTRIBUTE, result.Id);
            activity.SetTag(SPAN_NAME_RANDOM_FORTUNETEXT_ATTRIBUTE, result.Text);
            activity.SetStatus(Status.Ok);

            _logger.LogDebug("Finished RandomFortune()");
            return result;

        }

    }
}
