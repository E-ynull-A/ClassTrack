using ClassTrack.Application.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTrack.Application.Interfaces.Services
{
    public interface ICloudService
    {
        Task<CloudinaryResponceDTO> UploadAsync(IFormFile file);
        Task<string> DeleteFileAsync(string publicId);
        Task<ICollection<CloudinaryResponceDTO>> UploadManyAsync(ICollection<IFormFile> files);
    }
}
