using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Text;

namespace MArkB.OAuthStarter.Auth
{
  public class OpenIdToken
  {
    // Reference: https://azure.microsoft.com/en-us/documentation/articles/active-directory-v2-tokens/
    [JsonProperty("aud")]
    public string Audience;
    [JsonProperty("iss")]
    public string Issuer;
    [JsonProperty("iat")]
    public string IssuedAt;
    [JsonProperty("exp")]
    public string ExpirationTime;
    [JsonProperty("nbf")]
    public string NotBefore;
    [JsonProperty("ver")]
    public string Version;
    [JsonProperty("tid")]
    public string TenantId;
    [JsonProperty("c_hash")]
    public string CodeHash;
    [JsonProperty("at_hash")]
    public string AccessTokenHash;
    [JsonProperty("nonce")]
    public string Nonce;
    [JsonProperty("name")]
    public string Name;
    [JsonProperty("email")]
    public string Email;
    [JsonProperty("preferred_username")]
    public string PreferredUsername;
    [JsonProperty("sub")]
    public string Subject;
    [JsonProperty("oid")]
    public string ObjectId;

    public static OpenIdToken ParseOpenIdToken(string idToken)
    {
      string encodedOpenIdToken = idToken;

      string decodedToken = Base64UrlDecodeJwtTokenPayload(encodedOpenIdToken);

      OpenIdToken token = JsonConvert.DeserializeObject<OpenIdToken>(decodedToken);

      return token;
    }

    public bool Validate(string nonce)
    {
      if (string.IsNullOrEmpty(nonce) || string.IsNullOrEmpty(this.Nonce))
      { // nothing to validate
        return false;
      }
      if (this.Nonce.Equals(nonce))
      { 
        // TODO: Add validation of the token's signature
        return true;
      }

      return false;
    }

    private static string Base64UrlDecodeJwtTokenPayload(string base64UrlEncodedJwtToken)
    {
      string payload = base64UrlEncodedJwtToken.Split('.')[1];
      return Base64UrlEncoder.Decode(payload);
    }
  }

  public static class Base64UrlEncoder
  {
    static char Base64PadCharacter = '=';
    static string DoubleBase64PadCharacter = String.Format(CultureInfo.InvariantCulture, "{0}{0}", Base64PadCharacter);
    static char Base64Character62 = '+';
    static char Base64Character63 = '/';
    static char Base64UrlCharacter62 = '-';
    static char Base64UrlCharacter63 = '_';

    public static byte[] DecodeBytes(string arg)
    {
      string s = arg;
      s = s.Replace(Base64UrlCharacter62, Base64Character62); // 62nd char of encoding
      s = s.Replace(Base64UrlCharacter63, Base64Character63); // 63rd char of encoding
      switch (s.Length % 4) // Pad 
      {
        case 0:
          break; // No pad chars in this case
        case 2:
          s += DoubleBase64PadCharacter; break; // Two pad chars
        case 3:
          s += Base64PadCharacter; break; // One pad char
        default:
          throw new ArgumentException("Illegal base64url string!", arg);
      }
      return Convert.FromBase64String(s); // Standard base64 decoder
    }

    public static string Decode(string arg)
    {
      return Encoding.UTF8.GetString(DecodeBytes(arg));
    }
  }
}