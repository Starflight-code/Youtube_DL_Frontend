using Newtonsoft.Json;

namespace Youtube_DL_Frontnend {
    internal class DatabaseObject {
        public void updateSelf() {
           JsonConvert.SerializeObject(this);
           
        }
    }}