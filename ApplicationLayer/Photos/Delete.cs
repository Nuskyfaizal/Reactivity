using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationLayer.Core;
using ApplicationLayer.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PersistenceLayer;

namespace ApplicationLayer.Photos
{
    public class Delete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IPhotoAccessor _photoAccessor;
            private readonly IUserAccesor _userAccesor;
            public Handler(DataContext context, IPhotoAccessor photoAccessor, IUserAccesor userAccesor)
            {
                _userAccesor = userAccesor;
                _photoAccessor = photoAccessor;
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.Include(p => p.Photos)
                    .FirstOrDefaultAsync(x => x.UserName == _userAccesor.GetUsername());

                if (user == null) return null;

                var photo = user.Photos.FirstOrDefault(x => x.Id == request.Id);

                if(photo == null) return null;

                if(photo.IsMain) return Result<Unit>.Failure("You cannot delelet your main photo");

                var result = await _photoAccessor.DeletePhoto(photo.Id);

                if(result == null) return Result<Unit>.Failure("Problem deleting photo from Cloudinary");

                //user.Photos.Remove(photo);
                _context.Photos.Remove(photo);

                var success = await _context.SaveChangesAsync() > 0;

                if(success) return Result<Unit>.Success(Unit.Value);

                return Result<Unit>.Failure("Problem deleting photo from API");
            }
        }
    }
}