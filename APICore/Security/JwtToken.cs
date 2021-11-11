using System;
using System.Collections.Generic;
using JWT;
using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;
using Newtonsoft.Json;
using static APICore.Configurations;

namespace APICore.Security
{
    /// <summary>
    ///     JSON Web Token 
    /// </summary>
    public static class JwtToken
    {
        
        /// <summary>
        ///     Sign/Generate JWT Token
        /// </summary>
        /// <param name="model">Model for signature</param>
        /// <returns>string token</returns>
        public static string Sign<T>(T model)
        {
            var config = GetConfigurations()["JsonWebToken"];
            double hours = config["ExpiresAfter"]["Hours"];
            double minutes = config["ExpiresAfter"]["Minutes"];
            double seconds = config["ExpiresAfter"]["Seconds"];
            var expirationDate = DateTime.Now.AddHours(hours).AddMinutes(minutes).AddSeconds(seconds);
            var payload = new Dictionary<string, object>
            {
                { "data", model },
                {"exp", ((DateTimeOffset)expirationDate).ToUnixTimeSeconds()}
            };
            string secret = config["SecretKey"];

            IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // symmetric
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            return encoder.Encode(payload, secret);
        }

        /// <summary>
        ///     Decode/Verify JWT token
        /// </summary>
        /// <param name="token">Token for decrypt or verify</param>
        /// <returns>Verify status and decrypted token object</returns>
        public static (bool, object) Verify(string token)
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
                var rlt = JsonConvert.DeserializeObject(decoder.Decode(token, secret, true));
                return (true, rlt);
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
        
        /// <summary>
        ///     Decode/Verify JWT token with defined generic type for payload
        /// </summary>
        /// 
        /// <param name="token">Token for decrypt or verify</param>
        /// <returns>Verify status and decrypted token object</returns>
        public static (bool, T) Verify<T>(string token)
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