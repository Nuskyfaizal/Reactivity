using System.Threading;
using System.Threading.Tasks;
using ApplicationLayer.Core;
using ApplicationLayer.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PersistenceLayer;

namespace ApplicationLayer.Profiles
{
    public class Details
    {
        public class Query : IRequest<Result<Profile>>
        {
            public string Username { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<Profile>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccesor _userAccesor;
            public Handler(DataContext context, IMapper mapper, IUserAccesor userAccesor)
            {
                _userAccesor = userAccesor;
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<Profile>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.Users
                    .ProjectTo<Profile>(_mapper.ConfigurationProvider,
                        new {currentUserName = _userAccesor.GetUsername()})
                    .SingleOrDefaultAsync(x => x.Username == request.Username);

                if (user == null) return null;

                return Result<Profile>.Success(user);
            }
        }
    }
}