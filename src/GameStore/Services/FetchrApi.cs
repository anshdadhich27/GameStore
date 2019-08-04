using GameStore.Models;
using GameStore.Models.Entity;
using HttpHandler;
using System.Threading.Tasks;

namespace GameStore.Services
{
    public class FetchrApi
    {
        private Client client = null;
        private const string BaseUrl = "https://api.fetchr.us/v3/";

        public FetchrApi()
        {
            client = new Client();
        }

        /// <summary>
        /// Get Shipment Status From Fetchr
        /// </summary>
        /// <param name="tracking_Id">Tracking Id</param>
        /// <returns>Shipment Status</returns>
        public async Task<DataResult<ShipmentStatus>> Get_Shipment_Status(string tracking_Id)
        {
            client.ClearHeaders();
            client.AddHeader("Authorization", Constant.FetchrAuthToken);
            return await client.GetData_WithStatusCode_Async<ShipmentStatus>(string.Format("{0}tracking/status/{1}", BaseUrl, tracking_Id));
        }

        /// <summary>
        /// Get Shipment History From Fetchr
        /// </summary>
        /// <param name="tracking_Id">Tracking Id</param>
        /// <returns>Shipment History</returns>
        public async Task<DataResult<ShipmentHistory>> Get_Shipment_History(string tracking_Id)
        {
            client.ClearHeaders();
            client.AddHeader("Authorization", Constant.FetchrAuthToken);
            return await client.GetData_WithStatusCode_Async<ShipmentHistory>(string.Format("{0}tracking/history/{1}", BaseUrl, tracking_Id));
        }


        /// <summary>
        /// Create Order For Shipment
        /// </summary>
        /// <param name="obj">Dorship (Shipment details)</param>
        /// <returns>Shipment Order Response</returns>
        public async Task<DataResult<Shipment_Order_Response>> Create_Order_For_Shipment(Dorship obj)
        {
            client.ClearHeaders();
            client.AddHeader("Authorization", Constant.FetchrAuthToken);
            return await client.PostData_WithStatusCode_Async<Shipment_Order_Response>(string.Format("{0}orders/dropship", BaseUrl), obj);
        }


        /// <summary>
        /// Create Pickup Order
        /// </summary>
        /// <param name="obj">Pickup Order details</param>
        /// <returns>Pickup Response</returns>
        public async Task<DataResult<PickUp_Response>> Create_PickUp_Order(PickUp obj)
        {
            return await client.PostData_WithStatusCode_Async<PickUp_Response>("https://menavip.com/api/reverseorders/", obj);
        }


        /// <summary>
        /// Get Awb Bills
        /// </summary>
        /// <param name="obj">Awb Search options</param>
        /// <returns>Awb Bill</returns>
        public async Task<DataResult<Awb_Bill>> Get_Awb_Bills(Awb obj)
        {
            client.ClearHeaders();
            client.AddHeader("authorization", Constant.FetchrTokenId);
            return await client.PostData_WithStatusCode_Async<Awb_Bill>("https://business.fetchr.us/api/client/awb", obj);
        }

    }
}
