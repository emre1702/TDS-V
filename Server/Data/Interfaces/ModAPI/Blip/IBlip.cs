namespace TDS_Server.Data.Interfaces.ModAPI.Blip
{
    public interface IBlip
    {
        string Name { get; set; }
        uint Sprite { get; set; }

        void Delete();
    }
}
