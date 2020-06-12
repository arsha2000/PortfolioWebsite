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
using Microsoft.Extensions.Options;
using PortfolioWebsite.Entities;

namespace PortfolioWebsite.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly SupportEmailSettings _supportEmailSettings;
        private readonly JSONFileManager<PortfolioItem> PortfolioManager;
        private readonly IMailer Mailer;

        public List<PortfolioItem> PortfolioItems { get; private set; }

        public IndexModel(
            ILogger<IndexModel> logger,
            IOptions<SupportEmailSettings> supportEmailSettings,
            JSONFileManager<PortfolioItem> portfolioManager,
            IMailer mailer)
        {
            _logger = logger;
            PortfolioManager = portfolioManager;
            Mailer = mailer;
            _supportEmailSettings = supportEmailSettings.Value;
        }

        public async Task<IActionResult> OnGet()
        {
            var allItems = await PortfolioManager.ReadAllAsync("portfolioItems.json");
            PortfolioItems = allItems.ToList();

            
            return Page();
        }

        [BindProperty]
        public Contact Contact { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            

            // email to support email
            await Mailer.SendEmailAsync(
                _supportEmailSettings.SupportEmailAddress,
                _supportEmailSettings.AdministratorName,
                "Client Information - Arsha.Dev",
                Contact.ToHTML());

            //email to the client
            string thankYouBody = await PortfolioManager.FileManager.ReadFileAsTextAsync("thankYou.txt");
            await Mailer.SendEmailAsync(Contact.Email, Contact.Name, "Thank You - Arsha.Dev", thankYouBody);

            _logger.Log(LogLevel.Information, "Sent email to the client and the support email");

            return RedirectToPage("Index");
        }
    }
}
