This sample application gets, sets and deletes the secret values from Azure Key Vault.

Make sure a secret name is already set, and once set, you can manipulate the secret values.
Secret Name : Name of the secret
Secret Value : Password (e.g.)

Add nuget for the missing dlls, couple of nugets needed for the below namespaces are:

using Azure.Identity;
using Microsoft.Azure.KeyVault;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
