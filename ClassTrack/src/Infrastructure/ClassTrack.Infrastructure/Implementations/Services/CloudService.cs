using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Services;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Infrastructure.Implementations.Services
{
    internal class CloudService:ICloudService
    {
        private readonly Cloudinary _cloudinary;

        public CloudService(IConfiguration configuration)
        {
            _cloudinary = new Cloudinary(new Account(
                configuration["CloudService:CloudName"],
                configuration["CloudService:ApiKey"],
                configuration["CloudService:ApiSecret"]));         
        }

        public async Task<CloudinaryResponceDTO> UploadAsync(IFormFile file)
        {

            try
            {
                using var stream = file.OpenReadStream();

                var uploadParams = new AutoUploadParams
                {
                    File = new FileDescription(file.Name, stream),
                    Folder = "TaskWorks",
                    UseFilename = true,
                    UniqueFilename = true
                };
               
                RawUploadResult responce = await _cloudinary.UploadAsync(uploadParams);
               
                return new CloudinaryResponceDTO(responce.PublicId, responce.SecureUrl.ToString(),responce.ResourceType);
            }
            catch (Exception e)
            {
                throw new BadHttpRequestException("The File not Uploaded! Try Later.", e);
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
                    ResourceType = ResourceType.Raw
                };

                DeletionResult result = await _cloudinary.DestroyAsync(param);

                return result.Result;
            }
            catch (Exception e)
            {
                throw new BadHttpRequestException("The File not Deleted! Try Later.", e);
            }          
        }
    }
}
