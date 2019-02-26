
namespace PuntosColombia.MissingNumbers
{
    using AutoMapper;
    using PuntosColombia.MissingNumbers.Domain.Entities;
    using PuntosColombia.MissingNumbers.Models.Searchs;
    using PuntosColombia.MissingNumbers.Models.Security;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<User, UserViewModel>();
            CreateMap<UserViewModel, User>();

            CreateMap<Search, SearchViewModel>();
            CreateMap<SearchViewModel, Search>();
        }
    }
}
