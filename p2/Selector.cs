using System;
using System.Collections.Generic;

public class Selector
{
    public string TagName { get; set; }
    public string Id { get; set; }
    public List<string> Classes { get; set; }
    public Selector Parent { get; set; }
    public Selector Child { get; set; }

    public static Selector Parse(string query)
    {
        var parts = query.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        Selector root = null;
        Selector current = null;

        foreach (var part in parts)
        {
            var selector = new Selector();

            if (part.StartsWith("."))
            {
                selector.Classes = new List<string> { part.Substring(1) };
            }
            else if (part.StartsWith("#"))
            {
                string s= part.Substring(1);
                selector.Id =s;
            }
            else
            {
                selector.TagName = part;
            }

            if (root == null)
            {
                root = selector;
                current = selector;
            }
            else
            {
                current.Child = selector;
                selector.Parent = current;
                current = selector;
            }
        }

        return root;
    }
}