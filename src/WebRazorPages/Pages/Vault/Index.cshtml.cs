using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.eShopWeb.RazorPages.ViewModels;
using Microsoft.eShopWeb.RazorPages.Services;
using System;

namespace Microsoft.eShopWeb.RazorPages.Pages.Vault
{
    public class IndexModel : PageModel
    {
      public int Id { get; private set; }
      // Actual quote from MongoDB
      public string Quote { get; private set;}
      private QuoteViewModelService _service;

      public IndexModel() 
      {
        // What to store here??
        // string dummyJSON = "{}";
        if (_service == null) 
        {
          _service = new QuoteViewModelService();
        }
      }

      public async void OnGet()
      {
        // What to do here??
        // Call the QuoteiewModelService here .. but how?
        // var qvModel = new QuoteViewModel();
        // Get data and map to view??
        // qvModel.GetAllQuotes();
        Id = 1234;
        Quote = "{\"dude\":\"bob\"}";

        var viewModels = await _service.GetAllQuotes();

        Console.WriteLine("Items: " + viewModels.Count);
        foreach (var vm in viewModels)
        {
          Console.WriteLine("MODEL: " + vm.Id + ") " + vm.Name);
        }
      }
    }
}