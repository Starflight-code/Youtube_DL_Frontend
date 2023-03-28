using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
//using Newtonsoft.Json;

namespace Youtube_DL_Frontend
{
    public class dataStorage
    {
        public DateTimeOffset Date { get; set; }
        public int TemperatureCelsius { get; set; }
        public string? Summary { get; set; }
    }
    public class dataProcess
    {
        string? dir = null;
        string? output = null;
        string? ff = null;
        string? af = null;
        string? aq = null;
        string? auf = null;
        public void dataIn()
        {

        }
        public void dataStore() { //Sanitize current class in JSON
            var dataStorage = new dataStorage();
            string fileName = "data.json";
            using FileStream createStream = File.Create(fileName);

            //JsonConvert.SerializeObject(dataStorage);
            createStream.DisposeAsync();
        }
        public void databaseInitialize() {
            var dataStorage = new dataStorage();
            string fileName = "data.json";
            //string jsonString = JsonConvert.SerializeObject(dataStorage);
            var file = File.Create(fileName);
            Thread.Sleep(250);
            file.Dispose();
            Thread.Sleep(250);
            //File.WriteAllText(fileName, jsonString);
            
        }
    }
}
