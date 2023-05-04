namespace PC.Store.Repositories;

public interface IAddressRepository
{
    Task Save(CancellationToken cancellationToken);
    
    Task<Address> Add(Address address, CancellationToken cancellationToken);
}

public class AddressRepository : BaseRepository, IAddressRepository
{
    public AddressRepository(PostCrossContext dbContext) : base(dbContext)
    {
    }
    
    public async Task Save(CancellationToken cancellationToken)
    {
        await DbContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<Address> Add(Address address, CancellationToken cancellationToken)
    {
        await DbContext.Addresses.AddAsync(
            address, cancellationToken);

        await DbContext.SaveChangesAsync(cancellationToken);

        return address;
    }
}