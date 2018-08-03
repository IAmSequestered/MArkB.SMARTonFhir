using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MArkB.OAuthStarter.Cerner.Auth
{
  //  {{
  //  "need_patient_banner": true,
  //  "id_token": "eyJraWQiOiIyMDE4LTA1LTA5VDE1OjU3OjI4LjQyMS5yc2EiLCJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJzdWIiOiJwb3J0YWwiLCJhdWQiOiI0ZTc2ZDI4OS01ZjZmLTRhNzctYTU0Ny04ODQyNjMyNjQ0YTEiLCJwcm9maWxlIjoiaHR0cHM6XC9cL2ZoaXItZWhyLnNhbmRib3hjZXJuZXIuY29tXC9kc3R1MlwvMGI4YTAxMTEtZThlNi00YzI2LWE5MWMtNTA2OWNiYzZiMWNhXC9QcmFjdGl0aW9uZXJcLzQ0NjQwMDciLCJpc3MiOiJodHRwczpcL1wvYXV0aG9yaXphdGlvbi5zYW5kYm94Y2VybmVyLmNvbVwvdGVuYW50c1wvMGI4YTAxMTEtZThlNi00YzI2LWE5MWMtNTA2OWNiYzZiMWNhXC9vaWRjXC9pZHNwc1wvMGI4YTAxMTEtZThlNi00YzI2LWE5MWMtNTA2OWNiYzZiMWNhXC8iLCJuYW1lIjoiUG9ydGFsLCBQb3J0YWwiLCJleHAiOjE1MjU5Nzk3NjAsImlhdCI6MTUyNTk3OTE2MCwiZmhpclVzZXIiOiJodHRwczpcL1wvZmhpci1laHIuc2FuZGJveGNlcm5lci5jb21cL2RzdHUyXC8wYjhhMDExMS1lOGU2LTRjMjYtYTkxYy01MDY5Y2JjNmIxY2FcL1ByYWN0aXRpb25lclwvNDQ2NDAwNyJ9.GDd2DOxAQDbn4nvAL4XJ00QN3l4i-LDHV_GzRkJKH7ovr5IPBijhBFngt-O_l7z48t88QteoGZuTRog_IVzNXIfMZPjiM5aOmRAD-QHVwbrf8m5QYaD1URP5_x9bPUgs_GEE-b2fvXiFVyseWGyNXf5t8cVQ9rCQqHS_GEPadHk8-k5b20v4Ew-wk08NlgqS3CXsU1cN67axvbzLXExrEx0Teeb6bNi7JYaZLM3FTEoRC9eTC8Yc0SQ1xcZjzEqDxD9al-339h-fIZsUYRMZ4E8kxLbeUwctuJSnDbeXxXE-0Oq0V5zItHKH-o19yPo84jPIVLPcRoOfWQwarYZQcw",
  //  "smart_style_url": "https://smart.sandboxcerner.com/styles/smart-v1.json",
  //  "encounter": "4027930",
  //  "token_type": "Bearer",
  //  "access_token": "eyJraWQiOiIyMDE4LTA1LTA5VDE1OjU3OjI4LjQxNy5lYyIsInR5cCI6IkpXVCIsImFsZyI6IkVTMjU2In0.eyJzdWIiOiJwb3J0YWwiLCJ1cm46Y29tOmNlcm5lcjphdXRob3JpemF0aW9uOmNsYWltcyI6eyJ2ZXIiOiIxLjAiLCJlbmNvdW50ZXIiOiI0MDI3OTMwIiwidG50IjoiMGI4YTAxMTEtZThlNi00YzI2LWE5MWMtNTA2OWNiYzZiMWNhIiwiYXpzIjoicGF0aWVudFwvUGF0aWVudC5yZWFkIHBhdGllbnRcL09ic2VydmF0aW9uLnJlYWQgbGF1bmNoIG9ubGluZV9hY2Nlc3Mgb3BlbmlkIHByb2ZpbGUiLCJ1c2VyIjoiNDQ2NDAwNyIsInBhdGllbnQiOiI0MzQyMDEyIn0sImF6cCI6IjRlNzZkMjg5LTVmNmYtNGE3Ny1hNTQ3LTg4NDI2MzI2NDRhMSIsImlzcyI6Imh0dHBzOlwvXC9hdXRob3JpemF0aW9uLnNhbmRib3hjZXJuZXIuY29tXC8iLCJleHAiOjE1MjU5Nzk3NjAsImlhdCI6MTUyNTk3OTE2MCwianRpIjoiZjc5YWI2YjYtZTI5NC00MTI1LTllZDktZjA2MTRmNDQ3YmM3IiwidXJuOmNlcm5lcjphdXRob3JpemF0aW9uOmNsYWltczp2ZXJzaW9uOjEiOnsidmVyIjoiMS4wIiwicHJvZmlsZXMiOnsibWlsbGVubml1bS12MSI6eyJwZXJzb25uZWwiOiI0NDY0MDA3IiwiZW5jb3VudGVyIjoiNDAyNzkzMCJ9LCJzbWFydC12MSI6eyJwYXRpZW50cyI6WyI0MzQyMDEyIl0sImF6cyI6InBhdGllbnRcL1BhdGllbnQucmVhZCBwYXRpZW50XC9PYnNlcnZhdGlvbi5yZWFkIGxhdW5jaCBvbmxpbmVfYWNjZXNzIG9wZW5pZCBwcm9maWxlIn19LCJjbGllbnQiOnsibmFtZSI6IkFueSBuYW1lIHdpbGwgZG8iLCJpZCI6IjRlNzZkMjg5LTVmNmYtNGE3Ny1hNTQ3LTg4NDI2MzI2NDRhMSJ9LCJ1c2VyIjp7InByaW5jaXBhbCI6InBvcnRhbCIsInBlcnNvbmEiOiJwcm92aWRlciIsImlkc3AiOiIwYjhhMDExMS1lOGU2LTRjMjYtYTkxYy01MDY5Y2JjNmIxY2EiLCJwcmluY2lwYWxVcmkiOiJodHRwczpcL1wvbWlsbGVubmlhLnNhbmRib3hjZXJuZXIuY29tXC9pbnN0YW5jZVwvMGI4YTAxMTEtZThlNi00YzI2LWE5MWMtNTA2OWNiYzZiMWNhXC9wcmluY2lwYWxcLzAwMDAuMDAwMC4wMDQ0LjFEODciLCJpZHNwVXJpIjoiaHR0cHM6XC9cL21pbGxlbm5pYS5zYW5kYm94Y2VybmVyLmNvbVwvYWNjb3VudHNcL2ZoaXJwbGF5LnRlbXBfcmhvLmNlcm5lcmFzcC5jb21cLzBiOGEwMTExLWU4ZTYtNGMyNi1hOTFjLTUwNjljYmM2YjFjYVwvbG9naW4ifSwidGVuYW50IjoiMGI4YTAxMTEtZThlNi00YzI2LWE5MWMtNTA2OWNiYzZiMWNhIn19.DP5L5zdg7iQcirWFC5z4lYzmDXpvr3CYh330IypxfHQVRes9i7oSgDeVg3b3_GY3JWkVeVXbCTqorhgQGOy8ig",
  //  "refresh_token": "eyJpZCI6IjBkZGViZThjLWM0NDItNDRiYS04NGMyLTUxYmI0OWY2M2NkOCIsInNlY3JldCI6IjIxNmEwMGNiLTgyZjMtNDk2YS1hNmUxLTJhZGQ2OTJjN2YzYiIsInZlciI6IjEuMCIsInR5cGUiOiJvbmxpbmVfYWNjZXNzIiwicHJvZmlsZSI6InNtYXJ0LXYxIn0=",
  //  "patient": "4342012",
  //  "scope": "patient/Patient.read patient/Observation.read launch online_access openid profile",
  //  "expires_in": 570,
  //  "user": "4464007",
  //  "tenant": "0b8a0111-e8e6-4c26-a91c-5069cbc6b1ca",
  //  "username": "portal"
  //}}

  public partial class CernerAuth
  {
    public CernerAuth()
    {
      this.retrivaltime = DateTime.Now;
    }

    /// <summary>
    /// Boolean to indicate if the patient banner is needed by the UI.
    /// </summary>
    public bool need_patient_banner { get; set; }
    /// <summary>
    /// The authentication token needed to access the patient data.
    /// </summary>
    public string id_token { get; set; }
    /// <summary>
    /// Where the smart style is. For smart dressers
    /// </summary>
    public string smart_style_url { get; set; }
    /// <summary>
    /// ID for the current encounter.
    /// </summary>
    public string encounter { get; set; }
    /// <summary>
    /// ID for the current patient.
    /// </summary>
    public string patient { get; set; }
    /// <summary>
    /// Permissions scope granted by the system for the current activity.
    /// </summary>
    public string scope { get; set; }
    /// <summary>
    /// ID of the current user.
    /// </summary>
    public string user { get; set; }
    /// <summary>
    /// I think this is the unique ID of the system currently being accessed
    /// </summary>
    public string tenant { get; set; }
    /// <summary>
    /// Login name of the current user.
    /// </summary>
    public string username { get; set; }

    //}

    //public partial class CernerToken
    //{

    /// <summary>
    /// Type of token represented by this object. 'Bearer' or '' are the known options. 
    /// </summary>
    public string token_type { get; set; }
    /// <summary>
    /// The system generated unique identifier used to access the system.
    /// </summary>
    public string access_token { get; set; }
    /// <summary>
    /// Token used to get a new access_token after the one currently being used has expired.
    /// </summary>
    public string refresh_token { get; set; }
    /// <summary>
    /// Time, in seconds, until the token expires. Based on the retrivaltime
    /// </summary>
    public int expires_in { get; set; }


    //public partial class CernerAuth

    public DateTime retrivaltime { get; set; }

    public DateTime expiretime
    {
      get
      {
        return retrivaltime.AddSeconds(expires_in);
      }
    }


    public string CorrelationId { get; set; }

    public string ErrorDescription { get; set; }
  }
}