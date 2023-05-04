namespace PC.Common.Services;

public interface IPostCardService
{

    Task<(int countCreated, int countSent, int countReceived)> CountForSenderPostCard(Guid senderId, CancellationToken cancellationToken);
    Task<(int countCreated, int countSent, int countReceived)> CountForReceiverPostCard(Guid receiverId, CancellationToken cancellationToken);
}

public class PostCardService : IPostCardService
{
    private readonly IPostCardRepository _postCardRepository;

    public PostCardService(IPostCardRepository postCardRepository)
    {
        _postCardRepository = postCardRepository;
    }

    public async Task<(int countCreated, int countSent, int countReceived)> CountForSenderPostCard(Guid senderId, CancellationToken cancellationToken)
    {
        var countCreated = await _postCardRepository.CountForSender(senderId, PostCardStatus.Created, cancellationToken);
        var countSent = await _postCardRepository.CountForSender(senderId, PostCardStatus.Sent, cancellationToken);
        var countReceived = await _postCardRepository.CountForSender(senderId, PostCardStatus.Received, cancellationToken);

        return (countCreated, countSent, countReceived);
    }

    public async Task<(int countCreated, int countSent, int countReceived)> CountForReceiverPostCard(Guid receiverId, CancellationToken cancellationToken)
    {
        var countCreated = await _postCardRepository.CountForReceiver(receiverId, PostCardStatus.Created, cancellationToken);
        var countSent = await _postCardRepository.CountForReceiver(receiverId, PostCardStatus.Sent, cancellationToken);
        var countReceived = await _postCardRepository.CountForReceiver(receiverId, PostCardStatus.Received, cancellationToken);

        return (countCreated, countSent, countReceived);
    }
}