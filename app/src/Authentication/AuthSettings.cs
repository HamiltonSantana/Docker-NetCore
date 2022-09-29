using System.Net.NetworkInformation;

namespace ServerSide.Authentication
{
    public class AuthSettings
    {
        public const string SectionName = "AuthSettings";
        public string Secret { get; init; } = null!;
        public string Issuer { get; init; } = null!;
        public int Expire { get; init; }
    }
}
