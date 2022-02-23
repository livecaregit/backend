using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LC_Service
{
    public class ClassLORA
    {
        public enum HEADER_METHOD { HTTP_GET, HTTP_POST, HTTP_PUT, HTTP_DELETE };
        public enum HEADER_ACCEPT { RESPONSE_XML, RESPONSE_JSON };

        public struct API_INFO
        {
            public string URL;
            public string USER_KEY;
        }

        public struct HTTP_RESPONSE
        {
            public List<KeyValuePair<string, string>> HEADERS;
            public string BODY;
        }

        public struct LORA_LAST_DATA
        {
            public string entity_no;
            public string tag_id;
            public string tag_serial;
            public string dev_eui;
            public string data_type;
            public string last_date;
            public int interval;
            public int rssi;
            public int snr;
            public double battery;
            public string temp;
            public string sensorX;
            public string sensorY;
            public string sensorZ;
            public int hall_sensor;
            public string is_error;
        }

        public struct THINGPLUG_USER_RESPONSE
        {
            public string user_id;
            public string result_code;
            public string result_msg;
            public string total_list_count;
            public string m_url;
            public string topic_code;
            public string topic_id;
            public string topic_name;
            public string admin_yn;
            public string device_uri;
            public string m_uri;
            public string p_uri;
            public string d_access_token;
            public string m_access_token;
            public string p_access_token;
            public string assertion;
            public string token_signature;
            public string expire_time;
            public string device_model_name;
            public string u_key;
            public string d_key;
            public string device_id;
            public string att_device_id;
            public string device_api_id;
            public string auth_yn;
            public string commonDeviceVOList;
            public string commonDeviceVo;
            public string deviceSearchVO;
            public string topicSearchList;
            public string modelSearchList;
            public string device_collectionLists;
            public string device_mapLists;
            public string deviceCategoriesList;
            public string deviceSearchAPIList;
            public string topicCategoryList;
            public string device_in_topic;
            public string topic_search_list;
            public string appsApiVO;
            public string appsApiVOList;
            public string firmwareApiVO;
            public string firmwareUpgradeVOList;
            public string deviceAppUpgradeVOList;
            public string topicApiVO;
            public THINGPLUG_USER_INFO userVO;
            public string deviceUsageInfoVO;
        }

        public struct THINGPLUG_USER_INFO
        {
            public string user_id;
            public string uKey;
            public string password;
            public string admin_yn;
        }

        public struct THINGPLUG_DEVICE_RESPONSE
        {
            public string user_id;
            public string result_code;
            public string result_msg;
            public string total_list_count;
            public string m_url;
            public string topic_code;
            public string topic_id;
            public string topic_name;
            public string admin_yn;
            public string device_uri;
            public string m_uri;
            public string p_uri;
            public string d_access_token;
            public string m_access_token;
            public string p_access_token;
            public string assertion;
            public string token_signature;
            public string expire_time;
            public string device_model_name;
            public string u_key;
            public string d_key;
            public string device_id;
            public string att_device_id;
            public string device_api_id;
            public string auth_yn;
            public string commonDeviceVOList;
            public string commonDeviceVo;
            public string deviceSearchVO;
            public string topicSearchList;
            public string modelSearchList;
            public string device_collectionLists;
            public string device_mapLists;
            public string deviceCategoriesList;
            public List<THINGPLUG_DEVICE_INFO> deviceSearchAPIList;
            public string topicCategoryList;
            public string device_in_topic;
            public string topic_search_list;
            public string appsApiVO;
            public string appsApiVOList;
            public string firmwareApiVO;
            public string firmwareUpgradeVOList;
            public string deviceAppUpgradeVOList;
            public string topicApiVO;
            public string userVO;
            public string deviceUsageInfoVO;
        }

        public struct THINGPLUG_DEVICE_INFO
        {
            public string device_Id;
            public string device_Name;
            public string device_type;
            public string model_Type;
            public string model_Name;
            public string manufacturer_Name;
            public string category_Id;
            public string category_Name;
            public string location_Alt;
            public string location_Lon;
            public string location_Addr;
            public string status;
            public string fault_Yn;
            public string fault_Code;
            public string alive_Yn;
            public string public_Yn;
            public string discovery_Yn;
            public string owner_Id;
            public string category_Img_Link;
            public string regi_Date;
        }

        public struct THINGPLUG_INTERVAL_RESPONSE
        {
            public THINGPLUG_CONTROL_INFO mgc;
        }

        public struct THINGPLUG_RETRIEVE_RESPONSE
        {
            public THINGPLUG_COMMAND_INFO exin;
        }

        public struct THINGPLUG_CONTROL_INFO
        {
            public string cmt;
            public bool exe;
            public string ext;
            public List<THINGPLUG_COMMAND_INFO> exin;
            public int ty;
            public string ri;
            public string rn;
            public string pi;
            public string ct;
            public string lt;
        }

        public struct THINGPLUG_COMMAND_INFO
        {
            public int exs;
            public string ext;
            public string exra;
            public string ty;
            public string ri;
            public string rn;
            public string pi;
            public string ct;
            public string lt;
            public string et;
        }

        public struct THINGPLUG_TEMP_RESPONSE
        {
            public THINGPLUG_TEMP_INFO cin;
        }

        public struct THINGPLUG_TEMP_INFO
        {
            public string st;
            public string cr;
            public string cnf;
            public string cs;
            public string con;
            public string ty;
            public string ri;
            public string rn;
            public string pi;
            public string ct;
            public string lt;
            public string et;
        }

        public struct THINGPLUG_SUBSCRIPTION_RESPONSE
        {
            public THINGPLUG_SUBSCRIPTION_INFO sub;
        }

        public struct THINGPLUG_SUBSCRIPTION_INFO
        {
            public THINGPLUG_RSS_INFO enc;
            public List<string> nu;
            public int nct;
            public int ty;
            public string ri;
            public string rn;
            public string pi;
            public string ct;
            public string lt;
        }

        public struct THINGPLUG_RSS_INFO
        {
            public List<int> rss;
        }

        public struct CONTELA_DEVICE_RESPONSE
        {
            public string appEUI;
            public List<CONTELA_DEVICE_INFO> instance;
        }

        public struct CONTELA_DEVICE_INFO
        {
            public string devEUI;
            public int joinState;
            public int cseType;
        }

        public struct CONTELA_TEMP_RESPONSE
        {
            public string appEUI;
            public string devEUI;
            public List<CONTELA_TEMP_INFO> instance;
        }

        public struct CONTELA_TEMP_INFO
        {
            public string snr;
            public string content;
            public int payloadType;
            public double contentSize;
            public string createdTime;
            public int fPort;
            public string instanceId;
            public int rssi;
            public string gwLoc;
        }

        public struct CONTELA_SUBSCRIBE_RESPONSE
        {
            public List<CONTELA_SUBSCRIBE_INFO> instance;
        }

        public struct CONTELA_SUBSCRIBE_INFO
        {
            public string pushURI;
            public string pushSubsId;
            public string createdTime;
            public string devEUI;
            public int containerName;
            public string expiredTime;
            public int subsType;
            public string appEUI;
        }

        public struct CONTELA_SUBSCRIBE_REG_RESPONSE
        {
            public string appEUI;
            public string devEUI;
            public int containerName;
            public string pushURI;
            public string pushSubsId;
        }

        public struct CONTELA_SUBSCRIBE_REG_APP_RESPONSE
        {
            public string appEUI;
            public string pushURI;
            public string pushSubsId;
        }

        public struct CONTELA_SUBSCRIBE_DEL_RESPONSE
        {
            public string pushSubsId;
        }

        public struct CONTELA_COMMAND_RESPONSE
        {
            public string appEUI;
            public string devEUI;
            public List<CONTELA_COMMAND_INFO> instance;
        }

        public struct CONTELA_COMMAND_INFO
        {
            public string cmdDesc;
            public string cmdName;
        }

        public struct CONTELA_INTERVAL_RESPONSE
        {
            public string appEUI;
            public string devEUI;
            public string mgmtCmdName;
            public string execInstanceId;
            public int cmdFPort;
        }
    }
}