using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using DeepL;
using WebApp.ViewModels;
using WebApp.ViewModelHandler;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class TranslationsController : Controller
    {
        private readonly WebAppContext _context;

        public TranslationsController(WebAppContext context)
        {
            _context = context;
        }

        // GET: Translations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Translation.ToListAsync());
        } 

        // GET: Translations/Details/5
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

        // GET: Translations/Create
        public async Task<IActionResult> CreateAsync()
        {
            CreateTranslationViewModel viewModel = await CreateTranslationViewModelHandler.createViewModelAsync(_context);
            return View(viewModel);
        }

        // GET: Translations/Create
        [HttpPost]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(CreateTranslationViewModel returnedViewModel)
        {
            if (returnedViewModel.LanguageTo == 0 || returnedViewModel.LanguageFrom == 0)
            {
                ModelState.AddModelError("", "Ungültige Sprachauswahl. Bitte wählen Sie gültige Sprachen aus.");
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
            if (viewModel.Translation is not null)
            {
                viewModel.Translation.OriginalLanguage = languageTo;
                viewModel.Translation.TranslatedLanguage = languageFrom;
                if (returnedViewModel.Translation is not null)
                    viewModel.Translation.OriginalText = returnedViewModel.Translation.OriginalText;
            }
            else
            {
                viewModel.Translation = new Translation();
                viewModel.Translation.OriginalLanguage = languageTo;
                viewModel.Translation.TranslatedLanguage = languageFrom;
            }


            try
            {
                TranslationService service = new TranslationService(_context); // context wird eigentlich gar nicht gebraucht. 
                viewModel = service.TranslateTextAsync(viewModel).GetAwaiter().GetResult();

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
                TempData["ErrorMessage"] = "Das Kontigent an möglichen Übersetzungen der Software ist ereicht: " + quotaExceededException.Message;
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

        // GET: Translations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var translation = await _context.Translation.FindAsync(id);
            if (translation == null)
            {
                return NotFound();
            }
            return View(translation);
        }

        // POST: Translations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,OriginalText,TranslatedText,translated_at")] Translation translation)
        {
            if (id != translation.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(translation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TranslationExists(translation.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(translation);
        }

        // GET: Translations/Delete/5
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

        // POST: Translations/Delete/5
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
