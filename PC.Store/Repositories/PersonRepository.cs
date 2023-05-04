namespace PC.Store.Repositories;

public interface IPersonRepository
{
    Task<Person?> GetByChatId(long chatId);
    
    Task<Person> Add(long chatId, CancellationToken cancellationToken);
    
    Task Save(CancellationToken cancellationToken);
}

public class PersonRepository : BaseRepository, IPersonRepository
{
    public PersonRepository(PostCrossContext dbContext) : base(dbContext)
    {
    }

    public Task<Person?> GetByChatId(long chatId)
    {
        return DbContext.Persons
            .Include(x => x.Addresses)
            .FirstOrDefaultAsync(x => x.ChatId == chatId && !x.IsDeleted);
    }

    public async Task<Person> Add(long chatId, CancellationToken cancellationToken)
    {
        var person = await DbContext.Persons.AddAsync(
            new Person { ChatId = chatId}, cancellationToken);

        await DbContext.SaveChangesAsync(cancellationToken);

        return person.Entity;
    }

    public async Task Save(CancellationToken cancellationToken)
    {
        await DbContext.SaveChangesAsync(cancellationToken);
    }
}