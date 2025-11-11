using MediatR;
using Microsoft.EntityFrameworkCore;
using ZaposliMe.Persistance;

namespace ZaposliMe.Application.Commands.User.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly UserManagementDbContext _context;
        public DeleteUserCommandHandler(UserManagementDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id.Equals(request.Id));

                if (user == null)
                    throw new KeyNotFoundException($"User with Id {request.Id} not found.");

                user.IsDeleted = true;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}
