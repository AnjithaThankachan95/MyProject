using CustomerManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagement.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        /// <summary>
        /// Gets all customers.
        /// </summary>
        /// <returns>A list of customers.</returns>
        Task<List<Customer>> GetAllCustomersAsync();

        /// <summary>
        /// Gets a customer by ID.
        /// </summary>
        /// <param name="id">The customer's unique identifier.</param>
        /// <returns>The customer object if found, otherwise null.</returns>
        Task<Customer?> GetCustomerByIdAsync(int id);

        /// <summary>
        /// Adds a new customer.
        /// </summary>
        /// <param name="customer">The customer entity to add.</param>
        Task AddCustomerAsync(Customer customer);

        /// <summary>
        /// Updates an existing customer.
        /// </summary>
        /// <param name="customer">The updated customer entity.</param>
        Task UpdateCustomerAsync(Customer customer);

        /// <summary>
        /// Deletes a customer by ID.
        /// </summary>
        /// <param name="id">The customer's unique identifier.</param>
        Task DeleteCustomerAsync(int id);
    }
}
