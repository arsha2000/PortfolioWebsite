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
        private readonly IRazorPartialToStringRenderer RazorRenderer;
        private readonly JSONFileManager<PortfolioItem> PortfolioManager;
        private readonly IMailer Mailer;

        public List<PortfolioItem> PortfolioItems { get; private set; }

        public IndexModel(
            ILogger<IndexModel> logger,
            IOptions<SupportEmailSettings> supportEmailSettings,
            IRazorPartialToStringRenderer razorRenderer,
            JSONFileManager<PortfolioItem> portfolioManager,
            IMailer mailer)
        {
            _logger = logger;
            _supportEmailSettings = supportEmailSettings.Value;

            RazorRenderer = razorRenderer;
            PortfolioManager = portfolioManager;
            Mailer = mailer;
        }

        public async Task<IActionResult> OnGet()
        {
            try
            {
                var allItems = await PortfolioManager.ReadAllAsync("portfolioItems.json");
                PortfolioItems = allItems.ToList();
            } catch (Exception e)
            {
                PortfolioItems = new List<PortfolioItem>();
                _logger.Log(LogLevel.Error, $"Reading Portfolio items failed: {e.Message}");
            }
            
            return Page();
        }

        [BindProperty]
        public Contact Contact { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // email to support email
                string supportEmailBody = await RazorRenderer.RenderPartialToStringAsync("_SupportEmailTemplate", Contact);
                await Mailer.SendEmailAsync(
                    _supportEmailSettings.SupportEmailAddress,
                    _supportEmailSettings.AdministratorName,
                    "Client Information - Arsha.Dev",
                    supportEmailBody);
                

                _logger.Log(LogLevel.Information, $"Sent email to the support email");

                //email to the client
                string thankYouBody = await RazorRenderer.RenderPartialToStringAsync("_ThankYou", Contact);
                await Mailer.SendEmailAsync(Contact.Email, Contact.Name, "Thank You - Arsha.Dev", thankYouBody);

                _logger.Log(LogLevel.Information, "Sent email to the client email");

            } catch (Exception e)
            {
                _logger.Log(LogLevel.Error, $"Email operation failed: {e.Message}");
            }

            return RedirectToPage("Index");
        }
    }
}
