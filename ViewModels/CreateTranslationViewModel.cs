using WebApp.Models;

namespace WebApp.ViewModels
{
    public class CreateTranslationViewModel
    {
        public List<Language>? targetLanguages { get; set; }

        public List<Language>? originLanguages { get; set; }

        public Translation? Translation { get; set; }

        public int LanguageFrom { get; set; }

        public int LanguageTo { get; set; }

        public int English { get; set; }
        public int EnglishUS { get; set; }
        public int EnglishGB { get; set; }
        public int German { get; set; }
        public int DetectLanguage { get; set; }
    }
}
