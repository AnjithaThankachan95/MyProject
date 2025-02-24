using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagement.Domain.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private static List<Customer> _customers = new()
        {
            new Customer { Id = 1, Name = "John Paul", Email = "John@gmail.com", PhoneNumber = "9876543210", Address = "119 Street" },
            new Customer { Id = 2, Name = "Jaison Stephen", Email = "jaison@gmail.com", PhoneNumber = "8765432109", Address = "119 Avenue" },
            new Customer { Id = 2, Name = "Stacy Matt", Email = "stacy@gmail.com", PhoneNumber = "8765432109", Address = "119 Avenue" }
        };
        private readonly ILogger<CustomerRepository> _logger;
        public CustomerRepository(ILogger<CustomerRepository> logger)
        {
            _logger = logger;
        }
        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all customers...");
                return await Task.FromResult(_customers);
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
                var customer = _customers.FirstOrDefault(c => c.Id == id);

                if (customer == null)
                {
                    _logger.LogWarning($"Customer with ID {id} not found.");
                }

                return await Task.FromResult(customer);
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
                if (_customers.Any(c => (c.Email == customer.Email) && (c.Name == customer.Name) && (c.PhoneNumber == customer.PhoneNumber) && (c.Address == customer.Address)))
                {
                    _logger.LogWarning($"Customer with same details  already exists.");
                    throw new InvalidOperationException($"Customer with same details  already exists.");
                }
                _customers.Add(customer);
                _logger.LogInformation($"Customer added successfully with ID: {customer.Id}");
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding customer.");
                throw ;
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
                var existing = _customers.FirstOrDefault(c => c.Id == customer.Id);
                if (existing == null)
                {
                    _logger.LogWarning($"Customer with ID {customer.Id} not found.");
                    throw new KeyNotFoundException($"Customer with ID {customer.Id} not found.");
                }
                existing.Name = customer.Name;
                existing.Email = customer.Email;
                existing.PhoneNumber = customer.PhoneNumber;
                existing.Address = customer.Address;

                _logger.LogInformation($"Customer updated successfully with ID: {customer.Id}");
                await Task.CompletedTask;
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
                var customer = _customers.FirstOrDefault(c => c.Id == id);

                if (customer == null)
                {
                    _logger.LogWarning($"Customer with ID {id} not found.");
                    throw new KeyNotFoundException($"Customer with ID {id} not found.");
                }

                _customers.Remove(customer);
                _logger.LogInformation($"Customer deleted successfully with ID: {id}");
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting customer with ID {id}.");
                throw ;
            }
        }
    }
}
