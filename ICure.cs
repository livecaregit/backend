using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace LC_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICure" in both code and config file together.
    [ServiceContract]
    public interface ICure
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/diagnosis_list", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetDiagnoisList(ClassRequest.REQ_FARMSEQ parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/diagnosis_insert", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT SetDiagnosisInsert(ClassRequest.REQ_DIAGNOSIS_INFO parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/diagnosis_update", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT SetDiagnosisUpdate(ClassRequest.REQ_DIAGNOSIS_INFO parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/diagnosis_delete", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT SetDiagnosisDelete(ClassRequest.REQ_DIAGNOSIS_INFO parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/prescription_list", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetPrescriptionList(ClassRequest.REQ_FARMSEQ parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/prescription_insert", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT SetPrescriptionInsert(ClassRequest.REQ_PRESCRIPTION_INFO parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/prescription_update", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT SetPrescriptionUpdate(ClassRequest.REQ_PRESCRIPTION_INFO parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/prescription_delete", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT SetPrescriptionDelete(ClassRequest.REQ_PRESCRIPTION_INFO parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/cure_list", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetCureList(ClassRequest.REQ_FARMENTITY parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/cure_info", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT GetCureInfo(ClassRequest.REQ_CURE_INFO parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/cure_insert", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT SetCureInsert(ClassRequest.REQ_CURE_INSERT parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/cure_update", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT SetCureUpdate(ClassRequest.REQ_CURE_UPDATE parameters);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/cure_delete", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ClassResponse.RES_RESULT SetCureDelete(ClassRequest.REQ_CURE_INFO parameters);
    }
}
