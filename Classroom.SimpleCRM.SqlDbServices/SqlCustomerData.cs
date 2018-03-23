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
        private ColumnMapping mappingCustomer;

        public SqlCustomerData(CrmDbContext context)
        {
            this.context = context;
            mappingCustomer = new ColumnMapping(new[] {
                new ColumnMappingValue("FirstName"),
                new ColumnMappingValue("LastName"),
                new ColumnMappingValue("Name", new[] { "LastName", "FirstName" }), //custom sort option
                new ColumnMappingValue("EmailAddress"),
                new ColumnMappingValue("PhoneNumber"),
                new ColumnMappingValue("Status"),
                new ColumnMappingValue("LastContactDate")
            }); //TODO: create an overload to build base mappings using reflection and allow custom additions
        }

        public Customer Get(int customerId)
        {
            return context.Customer.FirstOrDefault(x => x.CustomerId == customerId );
        }
        public List<Customer> GetByStatus(int accountId, CustomerStatus status, int pageIndex, int take, string orderBy)
        {
            return context.Customer
                .Where(x => x.Status == status)
                .ApplySort(orderBy, mappingCustomer)
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
