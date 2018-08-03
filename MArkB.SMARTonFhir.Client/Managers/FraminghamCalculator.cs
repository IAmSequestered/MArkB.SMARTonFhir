using MArkB.Fhir._0._1.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FraminghamnBN;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Model;
using MArkB.SMARTonFhir.Client.Models;

namespace MArkB.SMARTonFhir.Client.Managers
{
  //Credit for the calculator to Alexandra Soboleva at:https://github.com/AlexandraSoboleva/FraminghamBN/



  public class FraminghamCalculator
  {

    public FraminghamCalculator()
    {
    }

    public FraminghamCalculator(FraminghamCalculatorPatient fcp)
    {
      this.FCP = fcp;
    }

    public FhirClient FhirClient { get; set; }

    string patientId = "4342010";
    static string loinc_Url = "http://loinc.org|";
    //string loinc_CholesterolType = "2085-9";


    FraminghamCalculatorPatient FCP { get; set; }


    public int DoCalculation()
    {
      //int sex, int age,int totchol, int smoke, int hdl, double sysbp, int bpmeds
      int myResult = FraminghamRiskScore.scorePoints(
        FCP.GenderAsInt
        , FCP.Age
        , FCP.TotalCholesterol
        , Convert.ToInt32(FCP.Smoker)
        , FCP.HDL
        , FCP.SystolicBP
        , Convert.ToInt32(FCP.OnAntihypertensiveMedication));

      return myResult;
    }


    /// <summary>
    /// Retrieves the Cholesterol values we're going to use to calculate one of the something something something
    /// </summary>
    /// <remarks>Send in an Id for a patient and the loinc code for the the type of Cholesterol you want returned.</remarks>
    /// <param name="loinc_CholesterolType"></param>
    /// <param name="patientId"></param>
    /// <returns></returns>
    public CholesterolValue GetLatestCholesterolValue(string patientId, string loinc_CholesterolType)
    {
      //Loinc code is made up of the url to the source as well as the code number itself.
      string loinc_path = loinc_Url + loinc_CholesterolType;

      var query = new string[] { string.Format("patient={0}", patientId), string.Format("code={0}", loinc_path) };
      var myObservations = FhirClient.Search<Observation>(query);


      List<CholesterolValue> valuesList = new List<CholesterolValue>();

      //Here we get all of the selected observations they'll let us have.
      // Fhir is supposed to use pagination so we should only get a limited set of what's available.
      // I'm assuming, for some reason, that the first ones to show up are the latest ones.
      foreach (Bundle.EntryComponent entry in myObservations.Entry)
      {

        CholesterolValue cholesterolValue = new CholesterolValue();

        Observation myObservation = (Hl7.Fhir.Model.Observation)entry.Resource;

        if (myObservation.Value is Hl7.Fhir.Model.Quantity valueQuantity)
        {

          Quantity myQuantity = (Hl7.Fhir.Model.Quantity)myObservation.Value;

          cholesterolValue.Value = myQuantity.Value;
          cholesterolValue.IssuedDate = myObservation.Issued.Value.DateTime;
          cholesterolValue.CholesterolType = loinc_CholesterolType;

          valuesList.Add(cholesterolValue);

        }
      }

      //Sort by the issued date so the most recent is on the top and then select that one.
      return valuesList.OrderByDescending(a => a.IssuedDate).FirstOrDefault();

    }

    public BloodPressureValue GetLatestBloodPressureValue(string patientId, string loinc_BloodPressureType)
    {
      //Loinc code is made up of the url to the source as well as the code number itself.
      string loinc_path = loinc_Url + loinc_BloodPressureType;

      var query = new string[] { string.Format("patient={0}", patientId), string.Format("code={0}", loinc_path) };
      var myObservations = FhirClient.Search<Observation>(query);

      List<BloodPressureValue> valuesList = new List<BloodPressureValue>();

      //Here we get all of the selected observations they'll let us have.
      // Fhir is supposed to use pagination so we should only get a limited set of what's available.
      // I'm assuming, for some reason, that the first ones to show up are the latest ones.
      foreach (Bundle.EntryComponent entry in myObservations.Entry)
      {

        BloodPressureValue bloodPressureValue = new BloodPressureValue();

        Observation myObservation = (Hl7.Fhir.Model.Observation)entry.Resource;

        var myComponents = myObservation.Component;

        //We have the observation entry. Now we have to pull the Systolic value out of that.
        foreach (var myCode in myComponents)
        {
          if (myCode.Code.Coding.FirstOrDefault<Coding>().Code.ToString() == loinc_BloodPressureType)
          {

            SimpleQuantity mySimpleQuantity = (Hl7.Fhir.Model.SimpleQuantity)myCode.Value;
            var bpValue = mySimpleQuantity.Value;

            bloodPressureValue.Value = bpValue;
            bloodPressureValue.IssuedDate = myObservation.Issued.Value.DateTime;
            bloodPressureValue.BloodPressureType = loinc_BloodPressureType;

            valuesList.Add(bloodPressureValue);
          }

          valuesList.Add(bloodPressureValue);
        }
      }
      //Sort by the issued date so the most recent is on the top and then select that one.
      return valuesList.OrderByDescending(a => a.IssuedDate).FirstOrDefault();
    }
  }
}