using System;
using System.ComponentModel.DataAnnotations;

namespace MArkB.SMARTonFhir.Cerner.Models
{
  public class CernerConnectionInformation
  {
    private Uri _serviceURI;
    private Uri _conformanceUri;
    private Uri _tokenUri;
    private Uri _authorizeUri;
    private string _serviceIdentifier;

    /// <summary>
    /// The root url for the Cerner environment you are accessing.
    /// </summary>
    [Display(Name = "Base URL",
      Description = "The root url for the Cerner environment you are accessing.")]
    public string BaseURL { get; set; }

    /// <summary>
    /// This is the Client ID obtained from registering at
    /// the Cerner portal. Note that it is NOT the same as the AppId.
    /// </summary>
    [Display(Name = "Client ID",
      Description = "This is the Client ID obtained from registering at the Cerner portal. Note that it is NOT the same as the AppId.")]
    public string ClientId { get; set; }

    /// <summary>
    /// This is the App ID obtained from registering at
    /// the Cerner portal. Note that it IS the same as the AppId.
    /// </summary>
    [Display(Name = "App Secret",
      Description = "This is the App ID obtained from registering at the Cerner portal. Note that it IS the same as the AppId.")]
    public string AppSecret { get; set; }

    /// <summary>
    /// A sub folder, if needed. Likely the version of FHIR thats being used by the system.
    /// </summary>
    /// <remarks>Okay to leave blank. Include the forward slash at the end only when not blank.</remarks>
    /// <example>"dstu2/"</example>
    [Display(Name = "FHIR Version Folder",
      Description = "A sub folder, if needed. Likely the version of FHIR thats being used by the system.")]
    public string FHIRVersionFolder { get; set; }

    /// <summary>
    /// The url utilized by your application to access Cerner resourses.
    /// The url is the combination of both the BaseURI and the ClientID.
    /// </summary>
    [Display(Name = "Service URL",
      Description = "The url utilized by your application to access Cerner resourses. The url is the combination of both the BaseURI and the ClientID.")]
    public string ServiceUrl
    {
      get { return _serviceURI.ToString(); }
      set
      {
        _serviceURI = new Uri(value);
      }
    }

    /// <summary>
    /// The url utilized by your application to access cerner resouses.
    /// The url is the combination of both the BaseURI and the ClientID.
    /// </summary>
    public Uri ServiceURI
    {
      get { return _serviceURI; }
      set
      {
        _serviceURI = value;
      }
    }

    /// <summary>
    /// Where we get the important stuff about where all the other things are kept.
    /// </summary>
    [Display(Name = "Conformance URL",
      Description = "Where we get the important stuff about where all the other things are kept.")]
    public string ConformanceUrl
    {
      get { return _conformanceUri.ToString(); }
      set
      {
        _conformanceUri = new Uri(value);
      }
    }

    ///// <summary>
    ///// Where we get the important stuff about where all the other things are kept.
    ///// </summary>
    //public Uri zConformanceUri
    //{
    //  get { return _conformanceUri; }
    //  set
    //  {
    //    _conformanceUri = value;
    //  }
    //}

    /// <summary>
    /// URL where the security token is retrieved from.
    /// </summary>
    [Display(Name = "Token URL",
      Description = "URL where the security token is retrieved from.")]
    public string TokenUrl
    {
      get { return _tokenUri.ToString(); }
      set
      {
        _tokenUri = new Uri(value);
      }
    }

    /// <summary>
    /// Where the token is retrieved from.
    /// </summary>
    public Uri TokenUri
    {
      get { return _tokenUri; }
      set
      {
        _tokenUri = value;
      }
    }

    /// <summary>
    /// I forgets what this is for. Not so much what it's for as why it's named whut it is.
    /// </summary>
    [Display(Name = "Authorize URL",
      Description = "I forgets what this is for. Not so much what it's for as why it's named whut it is.")]
    public string AuthorizeUrl
    {
      get { return _authorizeUri.ToString(); }
      set
      {
        _authorizeUri = new Uri(value);
      }
    }

    /// <summary>
    /// I forgets what this is for. Not so much what it's for as why it's named whut it is.
    /// </summary>
    public Uri AuthorizeUri
    {
      get { return _authorizeUri; }
      set
      {
        _authorizeUri = value;
      }
    }

    /// <summary>
    /// This stuff is tweeking my reality. I fogets what this is for too.
    /// </summary>
    [Display(Name = "Service Identifier",
      Description = "This stuff is tweeking my reality. I fogets what this is for too.")]
    public string ServiceIdentifier
    {
      get { return _serviceIdentifier.ToString(); }
      set
      {
        _serviceIdentifier = value;
      }
    }

    /// <summary>
    /// The root url for the Cerner environment you are accessing.
    /// </summary>
    [Display(Name = "Redirect URL",
      Description = "Where we go back to after authentication.")]
    public string RedirectURL { get; set; }

    [Display(Name = "Error CorrelationId",
      Description = "")]
    public string ErrorCorrelationId { get; set; }


    [Display(Name = "Error Description",
      Description = "")]
    public string ErrorDescription { get; set; }
  }
}