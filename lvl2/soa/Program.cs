// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

/*
Service-Oriented Architecture (SOA):
1. **Service Independence**: Each service is self-contained and provides a specific business capability.
2. **Loose Coupling**: Services interact through well-defined interfaces, minimizing dependencies.
3. **Reusability**: Services are designed to be reusable across different applications or contexts.
4. **Interoperability**: Services communicate using standard protocols (e.g., HTTP, REST, or messaging systems).
5. **Scalability**: Services can be scaled independently based on demand.
*/

// Service: UserService
namespace Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Service Independence: Provides user registration as a standalone capability.
        public void RegisterUser(string name, int age)
        {
            if (age < 18)
            {
                throw new ArgumentException("User must be at least 18 years old.");
            }

            var user = new User { Name = name, Age = age };
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

// Service: BookingService
namespace Services
{
    public class BookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;

        public BookingService(IBookingRepository bookingRepository, IUserRepository userRepository)
        {
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
        }

        // Loose Coupling: BookingService interacts with UserService through IUserRepository.
        public void CreateBooking(string userName, DateTime bookingDate)
        {
            var user = _userRepository.Get(userName);
            if (user == null)
            {
                throw new ArgumentException("User does not exist.");
            }

            var booking = new Booking { UserName = userName, BookingDate = bookingDate };
            _bookingRepository.Add(booking);
        }

        public List<Booking> GetBookingsForUser(string userName)
        {
            return _bookingRepository.GetByUser(userName);
        }
    }
}

// Repository Interfaces
namespace Repositories
{
    public interface IUserRepository
    {
        void Add(User user);
        User Get(string name);
        void Delete(string name);
    }

    public interface IBookingRepository
    {
        void Add(Booking booking);
        List<Booking> GetByUser(string userName);
    }
}

// Repository Implementations
namespace Repositories
{
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
}

// Models
namespace Models
{
    public class User
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class Booking
    {
        public string UserName { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
