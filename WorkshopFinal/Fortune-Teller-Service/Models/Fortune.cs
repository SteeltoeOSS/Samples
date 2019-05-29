using System;

namespace Fortune_Teller_Service.Models
{
    public class Fortune
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public override string ToString()
        {
            return $"Fortune[{this.Id},{this.Text}]";
        }

    }
}
