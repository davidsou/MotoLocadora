namespace MotoLocadora.Domain.Events;

public record MotorcycleCreatedEvent(int MotorcycleId, string Model, string LicensePlate, int Year);
