# Sample App for Secure endpoints using BasicAuth

ASP.NET Core sample app illustrating how to secure Steeltoe Management Endpoints with BasicAuth.  

> [!NOTE]
> This is just a sample that demonstrates a custom Actuators security policy.
> Do not secure your endpoints in production using Basic Auth.  

## Pre-requisites

1. Install .NET SDK

## Run the app in Visual Studio

1. Click on the "Actuators" link, you should not get a valid response, since the user doesnt have permissions to view endpoints

2. Use [ActuatorsWithBasicAuth.http](./ActuatorsWithBasicAuth.http) or any other tool  that can pass credentials encoded as a Basic Auth header, such as curl:

    ```shell
    curl -u actuatorUser:actuatorPassword http://localhost:5000/actuator | jq .
    ```

3. You should be able to access the endpoints. 

### See the Official [Steeltoe Management Documentation](https://steeltoe.io/docs/v3/management/) for a more in-depth walk-through of the samples and more detailed information
