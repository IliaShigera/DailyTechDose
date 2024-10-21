namespace DailyTechDose.Infrastructure.Data.Repo;

internal sealed class Repository : IRepository
{
    private readonly AppDbContext _dbContext;

    public Repository(AppDbContext dbContext) => _dbContext = dbContext;

    public async Task<IReadOnlyList<Source>> ListAllAsync(bool includePendingContent = false)
    {
        var query = _dbContext.Sources.AsQueryable();

        if (includePendingContent)
        {
            query = query.Where(src => src.ContentItems.Any(content => !content.IsPublished))
                .Include(src => src.ContentItems.Where(content => !content.IsPublished));
        }

        return await query.ToListAsync();
    }

    public void Update(Source source) => _dbContext.Sources.Update(source);

    public void UpdateRange(IReadOnlyList<Source> sources) => _dbContext.Sources.UpdateRange(sources);

    public async Task SaveChangesAsync() => await _dbContext.SaveChangesAsync();
}