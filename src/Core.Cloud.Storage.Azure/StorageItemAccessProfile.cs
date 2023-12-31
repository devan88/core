using AutoMapper;
using Azure.Storage.Sas;

namespace Core.Cloud.Storage.Azure
{
    public sealed class StorageItemAccessProfile : Profile
    {
        public StorageItemAccessProfile()
        {
            CreateMap<StorageItemAccess, BlobSasPermissions>()
                .ConvertUsing(src => MapStorageItemAccess(src));
        }

        private static BlobSasPermissions MapStorageItemAccess(StorageItemAccess storageItemAccess)
        {
            return storageItemAccess switch
            {
                StorageItemAccess.Read => BlobSasPermissions.Read,
                StorageItemAccess.Write => BlobSasPermissions.Write,
                _ => 0,
            };
        }
    }
}
