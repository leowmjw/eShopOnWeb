// using Microsoft.eShopWeb.RazorPages.Interfaces;
using Microsoft.eShopWeb.RazorPages.ViewModels;
// using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System;
// using System.Collections.Generic;
using System.Linq;
// using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
// For Vaullt
using VaultSharp;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.GitHub;
// For MongoDB
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
// using MongoDB.Bson.Serialization.Attributes;


namespace Microsoft.eShopWeb.RazorPages.Services
{
    // Get the needed data from the MongoDB repo ..
    public class QuoteViewModelService 
    {

      // TODO: Replace with MongoDB connector
      // The below resource will be good starting points ..
      // https://www.qappdesign.com/using-mongodb-with-net-core-webapi/
      // https://github.com/fpetru/WebApiMongoDB
      private readonly string _quoteRepository = null;
      // private readonly ILogger<QuoteViewModelService> _logger;
      private string _vaultClientToken = null;
      private MongoClient _mongoDBClient = null;

      public QuoteViewModelService() 
      {
        // Put MongoDB init here for now??
        // Singleton connection to DB passed in?
        if (_quoteRepository == null) {
          // Do stuff here ,.
        }
        if (_vaultClientToken == null) {
            // TODO: Refactor token generation here?
            // Pull from Redis persistent store later?
            // can also have the trigger to get MongoDB URI?? Maybe later
        }
      }

      // GetVaultConnection grabs the MongoDB connection string for data         
      public async Task<string> GetVaultConnection() 
      {
        var mongoConnectionURI = "";
        Console.WriteLine("Starting access to Vault Server ...");
        var vaultAddr = Environment.GetEnvironmentVariable("VAULT_ADDR");
        var personalAccessToken = Environment.GetEnvironmentVariable("GITHUB_PERSONAL_ACCESS_TOKEN");
        if (vaultAddr != "" && personalAccessToken != "") {
            // Check above; if null, what to do?
            IAuthMethodInfo authMethod = new GitHubAuthMethodInfo(personalAccessToken);
            var vaultClientSettings = new VaultClientSettings(vaultAddr, authMethod);
            IVaultClient vaultClient = new VaultClient(vaultClientSettings);

            // any operations done using the vaultClient will use the 
            // vault token/policies mapped to the github token.
            // NOTE: If ..LookupSelfAsync().Result; then it is synchronous
            var tokRes = await vaultClient.V1.Auth.Token.LookupSelfAsync();
            // foreach (var key in tokRes.Data.Metadata) {
            //     Console.WriteLine("LEOWMJW TOKEN KEY:" + key + " META:" + tokRes.Data.Metadata.ToString());
            // }
            // DEBUG
            // Console.WriteLine("RAW:" + JsonConvert.SerializeObject(tokRes, Formatting.Indented));

            try {
                // NOTE: If ..ReadSecretAsync(..).Result; then it is synchronous
                var res = await vaultClient.V1.Secrets.KeyValue.V1.ReadSecretAsync("example/leowmjw");
                // Use as per below; so much better ..
                // DEBUG:
                // Console.WriteLine("RAW:" + JsonConvert.SerializeObject(res, Formatting.Indented));
                // The not so elegant way below :(
                // Console.WriteLine("Found " + res.Data.Count + " item(s)");
                foreach (var key in res.Data.Keys) {
                    // Console.WriteLine("KEY:" + key + " VAL: " + res.Data.GetValueOrDefault(key));
                    if (key == "uri") {
                        mongoConnectionURI = (string) res.Data.GetValueOrDefault(key);
                        // DEBUG:
                        // Console.WriteLine("EXTRACTED: " + mongoConnectionURI);
                        return mongoConnectionURI;
                    }
                }
            } catch {
                Console.WriteLine("DIE!!!!");
            }

            // Give it some time; now is OK; as it is async
            // CreateAuthenticationProvider(authMethod, null);
            var authInfo = authMethod.ReturnedLoginAuthInfo;
            // Console.WriteLine("RAW:" + JsonConvert.SerializeObject(authInfo,Formatting.Indented));
            Console.WriteLine("=========================");
            _vaultClientToken = authInfo.ClientToken;
            // DEBUG:
            // Console.WriteLine("CACHED TOKEN: " + _vaultClientToken);

        } else {
            Console.WriteLine("Both VAULT_ADDR and GITHUB_PERSONAL_ACCESS_TOKEN needs to be defined!");
        }

        return mongoConnectionURI;
      }

      // Naive implementation using MongoDB as the data source 
      public async Task<List<QuoteViewModelMongoDB>> GetAllQuotesFromMongoDB(string mongoConnectionURI) {
          // Need to vaklidate mongoClkientURI??
          if (_mongoDBClient == null) {
            _mongoDBClient = new MongoClient(mongoConnectionURI);
          }
          Console.WriteLine("xxxxxxxxxxxxxxxxxxxxxxxxxxx");
          // Get quotes DB ..
          var db = _mongoDBClient.GetDatabase("cooljoe");
          // db.GetCollection("quotes");
          foreach (var colName in db.ListCollectionNames().ToList()) {
            Console.WriteLine("COLLECTION: " + colName);
          }

          var myCollection =  db.GetCollection<QuoteViewModelMongoDB>("quotes");
          return  await myCollection.Find(new BsonDocument()).ToListAsync();
          //   var json = myCollection.ToJson();
          // DEBUG:
          // Console.WriteLine("RAW_JSON:" + json);

          // Below for DEBUG
        //   using (IAsyncCursor<BsonDocument> cursor = await myCollection.FindAsync(new BsonDocument()))
        //   {
        //      while (await cursor.MoveNextAsync())
        //      {
        //         IEnumerable<BsonDocument> batch = cursor.Current;
        //         foreach (BsonDocument document in batch)
        //         {
        //             Console.WriteLine(document);
        //             Console.WriteLine();
        //         }
        //     }
        //   }
        //     Console.WriteLine("xxxxxxxxxxxxxxxxxxxxxxxxxxx");
        //     return null;
      }

      // Naive implementation; without any async until is needed
      public async Task<List<QuoteViewModel>> GetAllQuotes() 
      {
        Console.WriteLine("===========================");
        Console.WriteLine("In GetAllQuotes");
        var viewModel = new QuoteViewModel();

        using (var client = new HttpClient())
        {
            var endPoint = "https://jsonplaceholder.typicode.com/users";
            var json = await client.GetStringAsync(endPoint);
            // Console.WriteLine("RAW:" + JsonConvert.SerializeObject(json, Formatting.Indented));
            var myObject = JsonConvert.DeserializeObject<List<QuoteViewModel>>(json);
            // Double check ..
            // Console.WriteLine("OBJ:" + JsonConvert.SerializeObject(myObject, Formatting.Indented));
            Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
            return myObject;
            // Fake returns 
            // List<Car> cars = new List<Car>{
            //     new Car{Id = 1, Make="Audi",Model="R8",Year=2014,Doors=2,Colour="Red",Price=79995},
            //     new Car{Id = 2, Make="Aston Martin",Model="Rapide",Year=2010,Doors=2,Colour="Black",Price=54995},

        }
      }
    }
}