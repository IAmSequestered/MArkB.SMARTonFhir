using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
//using System.Web.Http;

using System.Web;
using System.Web.Mvc;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Model;
using static System.Net.WebRequestMethods;
using MArkB.SMARTonFhir.Cerner.Models;
using MArkB.SMARTonFhir.Cerner.Utilities;

using System.Threading.Tasks;
using MArkB.SMARTonFhir.Client.Models;

using MArkB.SMARTonFhir.Client.Managers;
using MArkB.SMARTonFhir.Client.Utilities;
using MArkB.OAuthStarter.Cerner.Auth;


using System.Web.Script.Serialization;
using System.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using MArkB.SMARTonFhir.Client.Models.ViewModels;



namespace MArkB.SMARTonFhir.Client.Controllers
{

  public static class SMARTOnFhirConstants
  {
    public const string TokenRequestGrantType = "authorization_code";
  }
  public class FC_AppController : Controller
  {

    string appID;
    string bearer;

    string urlBase;

    Uri uriBase;
    Uri uriAppBase;


    //* Most of these values should be in a config file.

    public string ClientID { get; set; }

    // This is the logon authority
    // i.e. https://login.microsoftonline.com/common
    public string Authority { get; set; }
    public string TokenUri { get; set; }
    // This is the application ID obtained from registering at
    public string AppId { get; set; }
    // This is the application secret obtained from registering at
    // At cerner it's called the bearer;
    public string AppSecret { get; set; }

    //public CernerHelper myCernerHelper;
    public CernerHelper myCernerHelper;

    public string[] Scopes { get; set; }

    //public string scopesString = "patient/Patient.read patient/Observation.read launch online_access openid profile";

    // private static string[] scopes = { "patient/Patient.read patient/Observation.read launch online_access openid profile" }; //https://graph.microsoft.com/User.Read" };

    private static string[] scopes = { "patient/Condition.read patient/DiagnosticReport.read patient/Patient.read patient/Observation.read launch online_access openid profile" };


    public FC_AppController()
    {

      

      myCernerHelper = new CernerHelper();

      ClientID = myCernerHelper.cci.ClientId;
      appID = myCernerHelper.cci.AppSecret;
      bearer = appID;

      //urlBase = ConfigurationManager.AppSettings["ida:BaseFhirUrl"];

      urlBase = myCernerHelper.cci.BaseURL;

      uriBase = new Uri(urlBase);
      uriAppBase = new Uri(uriBase, ClientID);

      //string tempFHIRVersionFolder = myCernerHelper.cci.FHIRVersionFolder;

      //string fhirVersionFolder = ConfigurationManager.AppSettings["ida:FHIRVersionFolder"];

      //var uriFhirVersionFolder = new Uri(uriBase, fhirVersionFolder);

      //string serviceIdentifier = ConfigurationManager.AppSettings["ida:ServiceIdentifier"];

      //uriAppBase = new Uri(uriFhirVersionFolder, serviceIdentifier);


      //Build the authority uri
      //Authority = uriAppBase.ToString() + "/protocols/oauth2/profiles/smart-v1/personas/provider/authorize";
      Authority = "";

      TokenUri = "https://authorization.sandboxcerner.com/tenants/" + ClientID +
        "/protocols/oauth2/profiles/smart-v1/token";

      AppId = ClientID;
      AppSecret = appID;

      Scopes = scopes;

    }

    public ActionResult Health()
    {
      ViewBag.Message = "Your application description page.";

      return View();
    }

    //public ActionResult Launch(CernerOAuth2ViewModels lvm)
    public ActionResult Launch()
    {

      ViewBag.Message = "Framingham Calculator Launch Page";

      string serviceUri = Request.Params["iss"];
      string launchContextId = Request.Params["launch"];

      CernerOAuth2ViewModels lvm = PhillLVM(serviceUri, launchContextId);

      return View(lvm);
    }

    private CernerAuth TokenGettingStuff()
    {
      //Get us some information from the URL
      string authCode = Request.Params["code"];
      string idToken = Request.Params["id_token"];
      string state = Request.Params["state"];

      //Have the helper get the meta data from Cerners server.
      myCernerHelper.GetMetaData(idToken);

      // Request an access token
      //We're going to have to do some work here for refreshing the token.
      OAuthHelper oauthHelper = new OAuthHelper(myCernerHelper.cci.AuthorizeUrl, myCernerHelper.cci.TokenUrl, ClientID, myCernerHelper.cci.AppSecret);

      // Dynamically create the redirect based on the current controllers name
      string redirectUri;

      var thisControllerName = HttpContext.Request.RequestContext.RouteData.Values["controller"].ToString();
      //var thisActionName = HttpContext.Request.RequestContext.RouteData.Values["action"].ToString();

      var urlBuilder = new System.UriBuilder(Request.Url.AbsoluteUri)
      {
        Path = thisControllerName, // Url.Action(thisActionName, thisControllerName),
        Query = null,
      };

      //We got's to add the slash at the end to keep the OAuth server happy.
      redirectUri = urlBuilder.Uri.ToString() + @"/";

      HttpSessionStateBase session = this.Session;

      Models.ViewModels.Patient viewPatient = new Models.ViewModels.Patient();

      CernerAuth myCernerAuth = new CernerAuth();

      //Let's go get all of that great tasting OAuth stuff with that OH SO SWEET Auth Token!
      myCernerAuth = oauthHelper.GetTokensFromAuthority(SMARTOnFhirConstants.TokenRequestGrantType, authCode, redirectUri, session);

      return myCernerAuth;
    }

