using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace Lykke.Service.QuorumTransactionSigner.MsSqlRepositories.Entities
{
    [Table("wallets")]
    public class WalletEntity
    {
        [Key, Required]
        [Column("address")]
        public string Address { get; set; }
        
        [Required]
        [Column("public_key")]
        public byte[] PublicKey { get; set; }
    }
}
