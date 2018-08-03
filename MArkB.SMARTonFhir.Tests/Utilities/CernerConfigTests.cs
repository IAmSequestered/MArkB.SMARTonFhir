using System;
using MArkB.SMARTonFhir.Cerner.Utilities;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;

namespace MArkB.SMARTonFhir.Tests.Utilities
{
  [TestClass]
  public class CernerConfigTests
  {
    //This guid is the real service identifier for the Cerner public sandbox
    string testServiceIdentifier = "0b8a0111-e8e6-4c26-a91c-5069cbc6b1ca";
    //This is a guid I generated using https://www.guidgenerator.com/online-guid-generator.aspx
    //It has no meaning and is simply used for these unit tests.
    //cea41836-544e-4d49-b4fb-ba0c64b47f8e
    string testAppClientId = "cea41836-544e-4d49-b4fb-ba0c64b47f8e";
    string testAppSecret = "ad29f362-9c17-4b28-a07e-20733cb892a3";

    // serviceIdentifier="0b8a0111-e8e6-4c26-a91c-5069cbc6b1ca"

    [TestMethod]
    public void CernerConfigGetGroup01()
    {

      SMARTonFhirConfigurationSection tempSfconfig = new SMARTonFhirConfigurationSection();

      SMARTonFhirConfigurationSection sfconfig =
        (SMARTonFhirConfigurationSection)ConfigurationManager.GetSection(
        "SMARTonFhirConfigurationGroup/SmartOnFhirConfiguration");

      string myBaseFhirUrl = sfconfig.FhirDataTargets.BaseFhirUrl;

      //Count of parent level items.
      Assert.AreEqual(3, sfconfig.ElementInformation.Properties.Count);
      //Count of items under FhirDataTargets.
      Assert.AreEqual(3, sfconfig.FhirDataTargets.ElementInformation.Properties.Count);
      //Count of items under FhirTokens.
      Assert.AreEqual(2, sfconfig.FhirTokens.ElementInformation.Properties.Count);

    }

    [TestMethod]
    public void CernerConfigGetBaseFhirUrl01()
    {

      SMARTonFhirConfigurationSection tempSfconfig = new SMARTonFhirConfigurationSection();

      SMARTonFhirConfigurationSection sfconfig =
        (SMARTonFhirConfigurationSection)ConfigurationManager.GetSection(
        "SMARTonFhirConfigurationGroup/SmartOnFhirConfiguration");

      string myBaseFhirUrl = sfconfig.FhirDataTargets.BaseFhirUrl;

      Assert.AreEqual("https://fhir-ehr.sandboxcerner.com/dstu2", myBaseFhirUrl);

    }

    [TestMethod]
    public void CernerConfigGetFhirVersionFolder01()
    {

      SMARTonFhirConfigurationSection tempSfconfig = new SMARTonFhirConfigurationSection();

      SMARTonFhirConfigurationSection sfconfig =
        (SMARTonFhirConfigurationSection)ConfigurationManager.GetSection(
        "SMARTonFhirConfigurationGroup/SmartOnFhirConfiguration");

      string myFhirVersionFolder = sfconfig.FhirDataTargets.FHIRVersionFolder;

      Assert.AreEqual("dstu2/", myFhirVersionFolder);

    }

    [TestMethod]
    public void CernerConfigGetServiceIdentifier01()
    {

      SMARTonFhirConfigurationSection tempSfconfig = new SMARTonFhirConfigurationSection();

      SMARTonFhirConfigurationSection sfconfig =
        (SMARTonFhirConfigurationSection)ConfigurationManager.GetSection(
        "SMARTonFhirConfigurationGroup/SmartOnFhirConfiguration");

      string myServiceIdentifier = sfconfig.FhirDataTargets.ServiceIdentifier;

      Assert.AreEqual(testServiceIdentifier, myServiceIdentifier);

    }

    [TestMethod]
    public void CernerConfigGetAppClientId01()
    {

      SMARTonFhirConfigurationSection tempSfconfig = new SMARTonFhirConfigurationSection();

      SMARTonFhirConfigurationSection sfconfig =
        (SMARTonFhirConfigurationSection)ConfigurationManager.GetSection(
        "SMARTonFhirConfigurationGroup/SmartOnFhirConfiguration");

      string myAppClientId = sfconfig.FhirTokens.AppClientId;

      Assert.AreEqual(testAppClientId, myAppClientId);

    }

    [TestMethod]
    public void CernerConfigGetAppSecret01()
    {

      SMARTonFhirConfigurationSection tempSfconfig = new SMARTonFhirConfigurationSection();

      SMARTonFhirConfigurationSection sfconfig =
        (SMARTonFhirConfigurationSection)ConfigurationManager.GetSection(
        "SMARTonFhirConfigurationGroup/SmartOnFhirConfiguration");

      string myAppSecret = sfconfig.FhirTokens.AppSecret;

      Assert.AreEqual(testAppSecret, myAppSecret);

    }
  }
}
