using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Services;
using CloudinaryDotNet.Actions;
using System.Threading.Tasks;

namespace Webminux.Optician
{
    public interface IMediaHelperService:IDomainService
    {
        Task<MediaUploadDto> AddMediaAsync(string base64String);
        Task<DeletionResult> DeleteImageAsync(string logoPublicId);
    }
}
