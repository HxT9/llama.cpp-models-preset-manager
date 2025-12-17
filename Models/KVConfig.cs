using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace llama.cpp_models_preset_manager.Models
{
    public class KVConfig
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string? Value { get; set; }
    }
}
