namespace DTO.API.Animals
{
    public class Root
    {
        public Meta meta { get; set; } = new();
        public List<Datum> data { get; set; } = [];
        public List<Included> included { get; set; } = [];
    }

    public class Meta
    {
        public int count { get; set; }
        public int countReturned { get; set; }
        public int pageReturned { get; set; }
        public int limit { get; set; }
        public int pages { get; set; }
        public string? transactionId { get; set; }
    }

    public class Datum
    {
        public string type { get; set; } = string.Empty;
        public string id { get; set; } = string.Empty;
        public Attributes attributes { get; set; } = new();
        public Relationships relationships { get; set; } = new();
    }

    public class Included
    {
        public string type { get; set; } = string.Empty;
        public string id { get; set; } = string.Empty;
        public Attributes attributes { get; set; } = new();
        public Links links { get; set; } = new();
    }

    public class Attributes
    {
        public double distance { get; set; }
        public string? name { get; set; }
        public string? street { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? citystate { get; set; }
        public string? postalcode { get; set; }
        public string? country { get; set; }
        public string? phone { get; set; }
        public double? lat { get; set; }
        public double? lon { get; set; }
        public string? coordinates { get; set; }

        public MediaFile? original { get; set; }
        public MediaFile? large { get; set; }
        public MediaFile? small { get; set; }

        public int? order { get; set; }
        public DateTime? created { get; set; }
        public DateTime? updated { get; set; }

        public string? email { get; set; }
        public string? url { get; set; }
        public string? facebookUrl { get; set; }
        public string? adoptionProcess { get; set; }
        public string? about { get; set; }
        public string? services { get; set; }
        public string? type { get; set; }
        public string? adoptionUrl { get; set; }
        public string? donationUrl { get; set; }
    }

    public class MediaFile
    {
        public int filesize { get; set; }
        public int resolutionX { get; set; }
        public int resolutionY { get; set; }
        public string url { get; set; } = string.Empty;
    }

    public class Relationships
    {
        public Breeds breeds { get; set; } = new();
        public Locations locations { get; set; } = new();
        public Pictures pictures { get; set; } = new();
        public Orgs orgs { get; set; } = new();
    }

    public class Breeds
    {
        public List<Datum> data { get; set; } = [];
    }

    public class Locations
    {
        public List<Datum> data { get; set; } = [];
    }

    public class Pictures
    {
        public List<Datum> data { get; set; } = [];
    }

    public class Orgs
    {
        public List<Datum> data { get; set; } = [];
    }

    public class Links
    {
        public string self { get; set; } = string.Empty;
    }
}
