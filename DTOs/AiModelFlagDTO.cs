using llama.cpp_models_preset_manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace llama.cpp_models_preset_manager.DTOs
{
    public class AiModelFlagDTO
    {
        public int Id { get; set; }
        public int AiModelId { get; set; }
        public string Flag { get; set; }
        public string? FlagValue { get; set; }
    }
}
