using System;

namespace Autod_ja_Omanikud.Models
{
    public class CarService
    {
        public int CarId { get; set; }
        public int ServiceId { get; set; }

        public DateTime DateOfService { get; set; }
        public int Mileage { get; set; }

        public Car Car { get; set; }
        public Service Service { get; set; }
    }
}
