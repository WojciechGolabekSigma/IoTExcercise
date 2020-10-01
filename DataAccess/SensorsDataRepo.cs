using IoTExcercise.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace IoTExcercise.DataAccess
{
    public class SensorsDataRepo : ISensorsDataRepo
    {
        private readonly AzureBlobContext blobContext;
        public SensorsDataRepo(AzureBlobContext context)
        {
            blobContext = context;
        }

        public async Task<List<string>> GetSensorsTypesForDeviceFromMetadata(string deviceId)
        {
            var dataFile = "metadata.csv";
            var blobClient = blobContext.BlobContainterClient.GetBlobClient(dataFile);
            var blobDownloadInfo = await blobClient.DownloadAsync();
            if (!blobDownloadInfo.Value.Content.CanRead)
            {
                return null;
            }
            var dataFileStream = blobDownloadInfo.Value.Content;
            var metadata = ReadStreamToStringArray(dataFileStream);
            var validSensors = new List<string>();
            foreach (var sensor in metadata)
            {
                var deviceSensorPair = sensor.Split(";");
                if (deviceSensorPair[0] == deviceId)
                {
                    validSensors.Add(deviceSensorPair[1]);
                }
            }
            return validSensors;
        }

        public async Task<string[]> GetSingleSensorReadings(DateTime date, string deviceId, string sensor)
        {
            var mainDirectoryPath = FilePathsUtils.GenerateDirectoryPath(deviceId, sensor);
            var dataFile = FilePathsUtils.GenerateCsvDateFilePath(mainDirectoryPath, date); //try to fetch file from temporary directory
            bool fetchingFromHistorical = false;
            bool fileExists = await VerifyAzurePathExistance(dataFile);
            if (!fileExists)
            {
                fetchingFromHistorical = true;
                dataFile = FilePathsUtils.GenerateZipFilePath(mainDirectoryPath, "historical");
                fileExists = await VerifyAzurePathExistance(dataFile);
                if (!fileExists) 
                {
                    return null;
                }
            }
            var blobClient = blobContext.BlobContainterClient.GetBlobClient(dataFile);
            var blobDownloadInfo = await blobClient.DownloadAsync();
            if (!blobDownloadInfo.Value.Content.CanRead)
            {
                return null;
            }
            var dataFileStream = blobDownloadInfo.Value.Content;
            if (fetchingFromHistorical)
            {
                var historicalZipFile = new ZipArchive(blobDownloadInfo.Value.Content);
                var zipFileEntryForDay = historicalZipFile.GetEntry(FilePathsUtils.GenerateCsvDateFilePath("", date));
                if (zipFileEntryForDay == null)
                {
                    return null;
                }
                dataFileStream = zipFileEntryForDay.Open();
            }

            var result = ReadStreamToStringArray(dataFileStream);
            return result;
        }

        private async Task<bool> VerifyAzurePathExistance(string path)
        {
            var blobClient = blobContext.BlobContainterClient.GetBlobClient(path);
            return await blobClient.ExistsAsync();
        }

        private string[] ReadStreamToStringArray(Stream dataStream)
        {
            if (dataStream.CanRead)
            {
                StreamReader reader = new StreamReader(dataStream);
                var lines = reader.ReadToEnd();
                var linesArray = lines.Split("\r\n");
                return linesArray;
            }
            return null;
        }



    }
}
