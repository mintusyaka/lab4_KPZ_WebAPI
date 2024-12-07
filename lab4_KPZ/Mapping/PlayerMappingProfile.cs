using AutoMapper;
using lab4_KPZ.Models;
using lab4_KPZ.ViewModels;

namespace lab4_KPZ.Mapping
{
	public class PlayerMappingProfile : Profile
	{
        public PlayerMappingProfile()
        {
            CreateMap<Player, PlayerViewModel>();

			CreateMap<PlayerViewModel, Player>();

			CreateMap<Player, PlayerUpdateViewModel>();
			CreateMap<PlayerUpdateViewModel, Player>()
				.ForMember(dest => dest.PlayerId, opt => opt.Ignore())
				.ForMember(dest => dest.Password, opt => opt.Ignore())
				.ForMember(dest => dest.RegistrationDate, opt => opt.Ignore())
				.ForMember(dest => dest.RegistrationTime, opt => opt.Ignore());


			CreateMap<Player, PlayerCreateViewModel>();
			CreateMap<PlayerCreateViewModel, Player>();
		}
	}
}
