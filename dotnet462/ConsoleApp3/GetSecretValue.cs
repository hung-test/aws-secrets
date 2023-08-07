using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using ConsoleApp3;
using System;
using System.IO;
using System.Threading.Tasks;

public class GetSecretValue
{
    public static async Task Class142()
    {
        try
        {
            string secretName = FileUtils.GetConfig();
            FileUtils.WriteLine(secretName);
            string region = "ap-southeast-1";
            IAmazonSecretsManager client = new AmazonSecretsManagerClient(Amazon.RegionEndpoint.GetBySystemName(region));

            GetSecretValueRequest request = new GetSecretValueRequest
            {
                SecretId = secretName,
                VersionStage = "AWSCURRENT", // VersionStage defaults to AWSCURRENT if unspecified.
            };

            GetSecretValueResponse response;

            try
            {
                response = client.GetSecretValueAsync(request).Result;
            }
            catch (Exception e)
            {
                // For a list of the exceptions thrown, see
                // https://docs.aws.amazon.com/secretsmanager/latest/apireference/API_GetSecretValue.html
                throw e;
            }

            string secret = response.SecretString;
            FileUtils.WriteLine("The decoded secret value is: " + secret);
        }
        catch (Exception ex)
        {
            FileUtils.WriteLine(ex.ToString());
        }
       
    }
   
}

