using System;
using System.Collections.Generic;
using System.Text;

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
