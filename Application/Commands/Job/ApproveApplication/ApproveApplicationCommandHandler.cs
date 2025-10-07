using MediatR;
using Microsoft.EntityFrameworkCore;
using ZaposliMe.Domain.Generic;

namespace ZaposliMe.Application.Commands.Job.ApproveApplication
{
    public class ApproveApplicationCommandHandler : IRequestHandler<ApproveApplicationCommand>
    {

        private readonly IUnitOfWork _unitOfWork;

        public ApproveApplicationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(ApproveApplicationCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var job = (await _unitOfWork.Repository<Domain.Entities.Job>()
                    .FindAsync(j => j.Id == request.JobId,
                        include: q => q.Include(j => j.Applications)))
                    .FirstOrDefault();


                if (job == null)
                    throw new KeyNotFoundException($"Job with Id {request.JobId} not found.");

                var application = job.Applications?.FirstOrDefault(a => a.Id == request.ApplicationId);

                if (application == null)
                    throw new KeyNotFoundException($"Application with Id {request.ApplicationId} not found.");

                application.Status = Domain.Enums.ApplicationStatus.Submitted;
                application.StatusChangedAt = DateTime.UtcNow;

                _unitOfWork.Repository<Domain.Entities.Job>().Update(job);

                await _unitOfWork.CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
    }
}
