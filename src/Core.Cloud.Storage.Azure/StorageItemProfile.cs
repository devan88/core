using AutoMapper;
using Azure.Storage.Blobs.Models;

namespace Core.Cloud.Storage.Azure
{
    public sealed class StorageItemProfile : Profile
    {
        public StorageItemProfile() 
        {
            CreateMap<BlobDownloadStreamingResult, StorageItem>()
                .ForMember(d => d.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(d => d.ContentType, opt => opt.MapFrom(src => src.Details.ContentType));
        }
    }
}
