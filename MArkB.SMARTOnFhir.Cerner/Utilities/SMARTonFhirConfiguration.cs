using System;
using System.Configuration;


namespace MArkB.SMARTonFhir.Cerner.Utilities
{
  /// <summary>
  /// Let's us put all of the Cerner SMART on FHIR configuration information into a group 
  /// instead of at the Application level for easier management of the settings.
  /// </summary>
  /// <see cref="https://msdn.microsoft.com/en-us/library/2tw134k3.aspx"/>
  public class SMARTonFhirConfigurationSection : ConfigurationSection
  {
    // Create a "requiresAuthentication" attribute.
    [ConfigurationProperty("requiresAuthentication", DefaultValue = "false", IsRequired = false)]
    public Boolean RequiresAuthentication
    {
      get
      {
        return (Boolean)this["requiresAuthentication"];
      }
      set
      {
        this["requiresAuthentication"] = value;
      }
    }
    // Create a "FhirDataTarget Element"
    [ConfigurationProperty("fhirDataTargets")]
    public FhirDataTargetElement FhirDataTargets
    {
      get
      {
        return (FhirDataTargetElement)this["fhirDataTargets"];
      }
      set
      { this["fhirDataTargets"] = value; }
    }


    // Create a "FhirToken Element"
    [ConfigurationProperty("fhirTokens")]
    public FhirTokenElement FhirTokens
    {
      get
      {
        return (FhirTokenElement)this["fhirTokens"];
      }
      set
      { this["fhirTokens"] = value; }
    }
  }


  // Define the "fhirDataTarget" element
  // with "BaseFhirUrl", "FHIRVersionFolder" and "ServiceIdentifier" attributes.
  public class FhirDataTargetElement : ConfigurationElement
  {
    [ConfigurationProperty("baseFhirUrl", DefaultValue = "https://fhir-ehr.sandboxcerner.com", IsRequired = true)]
    [StringValidator(MinLength = 1, MaxLength = 400)]
    public String BaseFhirUrl
    {
      get
      {
        return (String)this["baseFhirUrl"];
      }
      set
      {
        this["baseFhirUrl"] = value;
      }
    }

    [ConfigurationProperty("fhirVersionFolder", DefaultValue = "dstu2/", IsRequired = true)]
    [StringValidator(MinLength = 1, MaxLength = 400)]
    public String FHIRVersionFolder
    {
      get
      {
        return (String)this["fhirVersionFolder"];
      }
      set
      {
        this["fhirVersionFolder"] = value;
      }
    }

    [ConfigurationProperty("serviceIdentifier", DefaultValue = "0b8a0111-e8e6-4c26-a91c-5069cbc6b1ca", IsRequired = true)]
    [StringValidator(MinLength = 1, MaxLength = 400)]
    public String ServiceIdentifier
    {
      get
      {
        return (String)this["serviceIdentifier"];
      }
      set
      {
        this["serviceIdentifier"] = value;
      }
    }
  }


  // Define the "fhirDataTarget" element
  // with "BaseFhirUrl", "FHIRVersionFolder" and "ServiceIdentifier" attributes.
  public class FhirTokenElement : ConfigurationElement
  {
    [ConfigurationProperty("appClientId", DefaultValue = "Cow", IsRequired = true)]
    [StringValidator(MinLength = 1, MaxLength = 400)]
    public String AppClientId
    {
      get
      {
        return (String)this["appClientId"];
      }
      set
      {
        this["appClientId"] = value;
      }
    }

    [ConfigurationProperty("appSecret", DefaultValue = "Chicken", IsRequired = true)]
    [StringValidator(MinLength = 1, MaxLength = 400)]
    public String AppSecret
    {
      get
      {
        return (String)this["appSecret"];
      }
      set
      {
        this["appSecret"] = value;
      }
    }
  }
}
