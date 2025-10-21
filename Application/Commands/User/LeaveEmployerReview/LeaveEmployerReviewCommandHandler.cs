using MediatR;
using ZaposliMe.Domain.Generic;

namespace ZaposliMe.Application.Commands.User.LeaveEmployerReview
{
    public class LeaveEmployerReviewCommandHandler : IRequestHandler<LeaveEmployerReviewCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public LeaveEmployerReviewCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(LeaveEmployerReviewCommand request, CancellationToken cancellationToken)
        {
            var review = new Domain.Entities.EmployerReview
            {
                Rating = request.Rating,
                EmployerId = request.EmployerId,
                Comment = request.Comment,
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };


            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                using var _ = _unitOfWork.EmployerReviews.AddAsync(review);

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
