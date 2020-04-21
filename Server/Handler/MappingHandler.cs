using AutoMapper;
using System;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Converter.Mapping;
using TDS_Server.Handler.Player;

namespace TDS_Server.Handler
{
    public class MappingHandler
    {
        public IMapper Mapper { get; set; }

        public MappingHandler(TDSPlayerHandler tdsPlayerHandler)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<string, string>().ConvertUsing(a => a);
                cfg.CreateMap<string, char>().ConvertUsing(a => a[0]);
                cfg.CreateMap<string, int>().ConvertUsing(str => Convert.ToInt32(str));
                cfg.CreateMap<string, float>().ConvertUsing(str => Convert.ToSingle(str));
                cfg.CreateMap<string, double>().ConvertUsing(str => Convert.ToDouble(str));
                cfg.CreateMap<string, bool>().ConvertUsing(str => 
                        str.Equals("true", StringComparison.CurrentCultureIgnoreCase) || str == "1" || str.Equals("yes", StringComparison.CurrentCultureIgnoreCase));

                cfg.CreateMap<string, DateTime?>().ConvertUsing<StringToDateTimeConverter>();

                cfg.CreateMap<string, ITDSPlayer?>().ConvertUsing(new StringNameToPlayerConverter(tdsPlayerHandler));
                cfg.CreateMap<string, Task<Players?>>().ConvertUsing<StringNameToDBPlayerConverter>();

                cfg.CreateMap<IPlayer, ITDSPlayer?>().ConvertUsing(new IPlayerToITDSPlayerConverter(tdsPlayerHandler));
            });
            config.AssertConfigurationIsValid();

            Mapper = config.CreateMapper();
        }

        public Type GetCorrectDestType(Type sourceType)
        {
            if (sourceType == typeof(Players))
                return typeof(Task<Players?>);
            if (sourceType == typeof(DateTime))
                return typeof(DateTime?);
            return sourceType;
        }
    }
}
