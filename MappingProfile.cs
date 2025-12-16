using AutoMapper;
using llama.cpp_models_preset_manager.DTOs;
using llama.cpp_models_preset_manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace llama.cpp_models_preset_manager
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AiModel, AiModelDTO>().ReverseMap();
            CreateMap<AiModelFlag, AiModelFlagDTO>().ReverseMap().ForMember(f => f.AiModel, opt => opt.Ignore());
            CreateMap<Flag,  FlagDTO>().ReverseMap();
        }
    }
}
