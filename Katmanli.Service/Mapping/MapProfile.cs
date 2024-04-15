using AutoMapper;
using Katmanli.DataAccess.DTOs;
using Katmanli.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katmanli.Service.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<User, UserQuery>().ReverseMap();

        }
    }
}
