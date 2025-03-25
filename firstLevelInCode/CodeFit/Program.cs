// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

public class UserService
{
    private readonly UserRepository _userRepository;
    private readonly UserValidator _userValidator;

    public UserService(UserRepository userRepository, UserValidator userValidator)
    {
        _userRepository = userRepository;
        _userValidator = userValidator;
    }

    public void AddUser(User user)
    {
        ValidateUser(user); // Rule 9: Tell, don't ask. Validation is encapsulated in the UserValidator.
        SaveUser(user);     // Rule 9: Tell, don't ask. Saving logic is encapsulated in the UserRepository.
    }

    public User FindUserByName(UserName name)
    {
        return _userRepository.Get(name); // Rule 5: One Dot Per Line. Directly calls the repository method.
    }

    public void UpdateUser(User user)
    {
        ValidateUser(user); // Rule 9: Tell, don't ask.
        ReplaceUser(user);  // Rule 9: Tell, don't ask.
    }

    public void RemoveUser(UserName name)
    {
        DeleteUserByName(name); // Rule 9: Tell, don't ask.
    }

    private void ValidateUser(User user)
    {
        _userValidator.Validate(user); // Rule 9: Tell, don't ask.
    }

    private void SaveUser(User user)
    {
        _userRepository.Add(user); // Rule 9: Tell, don't ask.
    }

    private void ReplaceUser(User user)
    {
        _userRepository.Update(user); // Rule 9: Tell, don't ask.
    }

    private void DeleteUserByName(UserName name)
    {
        _userRepository.Delete(name); // Rule 9: Tell, don't ask.
    }
}

public class UserValidator
{
    public void Validate(User user)
    {
        if (!user.IsAdult())
        {
            throw new ArgumentException("User must be at least 18 years old.");
        }
    }
    // Rule 7: Keep All Entities Small. This class has a single responsibility and is concise.
}

public class UserRepository
{
    private readonly UserCollection _users = new();

    public void Add(User user)
    {
        _users.Add(user); // Rule 4: First Class Collections. UserCollection encapsulates collection behavior.
    }

    public User Get(UserName name)
    {
        return _users.FindByName(name); // Rule 5: One Dot Per Line. Delegates to UserCollection.
    }

    public void Update(User user)
    {
        _users.Replace(user); // Rule 5: One Dot Per Line. Delegates to UserCollection.
    }

    public void Delete(UserName name)
    {
        _users.RemoveByName(name); // Rule 5: One Dot Per Line. Delegates to UserCollection.
    }
}

public class User
{
    private readonly UserName _name;
    private readonly Age _age;

    public User(UserName name, Age age)
    {
        _name = name;
        _age = age;
    }

    public bool IsAdult()
    {
        return _age.IsAdult(); // Rule 5: One Dot Per Line. Delegates to Age.
    }

    public UserName Name => _name; // Rule 3: Wrap All Primitives And Strings. UserName is a value object.
    public Age Age => _age;       // Rule 3: Wrap All Primitives And Strings. Age is a value object.
}

public class UserName
{
    private readonly string _value;

    public UserName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Name cannot be empty.");
        }
        _value = value;
    }

    public override string ToString()
    {
        return _value;
    }
    // Rule 3: Wrap All Primitives And Strings. Encapsulates string behavior.
}

public class Age
{
    private readonly int _value;

    public Age(int value)
    {
        if (value < 0)
        {
            throw new ArgumentException("Age cannot be negative.");
        }
        _value = value;
    }

    public bool IsAdult()
    {
        return _value >= 18; // Rule 9: Tell, don't ask. Encapsulates the logic for determining adulthood.
    }

    public override string ToString()
    {
        return _value.ToString();
    }
    // Rule 3: Wrap All Primitives And Strings. Encapsulates integer behavior.
}

public class UserCollection
{
    private readonly List<User> _users = new();

    public void Add(User user)
    {
        _users.Add(user); // Rule 4: First Class Collections. Encapsulates collection behavior.
    }

    public User FindByName(UserName name)
    {
        return _users.FirstOrDefault(u => u.Name.ToString() == name.ToString()); // Rule 5: One Dot Per Line.
    }

    public void Replace(User user)
    {
        RemoveByName(user.Name); // Rule 9: Tell, don't ask.
        Add(user);               // Rule 9: Tell, don't ask.
    }

    public void RemoveByName(UserName name)
    {
        var user = FindByName(name); // Rule 5: One Dot Per Line.
        if (user != null)
        {
            _users.Remove(user);
        }
    }
    // Rule 4: First Class Collections. Encapsulates all collection-related logic.
}
