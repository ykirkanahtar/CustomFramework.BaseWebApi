using Microsoft.EntityFrameworkCore;

namespace CustomFramework.BaseWebApi.Data.Models
{
    public class EntityChange
    {
        public string EntityName { get; set; }
        public string FieldName { get; set; }
        public long? IdValue { get; set; }
        public EntityState EntityState { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}