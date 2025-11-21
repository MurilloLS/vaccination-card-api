using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace VaccinationCard.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Registra o AutoMapper procurando perfis neste projeto
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // Registra o MediatR e todos os Handlers neste projeto
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        // Registra todos os Validadores (FluentValidation)
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}