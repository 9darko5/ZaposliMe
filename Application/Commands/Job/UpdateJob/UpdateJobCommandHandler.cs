using MediatR;
using ZaposliMe.Domain.Generic;
using ZaposliMe.Domain.Entities;

namespace ZaposliMe.Application.Commands.Job.UpdateJob
{
    public class UpdateJobCommandHandler : IRequestHandler<UpdateJobCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateJobCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateJobCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var job = (await _unitOfWork.Repository<Domain.Entities.Job>()
                    .FindAsync(j => j.Id == request.Id))
                    .FirstOrDefault();

                if (job == null)
                    throw new KeyNotFoundException($"Job with Id {request.Id} not found.");

                job.Title = request.Title ?? job.Title;
                job.Description = request.Description ?? job.Description;
                job.NumberOfWorkers = request.NumberOfWorkers ?? job.NumberOfWorkers;

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
