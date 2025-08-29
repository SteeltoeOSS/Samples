# Sample App for Secure endpoints

ASP.NET Core sample app illustrating how to secure [Steeltoe Management Endpoints](https://github.com/SteeltoeOSS/Management) with UAA server.  

## Pre-requisites

1. Docker
2. Install .NET Core SDK

## Run UAA server in docker

From the project root folder run:

 ``` shell
 docker run --rm -ti -p 8080:8080 --name steeltoe-uaa -v %cd%/uaa.yml:/uaa/uaa.yml steeltoe.azurecr.io/uaa-server:pr-47
  ```

 This command volume mounts the current directory in Windows Command Line. For Power Shell use `${PWD}`, On Linux use `$(pwd)`

## Run the app in Visual Studio

1. Click on the "Log In" button, which will redirect you to UAA and sign in with username: `fortuneteller` and password: `password` .
2. Click on the "Actuators" link, you should get an "Access Denied" page, since the user doesn't have permissions to view endpoints
3. Click "Log Out", Clear cookies if needed
4. Click on the "Log In" again and this time, use "fortuneadmin", "password"
5. Click on "Actuators" and you should be able to view the endpoint information

### See the Official [Steeltoe Management Documentation](https://steeltoe.io/docs/steeltoe-management) for a more in-depth walk-through of the samples and more detailed information
