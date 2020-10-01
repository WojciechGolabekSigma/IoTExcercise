using IoTExcercise.DataAccess;
using IoTExcercise.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoTExcercise.Services
{
    public class SensorsService : ISensorsService
    {
        private readonly ISensorsDataRepo sensorsDataRepo;
        public SensorsService(ISensorsDataRepo sensorsDataRepo)
        {
            this.sensorsDataRepo = sensorsDataRepo;
        }

        public async Task<List<SingleSensorResponseDto>> GetAllSensorsDataForDevice(DateTime date, string deviceId)
        {
            var response = new List<SingleSensorResponseDto>();
            var sensorsList = await sensorsDataRepo.GetSensorsTypesForDeviceFromMetadata(deviceId);
            foreach (var sensorType in sensorsList)
            {
                var singleSensorData = await GetSingleSensorData(date, deviceId, sensorType);
                if (singleSensorData == null)
                {
                    continue;
                }
                response.Add(singleSensorData);
            }
            return response;
        }

        public async Task<SingleSensorResponseDto> GetSingleSensorData(DateTime date, string deviceId, string sensor)
        {
            var fileDataArray = await sensorsDataRepo.GetSingleSensorReadings(date, deviceId, sensor);
            if (fileDataArray == null || !fileDataArray.Any())
            {
                return null;
            }
            var entriesList = new List<SingleSensorEntry>();
            for(int i = 0; i < fileDataArray.Length - 1 ; i++) 
            {
                var dataEntryArray = fileDataArray[i].Split(";");
                var dateString = dataEntryArray[0];
                var valueString = dataEntryArray[1];
                DateTime dateTime = Convert.ToDateTime(dateString);

                entriesList.Add(new SingleSensorEntry()
                {
                    Date = dateTime,
                    ValueMeasured = valueString
                }); 
            }
            var responseDto = new SingleSensorResponseDto()
            {
                SensorName = sensor,
                SensorReadings = entriesList
            };
            return responseDto;
        }
    }
}
