namespace DailyTechDose.Core.Interfaces;

public interface IRepository
{ 
    Task<IReadOnlyList<Source>> ListAllAsync(bool includePendingContent = false);
    
    void Update(Source source);
    void UpdateRange(IReadOnlyList<Source> sources);
    
    Task SaveChangesAsync();
}