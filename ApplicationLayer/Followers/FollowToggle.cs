using System.Threading;
using System.Threading.Tasks;
using ApplicationLayer.Core;
using ApplicationLayer.Interfaces;
using DomainLayer;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PersistenceLayer;

namespace ApplicationLayer.Followers
{
    public class FollowToggle
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string TargetUserName { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IUserAccesor _userAccesor;
            public Handler(DataContext context, IUserAccesor userAccesor)
            {
                _userAccesor = userAccesor;
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var observer = await _context.Users
                    .FirstOrDefaultAsync(x => x.UserName == _userAccesor.GetUsername());

                var target = await _context.Users
                    .FirstOrDefaultAsync(x => x.UserName == request.TargetUserName);

                if(target == null) return null;

                var following = await _context.UserFollowings
                    .FindAsync(observer.Id, target.Id);

                if(following == null)
                {
                    following = new UserFollowing
                    {
                        Observer = observer,
                        Target = target
                    };

                    _context.UserFollowings.Add(following);
                }
                else
                {
                    _context.Remove(following);
                }

                var success = await _context.SaveChangesAsync() > 0;

                if(success) return Result<Unit>.Success(Unit.Value);

                return Result<Unit>.Failure("Failed to update following");
            }
        }
    }
}