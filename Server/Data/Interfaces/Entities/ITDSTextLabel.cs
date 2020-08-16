namespace TDS_Server.Data.Interfaces.Entities
{
    public interface ITDSTextLabel
    {
        string Text { get; set; }

        void Delete();
    }
}
