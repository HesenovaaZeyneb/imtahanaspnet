using System.ComponentModel.DataAnnotations;

namespace Logis.AspNet.ViewModel.Account
{
    public class LoginVm
    {
        public string UsernameorEmail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
