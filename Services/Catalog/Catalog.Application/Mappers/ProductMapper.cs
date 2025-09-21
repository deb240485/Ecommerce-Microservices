﻿using AutoMapper;
using MongoDB.Driver;

namespace Catalog.Application.Mappers
{
    public static class ProductMapper
    {
        public static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() => 
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfile<ProductMappingProfile>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        }); 
        
        public static IMapper Mapper => Lazy.Value;
    }
}
