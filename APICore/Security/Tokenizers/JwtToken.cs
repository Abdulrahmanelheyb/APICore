using System;
using System.Collections.Generic;
using JWT;
using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;

namespace APICore.Security.Tokenizers
{
    public static class JwtToken
    {
        public static string Sign(object model)
        {
            
            var payload = new Dictionary<string, object>
            {
                { "user", model },
                {"exp", ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds()}
            };
            const string secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";

            IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // symmetric
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            return encoder.Encode(payload, secret);
        }

        public static (bool, string) Verify(string token)
        {
            const string secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";

            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // symmetric
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);
    
                var decodedToken = decoder.Decode(token, secret, verify: true);
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