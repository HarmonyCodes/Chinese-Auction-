using Project.models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Roles
{
    User,
    Admin
}
public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string FullName { get; set; }
    public string Phone { get; set; }
    public string? Address { get; set; }
    public string? Email { get; set; }

    [NotMapped]
    public string Password { get; set; }
    public string PasswordHash { get; set; }= string.Empty;
    public Roles Role { get; set; } = Roles.User;
    public ICollection<GiftCart> GiftCarts { get; set; }
    public ICollection<Purchase> Purchases { get; set; }
}
