namespace TDS_Server.Data.Interfaces.Entities
{
    public interface ITDSBlip
    {
        byte Color { get; set; }
        string Name { get; set; }

        void Delete();
    }
}
