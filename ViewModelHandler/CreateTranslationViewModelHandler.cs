using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.ViewModelHandler
{
    public static class CreateTranslationViewModelHandler
    {
        public static async Task<CreateTranslationViewModel> createViewModelAsync(WebAppContext _context)
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
                    if (lan.Abbreviation == "de")
                        viewModel.German = lan.ID;
                    if (lan.Abbreviation == "en-US")
                        viewModel.EnglishUS = lan.ID;
                    if (lan.Abbreviation == "en-GB")
                        viewModel.EnglishGB = lan.ID;
                }

                if (lan.isOriginLanguage == true)
                {
                    originLanguages.Add(lan);
                    if (lan.Abbreviation == "DL")
                        viewModel.DetectLanguage = lan.ID;
                    if (lan.Abbreviation == "en")
                        viewModel.English = lan.ID;
                }

            }

            viewModel.originLanguages = originLanguages;
            viewModel.targetLanguages = targetLanguages;

            return viewModel;
        }
    }
}
