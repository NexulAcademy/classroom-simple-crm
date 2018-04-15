using System.ComponentModel.DataAnnotations;

namespace Classroom.SimpleCRM.WebApi.Models.Customers
{
    public class CustomerCreateViewModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        public InteractionMethod PreferredContactMethod { get; set; }
    }
}
