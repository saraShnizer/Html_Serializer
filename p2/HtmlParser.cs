using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace p2
{
    public class HtmlParser
    {
        private static readonly List<string> htmlTags;
        private static readonly List<string> htmlVoidTags;

        static HtmlParser()
        {
            htmlTags = HtmlHelper.Instance.HtmlTags;
            htmlVoidTags = HtmlHelper.Instance.HtmlVoidTags;
        }


        public string[] ExtractHtmlTags(string html)
        {

            string pattern = @"<[^>]*>|(?<=<[^>]+>)(.*\n?)(?=<\/[^>]+>)";
            //string pattern = @"<[^>]*>|(?<=<[^>]+>)([\s\S]*?)(?=<\/[^>]+>)";

            MatchCollection matches = Regex.Matches(html, pattern);
            string[] tags = new string[matches.Count];
            for (int i = 0; i < matches.Count; i++)
            {
                tags[i] = matches[i].Value;
                tags[i] = tags[i].Substring(0, tags[i].Length - 1);
            }
            return tags.ToArray();
        }


        public HtmlElement ParseHtml(string[] tags)
        {
            if (tags == null || tags.Length == 0)
                throw new ArgumentNullException(nameof(tags));

            var root = new HtmlElement();
            var currentElement = root;
            //string innerHtml="";

            foreach (var tag in tags)
            {
                string remaining = tag;
                var newElement = new HtmlElement { };
                if (!remaining.StartsWith("<"))
                    currentElement.InnerHtml = remaining;

                else
                {
                    while (!string.IsNullOrEmpty(remaining))
                    {

                        var firstWord = GetFirstWord(remaining);

                        if (firstWord == "</html" || firstWord == "/html")
                        {
                            // Reached the end of HTML
                            // HTML tag, create a new element and add it to the children
                            newElement.Name = firstWord.Substring(2);
                            newElement.Parent = currentElement;
                            currentElement.Children.Add(newElement);
                            break;
                        }
                        else if (firstWord.StartsWith("</") || firstWord.StartsWith("/"))
                        {

                            // Closing tag, go back to the parent element
                            if (currentElement.Parent != null)
                            {
                                currentElement = currentElement.Parent;


                            }
                        }
                        else if (firstWord.StartsWith('<') && htmlTags.Contains(firstWord.Substring(1)))
                        {
                            // HTML tag, create a new element and add it to the children
                            newElement.Name = firstWord.Substring(1);
                            newElement.Parent = currentElement;
                            //newElement.InnerHtml = innerHtml;
                            currentElement.Children.Add(newElement);

                            if (!htmlVoidTags.Contains(firstWord.Substring(1)))
                            {
                                currentElement = newElement;
                            }

                        }
                        else
                        {
                            // Attribute, parse the rest of the string for attributes
                            if (currentElement != null)
                            {
                                if (remaining.Contains("class="))
                                {
                                    var classes = ParseClasses(remaining);
                                    newElement.Classes.AddRange(classes);
                                }
                                else
                                  if (remaining.StartsWith("id="))
                                {
                                    newElement.Id = remaining.Substring(4, remaining.Length - 5);
                                }
                                else
                                {
                                    var attributeParts = ParseAttributes(remaining);

                                    foreach (var attribute in attributeParts)
                                    {
                                        newElement.Attributes.Add(attribute);
                                    }

                                }


                            }
                        }

                        remaining = remaining.Substring(firstWord.Length).TrimStart();
                    }
                }
               
            }
          

            return root;
        }

        private static string GetFirstWord(string input)
        {
             var match = Regex.Match(input, @"^\S+");
            return match.Success ? match.Value : string.Empty;
        }

        private static List<string> ParseAttributes(string input)
        {
            var attributesMatch = Regex.Match(input, @"(?<=)(\w+=""[^""]*"")+");

            if (attributesMatch.Success)
            {
                var attributes = attributesMatch.Value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                return new List<string>(attributes);
            }
            else
            {
            
         
                
                return new List<string>();
            }
        }
        private static List<string> ParseClasses(string input)
        {
            var match = Regex.Match(input, @"class=\""(.*?)\""");

            if (match.Success)
            {
                var classString = match.Groups[1].Value;
                return classString.Split(' ').ToList();
            }
            else
            {
                return new List<string>();
            }
        }
    }
}

