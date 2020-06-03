using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using PortfolioWebsite.Services;
using PortfolioWebsite.Models;

namespace PortfolioWebsite.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private JSONFileManager<PortfolioItem> PortfolioManager;
        private JSONFileManager<Contact> ContactManager;

        public List<PortfolioItem> PortfolioItems { get; private set; }

        public IndexModel(ILogger<IndexModel> logger, JSONFileManager<PortfolioItem> portfolioManager, JSONFileManager<Contact> contactManager)
        {
            _logger = logger;
            this.PortfolioManager = portfolioManager;
            ContactManager = contactManager;
        }

        public async Task<IActionResult> OnGet()
        {
            var allItems = await PortfolioManager.ReadAllAsync("portfolioItems.json");
            PortfolioItems = allItems.ToList();

            return Page();
        }

        [BindProperty]
        public Contact Contact { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ContactManager.WriteToFile("contacts.json", new List<Contact> { Contact });

            return RedirectToPage("Index");
        }
    }
}
