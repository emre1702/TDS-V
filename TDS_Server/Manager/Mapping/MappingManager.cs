using AutoMapper;
using GTANetworkAPI;
using System;
using System.Threading.Tasks;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Mapping.Converter;
using TDS_Server_DB.Entity;

namespace TDS_Server.Manager.Mapping
{
    class MappingManager
    {
        public static void Init()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<string, string>().ConvertUsing(a => a);
                cfg.CreateMap<string, char>().ConvertUsing(a => a[0]);
                cfg.CreateMap<string, int>().ConvertUsing(str => Convert.ToInt32(str));
                cfg.CreateMap<string, float>().ConvertUsing(str => Convert.ToSingle(str));
                cfg.CreateMap<string, double>().ConvertUsing(str => Convert.ToDouble(str));
                cfg.CreateMap<string, bool>().ConvertUsing(str => str.Equals("true", StringComparison.CurrentCultureIgnoreCase) || str == "1");

                cfg.CreateMap<string, DateTime?>().ConvertUsing<StringToDateTimeConverter>();

                cfg.CreateMap<string, TDSPlayer?>().ConvertUsing<StringNameToPlayerConverter>();
                cfg.CreateMap<string, Client?>().ConvertUsing<StringNameToClientConverter>();
                cfg.CreateMap<string, Task<Players?>>().ConvertUsing<StringNameToDBPlayerConverter>();
            });
            Mapper.AssertConfigurationIsValid();
        }

        public static Type GetCorrectDestType(Type sourceType)
        {
            if (sourceType == typeof(Players))
                return typeof(Task<Players?>);
            return sourceType;
        }
    }
}
