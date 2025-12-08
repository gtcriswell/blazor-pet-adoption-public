using System.Text.Json.Serialization;

namespace DTO.API
{

    public class SearchData
    {
        public FilterRadius filterRadius { get; set; } = new();
    }

    public class FilterRadius
    {
        public int miles { get; set; } = 25;
        public string postalcode { get; set; } = string.Empty;
        [JsonIgnore]
        public string? species { get; set; } = "haspic";
    }

    public class SearchParams
    {
        public SearchData data { get; set; } = new();
    }
}
