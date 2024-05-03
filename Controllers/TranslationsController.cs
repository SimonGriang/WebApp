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
            CreateTranslationViewModel viewModel = new CreateTranslationViewModel();

            viewModel.Translation = new Translation();

            List<Language> allLanguages = await _context.Language.ToListAsync();

            List<Language> originLanguages = new List<Language>();
            List<Language> targetLanguages = new List<Language>();

            foreach (Language lan in allLanguages)
            {
                if (lan.isTargetLanguage == true)
                {
                    targetLanguages.Add(lan);
                    if (lan.Abbreviation == "en-US")
                        viewModel.english = lan.ID;
                    if (lan.Abbreviation == "de")
                        viewModel.german = lan.ID;
                }

                if (lan.isOriginLanguage == true)
                {
                    originLanguages.Add(lan);
                    if (lan.Abbreviation == "DT")
                        viewModel.detectLanguage = lan.ID;
                }

            }

            viewModel.originLanguages = originLanguages;
            viewModel.targetLanguages = targetLanguages;

            return View(viewModel);
        }

        // POST: Translations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTranslationViewModel viewModel) //[Bind("ID,OriginalText,TranslatedText,translated_at")] Translation translation)
        {
            Language languageTo = await _context.Language.FindAsync(viewModel.LanguageTo);
            Language languageFrom = await _context.Language.FindAsync(viewModel.LanguageFrom);

            viewModel.Translation.OriginalLanguage = languageTo;
            viewModel.Translation.TranslatedLanguage = languageFrom;
            viewModel.targetLanguages = await _context.Language.ToListAsync();
            viewModel.originLanguages = await _context.Language.ToListAsync();

            Translation translation = new Translation();

            translation.OriginalLanguage = languageFrom;
            translation.TranslatedLanguage = languageTo;
            translation.OriginalText = viewModel.Translation.OriginalText;
            // translation.TranslatedText = model.Translation.TranslatedText;

            var authKey = "f2981bee-344a-4a1f-b65f-877950fa3855:fx"; // Replace with your key
            var translator = new Translator(authKey);
            var translatedText = await translator.TranslateTextAsync(viewModel.Translation.OriginalText, languageFrom.Abbreviation, languageTo.Abbreviation); // "Hallo, Welt!";
            Console.WriteLine(translatedText);


            translation.TranslatedText = translatedText.Text; // .Text einfügen wenn problem gelöst.
            viewModel.Translation.TranslatedText = translatedText.Text;  // .Text einfügen wenn problem gelöst.

            if (ModelState.IsValid)
            {
                _context.Add(translation);
                await _context.SaveChangesAsync();
                return View(viewModel);
            }
            return View(translation);
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
