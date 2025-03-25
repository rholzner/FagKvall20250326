// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

/*
Model-Driven Architecture (MDA):
1. **Platform-Independent Model (PIM)**: Defines the business logic and domain models independent of any specific platform or technology.
2. **Platform-Specific Model (PSM)**: Adapts the PIM to a specific platform or technology (e.g., database, REST API).
3. **Transformation**: PIMs are transformed into PSMs using tools or manual implementation.
4. **Separation of Concerns**: Business logic is separated from platform-specific concerns.
*/

// Platform-Independent Model (PIM)
namespace PIM
{
    // Domain Model: User
    public class User
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public User(string name, int age)
        {
            if (age < 18)
            {
                throw new ArgumentException("User must be at least 18 years old.");
            }
            Name = name;
            Age = age;
        }
    }

    // Domain Model: Booking
    public class Booking
    {
        public string UserName { get; set; }
        public DateTime BookingDate { get; set; }

        public Booking(string userName, DateTime bookingDate)
        {
            UserName = userName;
            BookingDate = bookingDate;
        }
    }

    // Service Interface: IUserService
    public interface IUserService
    {
        void RegisterUser(string name, int age);
        User GetUser(string name);
        void DeleteUser(string name);
    }

    // Service Interface: IBookingService
    public interface IBookingService
    {
        void CreateBooking(string userName, DateTime bookingDate);
        List<Booking> GetBookingsForUser(string userName);
    }
}

// Platform-Specific Model (PSM)
namespace PSM
{
    using PIM;

    // Repository: UserRepository
    public class UserRepository
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

    // Repository: BookingRepository
    public class BookingRepository
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

    // Service Implementation: UserService
    public class UserService : IUserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
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

    // Service Implementation: BookingService
    public class BookingService : IBookingService
    {
        private readonly BookingRepository _bookingRepository;
        private readonly UserRepository _userRepository;

        public BookingService(BookingRepository bookingRepository, UserRepository userRepository)
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
