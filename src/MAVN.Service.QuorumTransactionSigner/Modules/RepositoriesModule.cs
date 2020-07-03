using Autofac;
using JetBrains.Annotations;
using MAVN.Service.QuorumTransactionSigner.Domain.Repositories;
using MAVN.Service.QuorumTransactionSigner.MsSqlRepositories;
using MAVN.Service.QuorumTransactionSigner.MsSqlRepositories.Contexts;
using MAVN.Service.QuorumTransactionSigner.Settings;
using Lykke.SettingsReader;
using MAVN.Persistence.PostgreSQL.Legacy;

namespace MAVN.Service.QuorumTransactionSigner.Modules
{
    [UsedImplicitly]
    public class RepositoriesModule : Module
    {
        private readonly DbSettings _dbSettings;

        public RepositoriesModule(
            IReloadingManager<AppSettings> appSettings)
        {
            _dbSettings = appSettings.CurrentValue.QuorumTransactionSignerService.Db;
        }

        protected override void Load(
            ContainerBuilder builder)
        {
            builder
                .RegisterPostgreSQL(_dbSettings.DataConnString,
                    connString => new QtsContext(connString, false), 
                    dbConn => new QtsContext(dbConn));

            builder
                .Register(ctx => new WalletRepository
                (
                    ctx.Resolve<PostgreSQLContextFactory<QtsContext>>()
                ))
                .As<IWalletRepository>()
                .SingleInstance();
        }
    }
}
