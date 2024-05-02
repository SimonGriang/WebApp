Code for retrieving all available Languages:

List<Language> languages = new List<Language>();

var authKey = "f2981bee-344a-4a1f-b65f-877950fa3855:fx"; // Replace with your key
var translator = new Translator(authKey);

var sourceLanguages = await translator.GetSourceLanguagesAsync();
Console.WriteLine($"_______________________________sourcetLanguages: {sourceLanguages.Length}"); // Example: "English (EN)"


foreach (var lang in sourceLanguages)
{
    Console.WriteLine($"{lang.Name} ({lang.Code})"); // Example: "English (EN)"
    Language createlan = new Language(lang.Name, lang.Code);
    createlan.isOriginLanguage = true;
    languages.Add(createlan);

}
var targetLanguages = await translator.GetTargetLanguagesAsync();

List<Language> finallanguages = new List<Language>();


Console.WriteLine($"_______________________________targetLanguages: {targetLanguages.Length}"); // Example: "English (EN)"

foreach (var lang in targetLanguages)
{
    Console.WriteLine($"{lang.Name} ({lang.Code})"); // Example: "English (EN)"
    bool wasfound = false;

    Language createlan = new Language(lang.Name, lang.Code);
    foreach (Language lan in languages)
    {
        if (createlan.Abbreviation == (lan.Abbreviation))
        {
            lan.isTargetLanguage = true;
            finallanguages.Add(lan);
            wasfound = true;
            break;
        }
    }
    if (wasfound == false) 
    {
        createlan.isOriginLanguage = false;
        createlan.isTargetLanguage = true;
        finallanguages.Add(createlan);
    }

}


foreach (Language l in finallanguages)
{
    _context.Add(l);
    await _context.SaveChangesAsync();
}
