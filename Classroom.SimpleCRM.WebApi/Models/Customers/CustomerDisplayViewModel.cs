using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Classroom.SimpleCRM;
using System.Globalization;

namespace Classroom.SimpleCRM.WebApi.Models.Customers
{
    public class CustomerDisplayViewModel
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string PreferredContactMethod { get; set; }
        public string Status { get; set; }
        public string LastContactDate { get; set; }

        public CustomerDisplayViewModel() { }
        public CustomerDisplayViewModel(Customer source)
        {
            if (source == null)
                return;
            CustomerId = source.CustomerId;
            FirstName = source.FirstName;
            LastName = source.LastName;
            EmailAddress = source.EmailAddress;
            PhoneNumber = source.PhoneNumber;
            Status = Enum.GetName(typeof(CustomerStatus), source.Status);
            PreferredContactMethod = Enum.GetName(typeof(InteractionMethod), source.PreferredContactMethod);
            LastContactDate = source.LastContactDate.Year > 1 ? source.LastContactDate.ToString("s", CultureInfo.InstalledUICulture) : "";
        }
    }
}
