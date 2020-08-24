# Sample App for Secure endpoints using BasicAuth

ASP.NET Core sample app illustrating how to secure [Steeltoe Management Endpoints](https://github.com/SteeltoeOSS/Management) with BasicAuth.  

Note: This is just a sample that demonstrates securing Actuators using ANC Security framework. Do not secure your endpoints in production using Basic Auth - unless secured by another layer of security.  

## Pre-requisites

1. Install .NET Core SDK

## Run the app in Visual Studio

1. Click on the "Actuators" link, you should not get a valid response, since the user doesnt have permissions to view endpoints

2. Using curl or any tool that can pass credentials encoded as a BasicAuth header: 
``` shell
curl -u actuatorUser:actuatorPassword http://localhost:5000/actuator | jq .

```
3. You should be able to access the endpoints. 


### See the Official [Steeltoe Management Documentation](https://steeltoe.io/docs/steeltoe-management) for a more in-depth walk-through of the samples and more detailed information
