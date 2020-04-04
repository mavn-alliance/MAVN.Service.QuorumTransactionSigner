using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace MAVN.Service.QuorumTransactionSigner.Client.IntegrationTests
{
    [Collection(ClientCollection.Name)]
    [SuppressMessage("ReSharper", "xUnit1004")]
    public class WalletsApiTests : TestsBase
    {
        private readonly IQuorumTransactionSignerClient _client;
        
        public WalletsApiTests(
            ClientFixture fixture)
        {
            _client = fixture.Client;
        }
        
        [Fact(Skip = BecauseIntegrationTest)]
        public async Task CreateAsync__Should_Return_New_Wallet_Address()
        {
            (await _client.CreateWalletAsync())
                .Should()
                .MatchRegex(@"^0x[a-f0-9]{40}$");
        }

        [Fact(Skip = BecauseIntegrationTest)]
        public async Task SignTransactionAsync__Should_Return_V_R_and_S()
        {
            var address = await _client.CreateWalletAsync();
            var rawTxHash = new byte[32];

            var (v, r, s) = await _client.SignTransactionAsync(address, rawTxHash);

            v.Length
                .Should()
                .Be(1);

            r.Length
                .Should()
                .Be(32);

            s.Length
                .Should()
                .Be(32);
        }
    }
}
