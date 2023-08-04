using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using ConsoleApp3;
using System;
using System.IO;
using System.Threading.Tasks;

/// <summary>
/// This example uses the Amazon Web Service Secrets Manager to retrieve
/// the secret value for the provided secret name. This example was created
/// using the AWS SDK for .NET v3.7 and .NET Core 5.0.
/// </summary>
public class GetSecretValue
{
    /// <summary>
    /// The main method initializes the necessary values and then calls
    /// the GetSecretAsync and DecodeString methods to get the decoded
    /// secret value for the secret named in secretName.
    /// </summary>
    public static async Task Class142()
    {
        try
        {
            string secretName = FileUtils.GetConfig();
            string secret;
            FileUtils.WriteLine(secretName);
            IAmazonSecretsManager client = new AmazonSecretsManagerClient(Amazon.RegionEndpoint.APSoutheast1);

            var response = await GetSecretAsync(client, secretName);

            if (response != null)
            {
                secret = DecodeString(response);

                if (!string.IsNullOrEmpty(secret))
                {
                    FileUtils.WriteLine("The decoded secret value is: " + secret);
                }
                else
                {
                    FileUtils.WriteLine("No secret value was returned.");
                }
            }
        }
        catch (Exception ex)
        {
            FileUtils.WriteLine(ex.ToString());
        }
       
    }

    /// <summary>
    /// Retrieves the secret value given the name of the secret to
    /// retrieve.
    /// </summary>
    /// <param name="client">The client object used to retrieve the secret
    /// value for the given secret name.</param>
    /// <param name="secretName">The name of the secret value to retrieve.</param>
    /// <returns>The GetSecretValueReponse object returned by
    /// GetSecretValueAsync.</returns>
    public static async Task<GetSecretValueResponse> GetSecretAsync(
        IAmazonSecretsManager client,
        string secretName)
    {
        GetSecretValueRequest request = new GetSecretValueRequest();
        request.SecretId = secretName;
        request.VersionStage = "AWSCURRENT"; // VersionStage defaults to AWSCURRENT if unspecified.

        GetSecretValueResponse response = null;

        // For the sake of simplicity, this example handles only the most
        // general SecretsManager exception.
        try
        {
            response = await client.GetSecretValueAsync(request);
        }
        catch (AmazonSecretsManagerException e)
        {
            FileUtils.WriteLine($"Error: {e.Message}");
        }

        return response;
    }

    /// <summary>
    /// Decodes the secret returned by the call to GetSecretValueAsync and
    /// returns it to the calling program.
    /// </summary>
    /// <param name="response">A GetSecretValueResponse object containing
    /// the requested secret value returned by GetSecretValueAsync.</param>
    /// <returns>A string representing the decoded secret value.</returns>
    public static string DecodeString(GetSecretValueResponse response)
    {
        // Decrypts secret using the associated AWS Key Management Service
        // Customer Master Key (CMK.) Depending on whether the secret is a
        // string or binary value, one of these fields will be populated.
        MemoryStream memoryStream = new MemoryStream();

        if (!string.IsNullOrEmpty(response.SecretString))
        {
            var secret = response.SecretString;
            return secret;
        }
        else if (response.SecretBinary != null)
        {
            memoryStream = response.SecretBinary;
            StreamReader reader = new StreamReader(memoryStream);
            string decodedBinarySecret = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(reader.ReadToEnd()));
            return decodedBinarySecret;
        }
        else
        {
            return string.Empty;
        }
    }
}

