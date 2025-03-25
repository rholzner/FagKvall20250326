// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

/*
Feature Folders:
- Each feature (e.g., User Management, Booking Management) is organized into its own folder.
- Each folder contains all the components (e.g., services, repositories, models) related to that feature.
- This structure improves modularity, maintainability, and scalability.
*/

// Feature: User Management
namespace Features.UserManagement
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void AddUser(User user)
        {
            if (user.Age < 18)
            {
                throw new ArgumentException("User must be at least 18 years old.");
            }
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

    public class User
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}

// Feature: Booking Management
namespace Features.BookingManagement
{
    public class BookingService
    {
        private readonly BookingRepository _bookingRepository;
        private readonly Features.UserManagement.UserRepository _userRepository;

        public BookingService(BookingRepository bookingRepository, Features.UserManagement.UserRepository userRepository)
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

            var booking = new Booking
            {
                UserName = userName,
                BookingDate = bookingDate
            };

            _bookingRepository.Add(booking);
        }

        public List<Booking> GetBookingsForUser(string userName)
        {
            return _bookingRepository.GetByUser(userName);
        }
    }

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

    public class Booking
    {
        public string UserName { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
