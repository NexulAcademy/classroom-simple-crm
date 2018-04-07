using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Classroom.SimpleCRM.WebApi.Models.Customer;
using System;
using System.Globalization;
using Classroom.SimpleCRM.WebApi.Models.Customers;

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
            var customers = _customerData.GetAll(0, 0, 50, "");

            var models = customers.Select(c => new CustomerDisplayViewModel(c));
            return Ok(models);
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
            var customer = _customerData.Get(id);
            if (customer == null)
            {
                return NotFound();
            }
            var model = new CustomerDisplayViewModel(customer);
            return Ok(customer);
        }
        [HttpPost("")] //  ./api/customers
        public IActionResult Create([FromBody]Customer model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            _customerData.Add(model);
            _customerData.Commit();
            return Ok(model); //includes new auto-assigned id
        }
        [HttpPut("{id}")] //  ./api/customers/:id
        public IActionResult Update(int id, [FromBody]Customer model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            var customer = _customerData.Get(id);
            if (customer == null)
            {
                return NotFound();
            }

            //update only editable properties from model
            customer.EmailAddress = model.EmailAddress;
            customer.FirstName = model.FirstName;
            customer.LastName = model.LastName;
            customer.PhoneNumber = model.PhoneNumber;
            customer.PreferredContactMethod = model.PreferredContactMethod;
            customer.Status = model.Status;
            //customer.LastContactDate = model.LastContactDate;

            _customerData.Update(customer);
            _customerData.Commit();
            return Ok(customer); //server version, updated per request
        }
        [HttpDelete("{id}")] //  ./api/customers/:id
        public IActionResult Delete(int id)
        {
            var customer = _customerData.Get(id);
            if (customer == null)
            {
                return NotFound();
            }

            _customerData.Delete(customer);
            _customerData.Commit();
            return NoContent();
        }
    }
}
