namespace PC.Store;

public class PostCrossContext : DbContext
{
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Person> Persons => Set<Person>();
    public DbSet<PostCard> PostCards => Set<PostCard>();

    public PostCrossContext(DbContextOptions<PostCrossContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasSequence<long>(nameof(PostCard.Code), Consts.DbSchema)
            .StartsAt(10000)
            .IncrementsBy(1);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PersonConfiguration).Assembly);
        base.OnModelCreating(modelBuilder);
    }
    
    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = new())
    {
        UpdateDate(ChangeTracker);

        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        UpdateDate(ChangeTracker);

        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        UpdateDate(ChangeTracker);

        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override int SaveChanges()
    {
        UpdateDate(ChangeTracker);

        return base.SaveChanges();
    }

    private static void UpdateDate(ChangeTracker changeTracker)
    {
        var dateTimeNow = DateTime.UtcNow;

        foreach (var entityEntry in changeTracker.Entries())
        {
            if (entityEntry.Entity is not BaseEntity entity) continue;
            
            if (entityEntry.State == EntityState.Deleted)
            {
                entity.IsDeleted = true;
                entityEntry.State = EntityState.Modified;
            }

            if (entityEntry.State is EntityState.Modified or EntityState.Added)
                entity.UpdatedDate = dateTimeNow;

            if (entityEntry.State == EntityState.Added && entity.CreatedDate == default)
                entity.CreatedDate = dateTimeNow;
        }
    }
}