using MotoLocadoraBuildingBlocks.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoLocadora.Domain.Entities
{
    public class Tariff:BaseEntity
    {
        public decimal Price { get; set; }
        public int Days { get; set; }
    }
}
