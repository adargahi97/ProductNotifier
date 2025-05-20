using Microsoft.EntityFrameworkCore;
using ProductNotifier.Models;

namespace ProductNotifier.Repositories;
public class EuclidRecordsDAO
{
    private readonly ProductDbContext _dbContext;

    public EuclidRecordsDAO(ProductDbContext context)
    {
        _dbContext = context;
    }

    public IEnumerable<Record> GetCataloguedRecords()
    {
        return _dbContext.Set<Record>().ToList();

    }

    public async Task AddRecords(IEnumerable<Record> records)
    {
        _dbContext.AddRange(records);
        await _dbContext.SaveChangesAsync();
    }
}
