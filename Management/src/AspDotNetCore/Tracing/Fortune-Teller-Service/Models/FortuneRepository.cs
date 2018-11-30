using Microsoft.Extensions.Logging;
using Steeltoe.Management.Census.Trace;
using Steeltoe.Management.Census.Trace.Unsafe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FortuneTellerService.Models
{
    public class FortuneRepository : IFortuneRepository
    {
        private const string SPAN_NAME_RANDOM = "FortuneRepository-Random";
        private const string SPAN_NAME_ALL = "FortuneRepository-All";
        private const string SPAN_NAME_RANDOM_INDEX_ATTRIBUTE = SPAN_NAME_RANDOM + "-Index";
        private const string SPAN_NAME_RANDOM_FORTUNEID_ATTRIBUTE = SPAN_NAME_RANDOM + "-FortuneId";
        private const string SPAN_NAME_RANDOM_FORTUNETEXT_ATTRIBUTE = SPAN_NAME_RANDOM + "-FortuneText";

        private FortuneContext _db;
        private ILogger<FortuneRepository> _logger;
        private ITracing _tracing;
        Random _random = new Random();

        public FortuneRepository(FortuneContext db, ITracing tracing, ILogger<FortuneRepository> logger)
        {
            _db = db;
            _tracing = tracing;
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
            using (var scope = _tracing.Tracer.SpanBuilder(SPAN_NAME_RANDOM).StartScopedSpan())
            {
                int count = _db.Fortunes.Count();
                var index = _random.Next() % count;
                var result = GetAll().ElementAt(index);

                _logger.LogDebug("RandomFortune() ->" + result.Text);

                // Obtain the current span and add some attributes which will be captured along with the span itself
                var span = AsyncLocalContext.CurrentSpan;
                span.PutAttribute(SPAN_NAME_RANDOM_INDEX_ATTRIBUTE, AttributeValue.LongAttributeValue(index));
                span.PutAttribute(SPAN_NAME_RANDOM_FORTUNEID_ATTRIBUTE, AttributeValue.LongAttributeValue(result.Id));
                span.PutAttribute(SPAN_NAME_RANDOM_FORTUNETEXT_ATTRIBUTE, AttributeValue.StringAttributeValue(result.Text));
                span.Status = Status.OK;

                _logger.LogDebug("Finished RandomFortune()");
                return result;
            }
        }

    }
}
