using p2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace p2
{
    public class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; set; }
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }

        // Parent and Children properties for creating a tree structure
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; }

        public HtmlElement()
        {
            Attributes = new List<string>();
            Classes = new List<string>();
            Children = new List<HtmlElement>();
        }


        public void DisplayElementDetails()
        {
            Console.WriteLine($"Id: {Id}");
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Attributes: {string.Join(", ", Attributes)}");
            Console.WriteLine($"Classes: {string.Join(", ", Classes)}");
            Console.WriteLine($"InnerHtml: {InnerHtml}");
        }
        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                HtmlElement current = queue.Dequeue();
                yield return current;

                if (current.Children != null)
                {
                    foreach (var child in current.Children)
                    {
                        queue.Enqueue(child);
                    }
                }
            }
        }

        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement current = this;
            while (current.Parent != null)
            {
                yield return current.Parent;
                current = current.Parent;
            }
        }
    }

    public static class HtmlElementExtensions
    {
        public static List<HtmlElement> QuerySelectorAll(this HtmlElement element, Selector selector)
        {
            var result = new HashSet<HtmlElement>();
            QuerySelectorAllRecursive(element, selector, result);
            return new List<HtmlElement>(result);
        }

        private static void QuerySelectorAllRecursive(HtmlElement element, Selector selector, HashSet<HtmlElement> result)
        {
            if (MatchesSelector(element, selector))
            {
                result.Add(element);
            }

            if (element.Children != null)
            {
                foreach (var child in element.Children)
                {
                    QuerySelectorAllRecursive(child, selector, result);
                }
            }
        }

        private static bool MatchesSelector(HtmlElement element, Selector selector)
        {
            if (selector == null)
            {
                return true; // If selector is null, it matches everything
            }

            if (!string.IsNullOrEmpty(selector.TagName) && selector.TagName != element.Name)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(selector.Id) && selector.Id != element.Id)
            {
                return false;
            }

            if (selector.Classes != null)
            {
                foreach (var className in selector.Classes)
                {
                    if (!element.Classes.Contains(className))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

    }


}
