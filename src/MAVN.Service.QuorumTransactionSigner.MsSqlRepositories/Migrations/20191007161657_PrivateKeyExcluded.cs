using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MAVN.Service.QuorumTransactionSigner.MsSqlRepositories.Migrations
{
    public partial class PrivateKeyExcluded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "private_key_backup",
                schema: "quorum_transaction_signer",
                table: "wallets");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "private_key_backup",
                schema: "quorum_transaction_signer",
                table: "wallets",
                nullable: false,
                defaultValue: new byte[] {  });
        }
    }
}
