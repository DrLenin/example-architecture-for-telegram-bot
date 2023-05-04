namespace PC.Common.Services;

public interface IPersonService
{
    Task<Person> CreateByChatId(long chatId, CancellationToken cancellationToken);

    Task<Person> SetBiography(Person person, string bio, CancellationToken cancellationToken);

    Task<Person> SetAddress(Person person, Address address, CancellationToken cancellationToken);

    Task<Person> SetWishes(Person person, string wishes, CancellationToken cancellationToken);

    Task SetRegisterStep(Person person, RegistrationSteps step, CancellationToken cancellationToken);

    Task SetCurrentCommandStep(Person person, string step, CancellationToken cancellationToken);
    
    Task SetName(Person person, string? userName, string? firstName, string? lastName, CancellationToken cancellationToken);
    
    Task SetFullName(Person person, string fullName, CancellationToken cancellationToken);
}

public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;
    private readonly IAddressRepository _addressRepository;

    public PersonService(IPersonRepository personRepository, IAddressRepository addressRepository)
    {
        _personRepository = personRepository;
        _addressRepository = addressRepository;
    }

    public async Task<Person> CreateByChatId(long chatId, CancellationToken cancellationToken)
    {
        var person = await _personRepository.Add(chatId, cancellationToken);
        await _personRepository.Save(cancellationToken);

        return person;
    }
    
    public async Task<Person> SetEmail(Person person, string email, CancellationToken cancellationToken)
    {
        person.Email = email;
        await _personRepository.Save(cancellationToken);

        return person;
    }

    public async Task<Person> SetPassword(Person person, string password, CancellationToken cancellationToken)
    {
        person.Password = Crypto.HashPassword(password);
        await _personRepository.Save(cancellationToken);

        return person;
    }

    public async Task<Person> SetBiography(Person person, string bio, CancellationToken cancellationToken)
    {
        person.Bio = bio;
        await _personRepository.Save(cancellationToken);

        return person;
    }

    public async Task<Person> SetAddress(Person person, Address address, CancellationToken cancellationToken)
    {
        foreach (var personAddress in person.Addresses)
            personAddress.IsDeleted = true;
        
        await _addressRepository.Add(address, cancellationToken);

        return person;
    }

    public async Task<Person> SetWishes(Person person, string wishes, CancellationToken cancellationToken)
    {
        person.Wishes = wishes;
        await _personRepository.Save(cancellationToken);

        return person;
    }

    public async Task SetRegisterStep(Person person, RegistrationSteps step, CancellationToken cancellationToken)
    {
        person.CurrentRegistrationStep = step;
        await _personRepository.Save(cancellationToken);
    }
    
    public async Task SetCurrentCommandStep(Person person, string step, CancellationToken cancellationToken)
    {
        person.CurrentCommandStep = step;
        await _personRepository.Save(cancellationToken);
    }

    public async Task SetName(Person person, string? userName, string? firstName, string? lastName, CancellationToken cancellationToken)
    {
        person.Username = userName ?? string.Empty;
        person.FirstName = firstName ?? string.Empty;
        person.LastName = lastName ?? string.Empty;
        await _personRepository.Save(cancellationToken);
    }

    public async Task SetFullName(Person person, string fullName, CancellationToken cancellationToken)
    {
        person.ReceiverName = fullName;
        await _personRepository.Save(cancellationToken);
    }
}