using Newtonsoft.Json;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Microsoft.eShopWeb.RazorPages.ViewModels
{

    [BsonIgnoreExtraElements]
      public class QuoteViewModelMongoDB
    {
      // Actual quote from MongoDB
      [BsonElement("quote")]
      public string Quote {  get;  private set; }

      // property is needed to match non-case
      [BsonElement("name")]
      // private set is needed otherwise cannot serialize
      public string Name { get; private set; }
    }
    // Ignore the extra stuff ..
    public class QuoteViewModel
    {
      // Running number from MongoDB for quote Item ..
      [JsonProperty("id")]
      public int Id {  get; private set; }
      // Actual quote from MongoDB
      [JsonProperty("username")]
      public string Quote {  get;  private set; }

      [JsonProperty("address[0].city")]
      public string city { get; set; }

      // property is needed to match non-case
      [JsonProperty("name")]
      // private set is needed otherwise cannot serialize
      public string Name { get; private set; }

      public QuoteViewModel() {
        // test
      }

      public void FillStructFromJSON(string json) {
        // Get all quotes here??? 
        // Too many layers??
      }
    }
}