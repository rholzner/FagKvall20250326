// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

/*
CUPID Principles:
1. **Composability**: Components should be modular and reusable, allowing them to be composed together easily.
2. **Unix Philosophy**: Each component should do one thing well, focusing on a single responsibility.
3. **Predictability**: Behavior should be consistent and easy to understand, avoiding surprises.
4. **Idiomatic**: Code should follow the conventions and idioms of the language or framework being used.
5. **Domain-based**: Code should align with the domain it represents, making it intuitive and meaningful.
*/

public class UserService
{
    private readonly UserRepository _userRepository;
    private readonly UserValidator _userValidator;

    // Composability: UserService is composed of UserRepository and UserValidator,
    // allowing each component to focus on its specific responsibility.
    public UserService(UserRepository userRepository, UserValidator userValidator)
    {
        _userRepository = userRepository;
        _userValidator = userValidator;
    }

    // Unix Philosophy: Each method does one thing well. AddUser validates and adds a user.
    public void AddUser(User user)
    {
        _userValidator.Validate(user); // Predictability: Validation is clear and consistent.
        _userRepository.Add(user); // Composability: Delegates persistence to UserRepository.
    }

    public User GetUser(string name)
    {
        return _userRepository.Get(name); // Predictability: Retrieval logic is simple and consistent.
    }

    public void UpdateUser(User user)
    {
        _userValidator.Validate(user); // Predictability: Ensures user data is valid before updating.
        _userRepository.Update(user); // Composability: Delegates update logic to UserRepository.
    }

    public void DeleteUser(string name)
    {
        _userRepository.Delete(name); // Unix Philosophy: Focuses solely on deletion.
    }
}

public class UserValidator
{
    // Predictability: Validation logic is simple and predictable, ensuring clear error handling.
    public void Validate(User user)
    {
        if (user.Age < 18)
        {
            throw new ArgumentException("User must be at least 18 years old.");
        }
    }
}

public class UserRepository
{
    private readonly List<User> _users = new();

    // Composability: UserRepository is a reusable component for managing user data.
    public void Add(User user)
    {
        _users.Add(user);
    }

    public User Get(string name)
    {
        return _users.FirstOrDefault(u => u.Name == name); // Predictability: Retrieval is straightforward.
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

public class User
{
    // Predictability: User class is simple and predictable, holding only user data.
    public string Name { get; set; }
    public int Age { get; set; }
}
