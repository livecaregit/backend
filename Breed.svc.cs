using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LC_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Breed" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Breed.svc or Breed.svc.cs at the Solution Explorer and start debugging.
    public class Breed : IBreed
    {
        private ClassOLEDB _mClassDatabase = new ClassOLEDB();
        private readonly ClassError _mClassError = new ClassError();
        //private readonly ClassFunction _mClassFunction = new ClassFunction();

        ~Breed()
        {
            if (_mClassDatabase.GetConnectionState()) _mClassDatabase.CloseDatabase();
            _mClassDatabase = null;
        }

        public ClassResponse.RES_RESULT GetBreedMonth(ClassRequest.REQ_HISTORYMONTH parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Breed", "breed_month");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetBreedMonth  ==========", sModuleName);
            ClassLog._mLogger.InfoFormat("{0}  RECEIVE REQUEST DATA  [{1}]", sModuleName, requestData);

            ClassResponse.RES_RESULT response = new ClassResponse.RES_RESULT();

            #region Check Parameter Process
            try
            {
                if (string.IsNullOrEmpty(parameters.lang_code))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_LANG_CODE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_LANG_CODE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (Array.IndexOf(ClassStruct.LANGUAGE_CODE, parameters.lang_code) < 0)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_LANG_CODE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_LANG_CODE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (parameters.farm_seq < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_FARM_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_FARM_SEQ),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (parameters.search_year < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_SEARCH_YEAR,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_SEARCH_YEAR),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (parameters.search_month < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_SEARCH_MONTH,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_SEARCH_MONTH),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }
            }
            catch
            {
                response = new ClassResponse.RES_RESULT
                {
                    result = ClassError.RESULT_SYSTEM_ERROR_EXCEPTION,
                    message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_EXCEPTION),
                    data = string.Empty
                };

                ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                return response;
            }
            #endregion

            #region Check Database Process
            if (!_mClassDatabase.GetConnectionState())
            {
                if (!_mClassDatabase.OpenDatabase())
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }
            }
            #endregion

            #region Business Logic Process
            bool isError = false;
            OleDbDataReader dataReader = null; 

            List<ClassStruct.ST_BREEDMONTH> historyList = new List<ClassStruct.ST_BREEDMONTH>();

            Dictionary<string, string> estrusList = new Dictionary<string, string>();
            Dictionary<string, string> inseminateList = new Dictionary<string, string>();
            Dictionary<string, string> appraisalList = new Dictionary<string, string>();
            Dictionary<string, string> dryupList = new Dictionary<string, string>();
            Dictionary<string, string> calveList = new Dictionary<string, string>();

            try
            {
                int nLastDay = DateTime.DaysInMonth(parameters.search_year, parameters.search_month);

                string sFrom = string.Format("{0}-{1:D2}-01 00:00:00", parameters.search_year, parameters.search_month);
                string sTo = string.Format("{0}-{1:D2}-{2:D2} 23:59:59", parameters.search_year, parameters.search_month, nLastDay);

                string sQuery = string.Format("SELECT DISTINCT BREED_TYPE, BREED_DATE " +
                           "  FROM (SELECT A.BREED_TYPE, CONVERT(CHAR(10), A.BREED_DATE, 120) AS BREED_DATE " +
                           "          FROM BREED_HISTORY A " +
                           "          LEFT OUTER JOIN ENTITY_NEW_INFO B " +
                           "            ON A.ENTITY_SEQ = B.SEQ " +
                           "         WHERE A.FARM_SEQ = {0} " +
                           "           AND A.BREED_DATE >= '{1}' " +
                           "           AND A.BREED_DATE <= '{2}' " +
                           "           AND A.FLAG = 'Y'  " +
                           "           AND B.FLAG = 'Y' " +
                           "           AND B.ACTIVE_FLAG = 'Y' " +
                           "        ) A ", parameters.farm_seq, sFrom, sTo);
                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            string sKey = dataReader.GetString(1);

                            switch (dataReader.GetString(0))
                            {
                                case "E": if (!estrusList.ContainsKey(sKey)) estrusList.Add(sKey, "Y"); break;
                                case "I": if (!inseminateList.ContainsKey(sKey)) inseminateList.Add(sKey, "Y"); break;
                                case "A": if (!appraisalList.ContainsKey(sKey)) appraisalList.Add(sKey, "Y"); break;
                                case "D": if (!dryupList.ContainsKey(sKey)) dryupList.Add(sKey, "Y"); break;
                                case "C": if (!calveList.ContainsKey(sKey)) calveList.Add(sKey, "Y"); break;
                            }
                        }
                    }

                    dataReader.Close();
                    dataReader.Dispose();
                }
                else
                {
                    isError = true;

                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
                        data = string.Empty
                    };
                }

                if (!isError)
                {
                    for (int i = 1; i <= nLastDay; i++)
                    {
                        string sDate = string.Format("{0}-{1:D2}-{2:D2}", parameters.search_year, parameters.search_month, i);

                        ClassStruct.ST_BREEDMONTH historyMonth = new ClassStruct.ST_BREEDMONTH
                        {
                            history_date = sDate,
                            ed_flag = estrusList.ContainsKey(sDate) ? "Y" : "N",
                            id_flag = inseminateList.ContainsKey(sDate) ? "Y" : "N",
                            ad_flag = appraisalList.ContainsKey(sDate) ? "Y" : "N",
                            dd_flag = dryupList.ContainsKey(sDate) ? "Y" : "N",
                            cd_flag = calveList.ContainsKey(sDate) ? "Y" : "N"
                        };

                        historyList.Add(historyMonth);
                    }

                    if (!isError)
                    {
                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SUCCESS,
                            message = string.Empty,
                            data = JsonConvert.SerializeObject(historyList)
                        };
                    }
                }
            }
            catch
            {
                response = new ClassResponse.RES_RESULT
                {
                    result = ClassError.RESULT_SYSTEM_ERROR_EXCEPTION,
                    message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_EXCEPTION),
                    data = string.Empty
                };
            }
            finally
            {
                _mClassDatabase.CloseDatabase();
            }

            ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
            return response;
            #endregion
        }

        public ClassResponse.RES_RESULT GetBreedDayList(ClassRequest.REQ_HISTORYDATE parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Breed", "breed_date");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetBreedDayList  ==========", sModuleName);
            ClassLog._mLogger.InfoFormat("{0}  RECEIVE REQUEST DATA  [{1}]", sModuleName, requestData);

            ClassResponse.RES_RESULT response = new ClassResponse.RES_RESULT();

            #region Check Parameter Process
            try
            {
                if (string.IsNullOrEmpty(parameters.lang_code))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_LANG_CODE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_LANG_CODE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (Array.IndexOf(ClassStruct.LANGUAGE_CODE, parameters.lang_code) < 0)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_LANG_CODE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_LANG_CODE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (parameters.farm_seq < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_FARM_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_FARM_SEQ),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (string.IsNullOrEmpty(parameters.search_date))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_SEARCH_DATE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_SEARCH_DATE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }
            }
            catch
            {
                response = new ClassResponse.RES_RESULT
                {
                    result = ClassError.RESULT_SYSTEM_ERROR_EXCEPTION,
                    message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_EXCEPTION),
                    data = string.Empty
                };

                ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                return response;
            }
            #endregion

            #region Check Database Process
            if (!_mClassDatabase.GetConnectionState())
            {
                if (!_mClassDatabase.OpenDatabase())
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }
            }
            #endregion

            #region Business Logic Process
            OleDbDataReader dataReader = null;

            ClassStruct.ST_BREEDDAYLIST historyList = new ClassStruct.ST_BREEDDAYLIST();

            ClassStruct.ST_BREEDDAYINFO estrusInfo = new ClassStruct.ST_BREEDDAYINFO();
            ClassStruct.ST_BREEDDAYINFO inseminateInfo = new ClassStruct.ST_BREEDDAYINFO();
            ClassStruct.ST_BREEDDAYINFO appraisalInfo = new ClassStruct.ST_BREEDDAYINFO();
            ClassStruct.ST_BREEDDAYINFO dryupInfo = new ClassStruct.ST_BREEDDAYINFO();
            ClassStruct.ST_BREEDDAYINFO calveInfo = new ClassStruct.ST_BREEDDAYINFO();

            List<ClassStruct.ST_BREEDDAYENTITY> estrusList = new List<ClassStruct.ST_BREEDDAYENTITY>();
            List<ClassStruct.ST_BREEDDAYENTITY> inseminateList = new List<ClassStruct.ST_BREEDDAYENTITY>();
            List<ClassStruct.ST_BREEDDAYENTITY> appraisalList = new List<ClassStruct.ST_BREEDDAYENTITY>();
            List<ClassStruct.ST_BREEDDAYENTITY> dryupList = new List<ClassStruct.ST_BREEDDAYENTITY>();
            List<ClassStruct.ST_BREEDDAYENTITY> calveList = new List<ClassStruct.ST_BREEDDAYENTITY>();

            try
            {
                switch (parameters.lang_code)
                {
                    case "KR":
                        {
                            estrusInfo.breed_code = "ED";
                            estrusInfo.breed_title = "발정현황";

                            inseminateInfo.breed_code = "ID";
                            inseminateInfo.breed_title = "수정현황";

                            appraisalInfo.breed_code = "AD";
                            appraisalInfo.breed_title = "임신감정현황";

                            dryupInfo.breed_code = "DD";
                            dryupInfo.breed_title = "건유현황";

                            calveInfo.breed_code = "CD";
                            calveInfo.breed_title = "분만현황";

                            break;
                        }
                    case "JP":
                        {
                            estrusInfo.breed_code = "ED";
                            estrusInfo.breed_title = "発情現況";

                            inseminateInfo.breed_code = "ID";
                            inseminateInfo.breed_title = "授精現況";

                            appraisalInfo.breed_code = "AD";
                            appraisalInfo.breed_title = "妊娠鑑定現況";

                            dryupInfo.breed_code = "DD";
                            dryupInfo.breed_title = "乾乳現況";

                            calveInfo.breed_code = "CD";
                            calveInfo.breed_title = "分娩現況";

                            break;
                        }
                    case "US":
                        {
                            estrusInfo.breed_code = "ED";
                            estrusInfo.breed_title = "Estrus record";

                            inseminateInfo.breed_code = "ID";
                            inseminateInfo.breed_title = "Insemination record";

                            appraisalInfo.breed_code = "AD";
                            appraisalInfo.breed_title = "Diagnosis record";

                            dryupInfo.breed_code = "DD";
                            dryupInfo.breed_title = "Dry up record";

                            calveInfo.breed_code = "CD";
                            calveInfo.breed_title = "Calving record";

                            break;
                        }
                    case "CN":
                        {
                            estrusInfo.breed_code = "ED";
                            estrusInfo.breed_title = "发情现状";

                            inseminateInfo.breed_code = "ID";
                            inseminateInfo.breed_title = "授精现状";

                            appraisalInfo.breed_code = "AD";
                            appraisalInfo.breed_title = "鉴定现状";

                            dryupInfo.breed_code = "DD";
                            dryupInfo.breed_title = "干奶现状";

                            calveInfo.breed_code = "CD";
                            calveInfo.breed_title = "分娩现状";

                            break;
                        }
                    case "PT":
                        {
                            estrusInfo.breed_code = "ED";
                            estrusInfo.breed_title = "Histórico de Estro";

                            inseminateInfo.breed_code = "ID";
                            inseminateInfo.breed_title = "Histórico de Inseminação";

                            appraisalInfo.breed_code = "AD";
                            appraisalInfo.breed_title = "Histórico de Diagnóstico";

                            dryupInfo.breed_code = "DD";
                            dryupInfo.breed_title = "Histórico de Secagem";

                            calveInfo.breed_code = "CD";
                            calveInfo.breed_title = "Histórico de Parto";

                            break;
                        }
                    case "BR":
                        {
                            estrusInfo.breed_code = "ED";
                            estrusInfo.breed_title = "Histórico de Estro";

                            inseminateInfo.breed_code = "ID";
                            inseminateInfo.breed_title = "Histórico de Inseminação";

                            appraisalInfo.breed_code = "AD";
                            appraisalInfo.breed_title = "Histórico de Diagnóstico";

                            dryupInfo.breed_code = "DD";
                            dryupInfo.breed_title = "Histórico de Secagem";

                            calveInfo.breed_code = "CD";
                            calveInfo.breed_title = "Histórico de Parto";

                            break;
                        }
                    default:
                        {
                            estrusInfo.breed_code = "ED";
                            estrusInfo.breed_title = "Estrus record";

                            inseminateInfo.breed_code = "ID";
                            inseminateInfo.breed_title = "Insemination record";

                            appraisalInfo.breed_code = "AD";
                            appraisalInfo.breed_title = "Diagnosis record";

                            dryupInfo.breed_code = "DD";
                            dryupInfo.breed_title = "Dry up record";

                            calveInfo.breed_code = "CD";
                            calveInfo.breed_title = "Calving record";

                            break;
                        }
                }

                string sQuery;
                switch (parameters.lang_code)
                {
                    case "KR":
                        sQuery = "SELECT A.BREED_TYPE, A.FARM_SEQ, A.ENTITY_SEQ, A.BREED_DUE_DATE, A.BREED_METHOD, A.BREED_TEXT_VALUE, A.ADDED_DUE_DATE1, " +
                                 "       B.ENTITY_NO, B.IMAGE_URL, C.SEMEN_NO, D.CODE_NAME ";
                        break;
                    case "JP":
                        sQuery = "SELECT A.BREED_TYPE, A.FARM_SEQ, A.ENTITY_SEQ, A.BREED_DUE_DATE, A.BREED_METHOD, A.BREED_TEXT_VALUE, A.ADDED_DUE_DATE1, " +
                                 "       B.ENTITY_NO, B.IMAGE_URL, C.SEMEN_NO, D.JP_VALUE ";
                        break;
                    case "US":
                        sQuery = "SELECT A.BREED_TYPE, A.FARM_SEQ, A.ENTITY_SEQ, A.BREED_DUE_DATE, A.BREED_METHOD, A.BREED_TEXT_VALUE, A.ADDED_DUE_DATE1, " +
                                 "       B.ENTITY_NO, B.IMAGE_URL, C.SEMEN_NO, D.EN_VALUE ";
                        break;
                    case "CN":
                        sQuery = "SELECT A.BREED_TYPE, A.FARM_SEQ, A.ENTITY_SEQ, A.BREED_DUE_DATE, A.BREED_METHOD, A.BREED_TEXT_VALUE, A.ADDED_DUE_DATE1, " +
                                 "       B.ENTITY_NO, B.IMAGE_URL, C.SEMEN_NO, D.ZH_VALUE ";
                        break;
                    case "PT":
                        sQuery = "SELECT A.BREED_TYPE, A.FARM_SEQ, A.ENTITY_SEQ, A.BREED_DUE_DATE, A.BREED_METHOD, A.BREED_TEXT_VALUE, A.ADDED_DUE_DATE1, " +
                                 "       B.ENTITY_NO, B.IMAGE_URL, C.SEMEN_NO, D.PT_VALUE ";
                        break;
                    case "BR":
                        sQuery = "SELECT A.BREED_TYPE, A.FARM_SEQ, A.ENTITY_SEQ, A.BREED_DUE_DATE, A.BREED_METHOD, A.BREED_TEXT_VALUE, A.ADDED_DUE_DATE1, " +
                                 "       B.ENTITY_NO, B.IMAGE_URL, C.SEMEN_NO, D.PT_VALUE ";
                        break;
                    default:
                        sQuery = "SELECT A.BREED_TYPE, A.FARM_SEQ, A.ENTITY_SEQ, A.BREED_DUE_DATE, A.BREED_METHOD, A.BREED_TEXT_VALUE, A.ADDED_DUE_DATE1, " +
                                 "       B.ENTITY_NO, B.IMAGE_URL, C.SEMEN_NO, D.EN_VALUE ";
                        break;
                }

                sQuery += string.Format("  FROM BREED_HISTORY A " +
                                        "  LEFT OUTER JOIN ENTITY_NEW_INFO B " +
                                        "    ON A.ENTITY_SEQ = B.SEQ " +
                                        "   AND B.FLAG = 'Y' " +
                                        "   AND B.ACTIVE_FLAG = 'Y' " +
                                        "  LEFT OUTER JOIN FARM_SEMEN C " +
                                        "    ON A.BREED_TYPE = 'I' " +
                                        "   AND A.BREED_INT_VALUE2 = C.SEQ " +
                                        "   AND C.FLAG = 'Y' " +
                                        "  LEFT OUTER JOIN CODE_MST D " +
                                        "    ON A.BREED_TYPE = 'C' " +
                                        "   AND A.BREED_METHOD = D.CODE_NO " +
                                        "   AND D.CODE_DIV = '160' " +
                                        "   AND D.FLAG = 'Y' " +
                                        " WHERE A.FARM_SEQ = {0} " +
                                        "   AND CONVERT(varchar(10), A.BREED_DATE, 120) = '{1}' " +
                                        "   AND A.FLAG = 'Y' " +
                                        "   AND B.FLAG = 'Y' " +
                                        "   AND B.ACTIVE_FLAG = 'Y' " , parameters.farm_seq, parameters.search_date);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            string sBreedCode = dataReader.GetString(0);
                            ClassStruct.ST_BREEDDAYENTITY breedInfo = new ClassStruct.ST_BREEDDAYENTITY();

                            if (sBreedCode == "E")
                            {
                                breedInfo.entity_seq = dataReader.GetInt32(2);
                                breedInfo.entity_id = _mClassDatabase.GetSafeString(dataReader, 7);
                                breedInfo.image_url = _mClassDatabase.GetSafeString(dataReader, 8);
                                breedInfo.pregnancy_flag = string.Empty;
                                if (dataReader.IsDBNull(6)) breedInfo.inseminate_due_date = string.Empty;
                                else breedInfo.inseminate_due_date = dataReader.GetDateTime(6).ToString("yyyy-MM-dd");
                                breedInfo.semen_no = string.Empty;
                                breedInfo.calve_due_date = string.Empty;
                                if (dataReader.IsDBNull(3)) breedInfo.estrus_next_date = string.Empty;
                                else breedInfo.estrus_next_date = dataReader.GetDateTime(3).ToString("yyyy-MM-dd");
                                breedInfo.calve_flag = string.Empty;
                                breedInfo.calve_code_disp = string.Empty;

                                estrusList.Add(breedInfo);
                            }
                            else if (sBreedCode == "I")
                            {
                                breedInfo.entity_seq = dataReader.GetInt32(2);
                                breedInfo.entity_id = _mClassDatabase.GetSafeString(dataReader, 7);
                                breedInfo.image_url = _mClassDatabase.GetSafeString(dataReader, 8);
                                breedInfo.pregnancy_flag = string.Empty;
                                breedInfo.inseminate_due_date = string.Empty;
                                breedInfo.semen_no = _mClassDatabase.GetSafeString(dataReader, 9);
                                breedInfo.calve_due_date = string.Empty;
                                breedInfo.estrus_next_date = string.Empty;
                                breedInfo.calve_flag = string.Empty;
                                breedInfo.calve_code_disp = string.Empty;

                                inseminateList.Add(breedInfo);
                            }
                            else if (sBreedCode == "A")
                            {
                                breedInfo.entity_seq = dataReader.GetInt32(2);
                                breedInfo.entity_id = _mClassDatabase.GetSafeString(dataReader, 7);
                                breedInfo.image_url = _mClassDatabase.GetSafeString(dataReader, 8);
                                breedInfo.pregnancy_flag = _mClassDatabase.GetSafeString(dataReader, 5);
                                breedInfo.inseminate_due_date = string.Empty;
                                breedInfo.semen_no = string.Empty;
                                if (dataReader.IsDBNull(6)) breedInfo.calve_due_date = string.Empty;
                                else breedInfo.calve_due_date = dataReader.GetDateTime(6).ToString("yyyy-MM-dd");
                                breedInfo.estrus_next_date = string.Empty;
                                breedInfo.calve_flag = string.Empty;
                                breedInfo.calve_code_disp = string.Empty;

                                appraisalList.Add(breedInfo);
                            }
                            else if (sBreedCode == "D")
                            {
                                breedInfo.entity_seq = dataReader.GetInt32(2);
                                breedInfo.entity_id = _mClassDatabase.GetSafeString(dataReader, 7);
                                breedInfo.image_url = _mClassDatabase.GetSafeString(dataReader, 8);
                                breedInfo.pregnancy_flag = _mClassDatabase.GetSafeString(dataReader, 5);
                                breedInfo.inseminate_due_date = string.Empty;
                                breedInfo.semen_no = string.Empty;
                                if (dataReader.IsDBNull(6)) breedInfo.calve_due_date = string.Empty;
                                else breedInfo.calve_due_date = dataReader.GetDateTime(6).ToString("yyyy-MM-dd");
                                breedInfo.estrus_next_date = string.Empty;
                                breedInfo.calve_flag = string.Empty;
                                breedInfo.calve_code_disp = string.Empty;

                                dryupList.Add(breedInfo);
                            }
                            else if (sBreedCode == "C")
                            {
                                breedInfo.entity_seq = dataReader.GetInt32(2);
                                breedInfo.entity_id = _mClassDatabase.GetSafeString(dataReader, 7);
                                breedInfo.image_url = _mClassDatabase.GetSafeString(dataReader, 8);
                                breedInfo.pregnancy_flag = string.Empty;
                                breedInfo.inseminate_due_date = string.Empty;
                                breedInfo.semen_no = string.Empty;
                                breedInfo.calve_due_date = string.Empty;
                                if (dataReader.IsDBNull(3)) breedInfo.estrus_next_date = string.Empty;
                                else breedInfo.estrus_next_date = dataReader.GetDateTime(3).ToString("yyyy-MM-dd");
                                breedInfo.calve_flag = _mClassDatabase.GetSafeString(dataReader, 5);
                                breedInfo.calve_code_disp = _mClassDatabase.GetSafeString(dataReader, 10);

                                calveList.Add(breedInfo);
                            }
                        }

                        estrusInfo.entity_list = estrusList;
                        inseminateInfo.entity_list = inseminateList;
                        appraisalInfo.entity_list = appraisalList;
                        dryupInfo.entity_list = dryupList;
                        calveInfo.entity_list = calveList;

                        historyList.estrus_info = estrusInfo;
                        historyList.inseminate_info = inseminateInfo;
                        historyList.appraisal_info = appraisalInfo;
                        historyList.dryup_info = dryupInfo;
                        historyList.calve_info = calveInfo;

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SUCCESS,
                            message = string.Empty,
                            data = JsonConvert.SerializeObject(historyList)
                        };
                    }
                    else
                    {
                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SEARCH_NOTEXIST_SCHEDULE,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SEARCH_NOTEXIST_SCHEDULE),
                            data = string.Empty
                        };
                    }

                    dataReader.Close();
                    dataReader.Dispose();
                }
                else
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
                        data = string.Empty
                    };
                }
            }
            catch
            {
                response = new ClassResponse.RES_RESULT
                {
                    result = ClassError.RESULT_SYSTEM_ERROR_EXCEPTION,
                    message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_EXCEPTION),
                    data = string.Empty
                };
            }
            finally
            {
                _mClassDatabase.CloseDatabase();
            }

            ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
            return response;
            #endregion
        }

        public ClassResponse.RES_RESULT GetBreedDefaultInfo(ClassRequest.REQ_FARMSEQ parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Breed", "breed_default");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetBreedDefaultInfo  ==========", sModuleName);
            ClassLog._mLogger.InfoFormat("{0}  RECEIVE REQUEST DATA  [{1}]", sModuleName, requestData);

            ClassResponse.RES_RESULT response = new ClassResponse.RES_RESULT();

            #region Check Parameter Process
            try
            {
                if (string.IsNullOrEmpty(parameters.lang_code))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_LANG_CODE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_LANG_CODE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (Array.IndexOf(ClassStruct.LANGUAGE_CODE, parameters.lang_code) < 0)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_LANG_CODE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_LANG_CODE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (parameters.farm_seq < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_FARM_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_FARM_SEQ),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }
            }
            catch
            {
                response = new ClassResponse.RES_RESULT
                {
                    result = ClassError.RESULT_SYSTEM_ERROR_EXCEPTION,
                    message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_EXCEPTION),
                    data = string.Empty
                };

                ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                return response;
            }
            #endregion

            #region Check Database Process
            if (!_mClassDatabase.GetConnectionState())
            {
                if (!_mClassDatabase.OpenDatabase())
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }
            }
            #endregion

            #region Business Logic Process
            OleDbDataReader dataReader = null;
            ClassStruct.ST_BREEDDEFAULT breedDefault = new ClassStruct.ST_BREEDDEFAULT();

            try
            {
                // 개체번호 리스트를 먼저 추출한다
                string sQuery = "SELECT A.SEQ, A.FARM_SEQ, A.SEMEN_SEQ, A.INSEMINATE_CODE , A.APPRAISAL_CODE, A.DRYUP_METHOD, A.CALVE_METHOD, " +
                            "       B.INSEMINATE_DAY, B.CALVE_M_MONTH, B.CALVE_M_DAY, B.CALVE_D_DAY, B.DRYUP_M_MONTH, B.DRYUP_M_DAY, B.DRYUP_D_DAY,  ";

                switch (parameters.lang_code)
                {
                    case "KR": sQuery += "       C.APPRAISAL_DAY, D.ESTRUS_B_DAY, D.ESTRUS_N_DAY, E.CODE_NAME AS INSEMINATE_DISP, F.CODE_NAME AS APPRAISAL_DISP, G.SEMEN_NO "; break;
                    case "JP": sQuery += "       C.APPRAISAL_DAY, D.ESTRUS_B_DAY, D.ESTRUS_N_DAY, E.JP_VALUE AS INSEMINATE_DISP, F.JP_VALUE AS APPRAISAL_DISP, G.SEMEN_NO "; break;
                    case "US": sQuery += "       C.APPRAISAL_DAY, D.ESTRUS_B_DAY, D.ESTRUS_N_DAY, E.EN_VALUE AS INSEMINATE_DISP, F.EN_VALUE AS APPRAISAL_DISP, G.SEMEN_NO "; break;
                    case "CN": sQuery += "       C.APPRAISAL_DAY, D.ESTRUS_B_DAY, D.ESTRUS_N_DAY, E.ZH_VALUE AS INSEMINATE_DISP, F.ZH_VALUE AS APPRAISAL_DISP, G.SEMEN_NO "; break;
                    case "PT": sQuery += "       C.APPRAISAL_DAY, D.ESTRUS_B_DAY, D.ESTRUS_N_DAY, E.PT_VALUE AS INSEMINATE_DISP, F.PT_VALUE AS APPRAISAL_DISP, G.SEMEN_NO "; break;
                    case "BR": sQuery += "       C.APPRAISAL_DAY, D.ESTRUS_B_DAY, D.ESTRUS_N_DAY, E.PT_VALUE AS INSEMINATE_DISP, F.PT_VALUE AS APPRAISAL_DISP, G.SEMEN_NO "; break;
                    default: sQuery += "       C.APPRAISAL_DAY, D.ESTRUS_B_DAY, D.ESTRUS_N_DAY, E.EN_VALUE AS INSEMINATE_DISP, F.EN_VALUE AS APPRAISAL_DISP, G.SEMEN_NO "; break;
                }

                sQuery += string.Format("  FROM BREED_DEFAULT A " +
                                        "  LEFT OUTER JOIN BREED_INSEMINATE B " +
                                        "    ON A.FARM_SEQ = B.FARM_SEQ " +
                                        "   AND A.INSEMINATE_CODE = B.INSEMINATE_CODE " +
                                        "   AND B.FLAG = 'Y' " +
                                        "  LEFT OUTER JOIN BREED_APPRAISAL C " +
                                        "    ON A.FARM_SEQ = C.FARM_SEQ " +
                                        "   AND A.APPRAISAL_CODE = C.APPRAISAL_CODE " +
                                        "   AND C.FLAG = 'Y' " +
                                        "  LEFT OUTER JOIN BREED_ESTRUS D " +
                                        "    ON A.FARM_SEQ = D.FARM_SEQ " +
                                        "   AND D.FLAG = 'Y' " +
                                        "  LEFT OUTER JOIN CODE_MST E " +
                                        "    ON A.INSEMINATE_CODE = E.CODE_NO " +
                                        "   AND E.CODE_DIV = '140' " +
                                        "   AND E.FLAG = 'Y' " +
                                        "  LEFT OUTER JOIN CODE_MST F " +
                                        "    ON A.APPRAISAL_CODE = F.CODE_NO " +
                                        "   AND F.CODE_DIV = '150' " +
                                        "   AND F.FLAG = 'Y' " +
                                        "  LEFT OUTER JOIN FARM_SEMEN G " +
                                        "    ON A.FARM_SEQ = G.FARM_SEQ " +
                                        "   AND A.SEMEN_SEQ = G.SEQ " +
                                        "   AND G.FLAG = 'Y' " +
                                        " WHERE A.FLAG = 'Y' " +
                                       "   AND A.FARM_SEQ = {0} ", parameters.farm_seq);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        dataReader.Read();

                        breedDefault.semen_seq = dataReader.GetInt32(2);
                        breedDefault.semen_no = _mClassDatabase.GetSafeString(dataReader, 19);
                        breedDefault.inseminate_code = dataReader.GetInt32(3);
                        breedDefault.inseminate_code_disp = _mClassDatabase.GetSafeString(dataReader, 17);
                        breedDefault.inseminate_day = _mClassDatabase.GetSafeInteger(dataReader, 7);
                        breedDefault.appraisal_code = dataReader.GetInt32(4);
                        breedDefault.appraisal_code_disp = _mClassDatabase.GetSafeString(dataReader, 18);
                        breedDefault.appraisal_day = _mClassDatabase.GetSafeInteger(dataReader, 14);
                        breedDefault.dryup_method = _mClassDatabase.GetSafeString(dataReader, 5);
                        if (string.IsNullOrEmpty(breedDefault.dryup_method))
                        {
                            breedDefault.dryup_month = 0;
                            breedDefault.dryup_day = 0;
                        }
                        else
                        {
                            if (breedDefault.dryup_method == "M")
                            {
                                breedDefault.dryup_month = _mClassDatabase.GetSafeInteger(dataReader, 11);
                                breedDefault.dryup_day = _mClassDatabase.GetSafeInteger(dataReader, 12);
                            }
                            else
                            {
                                breedDefault.dryup_month = 0;
                                breedDefault.dryup_day = _mClassDatabase.GetSafeInteger(dataReader, 13);
                            }
                        }
                        breedDefault.calve_method = _mClassDatabase.GetSafeString(dataReader, 6);
                        if (string.IsNullOrEmpty(breedDefault.calve_method))
                        {
                            breedDefault.calve_month = 0;
                            breedDefault.calve_day = 0;
                        }
                        else
                        {
                            if (breedDefault.calve_method == "M")
                            {
                                breedDefault.calve_month = _mClassDatabase.GetSafeInteger(dataReader, 8);
                                breedDefault.calve_day = _mClassDatabase.GetSafeInteger(dataReader, 9);
                            }
                            else
                            {
                                breedDefault.calve_month = 0;
                                breedDefault.calve_day = _mClassDatabase.GetSafeInteger(dataReader, 10);
                            }
                        }
                        breedDefault.estrus_b_day = _mClassDatabase.GetSafeInteger(dataReader, 15);
                        breedDefault.estrus_n_day = _mClassDatabase.GetSafeInteger(dataReader, 16);

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SUCCESS,
                            message = string.Empty,
                            data = JsonConvert.SerializeObject(breedDefault)
                        };
                    }
                    else
                    {
                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SEARCH_NOTEXIST_SETTING,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SEARCH_NOTEXIST_SETTING),
                            data = string.Empty
                        };
                    }

                    dataReader.Close();
                    dataReader.Dispose();
                }
                else
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
                        data = string.Empty
                    };
                }
            }
            catch
            {
                response = new ClassResponse.RES_RESULT
                {
                    result = ClassError.RESULT_SYSTEM_ERROR_EXCEPTION,
                    message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_EXCEPTION),
                    data = string.Empty
                };
            }
            finally
            {
                _mClassDatabase.CloseDatabase();
            }

            ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
            return response;
            #endregion
        }

        public ClassResponse.RES_RESULT GetDueDayList(ClassRequest.REQ_FARMSEQ parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Breed", "dueday_list");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetDueDayList  ==========", sModuleName);
            ClassLog._mLogger.InfoFormat("{0}  RECEIVE REQUEST DATA  [{1}]", sModuleName, requestData);

            ClassResponse.RES_RESULT response = new ClassResponse.RES_RESULT();

            #region Check Parameter Process
            try
            {
                if (string.IsNullOrEmpty(parameters.lang_code))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_LANG_CODE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_LANG_CODE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (Array.IndexOf(ClassStruct.LANGUAGE_CODE, parameters.lang_code) < 0)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_LANG_CODE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_LANG_CODE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (parameters.farm_seq < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_FARM_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_FARM_SEQ),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }
            }
            catch
            {
                response = new ClassResponse.RES_RESULT
                {
                    result = ClassError.RESULT_SYSTEM_ERROR_EXCEPTION,
                    message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_EXCEPTION),
                    data = string.Empty
                };

                ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                return response;
            }
            #endregion

            #region Check Database Process
            if (!_mClassDatabase.GetConnectionState())
            {
                if (!_mClassDatabase.OpenDatabase())
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }
            }
            #endregion

            #region Business Logic Process
            bool isError = false;

            OleDbDataReader dataReader = null;
            ClassStruct.ST_DUEDAYINFO duedayInfo = new ClassStruct.ST_DUEDAYINFO();

            List<ClassStruct.ST_INSEMINATEDUE> inseminateList = new List<ClassStruct.ST_INSEMINATEDUE>();
            List<ClassStruct.ST_APPRAISALDUE> appraisalList = new List<ClassStruct.ST_APPRAISALDUE>();

            try
            {
                string sQuery = "SELECT A.SEQ, A.INSEMINATE_CODE, A.INSEMINATE_DAY, A.CALVE_M_MONTH, A.CALVE_M_DAY, A.CALVE_D_DAY, ";

                switch (parameters.lang_code)
                {
                    case "KR": sQuery += "       A.DRYUP_M_MONTH, A.DRYUP_M_DAY, A.DRYUP_D_DAY, B.CODE_NAME AS INSEMINATE_DISP "; break;
                    case "JP": sQuery += "       A.DRYUP_M_MONTH, A.DRYUP_M_DAY, A.DRYUP_D_DAY, B.JP_VALUE AS INSEMINATE_DISP "; break;
                    case "US": sQuery += "       A.DRYUP_M_MONTH, A.DRYUP_M_DAY, A.DRYUP_D_DAY, B.EN_VALUE AS INSEMINATE_DISP "; break;
                    case "CN": sQuery += "       A.DRYUP_M_MONTH, A.DRYUP_M_DAY, A.DRYUP_D_DAY, B.ZH_VALUE AS INSEMINATE_DISP "; break;
                    case "PT": sQuery += "       A.DRYUP_M_MONTH, A.DRYUP_M_DAY, A.DRYUP_D_DAY, B.PT_VALUE AS INSEMINATE_DISP "; break;
                    case "BR": sQuery += "       A.DRYUP_M_MONTH, A.DRYUP_M_DAY, A.DRYUP_D_DAY, B.PT_VALUE AS INSEMINATE_DISP "; break;
                    default: sQuery += "       A.DRYUP_M_MONTH, A.DRYUP_M_DAY, A.DRYUP_D_DAY, B.EN_VALUE AS INSEMINATE_DISP "; break;
                }

                sQuery += string.Format("  FROM BREED_INSEMINATE A " +
                                        "  LEFT OUTER JOIN CODE_MST B " +
                                        "    ON A.INSEMINATE_CODE = B.CODE_NO " +
                                        "   AND B.CODE_DIV = '140' " +
                                        "   AND B.FLAG = 'Y' " +
                                        " WHERE A.FLAG = 'Y' " +
                                        "   AND A.FARM_SEQ = {0} ", parameters.farm_seq);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            ClassStruct.ST_INSEMINATEDUE inseminateDue = new ClassStruct.ST_INSEMINATEDUE
                            {
                                inseminate_code = dataReader.GetInt32(1),
                                inseminate_code_disp = dataReader.GetString(9),
                                inseminate_day = dataReader.GetInt32(2),
                                calve_m_month = dataReader.GetInt32(3),
                                calve_m_day = dataReader.GetInt32(4),
                                calve_d_day = dataReader.GetInt32(5),
                                dryup_m_month = dataReader.GetInt32(6),
                                dryup_m_day = dataReader.GetInt32(7),
                                dryup_d_day = dataReader.GetInt32(8)
                            };

                            inseminateList.Add(inseminateDue);
                        }
                    }

                    dataReader.Close();
                    dataReader.Dispose();
                }
                else
                {
                    isError = true;

                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
                        data = string.Empty
                    };
                }

                if (!isError)
                {
                    // 임신감정예정일 계산일수 추출
                    switch (parameters.lang_code)
                    {
                        case "KR": sQuery = "SELECT A.SEQ, A.APPRAISAL_CODE, A.APPRAISAL_DAY, B.CODE_NAME AS APPRAISAL_DISP "; break;
                        case "JP": sQuery = "SELECT A.SEQ, A.APPRAISAL_CODE, A.APPRAISAL_DAY, B.JP_VALUE AS APPRAISAL_DISP "; break;
                        case "US": sQuery = "SELECT A.SEQ, A.APPRAISAL_CODE, A.APPRAISAL_DAY, B.EN_VALUE AS APPRAISAL_DISP "; break;
                        case "CN": sQuery = "SELECT A.SEQ, A.APPRAISAL_CODE, A.APPRAISAL_DAY, B.ZH_VALUE AS APPRAISAL_DISP "; break;
                        case "PT": sQuery = "SELECT A.SEQ, A.APPRAISAL_CODE, A.APPRAISAL_DAY, B.PT_VALUE AS APPRAISAL_DISP "; break;
                        case "BR": sQuery = "SELECT A.SEQ, A.APPRAISAL_CODE, A.APPRAISAL_DAY, B.PT_VALUE AS APPRAISAL_DISP "; break;
                        default: sQuery = "SELECT A.SEQ, A.APPRAISAL_CODE, A.APPRAISAL_DAY, B.EN_VALUE AS APPRAISAL_DISP "; break;
                    }

                    sQuery += string.Format("  FROM BREED_APPRAISAL A " +
                                            "  LEFT OUTER JOIN CODE_MST B " +
                                            "    ON A.APPRAISAL_CODE = B.CODE_NO " +
                                            "   AND B.CODE_DIV = '150' " +
                                            "   AND B.FLAG = 'Y' " +
                                            " WHERE A.FLAG = 'Y' " +
                                            "   AND A.FARM_SEQ = {0} ", parameters.farm_seq);

                    if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                ClassStruct.ST_APPRAISALDUE appraisalDue = new ClassStruct.ST_APPRAISALDUE
                                {
                                    appraisal_code = dataReader.GetInt32(1),
                                    appraisal_code_disp = dataReader.GetString(3),
                                    appraisal_day = dataReader.GetInt32(2)
                                };

                                appraisalList.Add(appraisalDue);
                            }
                        }

                        dataReader.Close();
                        dataReader.Dispose();
                    }
                    else
                    {
                        isError = true;

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
                            data = string.Empty
                        };
                    }
                }

                if (!isError)
                {
                    duedayInfo.inseminate_dueday = inseminateList;
                    duedayInfo.appraisal_dueday = appraisalList;

                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SUCCESS,
                        message = string.Empty,
                        data = JsonConvert.SerializeObject(duedayInfo)
                    };
                }
            }
            catch
            {
                response = new ClassResponse.RES_RESULT
                {
                    result = ClassError.RESULT_SYSTEM_ERROR_EXCEPTION,
                    message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_EXCEPTION),
                    data = string.Empty
                };
            }
            finally
            {
                _mClassDatabase.CloseDatabase();
            }

            ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
            return response;
            #endregion
        }

        public ClassResponse.RES_RESULT GetBreedMonthCount(ClassRequest.REQ_HISTORYMONTH parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Breed", "month_count");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetBreedMonthCount  ==========", sModuleName);
            ClassLog._mLogger.InfoFormat("{0}  RECEIVE REQUEST DATA  [{1}]", sModuleName, requestData);

            ClassResponse.RES_RESULT response = new ClassResponse.RES_RESULT();

            #region Check Parameter Process
            try
            {
                if (string.IsNullOrEmpty(parameters.lang_code))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_LANG_CODE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_LANG_CODE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (Array.IndexOf(ClassStruct.LANGUAGE_CODE, parameters.lang_code) < 0)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_LANG_CODE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_LANG_CODE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (parameters.farm_seq < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_FARM_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_FARM_SEQ),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (parameters.search_year < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_SEARCH_YEAR,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_SEARCH_YEAR),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (parameters.search_month < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_SEARCH_MONTH,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_SEARCH_MONTH),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }
            }
            catch
            {
                response = new ClassResponse.RES_RESULT
                {
                    result = ClassError.RESULT_SYSTEM_ERROR_EXCEPTION,
                    message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_EXCEPTION),
                    data = string.Empty
                };

                ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                return response;
            }
            #endregion

            #region Check Database Process
            if (!_mClassDatabase.GetConnectionState())
            {
                if (!_mClassDatabase.OpenDatabase())
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }
            }
            #endregion

            #region Business Logic Process
            bool isError = false;
            OleDbDataReader dataReader = null;

            ClassStruct.ST_COUNT_INFO countInfo = new ClassStruct.ST_COUNT_INFO();

            try
            {
                int nLastDay = DateTime.DaysInMonth(parameters.search_year, parameters.search_month);

                string sFrom = string.Format("{0}-{1:D2}-01", parameters.search_year, parameters.search_month);
                string sTo = string.Format("{0}-{1:D2}-{2:D2}", parameters.search_year, parameters.search_month, nLastDay);

                string sQuery = string.Format("SELECT BREED_TYPE, COUNT(SEQ) AS COUNTER " +
                           "  FROM (SELECT A.SEQ, A.BREED_TYPE " +
                           "          FROM BREED_HISTORY A " +
                           "          LEFT OUTER JOIN ENTITY_NEW_INFO B " +
                           "            ON A.ENTITY_SEQ = B.SEQ " +
                           "         WHERE A.FARM_SEQ = {0} " +
                           "           AND CONVERT(varchar(10), A.BREED_DATE, 120) >= '{1}' " +
                           "           AND CONVERT(varchar(10), A.BREED_DATE, 120) <= '{2}' " +
                           "           AND A.FLAG = 'Y' " +
                           "           AND B.FLAG = 'Y' " +
                           "           AND B.ACTIVE_FLAG = 'Y') A " +
                           " GROUP BY BREED_TYPE ", parameters.farm_seq, sFrom, sTo);
                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            string sType = dataReader.GetString(0);
                            int nCount = dataReader.GetInt32(1);

                            switch (sType)
                            {
                                case "E": countInfo.ed_count = nCount; break;
                                case "I": countInfo.id_count = nCount; break;
                                case "A": countInfo.ad_count = nCount; break;
                                case "D": countInfo.dd_count = nCount; break;
                                case "C": countInfo.cd_count = nCount; break;
                            }
                        }
                    }

                    dataReader.Close();
                    dataReader.Dispose();
                }
                else
                {
                    isError = true;

                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
                        data = string.Empty
                    };
                }

                if (!isError)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SUCCESS,
                        message = string.Empty,
                        data = JsonConvert.SerializeObject(countInfo)
                    };
                }
            }
            catch
            {
                response = new ClassResponse.RES_RESULT
                {
                    result = ClassError.RESULT_SYSTEM_ERROR_EXCEPTION,
                    message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_EXCEPTION),
                    data = string.Empty
                };
            }
            finally
            {
                _mClassDatabase.CloseDatabase();
            }

            ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
            return response;
            #endregion
        }

        public ClassResponse.RES_RESULT GetDueDayInfo(ClassRequest.REQ_FARMSEQ parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Breed", "dueday_info");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetDueDayInfo  ==========", sModuleName);
            ClassLog._mLogger.InfoFormat("{0}  RECEIVE REQUEST DATA  [{1}]", sModuleName, requestData);

            ClassResponse.RES_RESULT response = new ClassResponse.RES_RESULT();

            #region Check Parameter Process
            try
            {
                if (string.IsNullOrEmpty(parameters.lang_code))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_LANG_CODE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_LANG_CODE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (Array.IndexOf(ClassStruct.LANGUAGE_CODE, parameters.lang_code) < 0)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_LANG_CODE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_LANG_CODE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (parameters.farm_seq < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_FARM_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_FARM_SEQ),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }
            }
            catch
            {
                response = new ClassResponse.RES_RESULT
                {
                    result = ClassError.RESULT_SYSTEM_ERROR_EXCEPTION,
                    message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_EXCEPTION),
                    data = string.Empty
                };

                ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                return response;
            }
            #endregion

            #region Check Database Process
            if (!_mClassDatabase.GetConnectionState())
            {
                if (!_mClassDatabase.OpenDatabase())
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }
            }
            #endregion

            #region Business Logic Process
            bool isError = false;
            OleDbDataReader dataReader = null;

            List<ClassStruct.ST_CALVE_DUEDAY_INFO> calveList = new List<ClassStruct.ST_CALVE_DUEDAY_INFO>();
            List<ClassStruct.ST_ESTRUS_DUEDAY_INFO> estrusList = new List<ClassStruct.ST_ESTRUS_DUEDAY_INFO>();

            try
            {
                string sQuery = "SELECT A.SEQ, A.INSEMINATE_CODE, A.CALVE_D_DAY, ";

                switch (parameters.lang_code)
                {
                    case "KR": sQuery += "       B.CODE_NAME AS INSEMINATE_DISP "; break;
                    case "JP": sQuery += "       B.JP_VALUE AS INSEMINATE_DISP "; break;
                    case "US": sQuery += "       B.EN_VALUE AS INSEMINATE_DISP "; break;
                    case "CN": sQuery += "       B.ZH_VALUE AS INSEMINATE_DISP "; break;
                    case "PT": sQuery += "       B.PT_VALUE AS INSEMINATE_DISP "; break;
                    case "BR": sQuery += "       B.PT_VALUE AS INSEMINATE_DISP "; break;
                    default: sQuery += "       B.EN_VALUE AS INSEMINATE_DISP "; break;
                }

                sQuery += string.Format("  FROM BREED_INSEMINATE A " +
                                        "  LEFT OUTER JOIN CODE_MST B " +
                                        "    ON A.INSEMINATE_CODE = B.CODE_NO " +
                                        "   AND B.CODE_DIV = '140' " +
                                        "   AND B.FLAG = 'Y' " +
                                        " WHERE A.FLAG = 'Y' " +
                                        "   AND A.FARM_SEQ = {0} ", parameters.farm_seq);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            ClassStruct.ST_CALVE_DUEDAY_INFO calveInfo = new ClassStruct.ST_CALVE_DUEDAY_INFO
                            {
                                inseminate_code = dataReader.GetInt32(1),
                                inseminate_code_disp = _mClassDatabase.GetSafeString(dataReader, 3),
                                due_day = dataReader.GetInt32(2)
                            };

                            calveList.Add(calveInfo);
                        }
                    }

                    dataReader.Close();
                    dataReader.Dispose();
                }
                else
                {
                    isError = true;

                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
                        data = string.Empty
                    };
                }

                if (!isError)
                {
                    sQuery = string.Format("SELECT ESTRUS_B_DAY, ESTRUS_N_DAY " +
                                           "  FROM BREED_ESTRUS " +
                                           " WHERE FARM_SEQ = {0} " +
                                           "   AND FLAG = 'Y' ", parameters.farm_seq);

                    if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                    {
                        if (dataReader.HasRows)
                        {
                            ClassStruct.ST_ESTRUS_DUEDAY_INFO estrusInfo = new ClassStruct.ST_ESTRUS_DUEDAY_INFO();

                            dataReader.Read();

                            // 분만전 발정재귀일
                            estrusInfo.estrus_code = "BC";
                            switch (parameters.lang_code)
                            {
                                case "KR": estrusInfo.estrus_code_disp = "분만전 발정재귀일"; break;
                                case "JP": estrusInfo.estrus_code_disp = "分娩前発情再帰こと"; break;
                                case "US": estrusInfo.estrus_code_disp = "Estrus cycle"; break;
                                case "CN": estrusInfo.estrus_code_disp = "预计分娩后的第一次发情"; break;
                                case "PT": estrusInfo.estrus_code_disp = "Data do estro"; break;
                                case "BR": estrusInfo.estrus_code_disp = "Data do estro"; break;
                                default: estrusInfo.estrus_code_disp = "Estrus cycle"; break;
                            }
                            estrusInfo.due_day = dataReader.GetInt32(0);

                            estrusList.Add(estrusInfo);

                            // 분만후 초발정예정일
                            estrusInfo.estrus_code = "AC";
                            switch (parameters.lang_code)
                            {
                                case "KR": estrusInfo.estrus_code_disp = "분만후 초발정예정일"; break;
                                case "JP": estrusInfo.estrus_code_disp = "分娩後秒角予定日"; break;
                                case "US": estrusInfo.estrus_code_disp = "Return of Estrus, postpartum"; break;
                                case "CN": estrusInfo.estrus_code_disp = "预计分娩后的第一次发情"; break;
                                case "PT": estrusInfo.estrus_code_disp = "Retorno do estro (pós-parto)"; break;
                                case "BR": estrusInfo.estrus_code_disp = "Retorno do estro (pós-parto)"; break;
                                default: estrusInfo.estrus_code_disp = "Return of Estrus, postpartum"; break;
                            }
                            estrusInfo.due_day = dataReader.GetInt32(1);

                            estrusList.Add(estrusInfo);
                        }

                        dataReader.Close();
                        dataReader.Dispose();
                    }
                    else
                    {
                        isError = true;

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
                            data = string.Empty
                        };
                    }
                }

                if (!isError)
                {
                    ClassStruct.ST_DUEDAY_COMPUTE duedayInfo = new ClassStruct.ST_DUEDAY_COMPUTE
                    {
                        calve_dueday = calveList,
                        estrus_dueday = estrusList
                    };

                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SUCCESS,
                        message = string.Empty,
                        data = JsonConvert.SerializeObject(duedayInfo)
                    };
                }
            }
            catch
            {
                response = new ClassResponse.RES_RESULT
                {
                    result = ClassError.RESULT_SYSTEM_ERROR_EXCEPTION,
                    message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_EXCEPTION),
                    data = string.Empty
                };
            }
            finally
            {
                _mClassDatabase.CloseDatabase();
            }

            ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
            return response;
            #endregion
        }

        public ClassResponse.RES_RESULT SetCalveDueDay(ClassRequest.REQ_CALVE_DUE_DAY parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Breed", "calvedue_update");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START SetCalveDueDay  ==========", sModuleName);
            ClassLog._mLogger.InfoFormat("{0}  RECEIVE REQUEST DATA  [{1}]", sModuleName, requestData);

            ClassResponse.RES_RESULT response = new ClassResponse.RES_RESULT();

            #region Check Parameter Process
            try
            {
                if (string.IsNullOrEmpty(parameters.lang_code))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_LANG_CODE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_LANG_CODE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (Array.IndexOf(ClassStruct.LANGUAGE_CODE, parameters.lang_code) < 0)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_LANG_CODE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_LANG_CODE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (parameters.farm_seq < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_FARM_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_FARM_SEQ),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (parameters.inseminate_code < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_INSEMINATE_CODE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_INSEMINATE_CODE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }
            }
            catch
            {
                response = new ClassResponse.RES_RESULT
                {
                    result = ClassError.RESULT_SYSTEM_ERROR_EXCEPTION,
                    message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_EXCEPTION),
                    data = string.Empty
                };

                ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                return response;
            }
            #endregion

            #region Check Database Process
            if (!_mClassDatabase.GetConnectionState())
            {
                if (!_mClassDatabase.OpenDatabase())
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }
            }
            #endregion

            #region Business Logic Process
            bool isError = false;
            OleDbDataReader dataReader = null;

            int nSEQ = 0;

            try
            {
                string sQuery = string.Format("SELECT SEQ FROM BREED_INSEMINATE " +
                           " WHERE FARM_SEQ = {0} " +
                           "   AND INSEMINATE_CODE = {1} " +
                           "   AND FLAG = 'Y'", parameters.farm_seq, parameters.inseminate_code);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        dataReader.Read();
                        nSEQ = dataReader.GetInt32(0);
                    }

                    dataReader.Close();
                    dataReader.Dispose();
                }
                else
                {
                    isError = true;

                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
                        data = string.Empty
                    };
                }

                if (!isError && nSEQ > 0)
                {
                    sQuery = string.Format("UPDATE BREED_INSEMINATE " +
                                           "   SET CALVE_D_DAY = {1} " +
                                           " WHERE SEQ = {0} ", nSEQ, parameters.due_day);

                    int count = _mClassDatabase.QueryExecute(sQuery);

                    if (count < 1)
                    {
                        isError = true;

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
                            data = string.Empty
                        };
                    }
                }

                if (!isError)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SUCCESS,
                        message = string.Empty,
                        data = string.Empty
                    };
                }
            }
            catch
            {
                response = new ClassResponse.RES_RESULT
                {
                    result = ClassError.RESULT_SYSTEM_ERROR_EXCEPTION,
                    message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_EXCEPTION),
                    data = string.Empty
                };
            }
            finally
            {
                _mClassDatabase.CloseDatabase();
            }

            ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
            return response;
            #endregion
        }

        public ClassResponse.RES_RESULT SetEstrusDueDay(ClassRequest.REQ_ESTRUS_DUE_DAY parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Breed", "estrusdue_update");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START SetEstrusDueDay  ==========", sModuleName);
            ClassLog._mLogger.InfoFormat("{0}  RECEIVE REQUEST DATA  [{1}]", sModuleName, requestData);

            ClassResponse.RES_RESULT response = new ClassResponse.RES_RESULT();

            #region Check Parameter Process
            try
            {
                if (string.IsNullOrEmpty(parameters.lang_code))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_LANG_CODE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_LANG_CODE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (Array.IndexOf(ClassStruct.LANGUAGE_CODE, parameters.lang_code) < 0)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_LANG_CODE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_LANG_CODE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (parameters.farm_seq < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_FARM_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_FARM_SEQ),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (string.IsNullOrEmpty(parameters.estrus_code))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_ESTRUS_CODE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_ESTRUS_CODE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }
            }
            catch
            {
                response = new ClassResponse.RES_RESULT
                {
                    result = ClassError.RESULT_SYSTEM_ERROR_EXCEPTION,
                    message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_EXCEPTION),
                    data = string.Empty
                };

                ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                return response;
            }
            #endregion

            #region Check Database Process
            if (!_mClassDatabase.GetConnectionState())
            {
                if (!_mClassDatabase.OpenDatabase())
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }
            }
            #endregion

            #region Business Logic Process
            bool isError = false;
            OleDbDataReader dataReader = null;

            int nSEQ = 0;

            try
            {
                string sQuery = string.Format("SELECT SEQ FROM BREED_ESTRUS " +
                           " WHERE FARM_SEQ = {0} " +
                           "   AND FLAG = 'Y'", parameters.farm_seq);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        dataReader.Read();
                        nSEQ = dataReader.GetInt32(0);
                    }

                    dataReader.Close();
                    dataReader.Dispose();
                }
                else
                {
                    isError = true;

                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
                        data = string.Empty
                    };
                }

                if (!isError && nSEQ > 0)
                {
                    string sField = string.Empty;

                    switch (parameters.estrus_code)
                    {
                        case "BC": sField = "ESTRUS_B_DAY"; break;
                        case "AC": sField = "ESTRUS_N_DAY"; break;
                    }

                    sQuery = string.Format("UPDATE BREED_ESTRUS " +
                                           "   SET {1} = {2} " +
                                           " WHERE SEQ = {0} ", nSEQ, sField, parameters.due_day);

                    int count = _mClassDatabase.QueryExecute(sQuery);

                    if (count < 1)
                    {
                        isError = true;

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
                            data = string.Empty
                        };
                    }
                }

                if (!isError)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SUCCESS,
                        message = string.Empty,
                        data = string.Empty
                    };
                }
            }
            catch
            {
                response = new ClassResponse.RES_RESULT
                {
                    result = ClassError.RESULT_SYSTEM_ERROR_EXCEPTION,
                    message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_EXCEPTION),
                    data = string.Empty
                };
            }
            finally
            {
                _mClassDatabase.CloseDatabase();
            }

            ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
            return response;
            #endregion
        }

        public ClassResponse.RES_RESULT SetDueDayUpdate(ClassRequest.REQ_DUEDAY_UPDATE parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Breed", "dueday_update");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START SetDueDayUpdate  ==========", sModuleName);
            ClassLog._mLogger.InfoFormat("{0}  RECEIVE REQUEST DATA  [{1}]", sModuleName, requestData);

            ClassResponse.RES_RESULT response = new ClassResponse.RES_RESULT();

            #region Check Parameter Process
            try
            {
                if (string.IsNullOrEmpty(parameters.lang_code))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_LANG_CODE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_LANG_CODE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (Array.IndexOf(ClassStruct.LANGUAGE_CODE, parameters.lang_code) < 0)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_LANG_CODE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_LANG_CODE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (parameters.farm_seq < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_FARM_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_FARM_SEQ),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }
            }
            catch
            {
                response = new ClassResponse.RES_RESULT
                {
                    result = ClassError.RESULT_SYSTEM_ERROR_EXCEPTION,
                    message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_EXCEPTION),
                    data = string.Empty
                };

                ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                return response;
            }
            #endregion

            #region Check Database Process
            if (!_mClassDatabase.GetConnectionState())
            {
                if (!_mClassDatabase.OpenDatabase())
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }
            }

            #endregion
            #region Business Logic Process
            bool isError = false;

            try
            {
                _mClassDatabase.TransBegin();

                int count;
                string sQuery;

                foreach (ClassStruct.ST_CALVE_DUE_DAY calveDueDay in parameters.calve_info)
                {
                    sQuery = string.Format("UPDATE BREED_INSEMINATE " +
                                           "   SET CALVE_D_DAY = {2} " +
                                           " WHERE FARM_SEQ = {0} " +
                                           "   AND INSEMINATE_CODE = {1} " +
                                           "   AND FLAG = 'Y' ", parameters.farm_seq, calveDueDay.inseminate_code, calveDueDay.due_day);

                    count = _mClassDatabase.QueryExecute(sQuery);

                    if (count < 1)
                    {
                        isError = true;
                        _mClassDatabase.TransRollback();

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
                            data = string.Empty
                        };

                        break;
                    }
                }

                if (!isError)
                {
                    foreach (ClassStruct.ST_ESTRUS_DUE_DAY estrusDueDay in parameters.estrus_info)
                    {
                        string sField = string.Empty;

                        switch (estrusDueDay.estrus_code)
                        {
                            case "BC": sField = "ESTRUS_B_DAY"; break;
                            case "AC": sField = "ESTRUS_N_DAY"; break;
                        }

                        sQuery = string.Format("UPDATE BREED_ESTRUS " +
                                               "   SET {1} = {2} " +
                                               " WHERE FARM_SEQ = {0} " +
                                               "   AND FLAG = 'Y' ", parameters.farm_seq, sField, estrusDueDay.due_day);

                        count = _mClassDatabase.QueryExecute(sQuery);

                        if (count < 1)
                        {
                            isError = true;
                            _mClassDatabase.TransRollback();

                            response = new ClassResponse.RES_RESULT
                            {
                                result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
                                message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
                                data = string.Empty
                            };

                            break;
                        }
                    }
                }

                if (!isError)
                {
                    _mClassDatabase.TransCommit();

                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SUCCESS,
                        message = string.Empty,
                        data = string.Empty
                    };
                }
            }
            catch
            {
                _mClassDatabase.TransRollback();

                response = new ClassResponse.RES_RESULT
                {
                    result = ClassError.RESULT_SYSTEM_ERROR_EXCEPTION,
                    message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_EXCEPTION),
                    data = string.Empty
                };
            }
            finally
            {
                _mClassDatabase.CloseDatabase();
            }

            ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
            return response;
            #endregion
        }
    }
}
