using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain.Utilities;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.ObjectModel;

namespace ClassTrack.Infrastructure.Implementations.Services
{
    internal class FileService : IFileService
    {
        private readonly Cloudinary _cloudinary;

        public FileService(IOptions<CloudSettings> cloudConfig)
        {
            Account account = new Account
                (
                 cloudConfig.Value.Name,
                 cloudConfig.Value.Key,
                 cloudConfig.Value.Secret
                 );

            _cloudinary = new Cloudinary(account);
        }

        public async Task<CloudinaryResponceDTO> UploadAsync(IFormFile file)
        {

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new AutoUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        Folder = "TaskWorks",
                        UniqueFilename = true
                    };

                    RawUploadResult responce = await _cloudinary
                                                    .UploadAsync(uploadParams,"auto");

                    return new CloudinaryResponceDTO(file.FileName, responce.PublicId,
                                                        responce.SecureUrl.ToString(), responce.ResourceType);
                };
            }
            catch (Exception)
            {
                throw new InternalServerException("The File not Uploaded! Try Later");
            }
        }

        public async Task<ICollection<CloudinaryResponceDTO>> UploadManyAsync(ICollection<IFormFile> files)
        {
            ICollection<CloudinaryResponceDTO> responces = new Collection<CloudinaryResponceDTO>();

            foreach (IFormFile file in files)
            {
                responces.Add(await UploadAsync(file));
            }

            return responces;
        }

        public async Task<string> DeleteFileAsync(string publicId)
        {
            try
            {
                var param = new DeletionParams(publicId)
                {
                    ResourceType = ResourceType.Auto
                };

                DeletionResult result = await _cloudinary.DestroyAsync(param);

                return result.Result;
            }
            catch (Exception)
            {
                throw new InternalServerException("The File not Deleted! Try Later.");
            }
        }
    }
}