    public ActionResult Index(CernerOAuth2ViewModels lvm)
    {

      CernerAuth myCernerAuth = TokenGettingStuff();
      try
      {

        CernerFullConnectionInformationViewModel cfcivm = new CernerFullConnectionInformationViewModel
        {
          AccessToken = myCernerAuth.access_token,
          RefreshToken = myCernerAuth.refresh_token,

          PatientId = myCernerAuth.patient,

          ServiceUrl = myCernerHelper.cci.ServiceUrl
        };

        FraminghamCalculatorViewModel fcvm = FraminghamCalculator(cfcivm);

        return View("FraminghamCalculator", fcvm);

      }
      catch (Exception ex)
      {
        myCernerHelper.cci.ErrorCorrelationId = myCernerAuth.CorrelationId;

        return View("CernerHelper", myCernerHelper.cci);

      }
    }


    /// <summary>
    /// Builds the redirect uri needed for the OAuth2 process.
    /// </summary>
    /// <returns></returns>
    public string RedirctUriBuilder()
    {

      //Build out the redirectUri.

      //Get the name of the current action and controller.
      var thisControllerName = HttpContext.Request.RequestContext.RouteData.Values["controller"].ToString();
      var thisActionName = HttpContext.Request.RequestContext.RouteData.Values["action"].ToString();

      var urlBuilder = new System.UriBuilder(Request.Url.AbsoluteUri)
      {
        //Note that I'm not including the action name here. 
        // If your destination is not the Index page then you'll need to make some changes here.
        Path = thisControllerName, //Url.Action(thisActionName, thisControllerName),
        Query = null,
      };

      Uri uri = urlBuilder.Uri;

      //We got's to add the slash at the end to keep the OAuth server happy.
      return urlBuilder.Uri.ToString() + @"/"; ;

    }

    /// <summary>
    /// Given the ServiceURI and the LaunchContexID we can ask the cerner authentication server
    /// for the rest of the information we're going to need to pass further down the line.
    /// </summary>
    /// <param name="serviceUri"></param>
    /// <param name="launchContextId"></param>
    /// <returns></returns>
    private CernerOAuth2ViewModels PhillLVM(string serviceUri, string launchContextId)
    {

      CernerOAuth2ViewModels lvm = new CernerOAuth2ViewModels();

      myCernerHelper.GetMetaData(launchContextId);

      lvm.ClientId = ClientID;

      // For demonstration purposes, if you registered a confidential client
      // you can enter its secret here. The demo app will pretend it's a confidential
      // app (in reality it cannot be confidential, since it cannot keep secrets in the
      // browser)
      // set me, if confidential
      lvm.Secret = AppSecret;


      //I'm going to just get this here so I have it. Not sure I will need it.
      // These parameters will be received at launch time in the URL

      lvm.Scopes = string.Join(" ", Scopes);

      //Generate a unique ID for this session.
      lvm.State = Guid.NewGuid();

      lvm.ServiceUri = serviceUri;

      lvm.LaunchContextId = launchContextId;

      lvm.RedirectUri = RedirctUriBuilder();

      lvm.LaunchUri = lvm.RedirectUri + "Launch";

      lvm.ConformanceUri = uriAppBase + "/metadata";

      
      

      //lvm.AuthUri = Authority;
      //lvm.TokenUri = TokenUri;

      lvm.AuthUri = myCernerHelper.cci.AuthorizeUrl;
      lvm.TokenUri = myCernerHelper.cci.TokenUrl;

      //Make up that full redirect href
      lvm.RedirectHref = lvm.AuthUri
          + "?" +
          "response_type=code&" +
          "client_id=" + HttpUtility.UrlEncode(lvm.ClientId) + "&" +
          "scope=" + HttpUtility.UrlEncode(lvm.Scopes) + "&" +
          "redirect_uri=" + HttpUtility.UrlEncode(lvm.RedirectUri) + "&" +
          "aud=" + HttpUtility.UrlEncode(lvm.ServiceUri) + "&" +
          "launch=" + lvm.LaunchContextId + "&" +
          "state=" + lvm.State.ToString();

      //*authUri + "?" +
      //       "response_type=code&" +
      //       "client_id=" + encodeURIComponent('@Model.ClientId') + "&" +
      //       "scope=" + encodeURIComponent('@Model.Scopes') + "&" +
      //       "redirect_uri=" + encodeURIComponent('@Model.RedirectUri') + "&" +
      //       "aud=" + encodeURIComponent('@Model.ServiceUri') + "&" +
      //       "launch=" + '@Model.LaunchContextId' + "&" +
      //       "state=" + '@Model.State.ToString()';



      return lvm;
    }

