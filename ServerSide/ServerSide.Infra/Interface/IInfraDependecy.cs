using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ServerSide.Infra.Interface
{
    public interface IInfraDependecy
    {
        IServiceCollection ServiceInjection(IServiceCollection service);
    }
}