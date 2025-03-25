// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

/*
Domain-Driven Design (DDD):
1. **Entities**: Represent core domain objects with unique identities (e.g., User, Booking).
2. **Value Objects**: Represent immutable concepts without unique identities (not used in this example).
3. **Aggregates**: Group related entities under a root entity (e.g., User as the root of user-related operations).
4. **Repositories**: Handle persistence for aggregates.
5. **Services**: Contain domain logic that doesn't naturally belong to an entity or aggregate.
6. **Bounded Contexts**: Define clear boundaries for different parts of the domain (e.g., User Management, Booking Management).
*/

// Bounded Context: User Management
namespace Domain.UserManagement
{
    // Entity: User
    public class User
    {
        public string Name { get; private set; }
        public int Age { get; private set; }

        public User(string name, int age)
        {
            if (age < 18)
            {
                throw new ArgumentException("User must be at least 18 years old.");
            }
            Name = name;
            Age = age;
        }

        public void UpdateAge(int newAge)
        {
            if (newAge < 18)
            {
                throw new ArgumentException("User must be at least 18 years old.");
            }
            Age = newAge;
        }
    }

    // Repository: UserRepository
    public interface IUserRepository
    {
        void Add(User user);
        User Get(string name);
        void Delete(string name);
    }

    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users = new();

        public void Add(User user)
        {
            _users.Add(user);
        }

        public User Get(string name)
        {
            return _users.FirstOrDefault(u => u.Name == name);
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

    // Service: UserService
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void RegisterUser(string name, int age)
        {
            var user = new User(name, age);
            _userRepository.Add(user);
        }

        public User GetUser(string name)
        {
            return _userRepository.Get(name);
        }

        public void DeleteUser(string name)
        {
            _userRepository.Delete(name);
        }
    }
}

// Bounded Context: Booking Management
namespace Domain.BookingManagement
{
    using Domain.UserManagement;

    // Entity: Booking
    public class Booking
    {
        public string UserName { get; private set; }
        public DateTime BookingDate { get; private set; }

        public Booking(string userName, DateTime bookingDate)
        {
            UserName = userName;
            BookingDate = bookingDate;
        }
    }

    // Repository: BookingRepository
    public interface IBookingRepository
    {
        void Add(Booking booking);
        List<Booking> GetByUser(string userName);
    }

    public class BookingRepository : IBookingRepository
    {
        private readonly List<Booking> _bookings = new();

        public void Add(Booking booking)
        {
            _bookings.Add(booking);
        }

        public List<Booking> GetByUser(string userName)
        {
            return _bookings.Where(b => b.UserName == userName).ToList();
        }
    }

    // Service: BookingService
    public class BookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;

        public BookingService(IBookingRepository bookingRepository, IUserRepository userRepository)
        {
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
        }

        public void CreateBooking(string userName, DateTime bookingDate)
        {
            var user = _userRepository.Get(userName);
            if (user == null)
            {
                throw new ArgumentException("User does not exist.");
            }

            var booking = new Booking(userName, bookingDate);
            _bookingRepository.Add(booking);
        }

        public List<Booking> GetBookingsForUser(string userName)
        {
            return _bookingRepository.GetByUser(userName);
        }
    }
}
