using Abp.Application.Services.Dto;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Webminux.Optician.CommonDtos;

namespace Webminux.Optician
{
    public class MediaHelperService : IMediaHelperService
    {
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;
        private static readonly HashSet<string> imageFileExtensions = new HashSet<string>
        {
        "jpg", "jpeg", "png", "gif", "bmp", "tiff", "webp", "svg"
        };

        private static readonly HashSet<string> videoFileExtensions = new HashSet<string>
        {
        "mp4", "avi", "mov", "wmv", "mkv"
        };

        private static readonly HashSet<string> audioFileExtensions = new HashSet<string>
        {
        "mp3", "wav", "ogg", "flac", "aac", "wma"
        };

        private static readonly HashSet<string> documentFileExtensions = new HashSet<string>
        {
        "pdf", "doc", "docx", "xls", "xlsx", "ppt", "pptx", "txt", "rtf", "csv"
        };

        public MediaHelperService(
            IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _cloudinaryConfig = cloudinaryConfig;

            Account acc = new Account
            (
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }
        public async Task<MediaUploadDto> AddMediaAsync(string base64String)
        {
            var uploadResult = new RawUploadResult();

            if (string.IsNullOrWhiteSpace(base64String) == false)
            {
                var fileType = GetFileType(base64String);
                var fileName = string.Format("{0}.{1}", DateTime.Now.ToLongTimeString(), fileType);
                var mediaType = GetMediaType(base64String);
                if (mediaType == MediaType.Video)
                {
                    base64String = base64String.Substring(base64String.IndexOf(',') + 1);
                    var videoBuffer = Convert.FromBase64String(base64String);
                    using (var stream = new MemoryStream(videoBuffer))
                        uploadResult = await UploadVideoToCloudinary(fileName, stream);

                }
                else
                {
                    base64String = base64String.Substring(base64String.IndexOf(',') + 1);
                    var imageBuffer = Convert.FromBase64String(base64String);
                    using (var stream = new MemoryStream(imageBuffer))
                        uploadResult = await uploadImageToCloudinary(fileName, stream);
                }
            }

            var imageUploadResult = new MediaUploadDto();
            imageUploadResult.Url = uploadResult.Url == null ? string.Empty : uploadResult.Url.ToString();
            imageUploadResult.PublicId = string.IsNullOrWhiteSpace(uploadResult.PublicId) ? string.Empty : uploadResult.PublicId;

            return imageUploadResult;
        }

        private async Task<ImageUploadResult> uploadImageToCloudinary(string fileName, MemoryStream stream)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(fileName, stream),
            };

            return await _cloudinary.UploadAsync(uploadParams);
        }

        private async Task<VideoUploadResult> UploadVideoToCloudinary(string fileName, MemoryStream stream)
        {
            var uploadParams = new VideoUploadParams()
            {
                File = new FileDescription(fileName, stream),
                Transformation = new Transformation().VideoCodec("h264").AudioCodec("aac")
            };

            return await _cloudinary.UploadAsync(uploadParams);
        }

        public async Task<DeletionResult> DeleteImageAsync(string logoPublicId)
        {
            var result = new DeletionResult();
            if (string.IsNullOrWhiteSpace(logoPublicId) == false)
                result = await CreateDeleteParamsAndCallDestroyMethod(logoPublicId, result);

            return result;
        }

        private async Task<DeletionResult> CreateDeleteParamsAndCallDestroyMethod(string logoPublicId, DeletionResult result)
        {
            var deleteParams = new DeletionParams(logoPublicId);

            result = await _cloudinary.DestroyAsync(deleteParams);
            ThrowExceptionIfResultHasError(result);
            return result;
        }

        private static void ThrowExceptionIfResultHasError(DeletionResult result)
        {
            if (result.Error != null)
                throw new Exception(result.Error.Message);
        }

        private MediaType GetMediaType(string base64String)
        {
            var type = GetFileType(base64String);
            if (imageFileExtensions.Contains(type))
            {
                return MediaType.Image;
            }
            else if (videoFileExtensions.Contains(type))
            {
                return MediaType.Video;
            }
            else if (audioFileExtensions.Contains(type))
            {
                return MediaType.Audio;
            }
            else if (documentFileExtensions.Contains(type))
            {
                return MediaType.Document;
            }
            else
            {
                return MediaType.Other;
            }

        }

        private string GetFileType(string base64String)
        {
            var fromIndex = base64String.IndexOf('/') + 1;
            var toIndex = base64String.IndexOf(';');
            var length = toIndex - fromIndex;
            var fileType = base64String.Substring(fromIndex, length);
            return fileType;
        }
    }
}
