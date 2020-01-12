namespace CustomFramework.BaseWebApi.Data.Contracts
{
    public class Sorting
    {
        public Sorting(string fieldName, bool ascending)
        {
            FieldName = fieldName;
            Ascending = ascending;
        }

        public string FieldName { get; set; }
        public bool Ascending { get; set; }
    }
}