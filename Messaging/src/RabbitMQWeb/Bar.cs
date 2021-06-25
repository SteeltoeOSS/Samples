using System;

namespace RabbitMQWeb
{
    [Serializable]
    public class Bar : Foo
    {
        public Bar()
        {
        }

        public Bar(string foo)
            : base(foo)
        {

        }
    }
}
