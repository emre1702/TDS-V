using System;

namespace TDS.Server.Data.Interfaces.GangsSystem
{
#nullable enable
    public interface IGangChat
    {
        void SendMessage(Func<ILanguage, string> langGetter);
        void SendNotification(Func<ILanguage, string> langGetter);
    }
}