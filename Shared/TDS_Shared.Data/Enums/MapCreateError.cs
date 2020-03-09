namespace TDS_Shared.Data.Enums
{
    public enum MapCreateError
    {
        MapCreatedSuccessfully = 0,
        CouldNotDeserialize = 1,
        NameAlreadyExists = 2,
        Cooldown = 3,
        Unknown = 9999
    }
}
