﻿using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.Handler.Attributes;
using Steeltoe.Stream.Attributes;
using Steeltoe.Stream.Messaging;


namespace Basic
{
    [EnableBinding(typeof(IProcessor))]
    public class MyStreamProcessor
    {
        private ILogger<MyStreamProcessor> _logger;

        public MyStreamProcessor(ILogger<MyStreamProcessor> logger)
        {
            _logger = logger;
        }

        [StreamListener(IProcessor.INPUT)]
        [SendTo(IProcessor.OUTPUT)]
        public string Handle(string input)
        {
            var output = input.ToUpper();
            _logger?.LogInformation($"MyStreamProcessor changed input:{input} into output:{output}");
            return output;
        }
    }
}