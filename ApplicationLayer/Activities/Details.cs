using ApplicationLayer.Core;
using ApplicationLayer.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DomainLayer;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PersistenceLayer;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationLayer.Activities
{
    public class Details
    {
        public class Query : IRequest<Result<ActivityDto>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ActivityDto>>
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

            public async Task<Result<ActivityDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities
                    .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider,
                        new {currentUserName = _userAccesor.GetUsername()})
                    .FirstOrDefaultAsync(x => x.Id == request.Id);

                return Result<ActivityDto>.Success(activity);
            }
        }
    }
}
