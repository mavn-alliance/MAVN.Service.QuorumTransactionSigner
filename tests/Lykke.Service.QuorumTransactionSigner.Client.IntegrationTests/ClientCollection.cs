using Xunit;

namespace Lykke.Service.QuorumTransactionSigner.Client.IntegrationTests
{
    [CollectionDefinition(Name)]
    public class ClientCollection  : ICollectionFixture<ClientFixture>
    {
        public const string Name = "Client collection";
    }
}
