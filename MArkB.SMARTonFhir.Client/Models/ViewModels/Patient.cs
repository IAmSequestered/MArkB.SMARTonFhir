using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MArkB.SMARTonFhir.Client.Models.ViewModels
{
  public class Patient : Hl7.Fhir.Model.Patient
  {
    public string FirstName { get; set; }

    public string LastName { get; set; }
  }
}