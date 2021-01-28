using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestDynamicsData.Models
{
    public class GPU : ReactiveObject
    {
        public Guid Id { get; } 
        public string Name { get; set; }
        public double Price { get; set; }
        public Manifacturer Manifacturer { get; set; }
        public bool IsSoldOut { get; set; }

        public GPU()
        {
            Id = Guid.NewGuid();
        }
    }

    public enum Manifacturer 
    {
        MSI,
        PNY,
        Asus,
        EVGA,
        Gigabyte
    }
}
