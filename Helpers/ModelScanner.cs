using llama.cpp_models_preset_manager.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace llama.cpp_models_preset_manager.Helpers
{
    public class ModelScanner
    {
        /*
         * Returns a name ParentFolder_GGUFFilename
         * "C:\AI\Models\GGUFs\Google Gemma 3\Q4_K_M.gguf" -> "Google-Gemma-3_Q4_K_M"
         * If parentFolder is valid, all directories from parentFolder till the file will be included in the name
         */
        public static string getNameFromGGUFPath(string ggufFile, string parentFolder = "")
        {
            FileInfo gf = new FileInfo(ggufFile);

            string name = ggufFile;
            if (string.IsNullOrWhiteSpace(parentFolder) && gf.Directory != null && gf.Directory.Parent != null)
                parentFolder = gf.Directory.Parent.FullName;
            if (!string.IsNullOrWhiteSpace(parentFolder))
                name = ggufFile.Substring(parentFolder.Length + 1);
            name = name.Replace(" ", "-");
            name = name.Replace("\\", "_");
            name = name.Substring(0, name.Length - gf.Extension.Length);

            return name;
        }

        public static List<AiModelDTO> ScanAndAddModels(string folder)
        {
            List<AiModelDTO> ret = new List<AiModelDTO>();

            if (string.IsNullOrWhiteSpace(folder) || !Directory.Exists(folder))
                return ret;

            var ggufs = Directory.GetFiles(folder, "*.gguf", SearchOption.AllDirectories);
            foreach (var ggufFile in ggufs)
            {
                FileInfo file = new FileInfo(ggufFile);
                Match match = Regex.Match(file.Name, "(\\d+)-of-\\d+\\.gguf");
                if (match.Success && match.Groups.Count > 1 && int.Parse(match.Groups[1].Value) > 1)
                    continue;

                if (file.Name.ToLower().StartsWith("mmproj"))
                    continue;

                if (DatabaseManager.Instance.DbContext.AIModel.Any(m => m.Path == file.FullName))
                    continue;

                AiModelDTO m = new AiModelDTO() { Name = file.FullName, Path = file.FullName };
                m.Name = getNameFromGGUFPath(file.FullName, folder);
                ServiceModel.Instance.SaveAiModel(m);

                ret.Add(m);
            }

            return ret;
        }
    }
}
