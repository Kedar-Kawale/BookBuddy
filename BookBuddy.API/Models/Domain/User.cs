using Microsoft.AspNetCore.Hosting.Server;
using System.ComponentModel.DataAnnotations;

namespace BookBuddy.API.Models.Domain
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }  //PK as Guid
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PinCode { get; set; } = null!;
        public string Role { get; set; } = "Customer";
        public DateTime UserRegisteredAt { get; set; } = DateTime.UtcNow;  //UTC -Universal time instead of local IST
       /* Why UTC is critical:
          1. Server might be in USA/Europe(different timezone)
          2. API consumed globally
          3. No Daylight Saving confusion
          4. Consistent across all users/databases
*/
    }
}
