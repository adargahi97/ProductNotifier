using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductNotifier.Helpers;
using ProductNotifier.Models;
using ProductNotifier.Repositories;

namespace ProductNotifier.Services;
public class EuclidRecords
{
    private readonly HttpClient _httpClient;
    private readonly EuclidRecordsDAO _euclidDao;
    private const string japaneseRecordsUrl = "/?genre_filter%5B%5D=japan";

    public EuclidRecords(HttpClient httpClient, EuclidRecordsDAO euclidDao)
    {
        _httpClient = httpClient;
        _euclidDao = euclidDao;
    }

    public async Task<IEnumerable<Record>> GetRecords()
    {
        var html = await _httpClient.GetStringAsync("/shop/?genre_filter%5B%5D=japan");
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        var products = doc.DocumentNode.SelectNodes("//li[contains(@class,'product')]");
        var records = new List<Record>();

        if (products != null)
        {
            foreach (var product in products)
            {
                var linkNode = product.SelectSingleNode(".//a[@href]");
                var titleNode = product.SelectSingleNode(".//h2[contains(@class,'woocommerce-loop-product__title')]");
                var artistNode = product.SelectSingleNode(".//div[contains(@class,'euclid-artist')]");
                var priceNode = product.SelectSingleNode(".//span[contains(@class,'woocommerce-Price-amount')]");

                var link = linkNode?.GetAttributeValue("href", string.Empty)?.Trim();
                var title = titleNode?.InnerText.Trim();
                var artist = artistNode?.InnerText.Trim();
                var price = priceNode?.InnerText.Trim().Replace("$", "").Replace("&#36;", "");
                double.TryParse(price, out var parsedPrice);

                records.Add(new Record
                {
                    Title = title,
                    Artist = artist,
                    Source = "Euclid",
                    DateAdded = DateTime.Now,
                    Price = parsedPrice,
                    Link = link
                });
            }
        }
        var newRecords = await SortOnlyNewRecords(records);
        return newRecords;
    }

    private async Task<IEnumerable<Record>> SortOnlyNewRecords(IEnumerable<Record> records)
    {
        var existing = _euclidDao.GetCataloguedRecords();

        var comparer = new Comparer();
        var newRecords = records
            .Except(existing, comparer)
            .ToList();

        if (newRecords.Any())
        {
            await _euclidDao.AddRecords(newRecords);
        }
        return newRecords;
    }
}
