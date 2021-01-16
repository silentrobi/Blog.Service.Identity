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
**Clients** are stored in `Config.cs` file.

# Grant Types

**`client_credentials`**: Machine to Machine communication. This is the simplest type of communication. Tokens are always requested on behalf of a client, no interactive user is present.
**`authorization code`**: Interactive Clients. This is the most common type of client scenario: web applications, SPAs or native/mobile apps with interactive users.
**`password`**: Direct user authentication and authorize resources. `username` and `password` along with client credentials are sent to identity server. Identity server response with `access_token` that contains user profile and scopes.  


To learn moredetail about the `grant_type` visit [here](https://identityserver4.readthedocs.io/en/latest/topics/grant_types.html)