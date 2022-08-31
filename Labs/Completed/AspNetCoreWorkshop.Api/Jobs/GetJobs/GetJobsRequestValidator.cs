using FluentValidation;

namespace AspNetCoreWorkshop.Api.Jobs.GetJobs
{
    public class GetJobsRequestValidator : AbstractValidator<GetJobsRequest>
    {
        public GetJobsRequestValidator()
        {
        }
    }
}