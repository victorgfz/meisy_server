using BC = BCrypt.Net.BCrypt;
using Meisy.Domain.Security.Cryptography;

namespace Meisy.Infrastructure.Security.Cryptography
{
    public class Bcrypt : IPasswordEncrypter
    {
        public string Encrypt(string password) => BC.HashPassword(password);

        public bool Verify(string password, string passwordHashed) => BC.Verify(password, passwordHashed);
    }
}
