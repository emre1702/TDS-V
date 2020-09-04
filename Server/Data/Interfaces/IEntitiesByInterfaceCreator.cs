namespace TDS_Server.Data.Interfaces
{
    interface IEntitiesByInterfaceCreator
    {
        TInterface Create<TInterface>(params object[] parameters) where TInterface : class;
    }
}
