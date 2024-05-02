using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeepL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class LanguagesController : Controller
    {
        private readonly WebAppContext _context;

        public LanguagesController(WebAppContext context)
        {
            _context = context;
        }

        // GET: Languages
        public async Task<IActionResult> Index()
        {
            return View(await _context.Language.ToListAsync());
        }

        // GET: Languages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var language = await _context.Language
                .FirstOrDefaultAsync(m => m.ID == id);
            if (language == null)
            {
                return NotFound();
            }

            return View(language);
        }

        // GET: Languages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Languages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Abbreviation")] Language language)
        {
            List<Language> languages = new List<Language>(); 

            var authKey = "f2981bee-344a-4a1f-b65f-877950fa3855:fx"; // Replace with your key
            var translator = new Translator(authKey);

            var sourceLanguages = await translator.GetSourceLanguagesAsync();
            foreach (var lang in sourceLanguages)
            {
                Console.WriteLine($"{lang.Name} ({lang.Code})"); // Example: "English (EN)"
                Language createlan = new Language(lang.Name, lang.Code);
                createlan.isOriginLanguage = true;
                languages.Add(createlan);

            }
            var targetLanguages = await translator.GetTargetLanguagesAsync();
            foreach (var lang in targetLanguages)
            {
                Console.WriteLine($"{lang.Name} ({lang.Code})"); // Example: "English (EN)"

                Language createlan = new Language(lang.Name, lang.Code);
                foreach (Language lan in languages)
                {
                    if (createlan.Abbreviation.Equals(lan.Abbreviation))
                        lan.isTargetLanguage = true;
                    else {
                        createlan.isOriginLanguage = false;
                        createlan.isTargetLanguage = true;
                        languages.Add(createlan);

                    }
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(languages);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(language);
        }

        // GET: Languages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var language = await _context.Language.FindAsync(id);
            if (language == null)
            {
                return NotFound();
            }
            return View(language);
        }

        // POST: Languages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Abbreviation")] Language language)
        {
            if (id != language.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(language);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LanguageExists(language.ID))
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
            return View(language);
        }

        // GET: Languages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var language = await _context.Language
                .FirstOrDefaultAsync(m => m.ID == id);
            if (language == null)
            {
                return NotFound();
            }

            return View(language);
        }

        // POST: Languages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var language = await _context.Language.FindAsync(id);
            if (language != null)
            {
                _context.Language.Remove(language);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LanguageExists(int id)
        {
            return _context.Language.Any(e => e.ID == id);
        }
    }
}
