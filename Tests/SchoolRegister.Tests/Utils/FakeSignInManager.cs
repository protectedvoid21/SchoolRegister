using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SchoolRegister.Models;

namespace SchoolRegister.Tests.Utils;

public class FakeSignInManager : SignInManager<AppUser> {
    public FakeSignInManager()
        : base(new Mock<FakeUserManager>().Object, new HttpContextAccessor(), 
            new Mock<IUserClaimsPrincipalFactory<AppUser>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<ILogger<SignInManager<AppUser>>>().Object,
            new Mock<IAuthenticationSchemeProvider>().Object,
            new Mock<IUserConfirmation<AppUser>>().Object) {
    }
}