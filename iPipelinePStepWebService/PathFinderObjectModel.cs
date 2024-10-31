using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPipelinePStepWebService
{
    public class PathFinderObjectModel
    {
        private int _LISR_RETURN_CODE = -1;
        private string _SERVER_RESPONSE_MESSAGE = string.Empty;

        public PathFinderObjectModel()
        {

        }

        public PathFinderObjectModel(int _LISR_RETURN_CODE, string _SERVER_RESPONSE_MESSAGE) { 
        
            this._LISR_RETURN_CODE = _LISR_RETURN_CODE;
            this._SERVER_RESPONSE_MESSAGE = _SERVER_RESPONSE_MESSAGE;
        }

        public int LISR_RETURN_CODE { get { return _LISR_RETURN_CODE; } }
        public string SERVER_RESPONSE_MESSAGE { get { return this._SERVER_RESPONSE_MESSAGE; } } 
    }

    public class PathFinderResponseModel
    {
        public PathFinderResponseModel() { }
        
        [JsonProperty(PropertyName = "LSIR-RETURN-CD")]
        public int LSIR_RETURN_CD { get; set; }
    }
}
