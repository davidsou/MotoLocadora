using MotoLocadoraBuildingBlocks.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoLocadora.Domain.Entities
{
    public class Motorcycle: BaseEntity
    {
        public string Ano { get; set; }
        public string Modelo { get; set; }
        public string Placa { get; set; }
    }
}
