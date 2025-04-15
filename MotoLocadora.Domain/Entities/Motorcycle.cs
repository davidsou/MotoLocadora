﻿using MotoLocadora.BuildingBlocks.Entities;

namespace MotoLocadora.Domain.Entities;

public class Motorcycle: BaseEntity
{
    public int Ano { get; set; }
    public string Modelo { get; set; }
    public string Placa { get; set; }
}
