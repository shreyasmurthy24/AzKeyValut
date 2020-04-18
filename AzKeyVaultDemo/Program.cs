using Microsoft.Azure.KeyVault;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace AzKeyVaultDemo
{
    class Program
    {
        const string APP_CLIENT_ID = "CLIENT_ID";
        const string APP_CLIENT_SECRET = "CLIENT_SECRET";
        const string KEYVAULT_BASE_URI = "https://KEYVALUT_NAME.vault.azure.net/"; //Base uri

        static KeyVaultClient kvc = null;

        static void Main(string[] args)
        {
            CreateSecret();
            GetSecretAsyncVal();
            DeleteSecretAsyncVal();
            Console.WriteLine();
            Console.ReadKey();
        }

        public static async Task keyvaultGenerate()
        {
            var kvc = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(
            async (string authority, string resource, string scope) =>
            {
                var authContext = new Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationContext(authority);
                var credential = new ClientCredential(APP_CLIENT_ID, APP_CLIENT_SECRET);
                AuthenticationResult result = await authContext.AcquireTokenAsync(resource, credential);
                if (result == null)
                {
                    throw new InvalidOperationException("Failed to retrieve Secret Key");
                }
                return result.AccessToken;
            }
            ));
        }

        public static void CreateSecret()
        {
            keyvaultGenerate().GetAwaiter().GetResult();

            string secretName = "Name of the secret";
            string keyVaultName = Environment.GetEnvironmentVariable("KEY_VAULT_NAME");

            //Setting/Creating a Secret
            var client = new SecretClient(new Uri(KEYVAULT_BASE_URI), new DefaultAzureCredential());
            Console.Write("Input the value of your secret > ");
            string secretValue = Console.ReadLine();
            Console.Write("Creating a secret..");
            client.SetSecret(secretName, secretValue);
            Console.WriteLine("Done.");
        }

        public static async void GetSecretAsyncVal()
        {
            //Get
            keyvaultGenerate().GetAwaiter().GetResult();
            string secretName = "Name of the secret";
            var secretValue = await kvc.GetSecretAsync(KEYVAULT_BASE_URI, secretName);
            Console.WriteLine("The secret value : " + secretValue);

        }

        public static async void DeleteSecretAsyncVal()
        {
            //Delete
            keyvaultGenerate().GetAwaiter().GetResult();
            string secretName = "Name of the secret";
            var secretValue = await kvc.DeleteSecretAsync(KEYVAULT_BASE_URI, secretName);
            Console.WriteLine("Secret value deleted.");
        }
    }
}
