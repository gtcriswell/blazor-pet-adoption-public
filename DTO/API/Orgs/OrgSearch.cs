using Shared;

namespace DTO.API.Orgs
{
    public partial class Attributes
    {
        public double distance { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postalcode { get; set; }
        public string country { get; set; }
        public string email { get; set; }
        public string url { get; set; }
        public string facebookUrl { get; set; }
        public string adoptionUrl { get; set; }
        public string donationUrl { get; set; }
        public string services { get; set; }
        public string type { get; set; }
        public bool isCommonapplicationAccepted { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public string coordinates { get; set; }
        public string citystate { get; set; }
        public string street { get; set; }
        public string phone { get; set; }
        public string serveAreas { get; set; }
        public string adoptionProcess { get; set; }
        public string about { get; set; }
    }

    public class Datum
    {
        public string type { get; set; }
        public string id { get; set; }
        public Attributes attributes { get; set; }
    }

    public class Meta
    {
        public int count { get; set; }
        public int countReturned { get; set; }
        public int pageReturned { get; set; }
        public int limit { get; set; }
        public int pages { get; set; }
        public string transactionId { get; set; }
    }

    public class Root
    {
        public Meta meta { get; set; }
        public List<Datum> data { get; set; }
    }

}
