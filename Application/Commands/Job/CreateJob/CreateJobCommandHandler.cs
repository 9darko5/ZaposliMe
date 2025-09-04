using MediatR;
using ZaposliMe.Domain.Generic;

namespace ZaposliMe.Application.Commands.Job.CreateJob
{
    public class CreateJobCommandHandler : IRequestHandler<CreateJobCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateJobCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(CreateJobCommand request, CancellationToken cancellationToken)
        {
            var job = new Domain.Entities.Job
            {
                Title = request.Title,
                Description = request.Description,
                NumberOfWorkers = request.NumberOfWorkers,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                using var _ = _unitOfWork.Repository<Domain.Entities.Job>().AddAsync(job);

                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                return job.Id;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
    }
}