using Katmanli.Core.Response;
using Katmanli.Core.SharedLibrary;
using Katmanli.DataAccess.Connection;
using Katmanli.DataAccess.DTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StackExchange.Redis;

public interface IRedisServer 
{
    void StoreFilePath(string filekey, string filePath);
    string GetFilePath(string fileId);
    public void StartSyncScheduler(TimeSpan interval);
}

public class RedisServer : IRedisServer
{
    private readonly IDatabase _redisDatabase;
    string redisConnectionString = "localhost:6379,abortConnect=false";
    private readonly ConnectionMultiplexer _redisConnection;

    private readonly DatabaseExecutions _databaseExecutions;
    private readonly ParameterList _parameterList;


    public RedisServer(ConnectionMultiplexer redisConnection,ParameterList parameterList,DatabaseExecutions databaseExecutions)
    {
        _redisConnection = ConnectionMultiplexer.Connect(redisConnectionString);
        _redisDatabase = _redisConnection.GetDatabase();
        _databaseExecutions = databaseExecutions;
        _parameterList = parameterList;
    }

    public void StoreFilePath(string filekey, string filePath)
    {
        _redisDatabase.HashSet("filepaths", filekey, filePath);
    }

    public string GetFilePath(string filekey)
    {
        //  böyle olunca çalışıyor ancak redisi belirli zamanla çalıştır.

        //SynchronizeRedisWithDbFiles();
        //StartSyncScheduler(TimeSpan.FromMinutes(1));

        return _redisDatabase.HashGet("filepaths", filekey);
    }

    public string DeleteFileFromRedis(string filekey)
    {
        return _redisDatabase.HashGet("filepaths", filekey);
    }

    public void SynchronizeRedisWithDbFiles()
    {
        var dbFilekeysAndPaths = GetFilesAndFilepathsFromDb();
        var redisFilekeys = _redisDatabase.HashKeys("filepaths");

        // Veritabanında olmayan dosyaları Redis'ten sil
        foreach (var redisFilekey in redisFilekeys)
        {
            if (!dbFilekeysAndPaths.Any(file => file.FileKey == (string)redisFilekey))
            {
                DeleteFileFromRedis((string)redisFilekey);
            }
        }

        // Veritabanındaki dosyaları Redis'e ekle
        foreach (var file in dbFilekeysAndPaths)
        {
            // Redis'te dosya adı zaten varsa, dosyayı tekrar kaydetme
            if (!_redisDatabase.HashExists("filepaths", file.FileKey))
            {
                StoreFilePath(file.FileKey, file.FilePath);
            }
        }
    }

    private List<UploadImagesDTO> GetFilesAndFilepathsFromDb()
    {
        try
        {
            _parameterList.Reset();

            var dbFilekeysResult = _databaseExecutions.ExecuteQuery("Sp_GetFilekeys", _parameterList);

            var dbFilekeys = JsonConvert.DeserializeObject<List<UploadImagesDTO>>(dbFilekeysResult);

            return new List<UploadImagesDTO>(dbFilekeys);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new List<UploadImagesDTO>(); 
        }
    }

    public void StartSyncScheduler(TimeSpan interval)
    {
        var scheduler = new System.Threading.Timer(SyncWithDb, null, TimeSpan.Zero, interval);
    }

    private void SyncWithDb(object state)
    {
        SynchronizeRedisWithDbFiles();
    }



}
