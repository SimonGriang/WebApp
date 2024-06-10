using DeepL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebApp.Data;
using WebApp.Models;
using WebApp.Services;
using WebApp.ViewModelHandler;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WebAppContext _context;
        private readonly TranslationService _translationService;

        public HomeController(ILogger<HomeController> logger, WebAppContext context, TranslationService translationService)
        {
            _logger = logger;
            _context = context;
            _translationService = translationService;
        }

        // GET: Startpage
        public async Task<IActionResult> Index()
        {
            CreateTranslationViewModel viewModel = await CreateTranslationViewModelHandler.createViewModelAsync(_context);
            return View(viewModel);
        }

        // POST: Startpage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CreateTranslationViewModel returnedViewModel)
        {
            if (returnedViewModel.LanguageTo == 0 || returnedViewModel.LanguageFrom == 0)
            {
                ModelState.AddModelError("", "Ung�ltige Sprachauswahl. Bitte w�hlen Sie g�ltige Sprachen aus.");
                return View(returnedViewModel);
            }
            if (returnedViewModel.LanguageTo == 0)
            {
                // Handle the case when languageTo is 0
                return NotFound();
            }
            Language? languageTo = await _context.Language.FindAsync(returnedViewModel.LanguageTo);

            if (returnedViewModel.LanguageFrom == 0)
            {
                // Handle the case when languageFrom is 0
                return NotFound();
            }
            Language? languageFrom = await _context.Language.FindAsync(returnedViewModel.LanguageFrom);


            CreateTranslationViewModel viewModel = await CreateTranslationViewModelHandler.createViewModelAsync(_context);
            viewModel.LanguageFrom = returnedViewModel.LanguageFrom;
            viewModel.LanguageTo = returnedViewModel.LanguageTo;

            if (viewModel.Translation is not null)
            {
                viewModel.Translation.OriginalLanguage = languageFrom;
                viewModel.Translation.TranslatedLanguage = languageTo;
                if (returnedViewModel.Translation is not null)
                    viewModel.Translation.OriginalText = returnedViewModel.Translation.OriginalText;
            }
            else
            {
                viewModel.Translation = new Translation();
                viewModel.Translation.OriginalLanguage = languageFrom;
                viewModel.Translation.TranslatedLanguage = languageTo;
            }


            try
            {
                viewModel = await _translationService.TranslateTextAsync(viewModel);

                if (ModelState.IsValid)
                {
                    _context.Add(viewModel.Translation);
                    await _context.SaveChangesAsync();
                    return View(viewModel);
                }
                else
                    return View(viewModel);
            }
            catch (ConnectionException connectionException)
            {
                TempData["ErrorMessage"] = "Es konnte keine Verbindung zum Webservice aufgerufen werden: " + connectionException.Message;
                return View();
            }
            catch (QuotaExceededException quotaExceededException)
            {
                TempData["ErrorMessage"] = "Das Kontigent an m�glichen �bersetzungen der Software ist ereicht: " + quotaExceededException.Message;
                return View();
            }
            catch (DeepLException deeplException)
            {
                TempData["ErrorMessage"] = "Fehlerhafte Sprachkombination angegeben: " + deeplException.Message;
                return View();
            }
            catch (Exception exception)
            {
                TempData["ErrorMessage"] = "Ein unerwarteter Fehler ist aufgetreten: " + exception.Message;
                return View();
            }
        }

        // GET: History
        public async Task<IActionResult> History()
        {
            return View(await _context.Translation.ToListAsync());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // GET: Home/Details/X
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var translation = await _context.Translation
                .FirstOrDefaultAsync(m => m.ID == id);
            if (translation == null)
            {
                return NotFound();
            }

            return View(translation);
        }

        // GET: Home/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var translation = await _context.Translation
                .FirstOrDefaultAsync(m => m.ID == id);
            if (translation == null)
            {
                return NotFound();
            }

            return View(translation);
        }

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var translation = await _context.Translation.FindAsync(id);
            if (translation != null)
            {
                _context.Translation.Remove(translation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TranslationExists(int id)
        {
            return _context.Translation.Any(e => e.ID == id);
        }
    }
}



