namespace WebAPI.Models;

public class Customer
{
    public int Id { get; set; }
    public string CustomerRef { get; set; }
    public string? CustomerName { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? Town { get; set; }
    public string? County { get; set; }
    public string? Country { get; set; }
    public string? Postcode { get; set; }
}