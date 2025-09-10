using MediatR;
using ZaposliMe.Domain.Generic;

namespace ZaposliMe.Application.Commands.Job.CreateJob
{
    public class CreateJobApplicationCommandHandler : IRequestHandler<CreateJobApplicationCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateJobApplicationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(CreateJobApplicationCommand request, CancellationToken cancellationToken)
        {
            var job = await _unitOfWork.Repository<Domain.Entities.Job>().GetByIdAsync(request.jobId);

            var application = new Domain.Entities.Application()
            {
                JobId = request.jobId,
                EmployeeId = request.employeeId,
                Status = Domain.Enums.ApplicationStatus.InReview,
                AppliedAt = DateTime.UtcNow
            };

            job.Applications.Add(application);

            job.NumberOfWorkers -= 1;

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                _unitOfWork.Repository<Domain.Entities.Job>().Update(job);

                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                return application.Id;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
    }
}