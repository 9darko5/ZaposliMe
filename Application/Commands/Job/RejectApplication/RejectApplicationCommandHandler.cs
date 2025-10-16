using MediatR;
using Microsoft.EntityFrameworkCore;
using ZaposliMe.Domain.Generic;

namespace ZaposliMe.Application.Commands.Job.ApproveApplication
{
    public class RejectApplicationCommandHandler : IRequestHandler<RejectApplicationCommand>
    {

        private readonly IUnitOfWork _unitOfWork;

        public RejectApplicationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(RejectApplicationCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var job = (await _unitOfWork.Jobs
                    .FindAsync(j => j.Id == request.JobId,
                        include: q => q.Include(j => j.Applications)))
                    .FirstOrDefault();

                if (job == null)
                    throw new KeyNotFoundException($"Job with Id {request.JobId} not found.");

                var application = job.Applications?.FirstOrDefault(a => a.Id == request.ApplicationId);

                if (application == null)
                    throw new KeyNotFoundException($"Application with Id {request.ApplicationId} not found.");

                application.Status = Domain.Enums.ApplicationStatus.Rejected;
                application.StatusChangedAt = DateTime.UtcNow;

                _unitOfWork.Jobs.Update(job);

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
