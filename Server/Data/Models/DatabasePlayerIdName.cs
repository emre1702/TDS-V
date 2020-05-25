namespace TDS_Server.Data.Models
{
    public class DatabasePlayerIdName
    {
        #region Public Constructors

        public DatabasePlayerIdName(int id, string name)
        {
            Id = id;
            Name = name;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id { get; set; }
        public string Name { get; set; }

        #endregion Public Properties
    }
}
