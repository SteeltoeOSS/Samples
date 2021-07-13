using System.Collections.Concurrent;
using System.Diagnostics.Tracing;

namespace FortuneTeller.Service
{
    internal class FortuneEventListener : EventListener
    {
        public ConcurrentQueue<EventWrittenEventArgs> Events = new ();

        public FortuneEventListener(EventSource eventSource, EventLevel minLevel = EventLevel.Verbose)
        {
            EnableEvents(eventSource, minLevel);
        }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            Events.Enqueue(eventData);
        }
    }
}
