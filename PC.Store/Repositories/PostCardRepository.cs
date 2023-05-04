namespace PC.Store.Repositories;

public interface IPostCardRepository
{
    Task<int> CountForSender(Guid senderId, PostCardStatus status, CancellationToken cancellationToken);
    Task<int> CountForReceiver(Guid receiverId, PostCardStatus status, CancellationToken cancellationToken);
}

public class PostCardRepository : BaseRepository, IPostCardRepository
{
    public PostCardRepository(PostCrossContext dbContext) : base(dbContext)
    {
    }

    public Task<int> CountForSender(Guid senderId, PostCardStatus status, CancellationToken cancellationToken)
    {
        return DbContext.PostCards.Where(d => senderId == d.SenderId && d.Status == status).CountAsync(cancellationToken: cancellationToken);
    }

    public Task<int> CountForReceiver(Guid receiverId, PostCardStatus status, CancellationToken cancellationToken)
    {
        return DbContext.PostCards.Where(d => receiverId == d.ReceiverId && d.Status == status).CountAsync(cancellationToken: cancellationToken);
    }
}