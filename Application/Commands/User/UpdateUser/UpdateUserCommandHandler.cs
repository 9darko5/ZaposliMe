using MediatR;
using Microsoft.EntityFrameworkCore;
using ZaposliMe.Persistance;

namespace ZaposliMe.Application.Commands.User.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly UserManagementDbContext _context;
        public UpdateUserCommandHandler(UserManagementDbContext context)
        {
            _context = context;
        }

        public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {

            try
            {

                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id.Equals(request.Id));

                if (user == null)
                    throw new KeyNotFoundException($"User with Id {request.Id} not found.");

                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.PhoneNumber = request.PhoneNumber;
                user.Age = request.Age;

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
