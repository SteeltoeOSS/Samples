using System;

namespace Common
{
    [Serializable]
    public class Message
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }

}
