using MediatR;
using Microsoft.EntityFrameworkCore;
using ZaposliMe.Domain.Generic;

namespace ZaposliMe.Application.Commands.Job.DeleteJob
{
    public class UpdateJobCommandHandler : IRequestHandler<DeleteJobCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateJobCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(DeleteJobCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var job = (await _unitOfWork.Jobs
                    .FindAsync(j => j.Id == request.Id,
                        include: q => q.Include(j => j.Applications)))
                    .FirstOrDefault();

                if (job == null)
                {
                    throw new KeyNotFoundException($"Job with id {request.Id} was not found.");
                }
                await _unitOfWork.BeginTransactionAsync(cancellationToken);
                                
                _unitOfWork.Jobs.Remove(job);

                await _unitOfWork.CommitTransactionAsync(cancellationToken);
            }
            catch(Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw; 
            }
        }

    }
}
