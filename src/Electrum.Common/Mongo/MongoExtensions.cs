using Autofac;
using Electrum.Common.Domain;
using MongoDB.Driver;

namespace Electrum.Common.Mongo
{
    public static class MongoExtensions
    {
        public static void AddMongoRepository<TEntity>(this ContainerBuilder builder, string collectionName) where TEntity : IIdentifiable =>
            builder.Register(ctx => new MongoRepository<TEntity>(ctx.Resolve<IMongoDatabase>(), collectionName))
                .As<IMongoRepository<TEntity>>().InstancePerLifetimeScope();

    }
}
