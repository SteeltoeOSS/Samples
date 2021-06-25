using System;

namespace RabbitMQWeb
{

    [Serializable]
    public class Foo
    {
        public Foo()
            : base()
        {
        }

        public Foo(string foo)
        {
            Value = foo;
        }


        public string Value { get; set; }

        public override string ToString()
        {
            return GetType().Name + " [foo=" + Value + "]";
        }
    }
}
