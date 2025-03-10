using System.Text.Json.Serialization;
using Api.Domain.UseCases.Customers.Models;
using Api.Shared.Bases;
namespace Api.Domain.UseCases.Pdvs.Models
{
    [Table("pdv")]
    public class Pdv : AuditedBase
    {
        [Key]
        [Column("pvd_id")]
        public int PvdId { get; set; }

        [Column("customer_id")]
        public int CustomerId { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public Customer Customer { get; set; } = null!;

        [Column("days")]
        public int Days { get; set; }

        [Column("status")]
        public required bool Status { get; set; }
    }
}
