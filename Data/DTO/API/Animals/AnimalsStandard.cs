using Shared;

namespace Data.DTO.API.Animals
{
    public class AnimalsStandard
    {
        public string LocationName { get; set; } = string.Empty;
        public string WebsiteUrl { get; set; } = string.Empty;
        public string FacebookUrl { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Breed { get; set; } = string.Empty;
        public double MilesAway { get; set; } = 0;
        public string PhotoUrlSmall { get; set; } = string.Empty;
        public string PhotoUrlLarge { get; set; } = string.Empty;
        public string Id { get; set; } = "";

        public string PhoneNumberString => PhoneNumber.ToPhoneNumber();

        public string FacebookUrlString
        {
            get
            {
                string tmp = FacebookUrl.Replace("https://", "");
                if (string.IsNullOrEmpty(tmp))
                {
                    return string.Empty;
                }
                tmp = FacebookUrl.Replace("http://", "");
                return string.IsNullOrEmpty(tmp) ? string.Empty : FacebookUrl;
            }
        }
        public string WebsiteUrlString
        {
            get
            {
                string tmp = WebsiteUrl.Replace("https://", "");
                if (string.IsNullOrEmpty(tmp))
                {
                    return string.Empty;
                }
                tmp = WebsiteUrl.Replace("http://", "");
                return string.IsNullOrEmpty(tmp) ? string.Empty : WebsiteUrl;
            }
        }
    }
}
