using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace LC_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IBreed" in both code and config file together.
    [ServiceContract]
    public interface IBreed
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/breed_month", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetBreedMonth(ClassRequest.REQ_HISTORYMONTH parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/breed_date", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetBreedDayList(ClassRequest.REQ_HISTORYDATE parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/breed_default", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetBreedDefaultInfo(ClassRequest.REQ_FARMSEQ parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/dueday_list", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetDueDayList(ClassRequest.REQ_FARMSEQ parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/month_count", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetBreedMonthCount(ClassRequest.REQ_HISTORYMONTH parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/dueday_info", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetDueDayInfo(ClassRequest.REQ_FARMSEQ parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/calvedue_update", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT SetCalveDueDay(ClassRequest.REQ_CALVE_DUE_DAY parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/estrusdue_update", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT SetEstrusDueDay(ClassRequest.REQ_ESTRUS_DUE_DAY parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/dueday_update", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT SetDueDayUpdate(ClassRequest.REQ_DUEDAY_UPDATE parameters);
    }
}
