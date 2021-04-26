using System;
using System.Linq;
using System.Security.Cryptography;
using PlainClasses.Services.Identity.Application.Rules;
using PlainClasses.Services.Identity.Application.Utils;

namespace PlainClasses.Services.Identity.Application
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            using var algorithm = new Rfc2898DeriveBytes(password, Consts.SaltSize, Consts.Iterations, HashAlgorithmName.SHA256);
            var key = Convert.ToBase64String(algorithm.GetBytes(Consts.KeySize));
            var salt = Convert.ToBase64String(algorithm.Salt);

            return $"{ Consts.Iterations }.{ salt }.{ key }";
        }

        public bool Check(string hash, string password)
        {
            var parts = hash.Split('.', 3);

           ExceptionHelper.CheckRule(new SplitPasswordToPartLengthRule(parts));

            var iterations = Convert.ToInt32(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);
            var key = Convert.FromBase64String(parts[2]);

            using var algorithm = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
            var keyToCheck = algorithm.GetBytes(Consts.KeySize);
            var verified = keyToCheck.SequenceEqual(key);

            return verified;
        }
    }
}