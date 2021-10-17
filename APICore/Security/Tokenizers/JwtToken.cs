using System;
using System.Collections.Generic;
using JWT;
using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;
using static APICore.Configurations;

namespace APICore.Security.Tokenizers
{
    public static class JwtToken
    {
        public static string Sign(object model)
        {
            var config = GetConfigurations()["JsonWebToken"];
            double hours = config["ExpiresAfter"]["Hours"];
            double minutes = config["ExpiresAfter"]["Minutes"];
            double seconds = config["ExpiresAfter"]["Seconds"];
            var expirationDate = DateTime.Now.AddHours(hours).AddMinutes(minutes).AddSeconds(seconds);
            var payload = new Dictionary<string, object>
            {
                { "user", model },
                {"exp", ((DateTimeOffset)expirationDate).ToUnixTimeSeconds()}
            };
            string secret = config["SecretKey"];

            IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // symmetric
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            return encoder.Encode(payload, secret);
        }

        public static (bool, string) Verify(string token)
        {
            var config = GetConfigurations()["JsonWebToken"];
            string secret = config["SecretKey"];

            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // symmetric
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);
    
                var decodedToken = decoder.Decode(token, secret, true);
                return (true, decodedToken);
            }
            catch (TokenExpiredException)
            {
                Console.WriteLine("Token has expired");
                return (false, null);
            }
            catch (SignatureVerificationException)
            {
                Console.WriteLine("Token has invalid signature");
                return (false, null);
            }
            
        }
    }
}