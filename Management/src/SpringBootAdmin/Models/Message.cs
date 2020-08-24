using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudFoundry
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        public int Id { get; set; }
        public string Name { get; set; }
    }
}