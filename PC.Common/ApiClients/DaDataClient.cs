namespace PC.Common.ApiClients;

public interface IDaDataClient
{
    Task<AddressDaData> CheckAddress(string rowAddress);

    Task<Email> CheckEmail(string rowEmail);

    Task<Fullname> CheckFullname(string rowFullname);
}

public class DaDataClient : IDaDataClient
{
    private readonly CleanClientAsync _cleanClient;

    public DaDataClient(CleanClientAsync cleanClient)
    {
        _cleanClient = cleanClient;
    }
    
    public async Task<AddressDaData> CheckAddress(string rowAddress)
    {
        var address = await _cleanClient.Clean<AddressDaData>(rowAddress);

        return address;
    }
    
    public async Task<Email> CheckEmail(string rowEmail)
    {
        var email = await _cleanClient.Clean<Email>(rowEmail);

        return email;
    }
    
    public async Task<Fullname> CheckFullname(string rowFullname)
    {
        var fullname = await _cleanClient.Clean<Fullname>(rowFullname);

        return fullname;
    }
}