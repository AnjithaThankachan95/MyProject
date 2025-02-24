using CustomerManagement.Business.Interfaces;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerManagement.Business.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(ICustomerRepository customerRepository, ILogger<CustomerService> logger)
        {
            _customerRepository = customerRepository;
            _logger = logger;
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all customers...");
                return await _customerRepository.GetAllCustomersAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching customers.");
                throw new ApplicationException("An error occurred while retrieving customers.", ex);
            }
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching customer with ID: {id}");
                return await _customerRepository.GetCustomerByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching customer with ID {id}.");
                throw new ApplicationException($"An error occurred while retrieving the customer with ID {id}.", ex);
            }
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            try
            {
                if (customer == null)
                {
                    _logger.LogWarning("Attempted to add a null customer.");
                    throw new ArgumentNullException(nameof(customer), "Customer data cannot be null.");
                }

                await _customerRepository.AddCustomerAsync(customer);
                _logger.LogInformation($"Customer added successfully with ID: {customer.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding customer.");
                throw;
            }
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            try
            {
                if (customer == null)
                {
                    _logger.LogWarning("Attempted to update a null customer.");
                    throw new ArgumentNullException(nameof(customer), "Customer data cannot be null.");
                }
                _logger.LogInformation($"Updating customer with ID: {customer.Id}");
                await _customerRepository.UpdateCustomerAsync(customer);
                _logger.LogInformation($"Customer updated successfully with ID: {customer.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating customer with ID {customer.Id}.");
                throw;
            }
        }

        public async Task DeleteCustomerAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting customer with ID: {id}");
                await _customerRepository.DeleteCustomerAsync(id);
                _logger.LogInformation($"Customer deleted successfully with ID: {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting customer with ID {id}.");
                throw;
            }
        }
    }
}
