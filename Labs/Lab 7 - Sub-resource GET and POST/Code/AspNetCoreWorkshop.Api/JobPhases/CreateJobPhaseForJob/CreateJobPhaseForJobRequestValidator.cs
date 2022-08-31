using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreWorkshop.Api.JobPhases.CreateJobPhaseForJob
{
    public class CreateJobPhaseForJobRequestValidator : AbstractValidator<CreateJobPhaseForJobRequest>
    {
        public CreateJobPhaseForJobRequestValidator(WorkshopDbContext workshopDbContext)
        {
            WorkshopDbContext = workshopDbContext;

            RuleFor(r => r.Number).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithMessage("Number must be a non-empty string.")
                .MustAsync(NotHaveExistingJobPhaseWithNumberAsync)
                .WithMessage("A job phase already exists with that number.");

            RuleFor(r => r.Description).NotEmpty().WithMessage("Description must be a non-empty string.");
        }

        public WorkshopDbContext WorkshopDbContext { get; }

        protected async Task<bool> NotHaveExistingJobPhaseWithNumberAsync(CreateJobPhaseForJobRequest request, string number, CancellationToken cancellationToken)
        {
            //TODO: ensure Number does not already exist on a phase for this job. HINT: use WorkshopDbContext.JobPhases.AnyAsync!
            return true;
        }
    }
}