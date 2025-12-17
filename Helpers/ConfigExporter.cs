using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace llama.cpp_models_preset_manager.Helpers
{
    public class ConfigExporter
    {
        public static void Export(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
                return;

            using (StreamWriter sw = new StreamWriter(file, false))
            {
                foreach (var model in ServiceModel.Instance.GetAiModels())
                {
                    sw.WriteLine($"[{model.Name}]");
                    sw.WriteLine($"model = {model.Path}");
                    foreach (var flag in ServiceModel.Instance.GetAiModelFlags(model))
                    {
                        sw.Write(flag.Flag);
                        if (!string.IsNullOrEmpty(flag.FlagValue))
                            sw.Write(" = " + flag.FlagValue);
                        sw.WriteLine();
                    }
                    sw.WriteLine();
                }
            }
        }
    }
}
