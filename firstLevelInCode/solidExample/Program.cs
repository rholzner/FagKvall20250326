// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

/*
SOLID Principles:
1. **Single Responsibility Principle (SRP)**: A class should have only one reason to change, focusing on a single responsibility.
2. **Open/Closed Principle (OCP)**: Classes should be open for extension but closed for modification.
3. **Liskov Substitution Principle (LSP)**: Subtypes must be substitutable for their base types without altering the correctness of the program.
4. **Interface Segregation Principle (ISP)**: Clients should not be forced to depend on interfaces they do not use.
5. **Dependency Inversion Principle (DIP)**: High-level modules should depend on abstractions, not on low-level modules.
*/

public interface IUserRepository
{
    void Add(User user);
    User Get(string name);
    void Update(User user);
    void Delete(string name);
}

public class UserRepository : IUserRepository
{
    private readonly List<User> _users = new();

    // SRP: UserRepository is responsible only for managing user data persistence.
    public void Add(User user)
    {
        _users.Add(user);
    }

    public User Get(string name)
    {
        return _users.FirstOrDefault(u => u.Name == name);
    }

    public void Update(User user)
    {
        var existingUser = Get(user.Name);
        if (existingUser != null)
        {
            existingUser.Age = user.Age;
        }
    }

    public void Delete(string name)
    {
        var user = Get(name);
        if (user != null)
        {
            _users.Remove(user);
        }
    }
}

public interface IUserValidator
{
    void Validate(User user);
}

public class UserValidator : IUserValidator
{
    // SRP: UserValidator is responsible only for validation logic.
    public void Validate(User user)
    {
        if (user.Age < 18)
        {
            throw new ArgumentException("User must be at least 18 years old.");
        }
    }
}

public interface IUserService
{
    void AddUser(User user);
    User GetUser(string name);
    void UpdateUser(User user);
    void DeleteUser(string name);
}

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserValidator _userValidator;

    // DIP: UserService depends on abstractions (IUserRepository and IUserValidator),
    // not concrete implementations, making it more flexible and testable.
    public UserService(IUserRepository userRepository, IUserValidator userValidator)
    {
        _userRepository = userRepository;
        _userValidator = userValidator;
    }

    // SRP: UserService delegates validation to UserValidator and persistence to UserRepository,
    // ensuring it does not handle multiple responsibilities.
    public void AddUser(User user)
    {
        _userValidator.Validate(user); // OCP: Validation logic can be extended by modifying UserValidator.
        _userRepository.Add(user); // OCP: Persistence logic can be extended by modifying UserRepository.
    }

    public User GetUser(string name)
    {
        return _userRepository.Get(name); // LSP: Works seamlessly with any IUserRepository implementation.
    }

    public void UpdateUser(User user)
    {
        _userValidator.Validate(user); // OCP: Validation logic is extendable.
        _userRepository.Update(user); // OCP: Update logic is extendable.
    }

    public void DeleteUser(string name)
    {
        _userRepository.Delete(name); // LSP: Works seamlessly with any IUserRepository implementation.
    }
}

public class User
{
    // SRP: User is responsible only for holding its own data (Name and Age).
    public string Name { get; set; }
    public int Age { get; set; }
}