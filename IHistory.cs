using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace LC_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IHistory" in both code and config file together.
    [ServiceContract]
    public interface IHistory
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/history_list", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetHistoryList(ClassRequest.REQ_HISTORYLIST parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/history_month", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetHistoryMonth(ClassRequest.REQ_HISTORYMONTH parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/history_date", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetHistoryDayList(ClassRequest.REQ_HISTORYDATE parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/estrus", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT SetEstrus(ClassRequest.REQ_ESTRUS parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/inseminate", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT SetInseminate(ClassRequest.REQ_INSEMINNATE parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/appraisal", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT SetAppraisal(ClassRequest.REQ_APPRAISAL parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/dryup", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT SetDryup(ClassRequest.REQ_DRYUP parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/calve", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT SetCalve(ClassRequest.REQ_CALVE parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/month_count", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetHistoryMonthCount(ClassRequest.REQ_HISTORYMONTH parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/get_estrus", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetEstrusInfo(ClassRequest.REQ_BREEDINFO parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/get_inseminate", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetInseminateInfo(ClassRequest.REQ_BREEDINFO parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/get_appraisal", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetAppraisalInfo(ClassRequest.REQ_BREEDINFO parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/get_dryup", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetDryupInfo(ClassRequest.REQ_BREEDINFO parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/get_calve", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetCalveInfo(ClassRequest.REQ_BREEDINFO parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/last_inseminate", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetLastInseminate(ClassRequest.REQ_LAST_INSEMINATE parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/delete", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT SetDeleteHistory(ClassRequest.REQ_BREEDINFO parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/last_appraisal", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetLastAppraisal(ClassRequest.REQ_LAST_INSEMINATE parameters);
}
}
