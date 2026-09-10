using Microsoft.IdentityModel.Tokens;

namespace SL.Domain.Configurations
{
    public class UploadPathConfiguration
    {
        public string? ImageUrl { get; set; }
        public string FileUrl { get; set; } = null!;
        public int ChunkSize { get; set; }
    }

    public class JwtIssuerOptions
    {
        /// <summary>
        /// 4.1.1.  "iss" (Issuer) Claim - The "iss" (issuer) claim identifies the principal that issued the JWT.
        /// </summary>
        public string? Issuer { get; set; }
        /// <summary>
        /// 4.1.2.  "sub" (Subject) Claim - The "sub" (subject) claim identifies the principal that is the subject of the JWT.
        /// </summary>
        public string? Subject { get; set; }
        /// <summary>
        /// 4.1.3.  "aud" (UrlWebApi) Claim - The "aud" (UrlWebApi) claim identifies the recipients that the JWT is intended for.
        /// </summary>
        public string UrlWebApi { get; set; } = null!;
        /// <summary>
        /// Url Client Admin
        /// </summary>
        public string? UrlWebAdmin { get; set; }
        /// <summary>
        /// Url Client EndUser
        /// </summary>
        public string? UrlEndUser { get; set; }
        /// <summary>
        /// 4.1.4.  "exp" (Expiration Time) Claim
        /// </summary>
        public DateTime Expiration => IssuedAt.Add(ValidFor);
        /// <summary>
        /// 4.1.5.  "nbf" (Not Before) Claim
        /// </summary>
        public static DateTime NotBefore => DateTime.Now;
        /// <summary>
        /// 4.1.6.  "iat" (Issued At) Claim
        /// </summary>
        public static DateTime IssuedAt => DateTime.Now;
        /// <summary>
        /// Set the timespan the token will be valid for (default is 360 min)
        /// </summary>
        public TimeSpan ValidFor { get; set; } = TimeSpan.FromMinutes(360);
        /// <summary>
        /// "jti" (JWT ID) Claim (default ID is a GUID)
        /// </summary>
        public static Func<Task<string>> JtiGenerator => () => Task.FromResult(Guid.NewGuid().ToString());
        /// <summary>
        /// The signing key to use when generating tokens.
        /// </summary>
        public SigningCredentials? SigningCredentials { get; set; }
    }

    public class MemmoryObjectContainer
    {
        public Dictionary<string, string> Values { get; set; } = [];
    }

    public class AppMemmoryCacheOptions
    {
        public int OTPConfirmEmailExpiryMinutes { get; set; }
    }
}
