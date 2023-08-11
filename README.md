# RapidPay

appsettings.tmpl files are provided for the API and the IDP project.

RapidPay\RapidPay.API\appsettings.json

- Make sure that the ` "IDP": { "URI": "https://localhost:5001" }` is pointing correctly to the IDP server (usualy is port 5001)
- Make sure the connection string points to an SQL Server DB

RapidPay\RapidPay.IDP\appsettings.json:

- Make sure that the

  `"Clients": {
	"Swagger": {
  		"ClientSecret": "secret",
  		"SwaggerURI": "https://localhost:7131",
  		"RedirectUri": "https://localhost:7131/swagger/oauth2-redirect.html"
	}
} `
  is pointing correctly to the API and specify a secret of your choice

# Running

Configure both API and IDP projects to run at the same.

For Swagger The Client is APISwagger and the secret is the one that is configured in the appsettings.json of the IDP project.

Credentials for the IDP: alice/alice - bob/bob
