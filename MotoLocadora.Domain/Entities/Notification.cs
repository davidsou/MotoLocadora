using MotoLocadoraBuildingBlocks.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoLocadora.Domain.Entities
{
    public  class Notification : BaseEntity
    {
        public string Type { get; set; }
        public string Description { get; set; }
    }
}
