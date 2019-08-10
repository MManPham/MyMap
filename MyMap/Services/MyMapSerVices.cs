using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Channels;
using GetStartAspNet.vbd_services;
using System.Configuration;
using System.ServiceModel;
using AjaxPro;

namespace GetStartAspNet.Services
{
    public class MyMapSerVices
    {
        private readonly string _key = ConfigurationManager.AppSettings["Vbd_Service_Key"] ?? "NOT_VALID_KEY";

        [AjaxMethod]
        public int CountSearch(string searchKey)
        {
            using (var client = new PartnerPortalSoapServiceClient())
            {
                using (new OperationContextScope(client.InnerChannel))
                {
                    // Add a HTTP Header to an outgoing request
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers["RegisterKey"] = _key;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;
                    var res = client.SearchAll(searchKey, 1, 10, 0, 0, 0, 0, false, 1);
                    return res.TotalCount;
                }
            }
        }

        [AjaxMethod]
        public VietBandoPOI[] SearchAll(string searchKey, int page)
        {
            var client = new PartnerPortalSoapServiceClient();

            using (new OperationContextScope(client.InnerChannel))
            {
                // Add a HTTP Header to an outgoing request
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers["RegisterKey"] = _key;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;
                MultipleVietBandoPOI res = new MultipleVietBandoPOI();
                try
                {
                    res = client.SearchAll(searchKey, page, 10, 0, 0, 0, 0, false, 1);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e);
                    return new VietBandoPOI[] { };
                }

                if (res != null && res.List.Length > 0)
                {
                    return res.List;
                }
                client.Close();
                return new VietBandoPOI[] { };

            }
        }

        public DirectionResult FindShortPath(Point[] points, TransportType type)
        {
            using (var client = new PartnerPortalSoapServiceClient())
            {
                using (new OperationContextScope(client.InnerChannel))
                {
                    // Add a HTTP Header to an outgoing request
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers["RegisterKey"] = _key;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;
                    SingleDirectionResult res = client.FindShortestPath(points, type, true);
                    if (res.IsSuccess)
                    {
                        return res.Value;
                    }
                    return new DirectionResult { };
                }
            }
        }

        public FindShortestPathOrderResult FindShortestPathOrderFull(Point[] Points, TransportType type)
        {
            using (var client = new PartnerPortalSoapServiceClient())
            {
                using (new OperationContextScope(client.InnerChannel))
                {
                    // Add a HTTP Header to an outgoing request
                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers["RegisterKey"] = _key;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;
                    
                    var res = client.FindShortestPathOrderFull(Points, type, true);
                    if (res.IsSuccess)
                    {
                        return res.Value;
                    }
                    return new FindShortestPathOrderResult { };
                }
            }
        }
    }
}