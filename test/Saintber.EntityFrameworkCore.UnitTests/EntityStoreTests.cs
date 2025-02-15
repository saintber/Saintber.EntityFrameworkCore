using Microsoft.Extensions.DependencyInjection;
using Saintber.EntityFrameworkCore.Abstractions;
using Saintber.EntityFrameworkCore.Test;

namespace Saintber.EntityFrameworkCore.UnitTests
{
    [TestClass]
    public class EntityStoreTests : EntityStoreTestsBase
    {
        [TestInitialize]
        public virtual void ClassInit()
        {
            var services = new ServiceCollection();
            services.AddScoped<IAlterUserProvider, AlterUserProviderFake>();
            services.AddScoped<IAlterUserProvider<string>>(provider => provider.GetRequiredService<IAlterUserProvider>());
            services.AddScoped<IEntityStore<TestEntityNoInfo>, EntityStoreFake<TestEntityNoInfo>>();
            services.AddScoped<IEntityStore<TestEntityHasInfo>, EntityStoreFake<TestEntityHasInfo>>();
            services.AddScoped<IEntityStore<TestEntityGuid>, EntityStoreFake<TestEntityGuid>>();

            Provider = services.BuildServiceProvider();
        }
    }
}
