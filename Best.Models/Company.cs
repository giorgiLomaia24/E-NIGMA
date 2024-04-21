using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Best.Models;

public class Company
{

    public int Id { get; set; }

    [Required]
    public   string Name { get; set; }

    [DisplayName("Street Address")]
    public  string?  StreetAddress { get; set; }
    public   string? City { get; set; }

      [DisplayName("Postal Code")]
    public   string? PostalCode { get; set; }
    public   string? State { get; set; }

     [DisplayName("Phone Number")]
    public string? PhoneNumber { get; set; }


}
