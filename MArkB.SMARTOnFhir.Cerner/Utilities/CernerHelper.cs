using MArkB.Cerner.Models;
using MArkB.SMARTonFhir.Cerner.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using MArkB.SMARTonFhir.Cerner.Models;

namespace MAMC.SMARTonFhir.Client.Managers
{


  public class CernerHelper
  {
    public CernerConnectionInformation cci = new CernerConnectionInformation();

    private string _whichSection;

    public string WhichSection
    {
      set
      {
        this._whichSection = value;
      }
    }


    public CernerHelper()
    {

      if (String.IsNullOrEmpty(_whichSection))
      {
        _whichSection = "SmartOnFhirConfiguration";

      }

      SMARTonFhirConfigurationSection configSection =
        (SMARTonFhirConfigurationSection)ConfigurationManager.GetSection(
        "SMARTonFhirConfigurationGroup/" + _whichSection);

      string myBaseFhirUrl = configSection.FhirDataTargets.BaseFhirUrl;

      cci.ClientId = configSection.FhirTokens.AppClientId;

      cci.BaseURL = configSection.FhirDataTargets.BaseFhirUrl;
      cci.ServiceIdentifier = configSection.FhirDataTargets.ServiceIdentifier;
      cci.AppSecret = configSection.FhirTokens.AppSecret;
      cci.FHIRVersionFolder = configSection.FhirDataTargets.FHIRVersionFolder;

      //BaseURI = baseUri;
      cci.ServiceURI = new Uri(new Uri(cci.BaseURL), cci.FHIRVersionFolder + cci.ServiceIdentifier);

      string tempServiceIdentifier = @"/" + cci.ServiceIdentifier + @"/metadata";

      //cci.ConformanceUrl = cci.ServiceURL
      //cci.ConformanceUri = new Uri(cci.ServiceURL tempServiceIdentifier);
      string tempString01 = cci.ServiceUrl;
      string tempUri01 = (tempServiceIdentifier);

    }

    public CernerHelper(string baseUrl, string serviceIdentifier, string clientId)
    {
      cci.BaseURL = baseUrl;      
      cci.ServiceURI = new Uri(new Uri(cci.BaseURL), cci.FHIRVersionFolder + cci.ServiceIdentifier);
      cci.ServiceIdentifier = serviceIdentifier;
      //cci.ConformanceUri = new Uri(cci.ServiceURL, @"/" + serviceIdentifier + @"/metadata");
    }

    //ToDo: I might want to hange this to AuthorizationUrl without the Get
    public string GetAuthorizationUrl(string[] scopes, string redirectUri, string state, string launchContextId)
    {
      // Start with the base URL
      UriBuilder authUrl = new UriBuilder("junk"); // this.Authority + authEndpoint);

      authUrl.Query =
        "response_type=code+id_token" +
        "&scope=openid+profile+email+offline_access+" + EncodeScopes(scopes) +
        "&state=" + state +
        "&nonce=" + launchContextId +
        "&client_id=" + cci.ClientId +
        "&redirect_uri=" + HttpUtility.UrlEncode(redirectUri) +
        "&response_mode=form_post";

      return authUrl.ToString();
    }

    //ToDo: Change this method to Encode Scopes
    private string EncodeScopes(string[] scopes)
    {
      string encodedScopes = string.Empty;
      foreach (string scope in scopes)
      {
        if (!string.IsNullOrEmpty(encodedScopes)) { encodedScopes += '+'; }
        encodedScopes += HttpUtility.UrlEncode(scope);
      }
      return encodedScopes;
    }


    /// <summary>
    /// Converts the json object into a .net dictionary object.
    /// </summary>
    /// <param name="rss"></param>
    /// <returns></returns>
    private Dictionary<string, string> GetAuthenticationDictionaryFromJson(JObject rss)
    {
      JArray rssValue = (JArray)rss["rest"];

      //Get the json token that contains the list of urls we care about.
      JToken myJToken = rssValue[0]["security"]["extension"][0]["extension"];

      Dictionary<string, string> myDictionary = new Dictionary<string, string>();

      //Shove that list into a dictionary object.
      //Truth is, this should be done directly from the JToken but I can't figure that out.
      foreach (JObject content in myJToken.Children<JObject>())
      {
        myDictionary.Add(content["url"].ToString(), content["valueUri"].ToString());
      }

      return myDictionary;
    }

    /// <summary>
    /// Say something intelegent hear so anyone that reeds this things where smarmy.
    /// </summary>
    /// <param name="launchContextId"></param>
    /// <returns></returns>
    //public void GetMetaData(string launchContextId)
    //public async Task<bool> GetMetaData(string launchContextId)
    public bool GetMetaData(string launchContextId)
    {
      bool result = false;

      // Build the token request payload.
      // Not sure this is all necessary but I ain't going to change it.
      FormUrlEncodedContent tokenRequestForm = new FormUrlEncodedContent(
        new[]
        {
          new KeyValuePair<string,string>("name", "SMART on FHIR Testing Server"),
          new KeyValuePair<string,string>("description", "Dev server for SMART on FHIR"),
          new KeyValuePair<string,string>("url", cci.ServiceUrl)
        }
      );

      //I should comment on what this dictionary is for.
      Dictionary<string, string> myDictionary = new Dictionary<string, string>();

      //Make the call to the meta data location
      using (HttpClient httpClient = new HttpClient())
      {
        string requestString = tokenRequestForm.ReadAsStringAsync().Result;
        StringContent requestContent = new StringContent(requestString);
        requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

        httpClient.DefaultRequestHeaders.Add("Authorization", string.Format("Basic {0}", launchContextId));

        string medatadataUrl = cci.BaseURL + "/" + cci.ServiceIdentifier + "/metadata";

        // Set up the HTTP GET request
        var response = httpClient.GetAsync(medatadataUrl + "?_format=json").Result;

        JObject jsonResponse = JObject.Parse(response.Content.ReadAsStringAsync().Result);
        //JsonSerializer jsonSerializer = new JsonSerializer();

        myDictionary = GetAuthenticationDictionaryFromJson(jsonResponse);
      }

      //Put it in the dictionary.
      if (myDictionary.ContainsKey("token"))
      {
        cci.TokenUrl = new Uri(myDictionary["token"]).ToString();
        result = true;
      }
      else
      {
        //Should be throwing or logging an error here.
        cci.TokenUrl = "Sorry, couldn't find Token URI";
        result = false;
      }

      if (myDictionary.ContainsKey("authorize"))
      {
        cci.AuthorizeUrl = new Uri(myDictionary["authorize"]).ToString();
        result = true;
      }
      else
      {
        //Should be throwing or logging an error here.
        cci.AuthorizeUrl = "Sorry, couldn't find Authorize URI";
        result = false;
      }

      return result;
    }
  }
}