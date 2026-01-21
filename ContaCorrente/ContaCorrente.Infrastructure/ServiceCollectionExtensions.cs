using ContaCorrente.Application.Features.Commands.Cadastrar.Validation;
using ContaCorrente.Application.Features.Commands.Inativar.Validation;
using ContaCorrente.Application.Features.Commands.Movimentar.Service;
using ContaCorrente.Application.Features.Commands.Movimentar.Validation;
using ContaCorrente.Domain.Repositories;
using ContaCorrente.Infrastructure.Messengers.Consumers;
using ContaCorrente.Infrastructure.Repositories;
using Core.Infrastructure.Extensions;
using Core.Messengers;
using KafkaFlow;
using KafkaFlow.Serializer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ContaCorrente.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration)
                .AddSecurity(configuration)
                .AddSwaggerConfiguration("Conta Corrente")
                .ConfigureApiBehavior()
                .AddRepositories()
                .AddIdempotencia()
                .AddContaCorrenteMessengers(configuration)
                .AddValidators()
                .AddMediatR(c => c.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.Load("ContaCorrente.Application")))
                .AddHostedService()
                .AddServices();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IContaCorrenteCommandRepository, ContaCorrenteCommandsRepository>();
        services.AddScoped<IContaCorrenteQueryRepository, ContaCorrenteQueriesRepository>();

        return services;
    }

    private static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddScoped<ICadastrarValidator, CadastrarValidator>();
        services.AddScoped<IInativarValidator, InativarValidator>();
        services.AddScoped<ICriarMovimentoValidator, CriarMovimentoValidator>();

        return services;
    }

    private static IServiceCollection AddContaCorrenteMessengers(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<TarifasRealizadasMessageHandler>();

        services.AddKafka(kafka => kafka
                .UseConsoleLog()
                .AddCluster(cluster => cluster
                    .WithBrokers([configuration["Kafka:Brokers"] ?? "localhost:9092"])
                    .CreateTopicIfNotExists(KafkaConsts.TarifasRealizadasTopico, 5, 1)
                    .AddConsumer(consumer => consumer
                        .Topic(KafkaConsts.TarifasRealizadasTopico)
                        .WithGroupId("tarifas-consumer-group")
                        .WithName("contacorrente-tarifas-consumer")
                        .WithBufferSize(1000)
                        .WithWorkersCount(5)
                        .AddMiddlewares(m => m
                            .AddDeserializer<JsonCoreDeserializer>()
                            .AddTypedHandlers(h => h.AddHandler<TarifasRealizadasMessageHandler>())
                        )
                    )
                )
            );

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICriarMovimentoService, CriarMovimentoService>();

        return services;
    }
}
