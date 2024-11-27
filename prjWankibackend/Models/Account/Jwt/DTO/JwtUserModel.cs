using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
using prjWankibackend.Models.Account.Authenticate.DTO;
using prjWankibackend.Models.Database;

namespace prjWankibackend.Models.Account.Jwt.DTO
{
    public class JwtUserModel
    {

        public string UserId { get; set; }
        public string MemberId { get; set; }

        public string UserAccount { get; set; }

        public string UserType { get; set; }

        public static explicit operator JwtUserModel(TPersonMember user) => new()
        {
            UserId = user.FPersonSid.ToString(),
            MemberId = user.FMemberId,
            UserAccount = user.FAccount,
            UserType = "Person",
        };
    }


    public static class LoginDataEndpoints
    {
        public static void MapLoginDataEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/LoginData").WithTags(nameof(LoginData));

            group.MapGet("/", () =>
            {
                return new[] { new LoginData() };
            })
            .WithName("GetAllLoginData")
            .WithOpenApi();

            group.MapGet("/{id}", (int id) =>
            {
                //return new LoginData { ID = id };
            })
            .WithName("GetLoginDataById")
            .WithOpenApi();

            group.MapPut("/{id}", (int id, LoginData input) =>
            {
                return TypedResults.NoContent();
            })
            .WithName("UpdateLoginData")
            .WithOpenApi();

            group.MapPost("/", (LoginData model) =>
            {
                //return TypedResults.Created($"/api/LoginData/{model.ID}", model);
            })
            .WithName("CreateLoginData")
            .WithOpenApi();

            group.MapDelete("/{id}", (int id) =>
            {
                //return TypedResults.Ok(new LoginData { ID = id });
            })
            .WithName("DeleteLoginData")
            .WithOpenApi();
        }
    }
}
