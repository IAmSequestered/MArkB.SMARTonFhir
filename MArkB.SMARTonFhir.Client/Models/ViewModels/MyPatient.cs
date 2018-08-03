using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MArkB.SMARTonFhir.Models.ViewModels
{
  public class MyPatient //: Hl7.Fhir.Model.Patient
  {
    public MyPatient()
    {
      this.MyScoreDetails = new HashSet<MyScoreDetail>();
    }

    [Key]
    public int ID { get; set; }

    public int PatientID { get; set; }
    public double RoRScore { get; set; }

    public virtual ICollection<MyScoreDetail> MyScoreDetails { get; set; }
  }

  public class MyScoreDetail //: Hl7.Fhir.Model.Patient
  {
    [Key]
    public int ID { get; set; }
    

    public string Source { get; set; }
    public string Details { get; set; }


    public int MyPatientID { get; set; }
    public virtual MyPatient MyPatient { get; set; }
  }

}