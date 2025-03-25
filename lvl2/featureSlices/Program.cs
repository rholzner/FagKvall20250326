// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

/*
Feature Slices:
- Components are grouped by their type (e.g., services, repositories, models).
- Each feature (e.g., User Management, Booking Management) is represented by its own slice across these layers.
- This structure emphasizes separation of concerns and reusability while maintaining modularity.
*/

// Services
namespace Services
{
    public class UserService
    {
        private readonly Repositories.UserRepository _userRepository;

        public UserService(Repositories.UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void AddUser(Models.User user)
        {
            if (user.Age < 18)
            {
                throw new ArgumentException("User must be at least 18 years old.");
            }
            _userRepository.Add(user);
        }

        public Models.User GetUser(string name)
        {
            return _userRepository.Get(name);
        }

        public void DeleteUser(string name)
        {
            _userRepository.Delete(name);
        }
    }

    public class BookingService
    {
        private readonly Repositories.BookingRepository _bookingRepository;
        private readonly Repositories.UserRepository _userRepository;

        public BookingService(Repositories.BookingRepository bookingRepository, Repositories.UserRepository userRepository)
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

            var booking = new Models.Booking
            {
                UserName = userName,
                BookingDate = bookingDate
            };

            _bookingRepository.Add(booking);
        }

        public List<Models.Booking> GetBookingsForUser(string userName)
        {
            return _bookingRepository.GetByUser(userName);
        }
    }
}

// Repositories
namespace Repositories
{
    public class UserRepository
    {
        private readonly List<Models.User> _users = new();

        public void Add(Models.User user)
        {
            _users.Add(user);
        }

        public Models.User Get(string name)
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

    public class BookingRepository
    {
        private readonly List<Models.Booking> _bookings = new();

        public void Add(Models.Booking booking)
        {
            _bookings.Add(booking);
        }

        public List<Models.Booking> GetByUser(string userName)
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
