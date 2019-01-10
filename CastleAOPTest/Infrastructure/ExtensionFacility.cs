using Castle.Core;
using Castle.Core.Configuration;
using Castle.Core.Internal;
using Castle.MicroKernel;
using Castle.MicroKernel.ModelBuilder;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CastleAOPTest.Infrastructure
{
    public class ExtensionFacility : IFacility
    {
        public void Init(IKernel kernel, IConfiguration facilityConfig)
        {
            kernel.Resolver.AddSubResolver(new CollectionResolver(kernel));
            kernel.ComponentModelBuilder.AddContributor(new RequireLogContributor());
            kernel.Register(
                Component.For<LoggingInterceptor>().LifestyleTransient()
            );
        }

        public void Terminate()
        {

        }
    }

    public class RequireLogContributor : IContributeComponentModelConstruction
    {
        public void ProcessModel(IKernel kernel, ComponentModel model)
        {
            var cachedMethods = model.Implementation.GetMethods().Where(m => AttributesUtil.GetAttribute<LoggingAttribute>(m) != null).ToList();

            if (cachedMethods.Any())
            {
                model.Interceptors.AddIfNotInCollection(InterceptorReference.ForType<LoggingInterceptor>());
            }
        }
    }
}
