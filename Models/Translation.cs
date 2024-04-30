using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Translation
    {
        public int ID { get; set; }
        public string OriginalText { get; set; }
        public string? TranslatedText { get; set; }

        [DataType(DataType.Date)]
        public DateTime translated_at { get; set; }
        public virtual Language? OriginalLanguage { get; set; }
        public virtual Language? TranslatedLanguage { get; set; }
    }
}
