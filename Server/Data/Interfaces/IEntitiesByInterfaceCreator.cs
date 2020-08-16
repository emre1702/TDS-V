namespace TDS_Server.Data.Interfaces
{
    public interface IEntitiesByInterfaceCreator
    {
        TInterface Create<TInterface>(params object[] parameters) where TInterface : class;
    }
}
