using System.ComponentModel.DataAnnotations;

namespace EFCore
{
    public class Foo
    {
        [Key] public int id { get; set; }
        public string name { get; set; }
        public string tag { get; set; }
    }
}