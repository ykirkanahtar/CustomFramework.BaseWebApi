using System;
using CustomFramework.BaseWebApi.Contracts;

namespace CustomFramework.BaseWebApi.Data.Models
{
    public interface IBaseModelNonUser<TKey>
    {
        TKey Id { get; set; }
        DateTime CreateDateTime { get; set; }
        DateTime? UpdateDateTime { get; set; }
        DateTime? DeleteDateTime { get; set; }
        int? CreateClientApplicationId { get; set; }
        int? UpdateClientApplicationId { get; set; }
        int? DeleteClientApplicationId { get; set; }
        Status Status { get; set; }
    }
}