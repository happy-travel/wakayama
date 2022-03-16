using Nest;

namespace HappyTravel.Wakayama.Common.Models;

public record Place
{
    [PropertyName("osm_id")]
    public long OsmId { get; set; }
    
    [PropertyName("importance")]
    public double Importance { get; set; }
    
    [PropertyName("object_type")]
    public string ObjectType { get; set; }
    
    [PropertyName("countrycode")]
    public string CountryCode { get; set; }
    
    [PropertyName("postcode")]
    public string Postcode { get; set; }
    
    [PropertyName("osm_key")]
    public string OsmKey { get; set; }
    
    [PropertyName("osm_type")]
    public string OsmType { get; set; }
    
    [PropertyName("housenumber")]
    public string HouseNumber { get; set; }
    
    [PropertyName("osm_value")]
    public string OsmValue { get; set; }
    
    [PropertyName("coordinate")]
    public GeoLocation Coordinate {get; set; }
    
    [PropertyName("extent")]  
    public EnvelopeGeoShape Extent { get; set; }
    
    [PropertyName("name")]
    public MultiLanguageName Name {get; set; }

    [PropertyName("country")]
    public MultiLanguage Country { get; set; }
    
    [PropertyName("locality")]
    public MultiLanguage? Locality { get; set; }
    
    [PropertyName("state")]
    public MultiLanguage? State { get; set; }
    
    [PropertyName("district")]
    public MultiLanguage? District { get; set; }
    
    [PropertyName("county")]
    public MultiLanguage? County { get; set; }
    
    [PropertyName("city")]
    public MultiLanguage? City { get; set; }
    
    [PropertyName("street")]
    public MultiLanguage? Street { get; set; }
    
    [PropertyName("collector")]
    public MultiLanguage Collector { get; set; }
}