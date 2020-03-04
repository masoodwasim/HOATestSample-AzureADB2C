# HOATest-AzureAD
ASP.NET Core Web API 3.0 with Azure AD B2 Authentication, Swagger UI.

A sample test web application built in ASP.NET Core 3.0 to perform such tasks as:
- Authenticate users with Azure AD B2C( Single Sign In with Email, Facebook & Github)
- Protect Web APIs
- Redeem authorization code
- Call a protected Web API from Web Application


# Configuration

## HOATest.Web - ASP.NET CORE Web Portal

```
"Authentication": {
    "Authentication": {
    "AzureAd": {
      "Instance": "",
      "TenantId": "",
      "ClientId": "",
      "ClientSecret": "",
      "PostLogoutRedirectUri": "https://localhost:44397/",
      "ApiIdentifier": "",
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
      "Instance": "",
      "TenantId": "",
      "Audience": "",
      "SignInOrSignUpPolicy": ""
    }
  }
```

## HOATest.API -Swagger UI
```
Web API Swagger UI - https://localhost:44327/swagger
```

