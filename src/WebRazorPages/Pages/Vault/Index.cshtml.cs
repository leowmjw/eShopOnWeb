using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.eShopWeb.RazorPages.ViewModels;
using Microsoft.eShopWeb.RazorPages.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.RazorPages.Pages.Vault
{
    public class IndexModel : PageModel
    {
      public int Id { get; private set; }
      // Actual quote from MongoDB
      public string Quote { get; private set;}
      private QuoteViewModelService _service;

      public List<QuoteViewModel> Quotes { get; private set; }

      public IndexModel() 
      {
        // What to store here??
        // string dummyJSON = "{}";
        if (_service == null) 
        {
          _service = new QuoteViewModelService();
        }
      }

      public async Task OnGet()
      {
        // What to do here??
        // Call the QuoteiewModelService here .. but how?
        // var qvModel = new QuoteViewModel();
        // Get data and map to view??
        // qvModel.GetAllQuotes();
        Id = 1234;
        Quote = "{\"dude\":\"bob\"}";

        var viewModels = await _service.GetAllQuotes();
        // Set it so can be accesible from templkate
        Quotes = viewModels;
        Console.WriteLine("Items: " + viewModels.Count);
        foreach (var vm in viewModels)
        {
          // for DEBUG
          // Console.WriteLine("MODEL: " + vm.Id + ") " + vm.Name);
        }

        // Try the connect on the 
        Console.WriteLine("***********************************>");
        var mongoConnectionURI = await _service.GetVaultConnection();
        // Console.WriteLine("TRY1: " +  mongoConnectionURI);
        Console.WriteLine("Connect to MongoDB using: ", mongoConnectionURI);
        // Console.WriteLine("TRY2:" + await _service.GetVaultConnection());
        // TODO: Now connect into data to get result; pass in the connection string?
        Console.WriteLine("**********************************<");
      }
    }
}