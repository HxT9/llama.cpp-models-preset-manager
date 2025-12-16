using llama.cpp_models_preset_manager.DTOs;
using llama.cpp_models_preset_manager.Helpers;
using llama.cpp_models_preset_manager.Models;
using Microsoft.Extensions.DependencyModel.Resolution;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace llama.cpp_models_preset_manager
{
    public class DatabaseService
    {
        public static AiModelDTO GetAiModel(AiModel aiModel)
        {
            var ret = new AiModelDTO() { Id = aiModel.Id, Name = aiModel.Name, Path = aiModel.Path };
            return ret;
        }

        public static List<AiModelDTO> GetAiModels()
        {
            List<AiModelDTO> ret = new List<AiModelDTO>();
            var _context = new DbContext();
            _context.AIModel.ToList().ForEach(m => ret.Add(GetAiModel(m)));
            return ret;
        }

        public static FlagDTO GetFlag(Flag flag)
        {
            return new FlagDTO() { Id = flag.Id, Name = flag.Name, Description = flag.Description };
        }

        public static List<FlagDTO> GetFlags()
        {
            List<FlagDTO> ret = new List<FlagDTO>();
            var _context = new DbContext();
            _context.Flag.ToList().ForEach(f => ret.Add(GetFlag(f)));
            return ret;
        }

        public static AiModelFlagDTO GetAiModelFlag(AiModelFlag aiModelFlag)
        {
            return new AiModelFlagDTO() { Id = aiModelFlag.Id, Flag = aiModelFlag.Flag, AiModelId = aiModelFlag.AIModelId };
        }

        public static List<AiModelFlagDTO> GetAiModelFlags(AiModelDTO model)
        {
            List<AiModelFlagDTO> ret = new List<AiModelFlagDTO>();
            var _context = new DbContext();
            _context.AIModelFlag.Where(f => f.AIModelId == model.Id).ToList().ForEach(f => ret.Add(GetAiModelFlag(f)));
            return ret;
        }

        public static int SaveAiModel(AiModelDTO dto)
        {
            AiModel _entity = new AiModel() { Id = dto.Id, Name = dto.Name, Path = dto.Path };
            DatabaseManager.Upsert(_entity);
            return _entity.Id;
        }

        public static int SaveAiModelFlag(AiModelFlagDTO dto)
        {
            AiModelFlag _entity = new AiModelFlag() { Id = dto.Id, AIModelId = dto.AiModelId, Flag = dto.Flag, FlagValue = dto.FlagValue };
            DatabaseManager.Upsert(_entity);
            return _entity.Id;
        }

        public static int SaveFlag(FlagDTO dto)
        {
            Flag _entity = new Flag() { Id = dto.Id, Name = dto.Name, Description = dto.Description };
            DatabaseManager.Upsert(_entity);
            return _entity.Id;
        }

        private static void Delete<T>(T? entity) where T : class
        {
            if (entity != null)
                DatabaseManager.Delete(entity);
        }

        public static void DeleteAiModel(AiModelDTO dto)
        {
            var _context = new DbContext();
            var _entity = _context.AIModel.FirstOrDefault(e => e.Id == dto.Id);
            Delete(_entity);            
        }

        public static void DeleteAiModelFlag(AiModelFlagDTO dto)
        {
            var _context = new DbContext();
            var _entity = _context.AIModelFlag.FirstOrDefault(e => e.Id == dto.Id);
            Delete(_entity);
        }

        public static void DeleteFlag(FlagDTO dto)
        {
            var _context = new DbContext();
            var _entity = _context.Flag.FirstOrDefault(e => e.Id == dto.Id);
            Delete(_entity);
        }
    }
}
