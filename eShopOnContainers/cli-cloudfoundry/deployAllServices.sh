#!/bin/sh

./createServices.sh
../src/Services/Identity/Identity.API/PushCloudFoundry.sh
../src/Services/Location/Location.API/PushCloudFoundry.sh
../src/Services/Marketing/Marketing.API/PushCloudFoundry.sh
../src/Services/Ordering/Ordering.API/PushCloudFoundry.sh
../src/Services/Ordering/Ordering.BackgroundTasks/PushCloudFoundry.sh
../src/Services/Payment/Payment.API/PushCloudFoundry.sh

