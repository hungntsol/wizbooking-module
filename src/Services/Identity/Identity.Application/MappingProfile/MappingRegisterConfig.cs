using Identity.Application.Features.Commands.UpdateProfile;
using Identity.Application.Features.Queries.Profile;

namespace Identity.Application.MappingProfile;

public class MappingRegisterConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UserAccount, GetProfileResultView>();
        config.NewConfig<UpdateProfileCommand, UserAccount>();
    }
}
