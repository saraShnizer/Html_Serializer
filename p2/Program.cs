using p2;
using System;
HtmlSerializer serializer = new HtmlSerializer();
// HTML דוגמה
var html = await serializer.Load("https://learn.malkabruk.co.il/");

// יצירת אובייקט HtmlParser
HtmlParser parser = new HtmlParser();

// חילוץ תגיות HTML מתוך ה-HTML
string[] htmlTags = parser.ExtractHtmlTags(html.ToString());

// פירוק תגיות ה-HTML ויצירת עץ HTML
HtmlElement rootElement = parser.ParseHtml(htmlTags);
//PrintHtmlTree(rootElement, 0);
var selector = Selector.Parse("#profile-menu");

// חיפוש בעץ והדפסת התוצאה
var elements = rootElement.QuerySelectorAll(selector);
foreach (var element in elements)
{
    Console.WriteLine($"Element found: TagName={element.Name}, Id={element.Id}, Classes=[{string.Join(", ", element.Classes)}]");
}
static void PrintHtmlTree(HtmlElement element, int indent)
{
    // הדפסת התגית עם ההזחה הנוכחית
    //Console.WriteLine($"{new string(' ', indent * 4)}{element.Name + " id: " + element.Id/*+" classes: "+element.Classes+" atr: "+element.Attributes*/}");
    element.DisplayElementDetails();
    if (element.Parent != null)
        //Console.Write("parent: "+element.Parent.Name);



        Console.WriteLine();

    // הדפסת הילדים של האלמנט הנוכחי
    foreach (var child in element.Children)
    {
        PrintHtmlTree(child, indent + 1);
    }
}

