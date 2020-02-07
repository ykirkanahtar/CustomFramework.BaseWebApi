using System;
using CustomFramework.BaseWebApi.Contracts;

namespace CustomFramework.BaseWebApi.Data.Models
{
    public abstract class BaseModelNonUser<TKey> : IBaseModelNonUser<TKey>
    {
        public TKey Id { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public DateTime? DeleteDateTime { get; set; }
        public int? CreateClientApplicationId { get; set; }
        public int? UpdateClientApplicationId { get; set; }
        public int? DeleteClientApplicationId { get; set; }
        public Status Status { get; set; }
    }
}