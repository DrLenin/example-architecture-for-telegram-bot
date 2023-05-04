namespace PC.Common.Services;

public interface IAddressService
{
    Task<Address> SetPostalCode(Address address, string postalCode, CancellationToken cancellationToken);
}

public class AddressService : IAddressService
{
    private readonly IAddressRepository _personRepository;

    public AddressService(IAddressRepository addressRepository)
    {
        _personRepository = addressRepository;
    }

    public async Task<Address> SetPostalCode(Address address, string postalCode, CancellationToken cancellationToken)
    {
        address.PostalCode = postalCode;
        await _personRepository.Save(cancellationToken);

        return address;
    }
}