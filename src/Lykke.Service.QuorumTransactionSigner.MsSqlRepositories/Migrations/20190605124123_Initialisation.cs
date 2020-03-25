using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Migrations;
// ReSharper disable All

namespace Lykke.Service.QuorumTransactionSigner.MsSqlRepositories.Migrations
{
    [UsedImplicitly]
    public partial class Initialisation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "quorum_transaction_signer");

            migrationBuilder.CreateTable(
                name: "wallets",
                schema: "quorum_transaction_signer",
                columns: table => new
                {
                    address = table.Column<string>(nullable: false),
                    public_key = table.Column<byte[]>(nullable: false),
                    private_key_backup = table.Column<byte[]>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wallets", x => x.address);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "wallets",
                schema: "quorum_transaction_signer");
        }
    }
}
