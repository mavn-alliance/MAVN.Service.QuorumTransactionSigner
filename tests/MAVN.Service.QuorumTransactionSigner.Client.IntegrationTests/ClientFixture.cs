using JetBrains.Annotations;
using Lykke.HttpClientGenerator.Infrastructure;

using ClientGenerator = Lykke.HttpClientGenerator.HttpClientGenerator;

namespace MAVN.Service.QuorumTransactionSigner.Client.IntegrationTests
{
    [UsedImplicitly]
    public class ClientFixture
    {
        public ClientFixture()
        {
            var clientGenerator = ClientGenerator
                .BuildForUrl("http://localhost:5000")
                .WithAdditionalCallsWrapper(new ExceptionHandlerCallsWrapper())
                .Create();
            
            Client = new QuorumTransactionSignerClient(clientGenerator);
        }

        public IQuorumTransactionSignerClient Client { get; }
    }
}
