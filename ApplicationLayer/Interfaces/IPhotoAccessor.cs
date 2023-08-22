using System.Threading.Tasks;
using ApplicationLayer.Photos;
using Microsoft.AspNetCore.Http;

namespace ApplicationLayer.Interfaces
{
    public interface IPhotoAccessor
    {
        Task<PhotoUploadResult> AddPhoto(IFormFile file);
        Task<string> DeletePhoto(string publicId);
    }
}