using TDS_Client.Data.Interfaces.ModAPI.Entity;

namespace TDS_Client.Data.Interfaces.ModAPI.Pool
{
    public interface IPoolEntityAPI<T> where T : IEntity
    {
        T GetAtRemote(ushort handleValue);
        T GetAtHandle(int targetEntity);
    }
}
