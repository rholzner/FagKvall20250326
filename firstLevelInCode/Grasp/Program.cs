// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

/*
GRASP Principles:
1. **Information Expert**: Assign responsibility to the class that has the necessary information to fulfill it.
2. **Creator**: Assign responsibility for creating an object to the class that has the information to initialize it.
3. **Low Coupling**: Minimize dependencies between classes to make the system easier to maintain and extend.
4. **High Cohesion**: Ensure that a class has a focused responsibility, making it easier to understand and maintain.
5. **Controller**: Assign responsibility for handling system events to a controller class.
*/

public class UserService
{
    private readonly UserRepository _userRepository;
    private readonly UserValidator _userValidator;

    // Creator: UserService creates and uses UserRepository and UserValidator
    // because it needs their functionality to manage users and validate them.
    public UserService()
    {
        _userRepository = new UserRepository();
        _userValidator = new UserValidator();
    }

    // Information Expert: UserService delegates validation to UserValidator
    // and persistence to UserRepository, ensuring responsibilities are assigned
    // to the most knowledgeable classes.
    public void AddUser(string name, int age)
    {
        _userValidator.ValidateAge(age); // Low Coupling: Delegates validation to UserValidator.
        var user = new User { Name = name, Age = age }; // Creator: UserService creates the User object.
        _userRepository.Add(user); // Low Coupling: Delegates persistence to UserRepository.
    }

    public User GetUser(string name)
    {
        return _userRepository.Get(name); // Information Expert: UserRepository knows how to retrieve users.
    }

    public void UpdateUser(string name, int age)
    {
        var user = _userRepository.Get(name); // Information Expert: Retrieves the user from UserRepository.
        if (user != null)
        {
            _userValidator.ValidateAge(age); // Low Coupling: Delegates validation to UserValidator.
            user.Age = age; // High Cohesion: UserService focuses on updating user data.
            _userRepository.Update(user); // Low Coupling: Delegates persistence to UserRepository.
        }
    }

    public void DeleteUser(string name)
    {
        _userRepository.Delete(name); // Low Coupling: Delegates deletion to UserRepository.
    }
}

public class UserValidator
{
    // Information Expert: UserValidator is responsible for validation logic
    // because it has the necessary knowledge about validation rules.
    public void ValidateAge(int age)
    {
        if (age < 18)
        {
            throw new ArgumentException("User must be at least 18 years old.");
        }
    }
}

public class UserRepository
{
    private readonly List<User> _users = new();

    // Information Expert: UserRepository is responsible for managing the user list
    // because it has the necessary knowledge about the data structure.
    public void Add(User user)
    {
        _users.Add(user);
    }

    public User Get(string name)
    {
        return _users.FirstOrDefault(u => u.Name == name); // High Cohesion: Focuses on retrieving users.
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
    // Information Expert: User is responsible for holding its own data (Name and Age).
    public string Name { get; set; }
    public int Age { get; set; }
}
