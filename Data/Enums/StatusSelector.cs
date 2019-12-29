namespace CustomFramework.BaseWebApi.Data.Enums
{
    public enum StatusSelector
    {
        OnlyActives = 1,
        OnlyPassives = 2,
        OnlyDeleted = 3,
        ActivesAndPassives = 4,
        PassivesAndDeleted = 5,
        ActivesAndDeleted = 6,
        All= 7,
    }
}