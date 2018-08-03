# MArkB.SMARTonFhir

This project is targeted at a .NET implimentation of SMART on FHIR. I've attempted to recreate the Cerner JavaScript smart-on-fhir-tutorial in C# with a few additions. For accessing the FHIR server I'm utilizing the open source fhir-net-api library developed by Ewout Kramer. It's on get here: https://github.com/ewoutkramer/fhir-net-api

I'd like this to be the go-to solution for .Net developers getting started with SMART on FHIR. Please let me know if you have any issues or questions. Report bugs using the Issues tab above and add or request additional documentation as you see fit.

http://engineering.cerner.com/smart-on-fhir-tutorial/

You'll need Microsoft Visual Studio 2017 (I'm not sure on the minimum version) Community should work (Please let me know)

To get started:
Download/Fork the code to your local system. 
Register a new app on the Cerner Code Console: https://code.cerner.com/developer/smart-on-fhir/apps
  You'll want to pretty much match the settings used in in the Cerner tutorial.
- SMART Launch URI: http://localhost:9147/FC_App/Launch
- Redirect URI: http://localhost:9147/FC_App/

Copy the Client ID and the App ID to the web.config
      <fhirTokens
        appClientId="Put your App Client ID here"
        appSecret="Put your App Secret here"
       />

Compile and start the application in VS.

From the Cerner Developer Portal:
- Click the button labeled 'Begin Testing'.
- Select patient "SMART, Fred Rick" and click the 'Next' button.
  Note the login credentials 
    Username: portal
    Password: portal
- Click the 'Launch' button and enter the credentials if required.

The Launch page should open and then continue on to display the results for the Framingham Calculator. 
