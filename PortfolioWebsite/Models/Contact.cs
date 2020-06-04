using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace PortfolioWebsite.Models
{
    public class Contact
    {

        public string ID = Guid.NewGuid().ToString();

        [Required]
        public string Name { get; set; }

        [Required, Display(Name = "Email Address"), DataType(DataType.EmailAddress), RegularExpression(@"^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber), RegularExpression(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$", ErrorMessage = "Please enter a valid phone number")]
        public string PhoneNumber { get; set; }

        [Required]
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

        public string ToHTML()
        {
            string html = @$"
<h2>Name</h2><span>{Name}</span>
<h2>Email</h2><span>{Email}</span>
<h2>Phone Number</h2><span>{PhoneNumber}</span>
<h2>Message: </h2><p>{Message}</p>
";
            


            return html;
        }
    }
}
