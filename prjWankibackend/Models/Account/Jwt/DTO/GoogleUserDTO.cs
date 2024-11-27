namespace prjWankibackend.Models.Account.Jwt.DTO
{
    public class GoogleUserDTO
    {
        public string iss { get; set; }
        public string azp { get; set; }
        public string aud { get; set; }
        public string sub { get; set; }
        public string email { get; set; }
        public bool emailVerified { get; set; }
        public long nbf { get; set; }
        public string name { get; set; }
        public string picture { get; set; }
        public string givenName { get; set; }
        public string familyName { get; set; }
        public long iat { get; set; }
        public long exp { get; set; }
        public string jti { get; set; }
    }
}
