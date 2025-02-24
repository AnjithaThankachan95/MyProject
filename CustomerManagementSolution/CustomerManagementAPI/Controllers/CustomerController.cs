using AutoMapper;
using CustomerManagement.API.Models;
using CustomerManagement.Business.Interfaces;
using CustomerManagement.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerController> _logger;
        public CustomerController(ICustomerService customerService, IMapper mapper, ILogger<CustomerController> logger)
        {
            _customerService = customerService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Gets all customers.
        /// </summary>
        /// <returns>A list of all customers.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            try
            {
                _logger.LogInformation("Fetching all customers...");
                var customers = await _customerService.GetAllCustomersAsync();
                // Map domain models to DTOs
                var customerDtos = _mapper.Map<List<CustomerDto>>(customers);
                return Ok(customerDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching customers.");
                return StatusCode(500, new { message = "An error occurred while fetching customers." });
            }
        }

        /// <summary>
        /// Gets a customer by ID.
        /// </summary>
        /// <param name="id">The customer ID.</param>
        /// <returns>The customer object if found, otherwise NotFound.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching customer with Id: {id}");
                var customer = await _customerService.GetCustomerByIdAsync(id);
                if (customer == null)
                {
                    _logger.LogWarning($"Customer with ID {id} not found.");
                    return NotFound(new { message = "Customer not found." });
                }
                // Map domain model to DTO before returning to client
                var customerDto = _mapper.Map<CustomerDto>(customer);
                return Ok(customerDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching customer with ID {id}.");
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Adds a new customer.
        /// </summary>
        /// <param name="customer">The customer object.</param>
        /// <returns>The newly created customer.</returns>
        [HttpPost]
        public async Task<IActionResult> AddCustomer([FromBody] CustomerDto customerDto)
        {
            // Validate the incoming model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                // Map the DTO to the domain model
                var customer = _mapper.Map<Customer>(customerDto);
                await _customerService.AddCustomerAsync(customer);
                // Map the added customer back to DTO
                var createdCustomerDto = _mapper.Map<CustomerDto>(customer);
                return CreatedAtAction(nameof(GetCustomerById), new { id = createdCustomerDto.Id }, createdCustomerDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding customer.");
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing customer.
        /// </summary>
        /// <param name="id">The customer ID.</param>
        /// <param name="customer">The updated customer object.</param>
        /// <returns>No content if successful.</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateCustomer([FromBody] CustomerDto customerDto)
        {
            // Validate the incoming model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                // Map the DTO to the domain model
                var customer = _mapper.Map<Customer>(customerDto);
                await _customerService.UpdateCustomerAsync(customer);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating customer.");
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Deletes a customer by ID.
        /// </summary>
        /// <param name="id">The customer ID.</param>
        /// <returns>No content if successful.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting customer with ID: {id}");
                await _customerService.DeleteCustomerAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting customer with ID {id}.");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
