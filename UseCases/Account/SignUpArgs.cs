using System.Globalization;

namespace UseCases
{
    public class SignUpArgs
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public CultureInfo CultureInfo { get; set; }
    }
}