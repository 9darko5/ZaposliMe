using MediatR;
using Microsoft.EntityFrameworkCore;
using ZaposliMe.Application.Commands.Job.CreateJob;
using ZaposliMe.Domain.Entities;
using ZaposliMe.Domain.Generic;
using ZaposliMe.Domain.Interfaces;

public class CreateJobApplicationCommandHandler
    : IRequestHandler<CreateJobApplicationCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateJobApplicationCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateJobApplicationCommand request, CancellationToken ct)
    {
        await _unitOfWork.BeginTransactionAsync(ct);
        try
        {
            // If you want capacity logic, set reserveSlot:true
            var app = await _unitOfWork.Jobs.AddApplicationAsync(
                request.JobId,
                request.EmployeeId,
                cancellationToken: ct);

            await _unitOfWork.CommitTransactionAsync(ct);

            return app.Id;
        }
        catch (DbUpdateConcurrencyException)
        {
            await _unitOfWork.RollbackTransactionAsync(ct);
            throw;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(ct);
            throw;
        }
    }
}
