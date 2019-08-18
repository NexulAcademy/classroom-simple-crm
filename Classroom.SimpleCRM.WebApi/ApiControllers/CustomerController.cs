using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System;
using Classroom.SimpleCRM.WebApi.Models.Customers;
using Classroom.SimpleCRM.WebApi.Filters;
using Classroom.SimpleCRM.WebApi.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Classroom.SimpleCRM.WebApi.ApiControllers
{
    [Route("api/customers")]
    [Authorize(Policy = "ApiUser")]
    public class CustomerController : Controller
    {
        private readonly ICustomerData _customerData;
        private readonly ILogger<AuthController> _logger;
        private readonly IUrlHelper _urlHelper;

        public CustomerController(ICustomerData customerData,
            ILogger<AuthController> logger,
            IUrlHelper urlHelper)
        {
            _customerData = customerData;
            _logger = logger;
            _urlHelper = urlHelper;
        }

        /// <summary>
        /// Gets all customers visible in the account of the current user
        /// </summary>
        /// <returns></returns>
        //[Route("")] //  ./api/customers
        [HttpGet("", Name = "GetCustomers")]
        public IActionResult GetCustomers([FromQuery]CustomerListParameters resourceParameters)
        {
            if (resourceParameters.Page < 1)
            {
                resourceParameters.Page = 1; //0 is default when not specified
            }
            if (resourceParameters.Take == 0)
            {
                resourceParameters.Take = 50; //default when value is not specified.
            }
            if (resourceParameters.Take > 250)
            {
                _logger.LogError("Get Customers max items exceeded.");
                return new ValidationFailedResult("A request can only take maximum of 250 items.");
            }

            var customers = _customerData.Get(0, resourceParameters);

            var pagination = new PaginationModel
            {
                Previous = CreateCustomersResourceUri(resourceParameters, -1),
                Next = CreateCustomersResourceUri(resourceParameters, 1)
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(pagination));
            Response.Headers.Add("ETag", "\"abc\"");

            var models = customers.Select(c => new CustomerDisplayViewModel(c));
            return Ok(models);
        }

        private string CreateCustomersResourceUri(CustomerListParameters resourceParameters, int pageAdjust)
        {
            if (resourceParameters.Page + pageAdjust < 1)
                return null;
            return _urlHelper.Link("GetCustomers", new
            {
                take = resourceParameters.Take,
                page = resourceParameters.Page + pageAdjust,
                orderBy = resourceParameters.OrderBy,
                lastName = resourceParameters.LastName,
                term = resourceParameters.Term
            });
        }
        /// <summary>
        /// Retrieves a single customer by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")] //  ./api/customers/:id
        [HttpGet]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        public IActionResult Get(int id)
        {
            var customer = _customerData.Get(id);
            if (customer == null)
            {
                _logger.LogWarning("Customer {0} not found", id);
                return NotFound();
            }
            Response.Headers.Add("ETag", "\"" + customer.LastContactDate.ToString() + "\"");
            var model = new CustomerDisplayViewModel(customer);
            return Ok(customer);
        }
        [HttpPost("")] //  ./api/customers
        public IActionResult Create([FromBody]CustomerCreateViewModel model)
        {
            var customer = new Customer
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailAddress = model.EmailAddress,
                PhoneNumber = model.PhoneNumber,
                PreferredContactMethod = model.PreferredContactMethod,
                LastContactDate = DateTime.UtcNow
            };

            _customerData.Add(customer);
            _customerData.Commit();

            Response.Headers.Add("ETag", "\"" + customer.LastContactDate.ToString() + "\"");
            return Ok(new CustomerDisplayViewModel(customer));
        }
        [HttpPut("{id}")] //  ./api/customers/:id
        public IActionResult Update(int id, [FromBody]CustomerUpdateViewModel model)
        {
            var customer = _customerData.Get(id);
            if (customer == null)
            {
                _logger.LogWarning("Customer {0} not found", id);
                return NotFound();
            }
            string ifMatch = Request.Headers["If-Match"];
            if (ifMatch != customer.LastContactDate.ToString())
            {
                _logger.LogInformation("Customer update failed due to concurrency issue: {0}", id);
                return StatusCode(422, "Customer has been changed by another user since it was loaded. Reload and try again.");
            }

            // TODO: ensure User owns this customer record, else return 403.

            //update only editable properties from model
            customer.EmailAddress = model.EmailAddress;
            customer.FirstName = model.FirstName;
            customer.LastName = model.LastName;
            customer.PhoneNumber = model.PhoneNumber;
            customer.PreferredContactMethod = model.PreferredContactMethod;
            customer.LastContactDate = DateTime.UtcNow; // acting as a last date updated

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
                _logger.LogWarning("Customer {0} not found", id);
                return NotFound();
            }

            // TODO: ensure User owns this customer record, else return 403.

            _logger.LogInformation("Deleting customer: {0}", id);
            _customerData.Delete(customer);
            _customerData.Commit();
            return NoContent();
        }
    }
}
