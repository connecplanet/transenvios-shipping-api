using AutoMapper;
using Transenvios.Shipping.Api.Adapters.RouteService;
using Transenvios.Shipping.Api.Domains.CatalogService;
using Transenvios.Shipping.Api.Domains.ClientService;
using Transenvios.Shipping.Api.Domains.UserService;

namespace Transenvios.Shipping.Api.Infraestructure
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // User -> AuthenticateResponse
            CreateMap<User, UserAuthenticateResponse>();
            CreateMap<Client, UserAuthenticateResponse>();
            CreateMap<Person, UserAuthenticateResponse>();

            // RegisterRequest -> User
            CreateMap<UserRegisterRequest, User>();
            CreateMap<UserRegisterRequest, Client>();
            CreateMap<UserRegisterRequest, Person>();

            // UpdateRequest -> User
            CreateMap<UserUpdateRequest, User>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    {
                    // ignore null & empty string properties
                    if (prop == null) return false;
                        if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                        return true;
                    }
                ));

            // RegisterRequest -> User
            CreateMap<ClientUpdateRequest, User>();

            // UpdateRequest -> Client
            CreateMap<ClientUpdateRequest, Client>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    {
                        // ignore null & empty string properties
                        if (prop == null) return false;
                        if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                        return true;
                    }
                ));

            CreateMap<RouteCreateUpdateRequest, ShipmentRoute>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    {
                        // ignore null & empty string properties
                        if (prop == null) return false;
                        if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                        return true;
                    }
                ));
        }
    }
}