    // Let's request the conformance statement from the SMART on FHIR API server and
    // find out the endpoint URLs for the authorization server



    //public ActionResult FraminghamCalculator(CernerFullConnectionInformationViewModel cfcivm) //Models.ViewModels.Patient viewPatient)
    public FraminghamCalculatorViewModel FraminghamCalculator(CernerFullConnectionInformationViewModel cfcivm) //Models.ViewModels.Patient viewPatient)

    {
      ViewBag.Message = "Framingham Calculator";


      var client = new FhirClient(cfcivm.ServiceUrl)
      {
        PreferredFormat = ResourceFormat.Json
      }; ;

      client.OnBeforeRequest += (object sender, BeforeRequestEventArgs e) =>
      {
        e.RawRequest.Headers.Add("Authorization", "Bearer " + cfcivm.AccessToken);
      };


      var identity = ResourceIdentity.Build("Patient", cfcivm.PatientId);

      Hl7.Fhir.Model.Patient viewPatient = client.Read<Hl7.Fhir.Model.Patient>(identity);

      //Get those values we don't know how to retrive yet.
      FraminghamCalculatorPatient Fcp = PhillFCM(0);

      Fcp.LastName = viewPatient.Name[viewPatient.Name.Count - 1].FamilyElement[0].ToString();
      Fcp.FirstName = viewPatient.Name[viewPatient.Name.Count - 1].GivenElement[0].ToString();
      Fcp.Gender = viewPatient.Gender.ToString();
      Fcp.Age = Convert.ToDateTime(viewPatient.BirthDate).Age();


      #region Get HDL
      //Now we get some real data like the HDL
      FraminghamCalculator fc = new FraminghamCalculator(Fcp);
      fc.FhirClient = client;

      string loinc_hdl = "2085-9";
      string loinc_ldl = "13457-7";
      string loinc_Cholesterol_Total = "9830-1";
      string loinc_cholesterol = "2093-3";
      string loinc_Triglycerides = "2571-8";
      string loinc_SystolicBP = "8480-6";
      string loinc_DiastolicBP = "8462-4";

      BloodPressureValue bpv = fc.GetLatestBloodPressureValue(cfcivm.PatientId, loinc_SystolicBP);
      CholesterolValue cv_hdl = fc.GetLatestCholesterolValue(cfcivm.PatientId, loinc_hdl);
      CholesterolValue cv_total = fc.GetLatestCholesterolValue(cfcivm.PatientId, loinc_cholesterol);

      #endregion Get HDL

      Fcp.SystolicBP = (int)bpv.Value;
      Fcp.HDL = (int)cv_hdl.Value;
      Fcp.TotalCholesterol = (int)cv_total.Value;

      FraminghamCalculatorViewModel vm = new FraminghamCalculatorViewModel();

      vm.FraminghamScore = fc.DoCalculation();
      vm.FCM = Fcp;

      return vm;
    }

    //Populates a fake patient values for us
    FraminghamCalculatorPatient PhillFCM(int iWhichOne)
    {

      FraminghamCalculatorPatient myFCMP = new FraminghamCalculatorPatient();
      switch (iWhichOne)
      {
        //case 1:
        //  Console.WriteLine(1);
        //  break;
        //case 5:
        //  Console.WriteLine(5);
        //  break;


        default:

          iWhichOne = 999999999;

          myFCMP.Age = 65;
          myFCMP.Diabetic = true;
          myFCMP.FirstName = "Kevin";
          myFCMP.GenderAsInt = 1;
          myFCMP.HDL = 98;
          myFCMP.LastName = "GonnaDie";
          myFCMP.OnAntihypertensiveMedication = false;
          myFCMP.PatientId = iWhichOne.ToString();
          myFCMP.Smoker = true;
          myFCMP.SystolicBP = 156;
          myFCMP.TotalCholesterol = 425;

          break;
      }

      return myFCMP;
    }

    public bool IsPatientDiabetic(FhirClient client, Models.ViewModels.Patient patient)
    {
      bool result = false;

      var query = new string[] { "patient=" + patient.Id, "category=diagnosis" };
      var bundle = client.Search<Condition>(query);

      //Hl7.Fhir.Model.DiagnosticReport. Condition

      foreach (var item in bundle.Entry)
      {
        Console.WriteLine(item.FhirComments != null ? item.FhirComments.FirstOrDefault() : string.Empty);
      }

      return result;
    }
  }
}
