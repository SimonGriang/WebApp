namespace WebApp.Models
{
    public class Language
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public bool isTargetLanguage { get; set; } = true;
        public bool isOriginLanguage { get; set; } = true;

        public Language()
        {
        }

        public Language(string name, string code)
        {
            Name = name;
            Abbreviation = code;
        }
    }
}
