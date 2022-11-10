using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyRegistration.Interfaces;

public interface IDependencyInjection
{
    public IServiceCollection RegisterInfrastructureDependencies(IServiceCollection services);
}