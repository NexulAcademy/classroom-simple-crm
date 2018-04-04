using System;
using Microsoft.AspNetCore.Mvc;

namespace Classroom.SimpleCRM.WebApi.ApiControllers
{
    [Route("api/customers")]
    public class CustomerController : Controller
    {
        private readonly ICustomerData _customerData;

        public CustomerController(ICustomerData customerData)
        {
            _customerData = customerData;
        }

        /// <summary>
        /// Gets all customers visible in the account of the current user
        /// </summary>
        /// <returns></returns>
        [Route("")] //  ./api/customers
        [HttpGet]
        public IActionResult GetAll()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Retrieves a single customer by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")] //  ./api/customers/:id
        [HttpGet]
        public IActionResult Get(int id)
        {
            throw new NotImplementedException();
        }
        [HttpPost("")] //  ./api/customers
        public IActionResult Create([FromBody]Customer model)
        {
            throw new NotImplementedException();
        }
        [HttpPut("{id}")] //  ./api/customers/:id
        public IActionResult Update(int id, [FromBody]Customer model)
        {
            throw new NotImplementedException();
        }
        [HttpDelete("{id}")] //  ./api/customers/:id
        public IActionResult Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
