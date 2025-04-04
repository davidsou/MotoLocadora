using FluentValidation;

namespace MotoLocadora.Application.Features.Notifications.Validators;
public class CreateNotificationValidator : AbstractValidator<CreateNotification.Command>
{
    public CreateNotificationValidator()
    {
        RuleFor(x => x.Notification.Type).NotEmpty();
        RuleFor(x => x.Notification.Description).NotEmpty();
    }
}