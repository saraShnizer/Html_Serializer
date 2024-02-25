using System;
using System.IO;
using System.Text.Json;

namespace p2
{
        public class HtmlHelper
        {
            private readonly static HtmlHelper _instance=new HtmlHelper();
            public static HtmlHelper Instance => _instance;
            public List<string> HtmlTags { get; private set; }
            public List<string> HtmlVoidTags { get; private set; }

            private HtmlHelper()
            {
                // Load HTML tags from JSON files
                HtmlTags = LoadHtmlTags("HtmlTags.json").ToList();
                HtmlVoidTags = LoadHtmlTags("HtmlVoidTags.json").ToList();
            }

            private string[] LoadHtmlTags(string jsonFileName)
            {
                try
                {
                    // Read JSON file content
                    string jsonContent = File.ReadAllText(jsonFileName);

                    // Deserialize JSON content into string array
                    return JsonSerializer.Deserialize<string[]>(jsonContent);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading {jsonFileName}: {ex.Message}");
                    return Array.Empty<string>();
                }
            }
        }
}