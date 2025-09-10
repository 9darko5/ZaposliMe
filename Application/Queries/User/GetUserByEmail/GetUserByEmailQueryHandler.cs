using MediatR;
using Microsoft.EntityFrameworkCore;
using ZaposliMe.Domain.ViewModels.User;
using ZaposliMe.Persistance;

namespace ZaposliMe.Application.Queries.User.GetUserByEmail
{
    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, UserGridView?>
    {
        private readonly UserManagementDbContext _context;

        public GetUserByEmailQueryHandler(UserManagementDbContext context)
        {
            _context = context;
        }

        public async Task<UserGridView?> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.UserGridViews.FirstOrDefaultAsync(x=> x.Email.Equals(request.Email));
            return user;
        }
    }
}
