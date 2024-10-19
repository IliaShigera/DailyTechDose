namespace DailyTechDose.Core.Interfaces;

/// <summary>
/// Provides direct access to DbSets, enabling streamlined data access with Entity Framework Core's CRUD operations. 
/// Implementing this interface in the DbContext class eliminates the need for a separate repository service.
/// </summary>
public interface IRepository
{
    DbSet<Source> Sources { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}