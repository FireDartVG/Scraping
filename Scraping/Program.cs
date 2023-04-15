using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Web;

internal class Program
{
    private static void Main(string[] args)
    {
        string text = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Data\\HTML_Data.txt");

        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(text);


        List<Item> lstRecords = new List<Item>();
        foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//div[@class='item']"))
        {

            Item record = new Item();
            var title = node.SelectSingleNode(".//img");
            record.ItemName = HttpUtility.HtmlDecode(title.Attributes["alt"].Value);

            var price = node.SelectSingleNode(".//span[@itemprop='price']").FirstChild;
            record.Price = price.InnerText.Split('$')[1].Replace(",", "");

            var rating = node.Attributes["rating"].Value;

            if (decimal.TryParse(rating, out decimal result))
            {
                if (result > 5)
                {
                    result /= 2;
                }
                record.Rating = result;
            }

            lstRecords.Add(record);

        }
        var json = JsonConvert.SerializeObject(lstRecords, Formatting.Indented);
        

        Console.WriteLine(json);
    }
}

class Item
{

    public string ItemName { get; set; }

    public string Price { get; set; }

    public decimal Rating { get; set; }


}