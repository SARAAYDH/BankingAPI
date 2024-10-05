using AutoMapper;
using Banking.Data.Models;
using Banking.Service.Dtos;
namespace Banking.Service.MappingProfile;

public class DTOMappingProfile : Profile
{
    public DTOMappingProfile()
    {
        CreateMap<ClientDto, Client>().ReverseMap();
        CreateMap<CreateClientDto, Client>().ReverseMap();
        CreateMap<UpdateClientDto, Client>().ReverseMap();
        CreateMap<ClientQueryDto, SearchParameter>().ReverseMap();
        CreateMap<CreateClientDto, Client>()
            .ForMember(dest => dest.Accounts, opt => opt.MapFrom(src => src.Accounts));
        CreateMap<AccountDto, Account>();
    }
}
