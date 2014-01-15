using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowBasedLogParser
{
    /// <summary>
    /// This class contains required Information of a Request.
    /// </summary>
    public class RequestInformation
    {
        /// <summary>
        /// Maximum Time taken by a request to complete
        /// </summary>
        private string _maxRequestTime = null;
        public string MaxRequestTime
        {
            get
            {
                if(_maxRequestTime == null)
                {
                    return "";
                }
                return _maxRequestTime;
            }
            set
            {
                if(value != null)
                {
                    _maxRequestTime = value;
                }
            }
        }
        
        /// <summary>
        /// Minimum Time taken by a request to complete
        /// </summary>
        private string _minRequestTime = null;
        public string MinRequestTime
        { 
            get
            {
                if (_minRequestTime == null)
                {
                    return "";
                }
                return _minRequestTime;
            }
            set
            {
                if (value != null)
                {
                    _minRequestTime = value;
                }
            }
        }


        /// <summary>
        /// Request hit count.
        /// </summary>
        private int _requestCount = 0;
        public int RequestCount
        {
            get
            {
                return _requestCount;
            }
            set
            {
                _requestCount = value;
            }
        }

        /// <summary>
        /// Count of Request Status : 200 ie. OK
        /// </summary>
        private int _statusCount200 = 0;
        public int StatusCount200
        {
            get
            {
                return _statusCount200;
            }
            set
            {
                _statusCount200 = value;
            }
        }

        /// <summary>
        /// Count of Request Status : 500 ie. ERROR
        /// </summary>
        private int _statusCount500 = 0;
        public int StatusCount500
        {
            get
            {
                return _statusCount500;
            }
            set
            {
                _statusCount500 = value;
            }
        }

        /// <summary>
        /// Request Type ie. GET, POST etc.
        /// </summary>
        private string _methodType;
        public string MethodType
        {
            get
            {
                return _methodType;
            }
            set
            {
                _methodType = value;
            }
        }
    }
}
