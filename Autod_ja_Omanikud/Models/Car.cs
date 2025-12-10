using System.Collections.Generic;

namespace Autod_ja_Omanikud.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string RegistrationNumber { get; set; }

        // Foreign key
        public int OwnerId { get; set; }

        // Navigation property
        public Owner Owner { get; set; }
        public ICollection<CarService> CarServices { get; set; }
    }
}
