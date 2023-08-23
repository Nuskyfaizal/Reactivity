using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationLayer.Core;
using ApplicationLayer.Interfaces;
using AutoMapper;
using DomainLayer;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PersistenceLayer;

namespace ApplicationLayer.Comments
{
    public class Create
    {
        public class Command : IRequest<Result<CommentsDto>>
        {
            public string Body { get; set; }
            public Guid ActivityId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Body).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, Result<CommentsDto>>
        {
            private readonly DataContext _context;
            private readonly IUserAccesor _userAccesor;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IUserAccesor userAccesor, IMapper mapper)
            {
                _mapper = mapper;
                _userAccesor = userAccesor;
                _context = context;
            }

            public async Task<Result<CommentsDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities.FindAsync(request.ActivityId);

                if(activity == null) return null;

                var user = await _context.Users.Include(p => p.Photos)
                    .SingleOrDefaultAsync(x => x.UserName == _userAccesor.GetUsername());

                var comment = new Comment
                {
                    Author = user,
                    Activity = activity,
                    Body = request.Body
                };

                activity.Comments.Add(comment);

                var success = await _context.SaveChangesAsync() > 0;

                if(success) return Result<CommentsDto>.Success(_mapper.Map<CommentsDto>(comment));

                return Result<CommentsDto>.Failure("Failed to add comment");
            }
        }
    }
}