using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowBasedLogParser
{
    public class Parser
    {
        public static string BASE_URL = "/doodlydoo/rv1/";
        public static Dictionary<string, RequestInformation> RequestDictionary = new Dictionary<string, RequestInformation>();

        public Parser(string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    Console.WriteLine(fileName + " : Found");
                    Parse(fileName);
                }
                else
                {
                    Console.WriteLine(fileName + " : Not Found");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception Occured : " + e.Message);
            }
        }

        /// <summary>
        /// This will parse the access.log file and store the required info into RequestDictionary.
        /// </summary>
        /// <param name="fileName"></param>
        private void Parse(string fileName)
        {
            using (StreamReader streamReader = new StreamReader(fileName))
            {
                String line;

                while ((line = streamReader.ReadLine()) != null)
                {
                    if (!line.Equals("") && !line.Equals('\n') && line.Contains(Parser.BASE_URL))
                    {
                        String[] DelimitedString = line.Split('"');

                        String RequestName = GetRequestName(DelimitedString[1].Split(' ')[1]);
                        String StatusCode = DelimitedString[2].Split(' ')[1];

                        String RequestTime = this.GetRequestTime(DelimitedString[2]);


                        if (!Parser.RequestDictionary.ContainsKey(RequestName))
                        {
                            this.AddRequest(RequestName,
                                            DelimitedString[1].Split(' ')[0],
                                            RequestTime,
                                            StatusCode
                                            );
                        }
                        else
                        {
                            this.UpdateRequest(RequestName,
                                                RequestTime,
                                                StatusCode
                                                );
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This will Update already present Request.
        /// </summary>
        /// <param name="requestName"></param>
        /// <param name="requestTime"></param>
        /// <param name="statusCode"></param>
        private void UpdateRequest(string requestName, string newRequestTime, string statusCode)
        {
            (Parser.RequestDictionary[requestName]).RequestCount++;

            if (!String.IsNullOrEmpty(newRequestTime))
            {
                string prevMaxRequestTime = (Parser.RequestDictionary[requestName]).MaxRequestTime;
                string prevMinRequestTime = (Parser.RequestDictionary[requestName]).MinRequestTime;

                //If both Min and Max RequestTime is NULL.
                if (String.IsNullOrEmpty(prevMinRequestTime) && String.IsNullOrEmpty(prevMaxRequestTime))
                {
                    (Parser.RequestDictionary[requestName]).MaxRequestTime = newRequestTime;
                    (Parser.RequestDictionary[requestName]).MinRequestTime = newRequestTime;
                }
                else
                {   // Both not null
                    if (!String.IsNullOrEmpty(prevMinRequestTime) && !String.IsNullOrEmpty(prevMaxRequestTime))
                    {
                        //if newRequestTime is greater than PreviousMaxRequestTime
                        if (Convert.ToDouble(newRequestTime) > Convert.ToDouble(prevMaxRequestTime))
                        {
                            (Parser.RequestDictionary[requestName]).MaxRequestTime = newRequestTime;
                        } //if newRequestTime is smaller than PreviousMinRequestTime
                        else if (Convert.ToDouble(newRequestTime) < Convert.ToDouble(prevMinRequestTime))
                        {
                            (Parser.RequestDictionary[requestName]).MinRequestTime = newRequestTime;
                        }
                    } // if either of it is null
                    else
                    {   // If prevMinRequestTime is null 
                        if (String.IsNullOrEmpty(prevMinRequestTime))
                        {   //if new request is greater than prevMaxRequestTime
                            if (Convert.ToDouble(newRequestTime) > Convert.ToDouble(prevMaxRequestTime))
                            {
                                (Parser.RequestDictionary[requestName]).MaxRequestTime = newRequestTime;
                                (Parser.RequestDictionary[requestName]).MinRequestTime = newRequestTime;
                            }
                            else
                            {
                                (Parser.RequestDictionary[requestName]).MinRequestTime = newRequestTime;
                            }
                        }
                        else
                            // If prevMaxRequestTime is null 
                            if (String.IsNullOrEmpty(prevMaxRequestTime))
                            {   //if new request is smaller than prevMinRequestTime
                                if (Convert.ToDouble(newRequestTime) < Convert.ToDouble(prevMinRequestTime))
                                {
                                    (Parser.RequestDictionary[requestName]).MaxRequestTime = newRequestTime;
                                    (Parser.RequestDictionary[requestName]).MinRequestTime = newRequestTime;
                                }
                                else
                                {
                                    (Parser.RequestDictionary[requestName]).MaxRequestTime = newRequestTime;
                                }
                            }
                    }
                }
            }


            if (statusCode.Equals("200"))
            {
                (Parser.RequestDictionary[requestName]).StatusCount200++;
            }
            else if (statusCode.Equals("500"))
            {
                (Parser.RequestDictionary[requestName]).StatusCount500++;
            }
        }

        /// <summary>
        /// This will check whether RequestTime is present or not and return if Present.
        /// </summary>
        /// <param name="delimitedString"></param>
        /// <returns></returns>
        private string GetRequestTime(string delimitedString)
        {
            if (delimitedString.Contains('[') && delimitedString.Contains(']'))
            {
                return delimitedString.Split(new char[] { '[', ']' })[1];
            }
            return null;
        }

        /// <summary>
        /// This will add Request into RequestDictionary
        /// </summary>
        /// <param name="requestName"></param>
        /// <param name="methodType"></param>
        /// <param name="requestTime"></param>
        /// <param name="statusCode"></param>
        private void AddRequest(string requestName, string methodType, string requestTime, string statusCode)
        {
            Parser.RequestDictionary.Add(requestName, this.GetRequestInfoObject(methodType, requestTime, statusCode));
        }

        /// <summary>
        /// This will return RequestInformation object.
        /// </summary>
        /// <param name="methodType"></param>
        /// <param name="requestTime"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        private RequestInformation GetRequestInfoObject(string methodType, string requestTime, string statusCode)
        {
            RequestInformation requestInformation = new RequestInformation
            {
                MethodType = methodType,
                MinRequestTime = requestTime,
                MaxRequestTime = requestTime
            };

            requestInformation.RequestCount++;

            if (statusCode.Equals("200"))
            {
                requestInformation.StatusCount200++;
            }
            else if (statusCode.Equals("500"))
            {
                requestInformation.StatusCount500++;
            }

            return requestInformation;
        }


        /// <summary>
        /// This will return Request Name
        /// </summary>
        /// <param name="delimitedString"></param>
        /// <returns></returns>
        private string GetRequestName(string delimitedString)
        {
            string requestName = delimitedString.Substring(delimitedString.LastIndexOf('/') + 1);

            if (requestName.Contains('?') || requestName.Contains('-') || requestName.Contains('&'))
            {
                requestName = delimitedString.Substring(0, delimitedString.LastIndexOf('/'));
            }
            else
            {
                requestName = delimitedString;
            }
            return requestName;
        }
    }
}
