# Identity4 Server 

## Project Description

## Authenticate Client Application
To authenticate client application (mobile, web app etc.), use following endpoints.

**Request token** 

_Identity4 server verifies the client and return access token._

**URL** : `endpoint/connect/token`

**Method** : `POST`

**Auth required** : NO

**Content-Type** : `application/x-www-form-urlencoded`

**Request body required fields**: _client_id, client_secret, scope, and grant_type_  

**Example**
  ```
  curl --location --request POST 'https://localhost:5001/connect/token' \
--header 'Content-Type: application/x-www-form-urlencoded' \
--data-urlencode 'client_id=postman' \
--data-urlencode 'client_secret=secret' \
--data-urlencode 'scope=api1' \
--data-urlencode 'grant_type=client_credentials' \
--data-urlencode 'response_type=id_token token'
```