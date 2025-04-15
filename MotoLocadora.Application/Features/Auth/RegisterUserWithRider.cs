using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MotoLocadora.Application.Features.Auth.Dtos;
using MotoLocadora.Application.Features.Ryders;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.BuildingBlocks.Entities;

namespace MotoLocadora.Application.Features.Auth;
public class RegisterUserWithRider
{
    public record Command(RegisterRequest Request) : IRequest<OperationResult>;

    public class Handler(
        UserManager<ApplicationUser> userManager,
        IMediator mediator,
        ILogger<Handler> logger)
        : BaseHandler(logger), IRequestHandler<Command, OperationResult>
    {
        public async Task<OperationResult> Handle(Command command, CancellationToken cancellationToken)
        {
            var request = command.Request;

            return await TryCatchAsync(async () =>
            {
                // Criação do usuário Identity
                var user = new ApplicationUser
                {
                    UserName = request.Email,
                    Email = request.Email,
                    FullName = request.FullName
                };

                var result = await userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                {
                    return OperationResult.Failure(result.Errors.Select(e => e.Description));
                }

                await userManager.AddToRoleAsync(user, "Client");

                var riderDtoWithUserId = request.Rider with { UserId = user.Id };
                var createRiderCommand = new CreateRider.Command(riderDtoWithUserId);
                var riderResult = await mediator.Send(createRiderCommand, cancellationToken);

                if (!riderResult.IsSuccess)
                {
                    return OperationResult.Failure(riderResult.Errors);
                }

                return OperationResult.Success("Usuário e entregador registrados com sucesso!");
            }, "Registrar novo usuário com entregador");
        }
    }
}