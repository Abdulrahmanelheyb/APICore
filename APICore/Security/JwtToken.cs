using System;
using System.Collections.Generic;
using JWT;
using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;
using static APICore.Configurations;

namespace APICore.Security
{
    /// <summary>
    ///     JSON Web Token
    /// </summary>
    public static class JwtToken
    {
        private static readonly dynamic Configuration = GetConfigurations()["JsonWebToken"];

        private static IJwtAlgorithm GetAlgorithm()
        {
            string alg = Configuration["Algorithm"];
            return alg switch
            {
                "HMACSHA256" => new HMACSHA256Algorithm(),
                "HMACSHA384" => new HMACSHA384Algorithm(),
                "HMACSHA512" => new HMACSHA512Algorithm(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        /// <summary>
        ///     Sign/Generate JWT Token
        /// </summary>
        /// <param name="model">Model for signature</param>
        /// <returns>string token</returns>
        public static string Sign(object model)
        {
            double hours = Configuration["ExpiresAfter"]["Hours"];
            double minutes = Configuration["ExpiresAfter"]["Minutes"];
            double seconds = Configuration["ExpiresAfter"]["Seconds"];

            var expirationDate = DateTime.Now.AddHours(hours).AddMinutes(minutes).AddSeconds(seconds);
            var payload = new Dictionary<string, object>
            {
                { "data", model },
                {"exp", ((DateTimeOffset)expirationDate).ToUnixTimeSeconds()}
            };
            string secret = Configuration["SecretKey"];
            
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(GetAlgorithm(), serializer, urlEncoder);

            return encoder.Encode(payload, secret);
        }

        /// <summary>
        ///     Decode/Verify JWT token with defined generic type for payload
        /// </summary>
        /// 
        /// <param name="token">Token for decrypt or verify</param>
        /// <returns>Verify status and decrypted token object</returns>
        public static (bool, T) Verify<T>(string token)
        {
            string secret = Configuration["SecretKey"];

            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, GetAlgorithm());    
                // Deserialize payload object
                var decodePayload =  serializer.Deserialize<dynamic>(decoder.Decode(token, secret, true));
                // Serialize payload user data
                var serializedPayloadObject = serializer.Serialize(decodePayload["data"]);
                // Deserialize payload user data
                var rlt = serializer.Deserialize<T>(serializedPayloadObject);
                return (true, rlt);
            }
            catch (TokenExpiredException)
            {
                Console.WriteLine("Token has expired");
                return (false, default);
            }
            catch (SignatureVerificationException)
            {
                Console.WriteLine("Token has invalid signature");
                return (false, default);
            }
            
        }
    }
}