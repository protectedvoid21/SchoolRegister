using Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Tests.Utils;

public class FakeSignInManager : SignInManager<AppUser> {
    public FakeSignInManager()
        : base(new Mock<FakeUserManager>().Object, new HttpContextAccessor(),
            new Mock<IUserClaimsPrincipalFactory<AppUser>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<ILogger<SignInManager<AppUser>>>().Object,
            new Mock<IAuthenticationSchemeProvider>().Object) {
    }
}