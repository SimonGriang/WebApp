namespace WebApp.Models
{
    public class CreateTranslationViewModel
    {
        public List<Language>? Languages { get; set; }

        public Translation? Translation { get; set; }

        public int LanguageFrom { get; set; }

        public int LanguageTo { get; set; }
    }
}
