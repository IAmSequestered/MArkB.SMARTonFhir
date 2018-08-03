//using MArkB.Fhir._0._1.Client.Auth;

using MArkB.SMARTonFhir.Cerner.Utilities;
using MArkB.SMARTonFhir.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MArkB.OAuthStarter.Cerner.Auth;
using MArkB.SMARTonFhir.Cerner.Models;

namespace MArkB.SMARTonFhir.Client.Models.ViewModels
{
  public class CernerFullConnectionInformationViewModel
  {
    
    public Patient ViewPatient { get; set; }
    public CernerConnectionInformation CCI { get; set; }
    public CernerAuth CernerAuth { get; set; }

    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }

    public string ServiceUrl { get; set; }
    public string PatientId { get; set; }
    

  }
}