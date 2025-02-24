using System.ComponentModel.DataAnnotations;

namespace CustomerManagement.API.Models
{
    /// <summary>
    /// Data Transfer Object for Customer.
    /// This object is used for transferring customer data between the API and the client.
    /// </summary>
    public class CustomerDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Customer name.
        /// </summary>
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters.")]
        public string Name { get; set; }

        /// <summary>
        /// Customer email address.
        /// </summary>
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        /// <summary>
        /// Customer phone number (10 digits).
        /// </summary>
        [Required(ErrorMessage = "Phone is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits.")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Customer address.
        /// </summary>
        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }
    }
}
