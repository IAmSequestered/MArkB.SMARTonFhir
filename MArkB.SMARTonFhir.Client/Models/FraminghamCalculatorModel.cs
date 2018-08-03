using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MArkB.SMARTonFhir.Client.Models
{
  public class FraminghamCalculatorModel
  {
  }

  public class FraminghamCalculatorValues
  {
    private int _gender;

    public string Gender
    {
      get
      {
        if (_gender == 1)
        { return "Male"; }
        else
        {
          return "Female";
        }
      }
      set
      {
        if (value.ToLower() == "male")
        { _gender = 1; }
        else
        { _gender = 0; }

      }

    } //:	 Male Female
    public int GenderAsInt
    {
      get { return _gender; }
      set
      {
        _gender = value;
      }
    }

    public int Age { get; set; } //  (years):	
    public int TotalCholesterol { get; set; } // (mmol/L):	
    public int HDL { get; set; } // (mmol/L):	
    public bool Diabetic { get; set; } //:	 Yes No
    public bool Smoker { get; set; } //:	 Yes No
    public int SystolicBP { get; set; } // (mmHg):	
    public bool OnAntihypertensiveMedication { get; set; } //:	 Yes No

  }

  public class FCValues : FraminghamCalculatorValues
  {
  }
  public class FraminghamCalculatorPatient : FCValues
  {
    public string PatientId;

    public string FirstName;

    public string LastName;
  }

  public class CholesterolValue
  {
    public string CholesterolType; //Use the Loinc code for the type.
    public decimal? Value;
    public DateTime IssuedDate;
  }

  public class BloodPressureValue
  {
    public string BloodPressureType; //Use the Loinc code for the type.
    public decimal? Value;
    public DateTime IssuedDate;
  }

}