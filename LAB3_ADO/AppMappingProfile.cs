using AutoMapper;
using LAB3_ADO.Models;
using Microsoft.Data.SqlClient;

namespace LAB3_ADO
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<SqlDataReader, Client>()
                .ForMember(client => client.Id, opt => opt.MapFrom(reader => (int)reader.GetValue(0)))
                .ForMember(client => client.FirstName, opt => opt.MapFrom(reader => (string)reader.GetValue(1)))
                .ForMember(client => client.LastName, opt => opt.MapFrom(reader => (string)reader.GetValue(2)))
                .ForMember(client => client.DayOfBirth, opt => opt.MapFrom(reader => (DateTime)reader.GetValue(3)))
                .ForMember(client => client.PhoneNumber, opt => opt.MapFrom(reader => (string)reader.GetValue(4)))
                .ForMember(client => client.Email, opt => opt.MapFrom(reader => (string)reader.GetValue(5)))
                .ForMember(client => client.Status, opt => opt.MapFrom(reader => Enum.Parse<ClientStatus>((string)reader.GetValue(6))));
        }
    }

}
