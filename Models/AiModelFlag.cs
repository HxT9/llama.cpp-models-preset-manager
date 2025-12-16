using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace llama.cpp_models_preset_manager.Models
{
    public class AiModelFlag
    {
        public int Id { get; set; }
        public AiModel AIModel { get; set; }
        public string Flag { get; set; }
        public string? FlagValue { get; set; }
    }
}
