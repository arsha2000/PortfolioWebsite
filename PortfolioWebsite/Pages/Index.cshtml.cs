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
        private readonly JSONFileManager<PortfolioItem> PortfolioManager;
        private readonly IMailer Mailer;

        public List<PortfolioItem> PortfolioItems { get; private set; }

        public IndexModel(ILogger<IndexModel> logger, JSONFileManager<PortfolioItem> portfolioManager, IMailer mailer)
        {
            _logger = logger;
            PortfolioManager = portfolioManager;
            Mailer = mailer;
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

            // send the email in the background without waiting for the operation to finish
            Mailer.SendEmailAsync("mohammadhassas@hotmail.com", "Mohammad Hassas", "Portfolio Website - Contact", Contact.ToHTML())
                .ConfigureAwait(false);

            return RedirectToPage("Index");
        }
    }
}
