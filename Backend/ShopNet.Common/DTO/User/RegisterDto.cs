using System.ComponentModel.DataAnnotations;

namespace ShopNet.Common.DTO.User
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [EmailAddress]
        //[RegularExpression("^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+$",ErrorMessage ="Invalid email")]
        public string Email { get; set; }
        [Required]
        [RegularExpression("^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*\\W).+$"
            ,ErrorMessage ="Password must have: one upper, one lower, one number, one special character and at least 6 characters")]
        public string Password { get; set; }
    }
}