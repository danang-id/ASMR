//
// ASMR: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
// at 6/27/2021 8:46 PM
//
// MediaFileService.cs
//
using ASMR.Core.Entities;
using ASMR.Web.Data;
using ASMR.Web.Services.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ASMR.Web.Extensions;
using Microsoft.AspNetCore.Http;

namespace ASMR.Web.Services
{
    public interface IMediaFileService : IServiceBase
    {
        public Task<MediaFile> GetMediaFileById(string id);

        public Task<MediaFile> CreateMediaFile(MediaFile media);
        
        public Task<MediaFile> CreateMediaFile(User user, IFormFile formFile);

        public Task<MediaFile> CreateMediaFile(string userId, IFormFile formFile);

        public Task<MediaFile> RemoveMediaFile(string id);

    }

    public class MediaFileService : ServiceBase, IMediaFileService
    {
        public MediaFileService(ApplicationDbContext dbContext) : base(dbContext) {}
        
        public Task<MediaFile> GetMediaFileById(string id)
        {
            return DbContext.MediaFiles
                .Include(e => e.User)
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<MediaFile> CreateMediaFile(MediaFile mediaFile)
        {
            if (mediaFile.Id is null || mediaFile.Id == Guid.Empty.ToString())
            {
                mediaFile.Id = Guid.NewGuid().ToString();
            }
            await DbContext.MediaFiles.AddAsync(mediaFile);

            return mediaFile;
        }

        public Task<MediaFile> CreateMediaFile(User user, IFormFile formFile)
        {
            return CreateMediaFile(user.Id, formFile);
        }

        public async Task<MediaFile> CreateMediaFile(string userId, IFormFile formFile)
        {
            var mediaFile = await formFile.SaveResourceWithRandomNameAsync();
            mediaFile.UserId = userId;

            return await CreateMediaFile(mediaFile);
        }

        public async Task<MediaFile> RemoveMediaFile(string id)
        {
            var entity = await DbContext.MediaFiles
                .Include(e => e.User)
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();
            if (entity is null)
            {
                return null;
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), entity.Location);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            
            DbContext.MediaFiles.Remove(entity);

            return entity;
        }
    }
}
