using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System;
using Classroom.SimpleCRM.WebApi.Models.Customers;
using Classroom.SimpleCRM.WebApi.Filters;
using Classroom.SimpleCRM.WebApi.Models;
using Newtonsoft.Json;

namespace Classroom.SimpleCRM.WebApi.ApiControllers
{
    [Route("api/customers")]
    public class CustomerController : Controller
    {
        private readonly ICustomerData _customerData;
        private readonly IUrlHelper _urlHelper;

        public CustomerController(ICustomerData customerData,
            IUrlHelper urlHelper)
        {
            _customerData = customerData;
            _urlHelper = urlHelper;
        }

        /// <summary>
        /// Gets all customers visible in the account of the current user
        /// </summary>
        /// <returns></returns>
        [Route("")] //  ./api/customers
        [HttpGet(Name = "GetCustomers")]
        public IActionResult GetAll([FromQuery]int page = 1, [FromQuery]int take = 50)
        {
            page = Math.Max(1, page); //correct bad value automatically.
            //or
            if (take > 250)
            {   //tell the consumer the requested query cannot be fulfilled.
                return new ValidationFailedResult("A request can only take maximum of 250 items.");
            }
            // which is better? You decide. Discuss. Does it matter if the input is optional?

            var customers = _customerData.GetAll(0, page - 1, take, "");

            var pagination = new PaginationModel
            {
                Previous = page == 1 ? null : CreateCustomersResourceUri(page-1, take),
                Next = CreateCustomersResourceUri(page+1, take)
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(pagination));

            var models = customers.Select(c => new CustomerDisplayViewModel(c));
            return Ok(models);
        }

        private string CreateCustomersResourceUri(int page, int take)
        {
            return _urlHelper.Link("GetCustomers", new
            {
                take = take,
                page = page
            });
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
        public IActionResult Create([FromBody]CustomerCreateViewModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return new ValidationFailedResult(ModelState);
            }

            var customer = new Customer
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailAddress = model.EmailAddress,
                PhoneNumber = model.PhoneNumber,
                PreferredContactMethod = model.PreferredContactMethod
            };

            _customerData.Add(customer);
            _customerData.Commit();
            return Ok(new CustomerDisplayViewModel(customer));
        }
        [HttpPut("{id}")] //  ./api/customers/:id
        public IActionResult Update(int id, [FromBody]CustomerUpdateViewModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return new ValidationFailedResult(ModelState);
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
