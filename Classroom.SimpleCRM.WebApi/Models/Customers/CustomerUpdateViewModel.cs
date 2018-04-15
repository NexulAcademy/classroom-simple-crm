using System.ComponentModel.DataAnnotations;

namespace Classroom.SimpleCRM.WebApi.Models.Customers
{
    /// <summary>
    /// Update is currently the same as create model, but in the future they will likely diverge.
    /// </summary>
    public class CustomerUpdateViewModel
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
