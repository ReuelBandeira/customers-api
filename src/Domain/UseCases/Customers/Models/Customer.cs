using System.Text.Json.Serialization;
using Api.Shared.Bases;
namespace Api.Domain.UseCases.Customers.Models
{
    [Table("customer")]
    public class Customer : AuditedBase
    {
        [Key]
        [Column("customer_id")]
        public int CustomerId { get; set; }

        [Column("name")]
        public required string Name { get; set; }

        [Column("cnpj")]
        public required string Cnpj { get; set; }

        [Column("user")]
        public required string User { get; set; }

        [Column("password")]
        public required string Password { get; set; }

        [Column("key")]
        public required string Key { get; set; }

    }
}
