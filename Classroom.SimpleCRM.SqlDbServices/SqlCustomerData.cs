using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Classroom.SimpleCRM.SqlDbServices
{
    public class SqlCustomerData : ICustomerData
    {
        private readonly CrmDbContext context;

        public SqlCustomerData(CrmDbContext context)
        {
            this.context = context;
        }

        public Customer Get(int customerId)
        {
            return context.Customer.FirstOrDefault(x => x.CustomerId == customerId );
        }
        public List<Customer> GetByStatus(int accountId, CustomerStatus status, int pageIndex, int take, string orderBy)
        {
            var sortableFields = new string[] { "FIRSTNAME", "LASTNAME", "EMAILADDRESS", "PHONENUMBER", "STATUS", "LASTCONTACTDATE" };
            var fields = (orderBy ?? "").Split(new[]{','}, StringSplitOptions.RemoveEmptyEntries);
            foreach(var field in fields)
            {
                var x = field.Trim().ToUpper();
                var parts = x.Split(' ');
                if (parts.Length > 2)
                    throw new ArgumentException("Invalid sort option " + x);
                if (parts.Length > 1 && parts[1].ToUpper() != "DESC" && parts[1].ToUpper() != "ASC")
                    throw new ArgumentException("Invalid sort direction " + x);
                if (!sortableFields.Contains(x))
                    throw new ArgumentException("Invalid sort field " + x);
            } //all sort requested fields are valid.
            return context.Customer
                .Where(x => x.Status == status)
                .OrderBy(orderBy) //validated above to nothing unexpected, this is OK now
                .Skip(pageIndex * take)
                .Take(take)
                .ToList();
        }
        public void Add(Customer item)
        {
            context.Add(item);
        }
        public void Update(Customer item)
        {
            //do nothing, changes are tracked automatically
        }
        public void Delete(Customer item)
        {
            context.Remove(item);
        }
        public void Commit()
        {
            context.SaveChanges();
        }
    }
}
