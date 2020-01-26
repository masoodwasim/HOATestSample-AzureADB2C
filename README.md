# HOATest-AzureAD
ASP.NET Core Web API 3.0 with Azure AD B2 Authentication, Swagger UI.

A sample demonstrating how you can configure your ASP.NET Core 3.0 applications to take advantage of [Azure AD B2C](https://azure.microsoft.com/en-us/services/active-directory-b2c/) and [MSAL](https://github.com/AzureAD/microsoft-authentication-library-for-dotnet) to perform such tasks as:
- Authenticate users
- Protect Web APIs
- Redeem authorization code
- Call a protected Web API


# Configuration

## HOATest.Web - ASP.NET CORE Web Portal

```
"Authentication": {
    "Authentication": {
    "AzureAd": {
      "Instance": "https://datasquaredev.b2clogin.com/",
      "TenantId": "datasquaredev.onmicrosoft.com",
      "ClientId": "591e08c8-1a4d-4f23-bb88-d3164259ed59",
      "ClientSecret": "((I{%IsPa(k4Q9?$GR3#z56N",
      "PostLogoutRedirectUri": "https://localhost:44397/",
      "ApiIdentifier": "https://datasquaredev.onmicrosoft.com/testapi",
      "B2C": {
        "SignInOrSignUpPolicy": "B2C_1_testsignupandsigninpolicy",
        "EditProfilePolicy": "B2C_1_TestProfileEditPolicy",
        "ResetPasswordPolicy": "B2C_1_password-reset"
      }
    }
  },
  "TestServiceOptions": {
    "BaseUrl": "https://localhost:44327/"
  } 
```

## HOATest.API - ASP.NET CORE Web API

```
"Authentication": {
    "Authentication": {
    "AzureAd": {
      "Instance": "https://datasquaredev.b2clogin.com/",
      "TenantId": "datasquaredev.onmicrosoft.com",
      "Audience": "591e08c8-1a4d-4f23-bb88-d3164259ed59",
      "SignInOrSignUpPolicy": "B2C_1_testsignupandsigninpolicy"
    }
  }
```

## HOATest.API -Swagger UI
``` 
[Web API Swagger UI](https://localhost:44327/swagger)
```
