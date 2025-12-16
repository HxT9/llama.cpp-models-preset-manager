using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace llama.cpp_models_preset_manager.Models
{
    public class AiModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public List<AiModelFlag> Flags { get; set; }

        public AiModel()
        {
            Flags = new List<AiModelFlag>();
        }
    }
}
