namespace CustomFramework.BaseWebApi.Data.Models
{
    public abstract class BaseModel<TKey> : BaseModelNonUser<TKey>, IBaseModel<TKey>
    {
        public int CreateUserId { get; set; }
        public int? UpdateUserId { get; set; }
        public int? DeleteUserId { get; set; }
    }
}