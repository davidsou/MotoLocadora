using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.Application.Features.Rents.Dtos;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.Domain.Interfaces;

namespace MotoLocadora.Application.Features.Rents;

public class SimulateRent
{
    public record Query(int MotorcycleId, DateTime Start, DateTime EstimateEnd) : IRequest<OperationResult<SimulationResultDto>>;

    public class Handler(
        ITariffRepository tariffRepository,
        ILogger<Handler> logger)
        : BaseHandler(logger), IRequestHandler<Query, OperationResult<SimulationResultDto>>
    {
        public async Task<OperationResult<SimulationResultDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await TryCatchAsync(async () =>
            {
                var tariffs = await tariffRepository.GetAllAsync();
                var duration = (request.EstimateEnd.Date - request.Start.Date).Days;

                if (duration <= 0)
                    return OperationResult<SimulationResultDto>.Failure(["O período informado deve ser de pelo menos 1 dia."]);

                var selectedTariff = tariffs.FirstOrDefault(t => t.Days == duration);
                decimal selectedPrice;
                string message = string.Empty;

                if (selectedTariff is not null)
                {
                    selectedPrice = selectedTariff.Days * selectedTariff.Price;
                }
                else
                {
                    // Verificar se o período é menor que o menor plano (antecipação)
                    var closestTariff = tariffs.OrderBy(t => t.Days).First();
                    if (duration < closestTariff.Days)
                    {
                        decimal dailyRate = closestTariff.Price;
                        int unusedDays = closestTariff.Days - duration;
                        decimal basePrice = duration * dailyRate;
                        decimal finePercentage = closestTariff.Days == 7 ? 0.2m : 0.4m;
                        decimal fine = unusedDays * dailyRate * finePercentage;
                        selectedPrice = basePrice + fine;
                        message = $"Período informado ({duration} dias) não corresponde a nenhum plano. Cobrança com base na devolução antecipada: multa de {finePercentage * 100}% sobre {unusedDays} diária(s) restante(s).";
                    }
                    else
                    {
                        // Atraso
                        var longestTariff = tariffs.OrderByDescending(t => t.Days).First();
                        decimal basePrice = longestTariff.Days * longestTariff.Price;
                        int extraDays = duration - longestTariff.Days;
                        decimal extraCharge = extraDays * 50m;
                        selectedPrice = basePrice + extraCharge;
                        message = $"Período informado ({duration} dias) excede o plano mais longo. Cobrança com adicional de R$50 por diária excedente: {extraDays} dia(s).";
                    }
                }

                var result = new SimulationResultDto(
                    MotorcycleId: request.MotorcycleId,
                    Start: request.Start,
                    EstimateEnd: request.EstimateEnd,
                    SelectedPrice: selectedPrice,
                    Alternatives: tariffs.Select(t => new SimulationAlternative(t.Days, t.Days * t.Price)).ToList(),
                    Message: message
                );

                return OperationResult<SimulationResultDto>.Success(result);
            }, "Simular aluguel");
        }
    }
}
