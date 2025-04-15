namespace MotoLocadora.Domain.Events;



public record RentCreatedEvent(int RentId, int RiderId, int MotorcycleId, DateTime StartDate, DateTime EstimateEndDate);
