# Cherry DDNS 1.0
A .NET 7 Worker Service with a simple purpose: updating GoDaddy DNS A Records.

## Windows Installation and Configuration
You may use the CherryDDNS.zip archive on the [latest release page](https://github.com/jingounchained/CherryDDNS/releases/latest) to acquire the self-contained single-file executable and the accompanying config.json file, or you may clone this repo and Publish the application yourself. Unzip or publish the application and make note of the filepath. 

### Edit Configuration
In order to run properly, the Service needs to know what to update. You must configure your DNS Records and API Credentials in config.json. If you do not have an API Key you will need to [create one here](https://developer.godaddy.com/keys). 
Open config.json in your favorite text editor and fill in the json.
**Secret:** the API Secret  
**Key:** the API Key  
**Records:** An Array of  
 - **HostName:** This is the Subdomain you're targeting. You can use @ to target the root domain.  
 -  **DomainName:** The Domain you're targeting.  
   
![image](https://user-images.githubusercontent.com/32217493/234913815-22edb4d3-8761-4fa0-9a1c-ab5efce1cfcd.png)

### Install the Service
I recommend using PowerShell to install. You'll need to run as Administrator in order to register the Service with Windows.
Execute the command `sc.exe create CherryDDNS binpath="<path/to/CherryDDNS.exe"`
Optionally I would add a brief description such as `sc.exe description CherryDDNS "DDNS Service utilizing GoDaddy's API"
![image](https://user-images.githubusercontent.com/32217493/234915789-18a9c7fd-5674-43a2-9a56-edb55494f6b2.png)

If no issues were encountered you should now see the Service in your Services List and should be able to start it.
![image](https://user-images.githubusercontent.com/32217493/234916599-93cb248b-90c9-49c5-a5a9-b42405c7ccb2.png)
