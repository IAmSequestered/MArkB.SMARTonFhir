using System;
using MArkB.SMARTonFhir.Cerner.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;

namespace MAMC.SMARTonFhir.Tests.Utilities
{

  /// <summary>
  /// Series of tests to get all the expected values from the config file.
  /// There should be negative tests, looking for bad values and such, but I didn't do that.
  /// </summary>
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

    string _whichSectionToGet ="";

    #region Initialization

    //[AssemblyInitialize()]
    //  public static void AssemblyInit(TestContext context)
    //{
    //  MessageBox.Show("Assembly Init");
    //}

    //[ClassInitialize()]
    //public static void ClassInit(TestContext context)
    //{
    //  MessageBox.Show("ClassInit");
    //}

    [TestInitialize()]
    public void Initialize()
    {
      _whichSectionToGet = ConfigurationManager.AppSettings["WhichSmartOnFhirConfigurationToUse"];
    }

    //[TestCleanup()]
    //public void Cleanup()
    //{
    //  MessageBox.Show("TestMethodCleanup");
    //}

    //[ClassCleanup()]
    //public static void ClassCleanup()
    //{
    //  MessageBox.Show("ClassCleanup");
    //}

    //[AssemblyCleanup()]
    //public static void AssemblyCleanup()
    //{
    //  MessageBox.Show("AssemblyCleanup");
    //}

    #endregion Initialization

    [TestMethod]
    public void CernerConfigGetGroup01()
    {

      SMARTonFhirConfigurationSection sfconfig =
        (SMARTonFhirConfigurationSection)ConfigurationManager.GetSection(
        "SMARTonFhirConfigurationGroup/SmartOnFhirConfiguration01");

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

      SMARTonFhirConfigurationSection sfconfig =
        (SMARTonFhirConfigurationSection)ConfigurationManager.GetSection(
        "SMARTonFhirConfigurationGroup/SmartOnFhirConfiguration01");

      string myBaseFhirUrl = sfconfig.FhirDataTargets.BaseFhirUrl;

      Assert.AreEqual("https://fhir-ehr.sandboxcerner.com/dstu2", myBaseFhirUrl);

    }

    [TestMethod]
    public void CernerConfigGetFhirVersionFolder01()
    {

      SMARTonFhirConfigurationSection sfconfig =
        (SMARTonFhirConfigurationSection)ConfigurationManager.GetSection(
        "SMARTonFhirConfigurationGroup/SmartOnFhirConfiguration01");

      string myFhirVersionFolder = sfconfig.FhirDataTargets.FHIRVersionFolder;

      Assert.AreEqual("dstu2/", myFhirVersionFolder);

    }

    [TestMethod]
    public void CernerConfigGetServiceIdentifier01()
    {

      SMARTonFhirConfigurationSection sfconfig =
        (SMARTonFhirConfigurationSection)ConfigurationManager.GetSection(
        "SMARTonFhirConfigurationGroup/SmartOnFhirConfiguration01");

      string myServiceIdentifier = sfconfig.FhirDataTargets.ServiceIdentifier;

      Assert.AreEqual(testServiceIdentifier, myServiceIdentifier);

    }

    [TestMethod]
    public void CernerConfigGetAppClientId01()
    {

      SMARTonFhirConfigurationSection sfconfig =
        (SMARTonFhirConfigurationSection)ConfigurationManager.GetSection(
        "SMARTonFhirConfigurationGroup/SmartOnFhirConfiguration01");

      string myAppClientId = sfconfig.FhirTokens.AppClientId;

      Assert.AreEqual(testAppClientId, myAppClientId);

    }

    [TestMethod]
    public void CernerConfigGetAppSecret01()
    {

      SMARTonFhirConfigurationSection sfconfig =
        (SMARTonFhirConfigurationSection)ConfigurationManager.GetSection(
        "SMARTonFhirConfigurationGroup/SmartOnFhirConfiguration01");

      string myAppSecret = sfconfig.FhirTokens.AppSecret;

      Assert.AreEqual(testAppSecret, myAppSecret);

    }

    
    [TestMethod]
    public void CernerConfigGetAppClientId02()
    {

      string configSectionToGet = "SMARTonFhirConfigurationGroup/" + _whichSectionToGet;

      SMARTonFhirConfigurationSection sfconfig =
        (SMARTonFhirConfigurationSection)ConfigurationManager.GetSection(
        configSectionToGet);

      string myAppClientId = sfconfig.FhirTokens.AppClientId;

      Assert.AreEqual(testAppClientId, myAppClientId);

    }
  }
}
