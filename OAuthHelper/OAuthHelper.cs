using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;



//using System.Linq;

namespace MArkB.OAuthStarter.Cerner.Auth
{
  public class OAuthHelper
  {
    // The v2 app model endpoints
    private static string authEndpoint = "/oauth2/v2.0/authorize";
    private static string tokenEndpoint = "/oauth2/v2.0/token";



    // This is the logon authority
    // i.e. https://login.microsoftonline.com/common
    public string Authority { get; set; }

    public string Token { get; set; }

    // This is the application ID obtained from registering at
    // https://apps.dev.microsoft.com
    public string AppId { get; set; }
    // This is the application secret obtained from registering at
    // https://apps.dev.microsoft.com
    public string AppSecret { get; set; }

    
    public OAuthHelper(string authority, string token, string appId, string appSecret)
    {
      Authority = authority;
      Token = token;
      AppId = appId;
      AppSecret = appSecret;
    }

    // Builds the authorization URL where the app sends the user to sign in
    public string GetAuthorizationUrl(string[] scopes, string redirectUri, string state, string nonce)
    {
      // Start with the base URL
      UriBuilder authUrl = new UriBuilder(this.Authority);

      authUrl.Query =
        "response_type=code+id_token" +
        "&scope=openid+profile+email+offline_access+" + GetEncodedScopes(scopes) +
        "&state=" + state +
        "&nonce=" + nonce +
        "&client_id=" + this.AppId +
        "&redirect_uri=" + HttpUtility.UrlEncode(redirectUri) +
        "&response_mode=form_post";

      return authUrl.ToString();
    }


    private string GetEncodedScopes(string[] scopes)
    {
      string encodedScopes = string.Empty;
      foreach (string scope in scopes)
      {
        if (!string.IsNullOrEmpty(encodedScopes)) { encodedScopes += '+'; }
        encodedScopes += HttpUtility.UrlEncode(scope);
      }
      return encodedScopes;
    }

    // Makes a POST request to the token endopoint to get an access token using either
    // an authorization code or a refresh token. This will also add the tokens
    // to the local cache.
     
    //public async Task<TokenRequestSuccessResponse> GetTokensFromAuthority(string grantType, string grantParameter, string redirectUri, HttpSessionStateBase session)
    //public Task<TokenRequestSuccessResponse> GetTokensFromAuthority2(string grantType, string grantParameter, string redirectUri, HttpSessionStateBase session)
    public CernerAuth GetTokensFromAuthority(string grantType, string grantParameter, string redirectUri, HttpSessionStateBase session)
    {

      CernerAuth cernerAuthenticationInformation = new CernerAuth(); // = new CernerAuth();
      
      // Build the token request payload
      //This is whut is called 'data' in the java code within the Index page.
      FormUrlEncodedContent tokenRequestForm = new FormUrlEncodedContent(
        new[]
        {
          new KeyValuePair<string,string>("grant_type", grantType),
          new KeyValuePair<string,string>("code", grantParameter),
          new KeyValuePair<string,string>("redirect_uri", redirectUri),
          new KeyValuePair<string,string>("client_id", this.AppId)

        }
      );

      using (HttpClient httpClient = new HttpClient())
      {
        string requestString = tokenRequestForm.ReadAsStringAsync().Result;
        StringContent requestContent = new StringContent(requestString);
        requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(this.AppId + ":" + this.AppSecret);


        // Set up the HTTP POST request
        HttpRequestMessage tokenRequest = new HttpRequestMessage(HttpMethod.Post, this.Token);
        tokenRequest.Content = requestContent;
        tokenRequest.Headers.UserAgent.Add(new ProductInfoHeaderValue("MArksSMARTonFHIR", "1.0"));
        tokenRequest.Headers.Add("client-request-id", Guid.NewGuid().ToString());
        tokenRequest.Headers.Add("return-client-request-id", "true");
        tokenRequest.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(plainTextBytes));


        // Send the request and read the JSON body of the response
        HttpResponseMessage response = httpClient.SendAsync(tokenRequest).Result;
        JObject jsonResponse = JObject.Parse(response.Content.ReadAsStringAsync().Result);
        JsonSerializer jsonSerializer = new JsonSerializer();

        if (response.IsSuccessStatusCode)
        {
          // Parse the token response
          TokenRequestSuccessResponse s = (TokenRequestSuccessResponse)jsonSerializer.Deserialize(
            new JTokenReader(jsonResponse), typeof(TokenRequestSuccessResponse));

          cernerAuthenticationInformation = (CernerAuth)jsonSerializer.Deserialize(
            new JTokenReader(jsonResponse), typeof(CernerAuth));

          // Save the tokens
          //SaveUserTokens(session, s);
          //return null;
        }
        else
        {
          // Parse the error response
          TokenRequestErrorResponse e = (TokenRequestErrorResponse)jsonSerializer.Deserialize(
            new JTokenReader(jsonResponse), typeof(TokenRequestErrorResponse));

          cernerAuthenticationInformation.CorrelationId = e.CorrelationId;
          cernerAuthenticationInformation.ErrorDescription = e.Description;

          // Throw the error description
          throw new Exception(e.Description);
        }
      }

      return cernerAuthenticationInformation;

    }
  }
}