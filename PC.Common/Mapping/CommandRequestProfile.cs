namespace PC.Common.Mapping;

public class CommandRequestProfile : Profile
{
    public CommandRequestProfile()
    {
        CreateMap<Update, CommandRequest>()
            .ForMember(dest => dest.IsMechanicalCommand,
                opt => opt.MapFrom(src => src.Type == UpdateType.CallbackQuery))
            .ForMember(opt => opt.IsUnknownCommand,
                opt => opt.MapFrom(src => src.Type != UpdateType.Message && src.Type != UpdateType.CallbackQuery))
            .AfterMap<EnrichCommandRequest>();
    }
}