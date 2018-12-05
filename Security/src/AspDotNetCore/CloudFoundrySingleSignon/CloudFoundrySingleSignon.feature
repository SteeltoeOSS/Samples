@cloud
Feature: CloudFoundry Single SignOn
    In order to show you how to use Steeltoe with CloudFoundry Single SignOn
    You can run some CloudFoundry Single SignOn samples

    @netcoreapp2.1
    @win10-x64
    Scenario: CloudFoundry Single SignOn for .Net Core 2.1 (win10-x64)
        Given you have at least .Net Core SDK 2.1.300 installed
        And you have Java 8 installed
        And you have UAA Client 4 installed
        # build/deploy UAA server
        When you run: git clone https://github.com/cloudfoundry/uaa.git
        And you run: git -C uaa checkout 4.7.1
        And you run: uaa/gradlew -p uaa -Dapp=uaa -Dapp-domain=x.y.z manifests
        And you run in the background: cf push -f uaa/build/sample-manifests/uaa-cf-application.yml
        And you wait until CloudFoundry app uaa is started
        # configure UAA server
        When you run: uaac target https://uaa.x.y.z
        And you run: uaac token client get admin -s adminsecret
        And you run: uaac contexts
        And you run: uaac group add testgroup
        And you run: uaac user add testuser --given_name Test --family_name User --emails testuser@domain.com --password Password1!
        And you run: uaac member add testgroup testuser
        And you run: uaac client add myTestApp --scope cloud_controller.read,cloud_controller_service_permissions.read,openid,testgroup --authorized_grant_types authorization_code,refresh_token --authorities uaa.resource --redirect_uri http://single-signon.x.y.z/signin-cloudfoundry --autoapprove cloud_controller.read,cloud_controller_service_permissions.read,openid,testgroup --secret myTestApp
        And you run: cf cups myOAuthService -p "{\"client_id\": \"myTestApp\", \"client_secret\": \"myTestApp\", \"uri\": \"uaa://uaa.x.y.z\"}"
        # deploy single-signon app
        And you run: dotnet restore
        And you run: dotnet publish -f netcoreapp2.1 -r win10-x64
        And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/netcoreapp2.1/win10-x64/publish
        And you wait until CloudFoundry app single-signon is started
        # Test authentication
        When you get https://single-signon.x.y.z/Home/About
        Then you should be at https://uaa.x.y.z/login
        When you login with "baduser"/"badpass"
        Then you should be at https://uaa.x.y.z/login
        And you should see "Unable to verify email or password. Please try again."
        When you login with "testuser"/"Password1!"
        Then you should be at https://single-signon.x.y.z/Home/About
        And you should see "Your About page."

    @#153028887
    @netcoreapp2.1
    @ubuntu.14.04-x64
    Scenario: CloudFoundry Single SignOn for .Net Core 2.1 (ubuntu.14.04-x64)
        Given you have at least .Net Core SDK 2.1.300 installed
        And you have Java 8 installed
        And you have UAA Client 4 installed
        # build/deploy UAA server
        When you run: git clone https://github.com/cloudfoundry/uaa.git
        And you run: git -C uaa checkout 4.7.1
        And you run: uaa/gradlew -p uaa -Dapp=uaa -Dapp-domain=x.y.z manifests
        And you run in the background: cf push -f uaa/build/sample-manifests/uaa-cf-application.yml
        And you wait until CloudFoundry app uaa is started
        # configure UAA server
        When you run: uaac target https://uaa.x.y.z
        And you run: uaac token client get admin -s adminsecret
        And you run: uaac contexts
        And you run: uaac group add testgroup
        And you run: uaac user add testuser --given_name Test --family_name User --emails testuser@domain.com --password Password1!
        And you run: uaac member add testgroup testuser
        And you run: uaac client add myTestApp --scope cloud_controller.read,cloud_controller_service_permissions.read,openid,testgroup --authorized_grant_types authorization_code,refresh_token --authorities uaa.resource --redirect_uri http://single-signon.x.y.z/signin-cloudfoundry --autoapprove cloud_controller.read,cloud_controller_service_permissions.read,openid,testgroup --secret myTestApp
        And you run: cf cups myOAuthService -p "{\"client_id\": \"myTestApp\", \"client_secret\": \"myTestApp\", \"uri\": \"uaa://uaa.x.y.z\"}"
        # deploy single-signon app
        And you run: dotnet restore
        And you run: dotnet publish -f netcoreapp2.1 -r ubuntu.14.04-x64
        And you run in the background: cf push -f manifest.yml -p bin/Debug/netcoreapp2.1/ubuntu.14.04-x64/publish
        And you wait until CloudFoundry app single-signon is started
        # Test authentication
        When you get https://single-signon.x.y.z/Home/About
        Then you should be at https://uaa.x.y.z/login
        When you login with "baduser"/"badpass"
        Then you should be at https://uaa.x.y.z/login
        And you should see "Unable to verify email or password. Please try again."
        When you login with "testuser"/"Password1!"
        Then you should be at https://single-signon.x.y.z/Home/About
        And you should see "Your About page."

    @net461
    @win10-x64
    Scenario: CloudFoundry Single SignOn for .Net Framework 4.6.1 (win10-x64)
        Given you have at least .Net Core SDK 2.1.300 installed
        And you have Java 8 installed
        And you have UAA Client 4 installed
        # build/deploy UAA server
        When you run: git clone https://github.com/cloudfoundry/uaa.git
        And you run: git -C uaa checkout 4.7.1
        And you run: uaa/gradlew -p uaa -Dapp=uaa -Dapp-domain=x.y.z manifests
        And you run in the background: cf push -f uaa/build/sample-manifests/uaa-cf-application.yml
        And you wait until CloudFoundry app uaa is started
        # configure UAA server
        When you run: uaac target https://uaa.x.y.z
        And you run: uaac token client get admin -s adminsecret
        And you run: uaac contexts
        And you run: uaac group add testgroup
        And you run: uaac user add testuser --given_name Test --family_name User --emails testuser@domain.com --password Password1!
        And you run: uaac member add testgroup testuser
        And you run: uaac client add myTestApp --scope cloud_controller.read,cloud_controller_service_permissions.read,openid,testgroup --authorized_grant_types authorization_code,refresh_token --authorities uaa.resource --redirect_uri http://single-signon.x.y.z/signin-cloudfoundry --autoapprove cloud_controller.read,cloud_controller_service_permissions.read,openid,testgroup --secret myTestApp
        And you run: cf cups myOAuthService -p "{\"client_id\": \"myTestApp\", \"client_secret\": \"myTestApp\", \"uri\": \"uaa://uaa.x.y.z\"}"
        # deploy single-signon app
        And you run: dotnet restore
        And you run: dotnet publish -f net461 -r win10-x64
        And you run in the background: cf push -f manifest-windows.yml -p bin/Debug/net461/win10-x64/publish
        And you wait until CloudFoundry app single-signon is started
        # Test authentication
        When you get https://single-signon.x.y.z/Home/About
        Then you should be at https://uaa.x.y.z/login
        When you login with "baduser"/"badpass"
        Then you should be at https://uaa.x.y.z/login
        And you should see "Unable to verify email or password. Please try again."
        When you login with "testuser"/"Password1!"
        Then you should be at https://single-signon.x.y.z/Home/About
        And you should see "Your About page."
