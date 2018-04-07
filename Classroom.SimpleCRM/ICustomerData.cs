using System.Collections.Generic;

namespace Classroom.SimpleCRM
{
    public interface ICustomerData
    {
        /// <summary>
        /// Gets a single customer, by id.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Customer Get(int customerId);
        List<Customer> GetAll(int accountId,
            int pageIndex, int take, string orderBy);
        /// <summary>
        /// Gets paged and sorted records for a given CRM account and status.
        /// </summary>
        /// <param name="accountId">The CRM account id.</param>
        /// <param name="status">The desired status filter</param>
        /// <param name="pageIndex">The zero based page number</param>
        /// <param name="take">The max number of records to take (page size)</param>
        /// <param name="orderBy">The property name to order by and optional direction. (null for default order)</param>
        /// <returns></returns>
        List<Customer> GetByStatus(int accountId, CustomerStatus status, 
            int pageIndex, int take, string orderBy);
        /// <summary>
        /// Adds a new item, to be saved on next commit.
        /// </summary>
        /// <param name="item"></param>
        void Add(Customer item);
        /// <summary>
        /// Marks an item as updated, to be saved on next commit.
        /// </summary>
        /// <param name="item"></param>
        void Update(Customer item);
        /// <summary>
        /// Marks an item as deleted, to be hard-deleted on next commit.
        /// </summary>
        /// <param name="item"></param>
        void Delete(Customer item);
        /// <summary>
        /// Saves changes to new or modified customers.
        /// </summary>
        void Commit();
    }
}
