namespace ProductNotifier.Controllers;
using Microsoft.AspNetCore.Mvc;
using ProductNotifier.Models;
using ProductNotifier.Services;
using System.Net;

[ApiController]
public class ProductApi
{
    private readonly EuclidRecords _euclid;
    public ProductApi(EuclidRecords euclid)
    {
        _euclid = euclid;
    }

    [HttpGet("FetchRecords")]
    public async Task<IEnumerable<Record>> FetchRecords()
    {
        try
        {
            var euclidRecords = await _euclid.GetRecords();
            return euclidRecords;
        }
        catch (Exception ex)
        {
            throw new Exception("somebody had an oopsie", ex);
        }
    }
}
