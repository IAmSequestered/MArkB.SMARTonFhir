using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MArkB.SMARTonFhir.Client.Models.ViewModels
{
  public class CernerAuthTargetURIs
  {
    public string AuthUri { get; set; }

    public string TokenUri { get; set; }
  }

    public class CernerOAuth2ViewModels
  {
    public string ClientId { get; set; }

    public string Secret { get; set; }

    public string ServiceUri { get; set; }

    public string LaunchContextId { get; set; }

    public string Scopes { get; set; }

    /// <summary>
    /// An identifier sent via the URL used to access passed information.
    /// It's a Guid/string generated at run time and sent as a parameter in the LaunchURL.
    /// </summary>
    public Guid State { get; set; }
    /// <summary>
    /// URL for this Launch page. It's set on the applications portal page.
    /// </summary>
    public string LaunchUri { get; set; }
    /// <summary>
    /// Where the calling site is supposed to go after the launch page.
    /// It needs to match the one set in the applications portal page.
    /// </summary>
    public string RedirectUri { get; set; }
    /// <summary>
    /// Where information about which aspects of the standard this site complies to.
    /// </summary>
    public string ConformanceUri { get; set; }
    /// <summary>
    /// URL to where authentication happens.
    /// </summary>
    public string AuthUri { get; set; }
    /// <summary>
    /// Where to go to get the token.
    /// </summary>
    public string TokenUri { get; set; }
    public string SmartExtension { get; set; }

    public string RedirectHref { get; set; }

}
}
