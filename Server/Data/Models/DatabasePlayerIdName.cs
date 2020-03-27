namespace TDS_Server.Data.Models
{
    public class DatabasePlayerIdName
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public DatabasePlayerIdName(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
