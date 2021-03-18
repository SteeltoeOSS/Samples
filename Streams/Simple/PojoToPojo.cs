using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Steeltoe.Messaging;
using Steeltoe.Messaging.Handler.Attributes;
using Steeltoe.Messaging.Support;
using Steeltoe.Stream.Attributes;
using Steeltoe.Stream.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace steeltoe_stream_samples
{
    //public class PojoToPojoStreamListener
    //{
    //    [StreamListener(IProcessor.INPUT)]
    //    [SendTo(IProcessor.OUTPUT)]
    //    public Person Echo(Person value)
    //    {
    //        return value;
    //    }
    //}
    //public class StringToStringStreamListener
    //{
    //    [StreamListener(IProcessor.INPUT)]
    //    [SendTo(IProcessor.OUTPUT)]
    //    public string Echo(string value)
    //    {
    //        return value;
    //    }
    //}
    public class ByteArrayToPojoStreamListener
    {
        [StreamListener(IProcessor.INPUT)]
        [SendTo(IProcessor.OUTPUT)]
        public Person Echo(byte[] value)
        {
            var settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var serializer = JsonSerializer.Create(settings);
            var textReader = new StreamReader(new MemoryStream(value), true);
            return (Person)serializer.Deserialize(textReader, typeof(Person));
        }
    }

    [EnableBinding(typeof(IProcessor))]
    public class SomeClass
    {
        [StreamListener(IProcessor.INPUT)]
        [SendTo(IProcessor.OUTPUT)]
        public Person Echo(byte[] value)
        {
            var settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var serializer = JsonSerializer.Create(settings);
            var textReader = new StreamReader(new MemoryStream(value), true);
            try
            {
                return
                   (Person)serializer.Deserialize(textReader, typeof(Person));
            }
            catch
            {
                return new Person() { Name = "testperson" };
            }
        }
        //[SendTo(IProcessor.OUTPUT)]
        //public Person Outout()
        //{
        //    return new Person() { Name = "test123" };
        //}
    }
    
    //public interface ITestSource
    //{
    //    [Output("TestSink")]
    //    IMessageChannel TestCall();
    //}
    //public interface ITestSink
    //{
    //    [Input("TestSink")]
    //    ISubscribableChannel GetSubscribableChannel();
    //}
    //public class TestSourceComponent
    //{
    //    private ITestSource source;
    //    public TestSourceComponent(ITestSource source)
    //    {
    //        this.source = source;
    //    }
    //    public void Hello(string name)
    //    {
    //        source.TestCall().Send(MessageBuilder.WithPayload(name).Build());
    //    }
    //}
    public class Person
    {
        public string Name { get; set; }
        
        public override string ToString()
        {
            return Name;
        }
    }
}
