namespace ActivityLibrary.Entities
{
    using System;
    using System.Net;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public partial class Foursquare
    {
        [JsonProperty("meta")]
        public Meta Meta { get; set; }

        [JsonProperty("response")]
        public Response Response { get; set; }
    }

    public partial class Response
    {
        [JsonProperty("venue")]
        public Venue Venue { get; set; }
    }

    public partial class Venue
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("contact")]
        public Contact Contact { get; set; }

        [JsonProperty("location")]
        public LocationFoursquare Location { get; set; }

        [JsonProperty("canonicalUrl")]
        public string CanonicalUrl { get; set; }

        [JsonProperty("categories")]
        public Category[] Categories { get; set; }

        [JsonProperty("verified")]
        public bool Verified { get; set; }

        [JsonProperty("stats")]
        public Stats Stats { get; set; }

        [JsonProperty("likes")]
        public FluffyLikes Likes { get; set; }

        [JsonProperty("dislike")]
        public bool Dislike { get; set; }

        [JsonProperty("ok")]
        public bool Ok { get; set; }

        [JsonProperty("rating")]
        public double Rating { get; set; }

        [JsonProperty("ratingColor")]
        public string RatingColor { get; set; }

        [JsonProperty("ratingSignals")]
        public long RatingSignals { get; set; }

        [JsonProperty("allowMenuUrlEdit")]
        public bool AllowMenuUrlEdit { get; set; }

        [JsonProperty("beenHere")]
        public BeenHere BeenHere { get; set; }

        [JsonProperty("specials")]
        public Specials Specials { get; set; }

        [JsonProperty("photos")]
        public Photos Photos { get; set; }

        [JsonProperty("reasons")]
        public Specials Reasons { get; set; }

        [JsonProperty("hereNow")]
        public HereNow HereNow { get; set; }

        [JsonProperty("createdAt")]
        public long CreatedAt { get; set; }

        [JsonProperty("tips")]
        public Tips Tips { get; set; }

        [JsonProperty("shortUrl")]
        public string ShortUrl { get; set; }

        [JsonProperty("timeZone")]
        public string TimeZone { get; set; }

        [JsonProperty("listed")]
        public Listed Listed { get; set; }

        [JsonProperty("popular")]
        public Popular Popular { get; set; }

        [JsonProperty("pageUpdates")]
        public Specials PageUpdates { get; set; }

        [JsonProperty("inbox")]
        public Specials Inbox { get; set; }

        [JsonProperty("venueChains")]
        public object[] VenueChains { get; set; }

        [JsonProperty("attributes")]
        public Attributes Attributes { get; set; }

        [JsonProperty("bestPhoto")]
        public StickyItem BestPhoto { get; set; }
    }

    public partial class Tips
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("groups")]
        public PurpleGroup[] Groups { get; set; }
    }

    public partial class PurpleGroup
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("items")]
        public PurpleItem[] Items { get; set; }
    }

    public partial class PurpleItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("createdAt")]
        public long CreatedAt { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("canonicalUrl")]
        public string CanonicalUrl { get; set; }

        [JsonProperty("lang")]
        public string Lang { get; set; }

        [JsonProperty("likes")]
        public PurpleLikes Likes { get; set; }

        [JsonProperty("logView")]
        public bool LogView { get; set; }

        [JsonProperty("agreeCount")]
        public long AgreeCount { get; set; }

        [JsonProperty("disagreeCount")]
        public long DisagreeCount { get; set; }

        [JsonProperty("todo")]
        public Todo Todo { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("authorInteractionType")]
        public string AuthorInteractionType { get; set; }
    }

    public partial class PurpleLikes
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("groups")]
        public FluffyGroup[] Groups { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }
    }

    public partial class FluffyGroup
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("items")]
        public User[] Items { get; set; }
    }

    public partial class Stats
    {
        [JsonProperty("checkinsCount")]
        public long CheckinsCount { get; set; }

        [JsonProperty("usersCount")]
        public long UsersCount { get; set; }

        [JsonProperty("tipCount")]
        public long TipCount { get; set; }

        [JsonProperty("visitsCount")]
        public long VisitsCount { get; set; }
    }

    public partial class Popular
    {
        [JsonProperty("isOpen")]
        public bool IsOpen { get; set; }

        [JsonProperty("isLocalHoliday")]
        public bool IsLocalHoliday { get; set; }

        [JsonProperty("timeframes")]
        public Timeframe[] Timeframes { get; set; }
    }

    public partial class Timeframe
    {
        [JsonProperty("days")]
        public string Days { get; set; }

        [JsonProperty("includesToday")]
        public bool? IncludesToday { get; set; }

        [JsonProperty("open")]
        public Open[] Open { get; set; }

        [JsonProperty("segments")]
        public object[] Segments { get; set; }
    }

    public partial class Open
    {
        [JsonProperty("renderedTime")]
        public string RenderedTime { get; set; }
    }

    public partial class Photos
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("groups")]
        public TentacledGroup[] Groups { get; set; }
    }

    public partial class TentacledGroup
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("items")]
        public StickyItem[] Items { get; set; }
    }

    public partial class LocationFoursquare
    {
        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lng")]
        public double Lng { get; set; }

        [JsonProperty("labeledLatLngs")]
        public LabeledLatLng[] LabeledLatLngs { get; set; }

        [JsonProperty("cc")]
        public string Cc { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("formattedAddress")]
        public string[] FormattedAddress { get; set; }
    }

    public partial class LabeledLatLng
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lng")]
        public double Lng { get; set; }
    }

    public partial class Listed
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("groups")]
        public StickyGroup[] Groups { get; set; }
    }

    public partial class StickyGroup
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("items")]
        public FluffyItem[] Items { get; set; }
    }

    public partial class FluffyItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("editable")]
        public bool Editable { get; set; }

        [JsonProperty("public")]
        public bool Public { get; set; }

        [JsonProperty("collaborative")]
        public bool Collaborative { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("canonicalUrl")]
        public string CanonicalUrl { get; set; }

        [JsonProperty("createdAt")]
        public long CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public long UpdatedAt { get; set; }

        [JsonProperty("followers")]
        public Todo Followers { get; set; }

        [JsonProperty("listItems")]
        public ListItems ListItems { get; set; }
    }

    public partial class ListItems
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("items")]
        public TentacledItem[] Items { get; set; }
    }

    public partial class TentacledItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("createdAt")]
        public long CreatedAt { get; set; }
    }

    public partial class Todo
    {
        [JsonProperty("count")]
        public long Count { get; set; }
    }

    public partial class FluffyLikes
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("groups")]
        public IndigoGroup[] Groups { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }
    }

    public partial class IndigoGroup
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("items")]
        public object[] Items { get; set; }
    }

    public partial class Specials
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("items")]
        public object[] Items { get; set; }
    }

    public partial class HereNow
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("groups")]
        public object[] Groups { get; set; }
    }

    public partial class Contact
    {
    }

    public partial class Category
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("pluralName")]
        public string PluralName { get; set; }

        [JsonProperty("shortName")]
        public string ShortName { get; set; }

        [JsonProperty("icon")]
        public Icon Icon { get; set; }

        [JsonProperty("primary")]
        public bool Primary { get; set; }
    }

    public partial class StickyItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("createdAt")]
        public long CreatedAt { get; set; }

        [JsonProperty("source")]
        public Source Source { get; set; }

        [JsonProperty("prefix")]
        public string Prefix { get; set; }

        [JsonProperty("suffix")]
        public string Suffix { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("visibility")]
        public string Visibility { get; set; }
    }

    public partial class User
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("photo")]
        public Icon Photo { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }
    }

    public partial class Icon
    {
        [JsonProperty("prefix")]
        public string Prefix { get; set; }

        [JsonProperty("suffix")]
        public string Suffix { get; set; }
    }

    public partial class Source
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public partial class BeenHere
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("unconfirmedCount")]
        public long UnconfirmedCount { get; set; }

        [JsonProperty("marked")]
        public bool Marked { get; set; }

        [JsonProperty("lastCheckinExpiredAt")]
        public long LastCheckinExpiredAt { get; set; }
    }

    public partial class Attributes
    {
        [JsonProperty("groups")]
        public IndecentGroup[] Groups { get; set; }
    }

    public partial class IndecentGroup
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("items")]
        public IndigoItem[] Items { get; set; }
    }

    public partial class IndigoItem
    {
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("displayValue")]
        public string DisplayValue { get; set; }
    }

    public partial class Meta
    {
        [JsonProperty("code")]
        public long Code { get; set; }

        [JsonProperty("requestId")]
        public string RequestId { get; set; }
    }

    public partial class Foursquare
    {
        public static Foursquare FromJson(string json) => JsonConvert.DeserializeObject<Foursquare>(json, ConverterFoursquare.Settings);
    }

    public static class SerializeFoursquare
    {
        public static string ToJson(this Foursquare self) => JsonConvert.SerializeObject(self, ConverterFoursquare.Settings);
    }

    public class ConverterFoursquare
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}
