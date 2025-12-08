using Shared;

namespace Data.DTO.API.Orgs
{
    public partial class OrgStandard : Attributes
    {

        public string AddressString
        {
            get
            {
                string?[] parts = new[]{
                            street,
                            city,
                            state
                        };

                string address = string.Join(", ", parts.Where(p => !string.IsNullOrWhiteSpace(p)));
                return address;
            }
        }

        public string PhoneNumberString => phone.ToPhoneNumber();

        public string FacebookUrlString
        {
            get
            {
                if (!string.IsNullOrEmpty(facebookUrl))
                {
                    string tmp = facebookUrl.Replace("https://", "");
                    if (string.IsNullOrEmpty(tmp))
                    {
                        return string.Empty;
                    }
                    tmp = facebookUrl.Replace("http://", "");
                    return string.IsNullOrEmpty(tmp) ? string.Empty : facebookUrl;
                }
                return string.Empty;
            }
        }
        public string AdoptionUrlString
        {
            get
            {
                if (!string.IsNullOrEmpty(adoptionUrl))
                {
                    string tmp = adoptionUrl.Replace("https://", "");
                    if (string.IsNullOrEmpty(tmp))
                    {
                        return string.Empty;
                    }
                    tmp = adoptionUrl.Replace("http://", "");
                    return string.IsNullOrEmpty(tmp) ? string.Empty : adoptionUrl;
                }
                return string.Empty;
            }
        }

        public string DonattionUrlString
        {
            get
            {
                if (!string.IsNullOrEmpty(donationUrl))
                {
                    string tmp = donationUrl.Replace("https://", "");
                    if (string.IsNullOrEmpty(tmp))
                    {
                        return string.Empty;
                    }
                    tmp = donationUrl.Replace("http://", "");
                    return string.IsNullOrEmpty(tmp) ? string.Empty : donationUrl;
                }
                return string.Empty;
            }
        }

        public string UrlString
        {
            get
            {
                if (!string.IsNullOrEmpty(url))
                {
                    string tmp = url.Replace("https://", "");
                    if (string.IsNullOrEmpty(tmp))
                    {
                        return string.Empty;
                    }
                    tmp = url.Replace("http://", "");
                    return string.IsNullOrEmpty(tmp) ? string.Empty : url;
                }
                return string.Empty;
            }
        }
    }


}
