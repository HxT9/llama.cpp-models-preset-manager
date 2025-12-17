using AutoMapper;
using llama.cpp_models_preset_manager.DTOs;
using llama.cpp_models_preset_manager.Helpers;
using llama.cpp_models_preset_manager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyModel.Resolution;
using Microsoft.Extensions.Logging;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace llama.cpp_models_preset_manager
{
    public class ServiceModel
    {
        private static ServiceModel? _instance;
        private static readonly object _padlock = new object();

        public static ServiceModel Instance
        {
            get
            {
                lock (_padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new ServiceModel();
                    }
                    return _instance;
                }
            }
        }

        private readonly IMapper _mapper;

        public ServiceModel() {
            var cfg = new MapperConfigurationExpression();
            cfg.AddProfile<MappingProfile>();
            ILoggerFactory loggerFactory = new LoggerFactory();
            var mapperConfig = new MapperConfiguration(cfg, loggerFactory);
            _mapper = mapperConfig.CreateMapper();
        }

        public void UndoChanges()
        {
            DatabaseManager.UndoChanges();
        }

        public List<AiModelDTO> GetAiModels()
        {
            return _mapper.Map<List<AiModelDTO>>(DatabaseManager.Instance.DbContext.AIModel
                .AsNoTracking()
                .OrderBy(m => m.Name.ToUpper())
                .ToList());
        }

        public List<FlagDTO> GetFlags()
        {
            return _mapper.Map<List<FlagDTO>>(DatabaseManager.Instance.DbContext.Flag
                .AsNoTracking()
                .OrderBy(f => f.Name.ToUpper())
                .ToList());
        }

        public List<AiModelFlagDTO> GetAiModelFlags(AiModelDTO model)
        {
            var entities = DatabaseManager.Instance.DbContext.AIModelFlag
                .AsNoTracking()
                .Where(f => f.AiModelId == model.Id)
                .OrderBy(f => f.Flag.ToUpper())
                .ToList();

            return _mapper.Map<List<AiModelFlagDTO>>(entities);
        }

        public string? GetKV(string key)
        {
            string? v = DatabaseManager.Instance.DbContext.KVConfig.FirstOrDefault(kv => kv.Key == key)?.Value;
            if (string.IsNullOrWhiteSpace(v))
                return null;
            return v;
        }

        public void SaveKV(string key, string? value)
        {
            if (DatabaseManager.Instance.DbContext.KVConfig.FirstOrDefault(kv => kv.Key == key) != null)
                DatabaseManager.Instance.DbContext.KVConfig.First(kv => kv.Key == key).Value = value;
            else
                DatabaseManager.Instance.DbContext.KVConfig.Add(new KVConfig() { Key = key, Value = value });
            DatabaseManager.Instance.DbContext.SaveChanges();
        }

        public void SaveAiModel(AiModelDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Path))
                throw new ArgumentException("Name and Path are required");

            var entity = _mapper.Map<AiModel>(dto);
            DatabaseManager.Upsert(entity);
            dto.Id = entity.Id;
        }

        public void SaveFlag(FlagDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Name is required");

            var entity = _mapper.Map<Flag>(dto);
            DatabaseManager.Upsert(entity);
            dto.Id = entity.Id;
        }

        public void SaveAiModelFlag(AiModelFlagDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Flag))
                throw new ArgumentException("Flag is required");

            if (!ServiceModel.Instance.GetFlags().Any(f => f.Name == dto.Flag))
                ServiceModel.Instance.SaveFlag(new FlagDTO() { Name = dto.Flag });

            var entity = _mapper.Map<AiModelFlag>(dto);
            DatabaseManager.Upsert(entity);
            dto.Id = entity.Id;
        }

        public void DeleteAiModel(AiModelDTO dto)
        {
            var entity = DatabaseManager.Instance.DbContext.AIModel.FirstOrDefault(e => e.Id == dto.Id);
            if (entity != null)
                DatabaseManager.Delete(entity);
        }

        public void DeleteAllAiModel()
        {
            DatabaseManager.Instance.DbContext.AIModel.RemoveRange(DatabaseManager.Instance.DbContext.AIModel);
            DatabaseManager.Instance.DbContext.SaveChanges();
        }

        public void DeleteFlag(FlagDTO dto)
        {
            var entity = DatabaseManager.Instance.DbContext.Flag.FirstOrDefault(e => e.Id == dto.Id);
            if (entity != null)
                DatabaseManager.Delete(entity);
        }

        public void DeleteAllFlag()
        {
            DatabaseManager.Instance.DbContext.Flag.RemoveRange(DatabaseManager.Instance.DbContext.Flag);
            DatabaseManager.Instance.DbContext.SaveChanges();
        }

        public void DeleteAiModelFlag(AiModelFlagDTO dto)
        {
            var entity = DatabaseManager.Instance.DbContext.AIModelFlag.FirstOrDefault(e => e.Id == dto.Id);
            if (entity != null)
                DatabaseManager.Delete(entity);
        }

        public void DeleteAllAiModelFlag(AiModelDTO dto)
        {
            DatabaseManager.Instance.DbContext.AIModelFlag.RemoveRange(DatabaseManager.Instance.DbContext.AIModelFlag.Where(f => f.AiModelId == dto.Id));
            DatabaseManager.Instance.DbContext.SaveChanges();
        }
    }
}
