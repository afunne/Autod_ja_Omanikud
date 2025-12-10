using System.Collections.Generic;

namespace Autod_ja_Omanikud.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public ICollection<CarService> CarServices { get; set; }
    }
}
