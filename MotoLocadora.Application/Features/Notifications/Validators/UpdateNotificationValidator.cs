using FluentValidation;

namespace MotoLocadora.Application.Features.Notifications.Validators;

public class UpdateNotificationValidator : AbstractValidator<UpdateNotification.Command>
{
    public UpdateNotificationValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Notification.Type).NotEmpty();
        RuleFor(x => x.Notification.Description).NotEmpty();
    }
}
