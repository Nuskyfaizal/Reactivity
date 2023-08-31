using ApplicationLayer.Core;
using ApplicationLayer.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using PersistenceLayer;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationLayer.Activities
{
    public class List
    {
        public class Query : IRequest<Result<PagedList<ActivityDto>>> 
        {
            public ActivityParams Params { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<PagedList<ActivityDto>>>
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

            public async Task<Result<PagedList<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _context.Activities
                    .Where(d => d.Date >= request.Params.StartDate)
                    .OrderBy(d => d.Date)
                    .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider, 
                        new{currentUserName = _userAccesor.GetUsername()})
                    .AsQueryable();

                if(request.Params.IsGoing && !request.Params.IsHost)
                {
                    query = query.Where(x => x.Attendees.Any(a => a.Username == _userAccesor.GetUsername()));
                }

                if(request.Params.IsHost && !request.Params.IsGoing)
                {
                    query = query.Where(x => x.HostUsername == _userAccesor.GetUsername());
                }
                   
                return Result<PagedList<ActivityDto>>.Success(
                    await PagedList<ActivityDto>.CreateAsync(query, request.Params.PageNumber, request.Params.PageSize)
                );
            }
        }
    }
}
