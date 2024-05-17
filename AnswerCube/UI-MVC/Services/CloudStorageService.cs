using System.Security.Cryptography;
using Google.Apis.Auth.OAuth2;

namespace AnswerCube.UI.MVC.Services;

using Google.Cloud.Storage.V1;

public class CloudStorageService
{
    public readonly bool hasCredential;
    private readonly string _projectId;
    private readonly string _bucketName;
    private readonly GoogleCredential _credential;
    private readonly string _jsonAuthFile;    

    /// <summary>
    /// Check the documentation:
    ///   https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-8.0
    /// </summary>
    public CloudStorageService(IConfiguration configuration)
    {
        _projectId = configuration["GCLOUD_PROJECT"];
        //_projectId = "Integratie1-Deployment";
        _bucketName = configuration["GOOGLE_STORAGE_BUCKET"];
        //_bucketName = "answer-cube-bucket";
        _jsonAuthFile = configuration["GOOGLE_APPLICATION_CREDENTIALS"];
        if (File.Exists(_jsonAuthFile))
        {
            _credential= GoogleCredential.FromServiceAccountCredential(ServiceAccountCredential.FromServiceAccountData(File.OpenRead(_jsonAuthFile)));
            hasCredential = true;
        }
        else
        {
            hasCredential = false;
        }
    }
    /// <summary>
    /// Check the sample code from Google:
    ///   https://cloud.google.com/dotnet/docs/reference/Google.Cloud.Storage.V1/latest#sample-code
    /// </summary>
    /// 
    public string UploadFileToBucket(IFormFile formFile)
    {
        using var memoryStream = new MemoryStream();
        formFile.CopyTo(memoryStream);
        var objectName =FileNameGenerator(formFile.ContentType);
        var client = StorageClient.Create(_credential);
        var storageObject = client.UploadObject(_bucketName, objectName,
            formFile.ContentType,
            memoryStream);
        return $"https://storage.googleapis.com/{_bucketName}/{objectName}";
    }
    
    private string FileNameGenerator(string input)
    {
        DateTime now = DateTime.Now;
        string replacement = "_" + now.ToString("yyyyMMddHHmmssffff") + ".";
        int slashIndex = input.IndexOf('/');
        if (slashIndex > 0 && slashIndex < input.Length - 1)
        {
            return input.Substring(0, slashIndex) + replacement + input.Substring(slashIndex + 1);
        }
        return input;
    }
}