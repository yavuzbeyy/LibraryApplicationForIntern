using Confluent.Kafka;
using Katmanli.Core.Response;
using Katmanli.Core.SharedLibrary;
using Katmanli.DataAccess.Connection;
using Katmanli.DataAccess.DTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

public interface IRedisServer 
{
    void StoreFilePath(string filekey, string filePath);
    string GetFilePath(string fileId);
    void StartSyncScheduler(TimeSpan interval);
    void SynchronizeRedisWithDbFiles();
    void SyncWithDb(object state);
}

public class RedisServer : IRedisServer
{
    private readonly IDatabase _redisDatabase;
    string redisConnectionString = "localhost:6379,abortConnect=false";
    private readonly ConnectionMultiplexer _redisConnection;

    private readonly DatabaseExecutions _databaseExecutions;
    private readonly ParameterList _parameterList;

    public RedisServer(ConnectionMultiplexer redisConnection, ParameterList parameterList, DatabaseExecutions databaseExecutions)
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
        return _redisDatabase.HashGet("filepaths", filekey);
    }

    public string DeleteFileFromRedis(string filekey)
    {
        // silme işlemine çevir
        return _redisDatabase.HashGet("filepaths", filekey);
    }

    public void SynchronizeRedisWithDbFiles()
    {
        try
        {
            var dbFilekeysAndPaths = GetFilesAndFilepathsFromDb();
            var redisFilekeys = _redisDatabase.HashKeys("filepaths");

            // Veritabanındaki dosyaları Redis'e ekle
            foreach (var file in dbFilekeysAndPaths)
            {
                // Redis'te dosya adı zaten varsa, dosyayı tekrar kaydetme
                if (!_redisDatabase.HashExists("filepaths", file.FileKey))
                {
                    StoreFilePath(file.FileKey, file.FilePath);
                }
            }

            // Veritabanında olmayan dosyaları Redis'ten sil
            foreach (var redisFilekey in redisFilekeys)
            {
                if (!dbFilekeysAndPaths.Any(file => file.FileKey == (string)redisFilekey))
                {
                    DeleteFileFromRedis((string)redisFilekey);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        //  new System.Threading.Timer(SyncWithDb, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
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

    public void SyncWithDb(object state)
    {
        SynchronizeRedisWithDbFiles();
    }
 
}
