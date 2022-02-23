using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace LC_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IBasic" in both code and config file together.
    [ServiceContract]
    public interface IBasic
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/db_info", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetDatabaseInfo();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/login", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT SetLogin(ClassRequest.REQ_LOGIN parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/code_list", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetCodeList(ClassRequest.REQ_CODELIST parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/farm_list", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetFarmList(ClassRequest.REQ_FARMLIST parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "upload?filename={fileName}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT UploadImage(string fileName, System.IO.Stream fileContents);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/tag_list", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetTagList(ClassRequest.REQ_FARMSEQ parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/semen_list", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetSemenList(ClassRequest.REQ_FARMSEQ parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/semen_insert", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT SetSemenInsert(ClassRequest.REQ_SEMENNO parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/semen_delete", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT SetSemenDelete(ClassRequest.REQ_SEMENSEQ parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/semen_set", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT SetSemenSetting(ClassRequest.REQ_SEMENSEQ parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/upload_base64", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT UploadeImageFromBase64(ClassRequest.REQ_UPLOADIMAGE parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/version", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT CheckAppVersion(ClassRequest.REQ_VERSION parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/upload_file?filename={fileName}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT UploadFile(string fileName, System.IO.Stream fileContents);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/profile_insert", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT SetUserProfileInsert(ClassRequest.REQ_PROFILE parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/profile_delete", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT SetUserProfileDelete(ClassRequest.REQ_USER_ID parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/chat_list", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetChattingList(ClassRequest.REQ_CHATLIST parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/profile_get", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetProfileImage(ClassRequest.REQ_UID parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/worker_list", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetWorkerList(ClassRequest.REQ_WORKERLIST parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/notice_check", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT CheckExistNewNotice(ClassRequest.REQ_NOTICE_INFO parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/notice_list", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetNoticeList(ClassRequest.REQ_NOTICE_INFO parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/notice_info", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetNoticeDetailInfo(ClassRequest.REQ_NOTICE_DETAIL parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/push_regist", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT SetUserPushSetting(ClassRequest.REQ_PUSH_REGIST parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/push_list", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetUserPushSetting(ClassRequest.REQ_UID parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/useralarm_list", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetUserAlarmList(ClassRequest.REQ_FARMSEQ parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/useralarm_insert", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT SetUserAlarmInsert(ClassRequest.REQ_USER_ALARM_INSERT parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/useralarm_update", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT SetUserAlarmUpdate(ClassRequest.REQ_USER_ALARM_UPDATE parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/useralarm_delete", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT SetUserAlarmDelete(ClassRequest.REQ_USER_ALARM_DELETE parameters);
    }
}
