using System;

namespace Classroom.SimpleCRM
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public InteractionMethod PreferredContactMethod { get; set; }
        public CustomerStatus Status { get; set; }
        /// <summary>
        /// The last date and time this contact was updated.
        /// TODO: rename to better align with purpose
        /// </summary>
        public DateTime LastContactDate { get; set; }

    }
}
