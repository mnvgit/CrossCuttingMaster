using CrossCuttingMaster.MediatRPipeline.Behaviors;
using CrossCuttingMaster.MediatRPipeline.Handlers.Commands;
using CrossCuttingMaster.MediatRPipeline.Handlers.Responses;
using CrossCuttingMaster.MediatRPipeline.Handlers.Settings;
using CrossCuttingMaster.Services.AuditService;
using CrossCuttingMaster.Settings;
using FluentValidation;
using MediatR;

namespace CrossCuttingMaster
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PerformanceBehaviorSettings>(configuration.GetSection("PerformanceBehavior"));
            services.Configure<CreateOrderOptions>(configuration.GetSection("CreateOrder"));
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IAuditLogger, AuditLogger>();
            services.AddHttpContextAccessor();
            services.AddMemoryCache();
            services.AddValidatorsFromAssembly(typeof(Program).Assembly);

            return services;
        }

        public static IServiceCollection AddMediatrPipeline(this IServiceCollection services)
        {
            services.AddTransient(typeof(IRequestHandler<CreateOrderCommand, ApiResponse<Guid>>), typeof(CreateOrderHandler));

            services.AddMediatR(config =>
            {
                // Register all MediatR handlers from the assembly
                config.RegisterServicesFromAssembly(typeof(Program).Assembly);

                config.AddOpenBehavior(typeof(UnhandledExceptionBehavior<,>));
                config.AddOpenBehavior(typeof(LoggingPipelineBehavior<,>));
                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
                config.AddOpenBehavior(typeof(PerformanceBehavior<,>));
                config.AddOpenBehavior(typeof(IdempotentCachingBehavior<,>));
                config.AddOpenBehavior(typeof(AuditLogBehavior<,>));
                config.AddOpenBehavior(typeof(TransactionBehavior<,>));
            });

            return services;
        }
    }
}
