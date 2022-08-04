using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace LC_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IEntity" in both code and config file together.
    [ServiceContract]
    public interface IEntity
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/entity_list", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetEntityList(ClassRequest.REQ_ENTITYLIST parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/chart_donut", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetChartDonutInfo(ClassRequest.REQ_FARMENTITY parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/chart_line", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetChartLineInfo(ClassRequest.REQ_CHART_LINE parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/chart_line_range", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetChartLineInfoRange(ClassRequest.REQ_CHART_LINE_RANGE parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/chart_color", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetChartColorInfo(ClassRequest.REQ_CHART_COLOR parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/chart_list", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetChartListInfo(ClassRequest.REQ_FARMPAGESEQ parameters);

        //[OperationContract]
        //[WebInvoke(Method = "POST", UriTemplate = "/chart_alarm", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        //ClassResponse.RES_RESULT GetChartAlarmInfo(ClassRequest.REQ_FARMPAGESEQ parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/insert", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT SetInsertEntity(ClassRequest.REQ_ENTITYINSERT parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/update", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT SetUpdateEntity(ClassRequest.REQ_ENTITYUPDATE parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/delete", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT SetDeleteEntity(ClassRequest.REQ_FARMENTITY parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/alarm_list", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetPushAlarmList(ClassRequest.REQ_ALARM_LIST parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/pregnancy", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetEntityPregnancy(ClassRequest.REQ_FARMENTITY parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/history_list", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetEntityHistoryList(ClassRequest.REQ_ENTITYHISTORY parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/entity_info", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetEntityInfo(ClassRequest.REQ_ENTITYINFO parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/favorite_insert", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT SetFavoriteInsert(ClassRequest.REQ_FAVORITE parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/favorite_delete", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT SetFavoriteDelete(ClassRequest.REQ_FAVORITE parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/dashboard", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetDashboardList(ClassRequest.REQ_UID parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/history_info", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetHistoryDetailInfo(ClassRequest.REQ_FARMENTITY parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/statistics", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetEntityStatistics(ClassRequest.REQ_FARMSEQ parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/chart_activity", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetChartActivity(ClassRequest.REQ_CHART_LINE parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/chart_activity_range", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetChartActivityRange(ClassRequest.REQ_CHART_LINE_RANGE parameters);


        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/inner_status", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetInnerActivityStatus(ClassRequest.REQ_FARMENTITY parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/pin_info", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetPinInfo(ClassRequest.REQ_PIN_INFO parameters);


        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/chart_interupt", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetChartInterupt(ClassRequest.REQ_CHART_LINE parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/chart_interupt_range", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetChartInteruptRange(ClassRequest.REQ_CHART_LINE_RANGE parameters);
    }
}
