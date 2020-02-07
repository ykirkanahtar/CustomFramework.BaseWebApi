namespace CustomFramework.BaseWebApi.Data.Models
{
    public interface IBaseModel<TKey> : IBaseModelNonUser<TKey>
    {
        int CreateUserId { get; set; }
        int? UpdateUserId { get; set; }
        int? DeleteUserId { get; set; }
    }
}