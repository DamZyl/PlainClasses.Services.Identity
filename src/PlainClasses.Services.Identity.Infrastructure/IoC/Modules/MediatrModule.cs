using System.Reflection;
using Autofac;
using MediatR;
using MediatR.Pipeline;
using PlainClasses.Services.Identity.Infrastructure.Utils;

namespace PlainClasses.Services.Identity.Infrastructure.IoC.Modules
{
     public class MediatrModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

            var mediatrOpenTypes = new[]
            {
                typeof(IRequestHandler<,>),
                typeof(INotificationHandler<>),
                //typeof(IValidator<>)
            };
            
            foreach (var mediatrOpenType in mediatrOpenTypes)
            {
                builder
                    .RegisterAssemblyTypes(Assembly.LoadFrom(Consts.ApplicationAssemblyPath))
                    .AsClosedTypesOf(mediatrOpenType)
                    .AsImplementedInterfaces();
            }
            
            builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            
            builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });
            
            //builder.RegisterGeneric(typeof(CommandValidationBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        }
    }
}