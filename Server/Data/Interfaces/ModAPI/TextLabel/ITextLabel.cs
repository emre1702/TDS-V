namespace TDS_Server.Data.Interfaces.ModAPI.TextLabel
{
    public interface ITextLabel
    {
        System.Drawing.Color Color { get; set; }
        string Text { get; set; }
        bool Seethrough { get; set; }
        float Range { get; set; }

        void Delete();
    }
}
