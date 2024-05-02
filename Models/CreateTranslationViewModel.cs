namespace WebApp.Models
{
    public class CreateTranslationViewModel
    {
        public List<Language>? targetLanguages { get; set; }

        public List<Language>? originLanguages { get; set; }

        public Translation? Translation { get; set; }

        public int LanguageFrom { get; set; }

        public int LanguageTo { get; set; }

        public int english { get; set; }
        public int german { get; set; }
        public int detectLanguage { get; set; }
    }
}
