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

      public QuoteViewModelService() 
      {
        // Put MongoDB init here for now??
        // Singleton connection to DB passed in?
        if (_quoteRepository == null) {
          // Do stuff here ,.
        }
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
            Console.WriteLine("OBJ:" + JsonConvert.SerializeObject(myObject, Formatting.Indented));
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