using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MArkB.Fhir._0._1.Client.Models.ViewModels
{
  public class LaunchViewModel //: Hl7.Fhir.Model.Patient
  {
   

    public string Client_Id { get; set; }
    public string Scope { get; set; }

  }

}