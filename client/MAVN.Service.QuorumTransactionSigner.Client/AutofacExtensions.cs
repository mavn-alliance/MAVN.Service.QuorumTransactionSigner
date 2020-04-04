using Autofac;
using JetBrains.Annotations;
using Lykke.HttpClientGenerator;
using Lykke.HttpClientGenerator.Infrastructure;
using System;

namespace MAVN.Service.QuorumTransactionSigner.Client
{
    /// <summary>
    ///    Extensions for client registration
    /// </summary>
    [PublicAPI]
    public static class AutofacExtensions
    {
        /// <summary>
        ///    Registers <see cref="IQuorumTransactionSignerClient"/> in Autofac container using <see cref="QuorumTransactionSignerServiceClientSettings"/>.
        /// </summary>
        /// <param name="builder">
        ///    Autofac container builder.
        /// </param>
        /// <param name="settings">
        ///    QuorumTransactionSigner client settings.
        /// </param>
        /// <param name="builderConfigure">
        ///    Optional <see cref="HttpClientGeneratorBuilder"/> configure handler.
        /// </param>
        public static void RegisterQuorumTransactionSignerClient(
            [NotNull] this ContainerBuilder builder,
            [NotNull] QuorumTransactionSignerServiceClientSettings settings,
            [CanBeNull] Func<HttpClientGeneratorBuilder, HttpClientGeneratorBuilder> builderConfigure)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (string.IsNullOrWhiteSpace(settings.ServiceUrl))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(QuorumTransactionSignerServiceClientSettings.ServiceUrl));
            }

            var clientBuilder = HttpClientGenerator.HttpClientGenerator
                .BuildForUrl(settings.ServiceUrl)
                .WithAdditionalCallsWrapper(new ExceptionHandlerCallsWrapper());

            clientBuilder = builderConfigure?.Invoke(clientBuilder) ?? clientBuilder.WithoutRetries();

            builder
                .RegisterInstance(new QuorumTransactionSignerClient(clientBuilder.Create()))
                .As<IQuorumTransactionSignerClient>()
                .SingleInstance();
        }
    }
}
