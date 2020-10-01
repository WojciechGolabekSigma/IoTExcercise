using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoTExcercise.Dtos
{
    public class SingleSensorResponseDto
    {
        public string SensorName { get; set; }
        public List<SingleSensorEntry> SensorReadings { get; set; }

    }
}
