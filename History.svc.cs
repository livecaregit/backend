using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web;

namespace LC_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "History" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select History.svc or History.svc.cs at the Solution Explorer and start debugging.
    public class History : IHistory
    {
        private ClassOLEDB _mClassDatabase = new ClassOLEDB();
        private readonly ClassError _mClassError = new ClassError();
        //private readonly ClassFunction _mClassFunction = new ClassFunction();

        ~History()
        {
            if (_mClassDatabase.GetConnectionState()) _mClassDatabase.CloseDatabase();
            _mClassDatabase = null;
        }

        public ClassResponse.RES_RESULT GetHistoryList(ClassRequest.REQ_HISTORYLIST parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "History", "history_list");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetHistoryList  ==========", sModuleName);
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

                if (string.IsNullOrEmpty(parameters.uid))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_UID,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_UID),
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

                if (string.IsNullOrEmpty(parameters.search_type))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_SEARCH_TYPE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_SEARCH_TYPE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }
                else
                {
                    if (parameters.search_type != "A" && parameters.search_type != "F" && parameters.search_type != "E" && parameters.search_type != "C")
                    {
                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_PARAM_ERROR_SEARCH_TYPE,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_SEARCH_TYPE),
                            data = string.Empty
                        };

                        ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                        return response;
                    }
                }

                if (parameters.search_type == "E" && string.IsNullOrEmpty(parameters.entity_id))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_ENTITY_ID,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_ENTITY_ID),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (parameters.page_index < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_PAGE_INDEX,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_PAGE_INDEX),
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

            List<ClassStruct.ST_BREEDINFO> breedList = new List<ClassStruct.ST_BREEDINFO>();

            try
            {
                // 페이지 인덱스
                int nFromIndex = (parameters.page_index - 1) * 20 + 1;
                int nToIndex = parameters.page_index * 20;

                string sQuery = string.Format("SELECT SEQ, ENTITY_NO, IMAGE_URL, ENTITY_KIND, CALVE_FLAG, FAVORITE_FLAG, INSEMINATE_FLAG, " +
                                       "       RESULT_CODE, RESULT_DATE, RESULT_DAY " +
                                       "  FROM (SELECT ROW_NUMBER() OVER (ORDER BY RESULT_DAY) AS ROW_NUM, " +
                                       "               SEQ, ENTITY_NO, IMAGE_URL, ENTITY_KIND, CALVE_FLAG, FAVORITE_FLAG, INSEMINATE_FLAG, " +
                                       "               RESULT_CODE, RESULT_DATE, RESULT_DAY " +
                                       "          FROM (SELECT SEQ, ENTITY_NO, IMAGE_URL, ENTITY_KIND, PREGNANCY_CODE, CALVE_FLAG, FAVORITE_FLAG, INSEMINATE_FLAG, " +
                                       "                       BREED_TYPE, BREED_DATE, ADDED_DUE_DATE1, LAST_CALVE_DATE, TO_DATE, " +
                                       "                       CASE BREED_TYPE " +
                                       "                            WHEN 'E' THEN " +
                                       "                                 CASE WHEN LAST_CALVE_DATE IS NOT NULL THEN 'AC' " +
                                       "                                      ELSE NULL " +
                                       "                                  END " +
                                       "                            WHEN 'I' THEN 'AI' " +
                                       "                            WHEN 'A' THEN " +
                                       "                                 CASE WHEN ADDED_DUE_DATE1 IS NOT NULL THEN 'BC' " +
                                       "                                      WHEN ADDED_DUE_DATE1 IS NULL AND BREED_TEXT_VALUE = 'N' AND LAST_CALVE_DATE IS NOT NULL THEN 'AC' " +
                                       "                                      ELSE NULL " +
                                       "                                  END " +
                                       "                            WHEN 'D' THEN " +
                                       "                                 CASE WHEN ADDED_DUE_DATE1 IS NOT NULL THEN 'BC' " +
                                       "                                      ELSE NULL " +
                                       "                                  END " +
                                       "                            WHEN 'C' THEN 'AC' " +
                                       "                        END RESULT_CODE, " +
                                       "                       CASE BREED_TYPE " +
                                       "                            WHEN 'E' THEN " +
                                       "                                 CASE WHEN LAST_CALVE_DATE IS NOT NULL THEN CONVERT(CHAR(10), LAST_CALVE_DATE, 23) " +
                                       "                                      ELSE NULL " +
                                       "                                  END " +
                                       "                            WHEN 'I' THEN CONVERT(CHAR(10), BREED_DATE, 23) " +
                                       "                            WHEN 'A' THEN " +
                                       "                                 CASE WHEN ADDED_DUE_DATE1 IS NOT NULL THEN CONVERT(CHAR(10), ADDED_DUE_DATE1, 23) " +
                                       "                                      WHEN ADDED_DUE_DATE1 IS NULL AND BREED_TEXT_VALUE = 'N' AND LAST_CALVE_DATE IS NOT NULL " +
                                       "                                      THEN CONVERT(CHAR(10), LAST_CALVE_DATE, 23) " +
                                       "                                      ELSE NULL " +
                                       "                                  END " +
                                       "                            WHEN 'D' THEN " +
                                       "                                 CASE WHEN ADDED_DUE_DATE1 IS NOT NULL THEN CONVERT(CHAR(10), ADDED_DUE_DATE1, 23) " +
                                       "                                 ELSE NULL " +
                                       "                                  END " +
                                       "                            WHEN 'C' THEN CONVERT(CHAR(10), BREED_DATE, 23) " +
                                       "                        END RESULT_DATE, " +
                                       "                       CASE BREED_TYPE " +
                                       "                            WHEN 'E' THEN " +
                                       "                                 CASE WHEN LAST_CALVE_DATE IS NOT NULL THEN DATEDIFF(DAY, LAST_CALVE_DATE, TO_DATE) " +
                                       "                                      ELSE 999999 " +
                                       "                                  END " +
                                       "                            WHEN 'I' THEN DATEDIFF(DAY, BREED_DATE, TO_DATE) " +
                                       "                            WHEN 'A' THEN " +
                                       "                                 CASE WHEN ADDED_DUE_DATE1 IS NOT NULL THEN DATEDIFF(DAY, TO_DATE, ADDED_DUE_DATE1) " +
                                       "                                      WHEN ADDED_DUE_DATE1 IS NULL AND BREED_TEXT_VALUE = 'N' AND LAST_CALVE_DATE IS NOT NULL " +
                                       "                                           THEN DATEDIFF(DAY, LAST_CALVE_DATE, TO_DATE) " +
                                       "                                      ELSE 99999 " +
                                       "                                  END " +
                                       "                            WHEN 'D' THEN " +
                                       "                                 CASE WHEN ADDED_DUE_DATE1 IS NOT NULL THEN DATEDIFF(DAY, TO_DATE, ADDED_DUE_DATE1) " +
                                       "                                      ELSE 99999 " +
                                       "                                  END " +
                                       "                            WHEN 'C' THEN DATEDIFF(DAY, BREED_DATE, TO_DATE) " +
                                       "                            ELSE 999999 " +
                                       "                        END RESULT_DAY " +
                                       "                  FROM (SELECT A.SEQ, A.ENTITY_NO, A.IMAGE_URL, A.ENTITY_KIND, A.PREGNANCY_CODE, A.CALVE_FLAG, A.FAVORITE_FLAG, A.INSEMINATE_FLAG, " +
                                       "                               B.BREED_TYPE, B.BREED_DATE, B.ADDED_DUE_DATE1, B.BREED_TEXT_VALUE, C.LAST_CALVE_DATE, D.TO_DATE " +
                                       "                          FROM UDF_HISTORY_ENTITYLIST({0}, '{1}') A " +
                                       "                          LEFT OUTER JOIN UDF_HISTORY_LASTDATA({0}) B " +
                                       "                            ON A.SEQ = B.ENTITY_SEQ " +
                                       "                          LEFT OUTER JOIN UDF_HISTORY_LASTINFO({0}) C " +
                                       "                            ON A.SEQ = C.ENTITY_SEQ " +
                                       "                          LEFT OUTER JOIN UDF_FARM_UTCDATE(0) D " +
                                       "                            ON D.SEQ = {0} " +
                                       "                         WHERE A.SEQ > 0 ", parameters.farm_seq, parameters.uid);

                // 즐겨찾기 / 개체번호 검색 / 조건검색에 따라서 처리
                if (parameters.search_type == "F")
                {
                    sQuery += "                           AND A.FAVORITE_FLAG = 'Y' ";
                }
                else if (parameters.search_type == "E")
                {
                    sQuery += string.Format("                           AND A.ENTITY_NO LIKE '%{0}%' ", parameters.entity_id);
                }
                else if (parameters.search_type == "C")
                {
                    // 건유소 / 착유소 구분
                    if (!string.IsNullOrEmpty(parameters.dryup_flag))
                    {
                        int nKind = 0;

                        if (parameters.dryup_flag == "Y") nKind = 4;
                        else nKind = 3;

                        sQuery += string.Format("                           AND A.ENTITY_KIND = {0} ", nKind);
                    }

                    // 경산우 / 미경산우 구분
                    if (!string.IsNullOrEmpty(parameters.calve_flag))
                    {
                        sQuery += string.Format("                           AND A.CALVE_FLAG = '{0}' ", parameters.calve_flag);
                    }

                    // 임신중 / 미임신중 구분
                    if (!string.IsNullOrEmpty(parameters.pregnancy_flag))
                    {
                        int nPregnancy = 0;

                        if (parameters.pregnancy_flag == "Y") nPregnancy = 1;
                        else nPregnancy = 2;

                        sQuery += string.Format("                           AND A.PREGNANCY_CODE = {0} ", nPregnancy);
                    }

                    // 수정함 / 미수정 구분
                    if (!string.IsNullOrEmpty(parameters.breed_status))
                    {
                        sQuery += string.Format("                           AND A.INSEMINATE_FLAG = '{0}' ", parameters.breed_status);
                    }
                }

                sQuery += "                       ) BREED_LIST ";
                sQuery += "               ) RESULT_LIST ";
                sQuery += "      ) RESULT ";
                sQuery += string.Format(" WHERE RESULT.ROW_NUM BETWEEN {0} AND {1} ", nFromIndex, nToIndex);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            ClassStruct.ST_BREEDINFO breedInfo = new ClassStruct.ST_BREEDINFO
                            {
                                entity_seq = dataReader.GetInt32(0),
                                entity_id = _mClassDatabase.GetSafeString(dataReader, 1),
                                image_url = _mClassDatabase.GetSafeString(dataReader, 2)
                            };
                            if (string.IsNullOrEmpty(_mClassDatabase.GetSafeString(dataReader, 7)))
                            {
                                breedInfo.breed_code = string.Empty;
                                breedInfo.breed_title = string.Empty;
                                breedInfo.breed_date = string.Empty;
                                breedInfo.breed_day = 0;
                            }
                            else
                            {
                                // 마지막 번식일정에 따라서 처리한다
                                // 발정 : 마지막 분만일이 있으면 -> 분만 후
                                //        마지막 분만일이 없으면 -> 빈 값
                                // 수정 : 수정 후
                                // 감정 : 분만예정일이 있으면 -> 분만까지
                                //        분만예정일이 없고 마지막 분만일이 있으면 -> 분만 후
                                //        분만예정일이 없고 마지막 분만일이 없으면 -> 빈 값
                                // 건유 : 분만예정일이 있으면 -> 분만까지
                                // 분만 : 분만 후
                                breedInfo.breed_date = dataReader.GetString(8);
                                breedInfo.breed_code = dataReader.GetString(7);
                                breedInfo.breed_title = string.Empty;
                                breedInfo.breed_day = dataReader.GetInt32(9);

                                switch (parameters.lang_code)
                                {
                                    case "KR":
                                        {
                                            switch (breedInfo.breed_code)
                                            {
                                                case "AI": breedInfo.breed_title = "수정 후"; break;
                                                case "BC": breedInfo.breed_title = "분만예정일"; break;
                                                case "AC": breedInfo.breed_title = "분만 후"; break;
                                            }

                                            break;
                                        }
                                    case "JP":
                                        {
                                            switch (breedInfo.breed_code)
                                            {
                                                case "AI": breedInfo.breed_title = "授精後"; break;
                                                case "BC": breedInfo.breed_title = "分娩予定日"; break;
                                                case "AC": breedInfo.breed_title = "分娩後"; break;
                                            }

                                            break;
                                        }
                                    case "US":
                                        {
                                            switch (breedInfo.breed_code)
                                            {
                                                case "AI": breedInfo.breed_title = "Insemination"; break;
                                                case "BC": breedInfo.breed_title = "Estimated calving date"; break;
                                                case "AC": breedInfo.breed_title = "Calving"; break;
                                            }

                                            break;
                                        }
                                    case "CN":
                                        {
                                            switch (breedInfo.breed_code)
                                            {
                                                case "AI": breedInfo.breed_title = "Insemination"; break;
                                                case "BC": breedInfo.breed_title = "Estimated calving date"; break;
                                                case "AC": breedInfo.breed_title = "Calving"; break;
                                            }

                                            break;
                                        }
                                    case "PT":
                                        {
                                            switch (breedInfo.breed_code)
                                            {
                                                case "AI": breedInfo.breed_title = "Inseminação"; break;
                                                case "BC": breedInfo.breed_title = "D-prevista de Parto"; break;
                                                case "AC": breedInfo.breed_title = "Parto"; break;
                                            }

                                            break;
                                        }
                                    case "BR":
                                        {
                                            switch (breedInfo.breed_code)
                                            {
                                                case "AI": breedInfo.breed_title = "Inseminação"; break;
                                                case "BC": breedInfo.breed_title = "D-prevista de Parto"; break;
                                                case "AC": breedInfo.breed_title = "Parto"; break;
                                            }

                                            break;
                                        }
                                    default:
                                        {
                                            switch (breedInfo.breed_code)
                                            {
                                                case "AI": breedInfo.breed_title = "Insemination"; break;
                                                case "BC": breedInfo.breed_title = "Estimated calving date"; break;
                                                case "AC": breedInfo.breed_title = "Calving"; break;
                                            }

                                            break;
                                        }
                                }
                            }

                            breedList.Add(breedInfo);
                        }

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SUCCESS,
                            message = string.Empty,
                            data = JsonConvert.SerializeObject(breedList)
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
            catch (Exception Exp)
            {
                response = new ClassResponse.RES_RESULT
                {
                    result = ClassError.RESULT_SYSTEM_ERROR_EXCEPTION,
                    //message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_EXCEPTION),
                    message = Exp.Message,
                    data = string.Empty
                };
            }
            finally
            {
                _mClassDatabase.CloseDatabase();
            }

            //ClassLog._mLogger.Info(String.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
            ClassLog._mLogger.InfoFormat("{0}  데이타 전송완료", sModuleName);
            return response;
            #endregion
        }

        public ClassResponse.RES_RESULT GetHistoryMonth(ClassRequest.REQ_HISTORYMONTH parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "History", "history_month");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetHistoryMonth  ==========", sModuleName);
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

            List<ClassStruct.ST_HISTORYMONTH> historyList = new List<ClassStruct.ST_HISTORYMONTH>();

            Dictionary<string, string> estrusList = new Dictionary<string, string>();
            Dictionary<string, string> inseminateList = new Dictionary<string, string>();
            Dictionary<string, string> appraisalList = new Dictionary<string, string>();
            Dictionary<string, string> dryupList = new Dictionary<string, string>();
            Dictionary<string, string> calveList = new Dictionary<string, string>();

            try
            {
                int nLastDay = DateTime.DaysInMonth(parameters.search_year, parameters.search_month);

                string sFrom = string.Format("{0}-{1:D2}-01", parameters.search_year, parameters.search_month);
                string sTo = string.Format("{0}-{1:D2}-{2:D2}", parameters.search_year, parameters.search_month, nLastDay);

                string sQuery = string.Format("SELECT A.BREED_TYPE, A.BREED_DUE_DATE, A.ADDED_DUE_DATE1, A.ADDED_DUE_DATE2 " +
                           "  FROM UDF_HISTORY_LASTDATA({0}) A " +
                           "  LEFT OUTER JOIN ENTITY_NEW_INFO B " +
                           "    ON A.ENTITY_SEQ = B.SEQ " +
                           "   AND B.FLAG = 'Y' " +
                           "   AND B.ACTIVE_FLAG = 'Y' " +
                           " WHERE ((CONVERT(CHAR(10), A.BREED_DUE_DATE, 120) >= '{1}' " +
                           "   AND   CONVERT(CHAR(10), A.BREED_DUE_DATE, 120) <= '{2}') " +
                           "    OR  (CONVERT(CHAR(10), A.ADDED_DUE_DATE1, 120) >= '{1}' " +
                           "   AND   CONVERT(CHAR(10), A.ADDED_DUE_DATE1, 120) <= '{2}') " +
                           "    OR  (CONVERT(CHAR(10), A.ADDED_DUE_DATE2, 120) >= '{1}' " +
                           "   AND   CONVERT(CHAR(10), A.ADDED_DUE_DATE2, 120) <= '{2}')) " +
                           "   AND B.ENTITY_NO IS NOT NULL ", parameters.farm_seq, sFrom, sTo);
                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            string sBreedType = string.Empty;
                            string sBreedDueDate = string.Empty;
                            string sAddedDueDate1 = string.Empty;
                            string sAddedDueDate2 = string.Empty;

                            DateTime dtCheckDate = DateTime.Now;

                            sBreedType = dataReader.GetString(0);

                            if (!dataReader.IsDBNull(1))
                            {
                                dtCheckDate = dataReader.GetDateTime(1);
                                if (dtCheckDate >= Convert.ToDateTime(sFrom) && dtCheckDate <= Convert.ToDateTime(sTo)) sBreedDueDate = dtCheckDate.ToString("yyyy-MM-dd");
                            }
                            if (!dataReader.IsDBNull(2))
                            {
                                dtCheckDate = dataReader.GetDateTime(2);
                                if (dtCheckDate >= Convert.ToDateTime(sFrom) && dtCheckDate <= Convert.ToDateTime(sTo)) sAddedDueDate1 = dtCheckDate.ToString("yyyy-MM-dd");
                            }
                            if (!dataReader.IsDBNull(3))
                            {
                                dtCheckDate = dataReader.GetDateTime(3);
                                if (dtCheckDate >= Convert.ToDateTime(sFrom) && dtCheckDate <= Convert.ToDateTime(sTo)) sAddedDueDate2 = dtCheckDate.ToString("yyyy-MM-dd");
                            }

                            switch (sBreedType)
                            {
                                case "E":
                                    if (!string.IsNullOrEmpty(sBreedDueDate) && !estrusList.ContainsKey(sBreedDueDate)) estrusList.Add(sBreedDueDate, sBreedType);
                                    if (!string.IsNullOrEmpty(sAddedDueDate1) && !inseminateList.ContainsKey(sAddedDueDate1)) inseminateList.Add(sAddedDueDate1, sBreedType);
                                    break;
                                case "I":
                                    if (!string.IsNullOrEmpty(sAddedDueDate1) && !appraisalList.ContainsKey(sAddedDueDate1)) appraisalList.Add(sAddedDueDate1, sBreedType);
                                    break;
                                case "A":
                                    if (!string.IsNullOrEmpty(sAddedDueDate1) && !calveList.ContainsKey(sAddedDueDate1)) calveList.Add(sAddedDueDate1, sBreedType);
                                    if (!string.IsNullOrEmpty(sAddedDueDate2) && !dryupList.ContainsKey(sAddedDueDate2)) dryupList.Add(sAddedDueDate2, sBreedType);
                                    break;
                                case "D":
                                    if (!string.IsNullOrEmpty(sAddedDueDate1) && !calveList.ContainsKey(sAddedDueDate1)) calveList.Add(sAddedDueDate1, sBreedType);
                                    break;
                                case "C":
                                    if (!string.IsNullOrEmpty(sBreedDueDate) && !estrusList.ContainsKey(sBreedDueDate)) estrusList.Add(sBreedDueDate, sBreedType);
                                    break;
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
                    for (int i = 1 ; i <= nLastDay ; i++)
                    {
                        string sDate = string.Format("{0}-{1:D2}-{2:D2}", parameters.search_year, parameters.search_month, i);

                        ClassStruct.ST_HISTORYMONTH historyMonth = new ClassStruct.ST_HISTORYMONTH
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
            catch (Exception Exp)
            {
                response = new ClassResponse.RES_RESULT
                {
                    result = ClassError.RESULT_SYSTEM_ERROR_EXCEPTION,
                    //message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_EXCEPTION),
                    message = Exp.Message,
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

        public ClassResponse.RES_RESULT GetHistoryDayList(ClassRequest.REQ_HISTORYDATE parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "History", "history_date");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetHistoryDayList  ==========", sModuleName);
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

            ClassStruct.ST_HISTORYDAYLIST historyList = new ClassStruct.ST_HISTORYDAYLIST();
            
            ClassStruct.ST_HISTORYDAYINFO estrusInfo = new ClassStruct.ST_HISTORYDAYINFO();
            ClassStruct.ST_HISTORYDAYINFO inseminateInfo = new ClassStruct.ST_HISTORYDAYINFO();
            ClassStruct.ST_HISTORYDAYINFO appraisalInfo = new ClassStruct.ST_HISTORYDAYINFO();
            ClassStruct.ST_HISTORYDAYINFO dryupInfo = new ClassStruct.ST_HISTORYDAYINFO();
            ClassStruct.ST_HISTORYDAYINFO calveInfo = new ClassStruct.ST_HISTORYDAYINFO();

            List<ClassStruct.ST_HISTORYDAYENTITY> estrusList = new List<ClassStruct.ST_HISTORYDAYENTITY>();
            List<ClassStruct.ST_HISTORYDAYENTITY> inseminateList = new List<ClassStruct.ST_HISTORYDAYENTITY>();
            List<ClassStruct.ST_HISTORYDAYENTITY> appraisalList = new List<ClassStruct.ST_HISTORYDAYENTITY>();
            List<ClassStruct.ST_HISTORYDAYENTITY> dryupList = new List<ClassStruct.ST_HISTORYDAYENTITY>();
            List<ClassStruct.ST_HISTORYDAYENTITY> calveList = new List<ClassStruct.ST_HISTORYDAYENTITY>();

            try
            {
                switch (parameters.lang_code)
                {
                    case "KR":
                        {
                            estrusInfo.breed_code = "ED";
                            estrusInfo.breed_title = "발정일정";

                            inseminateInfo.breed_code = "ID";
                            inseminateInfo.breed_title = "수정일정";

                            appraisalInfo.breed_code = "AD";
                            appraisalInfo.breed_title = "임신감정일정";

                            dryupInfo.breed_code = "DD";
                            dryupInfo.breed_title = "건유일정";

                            calveInfo.breed_code = "CD";
                            calveInfo.breed_title = "분만일정";

                            break;
                        }
                    case "JP":
                        {
                            estrusInfo.breed_code = "ED";
                            estrusInfo.breed_title = "発情日程";

                            inseminateInfo.breed_code = "ID";
                            inseminateInfo.breed_title = "授精日程 ";

                            appraisalInfo.breed_code = "AD";
                            appraisalInfo.breed_title = "妊娠鑑定日程";

                            dryupInfo.breed_code = "DD";
                            dryupInfo.breed_title = "乾乳日程";

                            calveInfo.breed_code = "CD";
                            calveInfo.breed_title = "分娩日程";

                            break;
                        }
                    case "US":
                        {
                            estrusInfo.breed_code = "ED";
                            estrusInfo.breed_title = "Estrus schedule";

                            inseminateInfo.breed_code = "ID";
                            inseminateInfo.breed_title = "Insemination schedule";

                            appraisalInfo.breed_code = "AD";
                            appraisalInfo.breed_title = "Diagnosis schedule";

                            dryupInfo.breed_code = "DD";
                            dryupInfo.breed_title = "Dry up schedule";

                            calveInfo.breed_code = "CD";
                            calveInfo.breed_title = "Calving schedule";

                            break;
                        }
                    case "CN":
                        {
                            estrusInfo.breed_code = "ED";
                            estrusInfo.breed_title = "发情日程";

                            inseminateInfo.breed_code = "ID";
                            inseminateInfo.breed_title = "授精日程";

                            appraisalInfo.breed_code = "AD";
                            appraisalInfo.breed_title = "鉴定日程";

                            dryupInfo.breed_code = "DD";
                            dryupInfo.breed_title = "干奶日程";

                            calveInfo.breed_code = "CD";
                            calveInfo.breed_title = "分娩日程";

                            break;
                        }
                    case "PT":
                        {
                            estrusInfo.breed_code = "ED";
                            estrusInfo.breed_title = "Cronogr. Estro";

                            inseminateInfo.breed_code = "ID";
                            inseminateInfo.breed_title = "Cronogr. Inseminação";

                            appraisalInfo.breed_code = "AD";
                            appraisalInfo.breed_title = "Cronogr. Diagnóstico";

                            dryupInfo.breed_code = "DD";
                            dryupInfo.breed_title = "Cronogr. Secagem";

                            calveInfo.breed_code = "CD";
                            calveInfo.breed_title = "Cronogr. Parto";

                            break;
                        }
                    case "BR":
                        {
                            estrusInfo.breed_code = "ED";
                            estrusInfo.breed_title = "Cronogr. Estro";

                            inseminateInfo.breed_code = "ID";
                            inseminateInfo.breed_title = "Cronogr. Inseminação";

                            appraisalInfo.breed_code = "AD";
                            appraisalInfo.breed_title = "Cronogr. Diagnóstico";

                            dryupInfo.breed_code = "DD";
                            dryupInfo.breed_title = "Cronogr. Secagem";

                            calveInfo.breed_code = "CD";
                            calveInfo.breed_title = "Cronogr. Parto";

                            break;
                        }
                    default:
                        {
                            estrusInfo.breed_code = "ED";
                            estrusInfo.breed_title = "Estrus schedule";

                            inseminateInfo.breed_code = "ID";
                            inseminateInfo.breed_title = "Insemination schedule";

                            appraisalInfo.breed_code = "AD";
                            appraisalInfo.breed_title = "Diagnosis schedule";

                            dryupInfo.breed_code = "DD";
                            dryupInfo.breed_title = "Dry off schedule";

                            calveInfo.breed_code = "CD";
                            calveInfo.breed_title = "Calving schedule";

                            break;
                        }
                }

                string sQuery;
                switch (parameters.lang_code)
                {
                    case "KR":
                        sQuery = "SELECT A.BREED_TYPE, A.ENTITY_SEQ, A.BREED_METHOD, A.BREED_DUE_DATE, A.ADDED_DUE_DATE1, A.ADDED_DUE_DATE2, " +
                                 "       B.ENTITY_NO, B.IMAGE_URL, C.CODE_NAME AS INSEMINATE_METHOD, D.CODE_NAME AS APPRAISAL_METHOD ";
                        break;
                    case "JP":
                        sQuery = "SELECT A.BREED_TYPE, A.ENTITY_SEQ, A.BREED_METHOD, A.BREED_DUE_DATE, A.ADDED_DUE_DATE1, A.ADDED_DUE_DATE2, " +
                                 "       B.ENTITY_NO, B.IMAGE_URL, C.JP_VALUE AS INSEMINATE_METHOD, D.JP_VALUE AS APPRAISAL_METHOD ";
                        break;
                    case "US":
                        sQuery = "SELECT A.BREED_TYPE, A.ENTITY_SEQ, A.BREED_METHOD, A.BREED_DUE_DATE, A.ADDED_DUE_DATE1, A.ADDED_DUE_DATE2, " +
                                 "       B.ENTITY_NO, B.IMAGE_URL, C.EN_VALUE AS INSEMINATE_METHOD, D.EN_VALUE AS APPRAISAL_METHOD ";
                        break;
                    case "CN":
                        sQuery = "SELECT A.BREED_TYPE, A.ENTITY_SEQ, A.BREED_METHOD, A.BREED_DUE_DATE, A.ADDED_DUE_DATE1, A.ADDED_DUE_DATE2, " +
                                 "       B.ENTITY_NO, B.IMAGE_URL, C.ZH_VALUE AS INSEMINATE_METHOD, D.ZH_VALUE AS APPRAISAL_METHOD ";
                        break;
                    case "PT":
                        sQuery = "SELECT A.BREED_TYPE, A.ENTITY_SEQ, A.BREED_METHOD, A.BREED_DUE_DATE, A.ADDED_DUE_DATE1, A.ADDED_DUE_DATE2, " +
                                 "       B.ENTITY_NO, B.IMAGE_URL, C.PT_VALUE AS INSEMINATE_METHOD, D.PT_VALUE AS APPRAISAL_METHOD ";
                        break;
                    case "BR":
                        sQuery = "SELECT A.BREED_TYPE, A.ENTITY_SEQ, A.BREED_METHOD, A.BREED_DUE_DATE, A.ADDED_DUE_DATE1, A.ADDED_DUE_DATE2, " +
                                 "       B.ENTITY_NO, B.IMAGE_URL, C.PT_VALUE AS INSEMINATE_METHOD, D.PT_VALUE AS APPRAISAL_METHOD ";
                        break;
                    default:
                        sQuery = "SELECT A.BREED_TYPE, A.ENTITY_SEQ, A.BREED_METHOD, A.BREED_DUE_DATE, A.ADDED_DUE_DATE1, A.ADDED_DUE_DATE2, " +
                                 "       B.ENTITY_NO, B.IMAGE_URL, C.EN_VALUE AS INSEMINATE_METHOD, D.EN_VALUE AS APPRAISAL_METHOD ";
                        break;
                }

                sQuery += string.Format("  FROM UDF_HISTORY_LASTDATA({0}) A " +
                                        "  LEFT OUTER JOIN ENTITY_NEW_INFO B " +
                                        "    ON A.ENTITY_SEQ = B.SEQ " +
                                        "   AND B.FLAG = 'Y' " +
                                        "   AND B.ACTIVE_FLAG = 'Y' " +
                                        "  LEFT OUTER JOIN CODE_MST C " +
                                        "    ON A.BREED_TYPE = 'I' " +
                                        "   AND C.CODE_DIV = '140' " +
                                        "   AND A.BREED_METHOD = C.CODE_NO " +
                                        "   AND C.FLAG = 'Y' " +
                                        "  LEFT OUTER JOIN CODE_MST D " +
                                        "    ON A.BREED_TYPE = 'A' " +
                                        "   AND D.CODE_DIV = '150' " +
                                        "   AND A.BREED_METHOD = D.CODE_NO " +
                                        "   AND D.FLAG = 'Y' " +
                                        " WHERE A.FARM_SEQ = {0} " +
                                        "   AND (CONVERT(CHAR(10), A.BREED_DUE_DATE, 120) = '{1}' " +
                                        "    OR  CONVERT(CHAR(10), A.ADDED_DUE_DATE1, 120) = '{1}' " +
                                        "    OR  CONVERT(CHAR(10), A.ADDED_DUE_DATE2, 120) = '{1}') " +
                                        "   AND A.FLAG = 'Y' " +
                                        "   AND B.ENTITY_NO IS NOT NULL", parameters.farm_seq, parameters.search_date);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            string sBreedCode = dataReader.GetString(0);
                            string sBreedDueDate = _mClassDatabase.GetSafeDateTime(dataReader, 3);
                            string sAddedDue1Date = _mClassDatabase.GetSafeDateTime(dataReader, 4);
                            string sAddedDue2Date = _mClassDatabase.GetSafeDateTime(dataReader, 5);

                            ClassStruct.ST_HISTORYDAYENTITY breedInfo = new ClassStruct.ST_HISTORYDAYENTITY();

                            if (sBreedCode == "E")
                            {
                                // 예정일이 기간에 있으면 발정예정일
                                if (!string.IsNullOrEmpty(sBreedDueDate))
                                {
                                    if (Convert.ToDateTime(sBreedDueDate.Substring(0, 10)) == Convert.ToDateTime(parameters.search_date))
                                    {
                                        breedInfo.entity_seq = dataReader.GetInt32(1);
                                        breedInfo.entity_id = _mClassDatabase.GetSafeString(dataReader, 6);
                                        breedInfo.image_url = _mClassDatabase.GetSafeString(dataReader, 7);
                                        breedInfo.breed_date = parameters.search_date;
                                        breedInfo.breed_day = 0;
                                        breedInfo.breed_method = string.Empty;

                                        estrusList.Add(breedInfo);
                                    }
                                }

                                // 예정일 1일 기간에 있으면 수정예정일
                                if (!string.IsNullOrEmpty(sAddedDue1Date))
                                {
                                    if (Convert.ToDateTime(sAddedDue1Date.Substring(0, 10)) == Convert.ToDateTime(parameters.search_date))
                                    {
                                        breedInfo.entity_seq = dataReader.GetInt32(1);
                                        breedInfo.entity_id = _mClassDatabase.GetSafeString(dataReader, 6);
                                        breedInfo.image_url = _mClassDatabase.GetSafeString(dataReader, 7);
                                        breedInfo.breed_date = parameters.search_date;
                                        breedInfo.breed_day = 0;
                                        breedInfo.breed_method = string.Empty;

                                        inseminateList.Add(breedInfo);
                                    }
                                }
                            }
                            else if (sBreedCode == "I")
                            {
                                // 예정일 1일 기간에 있으면 감정예정일
                                if (!string.IsNullOrEmpty(sAddedDue1Date))
                                {
                                    if (Convert.ToDateTime(sAddedDue1Date.Substring(0, 10)) == Convert.ToDateTime(parameters.search_date))
                                    {
                                        breedInfo.entity_seq = dataReader.GetInt32(1);
                                        breedInfo.entity_id = _mClassDatabase.GetSafeString(dataReader, 6);
                                        breedInfo.image_url = _mClassDatabase.GetSafeString(dataReader, 7);
                                        breedInfo.breed_date = parameters.search_date;
                                        breedInfo.breed_day = 0;
                                        breedInfo.breed_method = _mClassDatabase.GetSafeString(dataReader, 8);

                                        appraisalList.Add(breedInfo);
                                    }
                                }
                            }
                            else if (sBreedCode == "A")
                            {
                                // 예정일 1이 기간에 있으면 분만예정일
                                if (!string.IsNullOrEmpty(sAddedDue1Date))
                                {
                                    if (Convert.ToDateTime(sAddedDue1Date.Substring(0, 10)) == Convert.ToDateTime(parameters.search_date))
                                    {
                                        breedInfo.entity_seq = dataReader.GetInt32(1);
                                        breedInfo.entity_id = _mClassDatabase.GetSafeString(dataReader, 6);
                                        breedInfo.image_url = _mClassDatabase.GetSafeString(dataReader, 7);
                                        breedInfo.breed_date = parameters.search_date;
                                        breedInfo.breed_day = 0;
                                        breedInfo.breed_method = _mClassDatabase.GetSafeString(dataReader, 9);

                                        calveList.Add(breedInfo);
                                    }
                                }

                                // 예정일 2가 기간에 있으면 건유예정일
                                if (!string.IsNullOrEmpty(sAddedDue2Date))
                                {
                                    if (Convert.ToDateTime(sAddedDue2Date.Substring(0, 10)) == Convert.ToDateTime(parameters.search_date))
                                    {
                                        breedInfo.entity_seq = dataReader.GetInt32(1);
                                        breedInfo.entity_id = _mClassDatabase.GetSafeString(dataReader, 6);
                                        breedInfo.image_url = _mClassDatabase.GetSafeString(dataReader, 7);
                                        breedInfo.breed_date = parameters.search_date;
                                        breedInfo.breed_day = 0;
                                        breedInfo.breed_method = string.Empty;

                                        dryupList.Add(breedInfo);
                                    }
                                }
                            }
                            else if (sBreedCode == "D")
                            {
                                // 예정일 1이 기간에 있으면 분만예정일
                                if (!string.IsNullOrEmpty(sAddedDue1Date))
                                {
                                    if (Convert.ToDateTime(sAddedDue1Date.Substring(0, 10)) == Convert.ToDateTime(parameters.search_date))
                                    {
                                        breedInfo.entity_seq = dataReader.GetInt32(1);
                                        breedInfo.entity_id = _mClassDatabase.GetSafeString(dataReader, 6);
                                        breedInfo.image_url = _mClassDatabase.GetSafeString(dataReader, 7);
                                        breedInfo.breed_date = parameters.search_date;
                                        breedInfo.breed_day = 0;
                                        breedInfo.breed_method = _mClassDatabase.GetSafeString(dataReader, 9);

                                        calveList.Add(breedInfo);
                                    }
                                }
                            }
                            else if (sBreedCode == "C")
                            {
                                if (!string.IsNullOrEmpty(sBreedDueDate))
                                {
                                    if (Convert.ToDateTime(sBreedDueDate.Substring(0, 10)) == Convert.ToDateTime(parameters.search_date))
                                    {
                                        breedInfo.entity_seq = dataReader.GetInt32(1);
                                        breedInfo.entity_id = _mClassDatabase.GetSafeString(dataReader, 6);
                                        breedInfo.image_url = _mClassDatabase.GetSafeString(dataReader, 7);
                                        breedInfo.breed_date = parameters.search_date;
                                        breedInfo.breed_day = 0;
                                        breedInfo.breed_method = string.Empty;

                                        estrusList.Add(breedInfo);
                                    }
                                }
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

        public ClassResponse.RES_RESULT SetEstrus(ClassRequest.REQ_ESTRUS parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "History", "estrus");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START SetEstrus  ==========", sModuleName);
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

                if (string.IsNullOrEmpty(parameters.uid))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_UID,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_UID),
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

                if (parameters.entity_seq < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_ENTITY_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_ENTITY_SEQ),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (string.IsNullOrEmpty(parameters.estrus_date))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_ESTRUS_DATE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_ESTRUS_DATE),
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

            try
            {
                string sEntityNo = string.Empty;

                string sQuery = string.Format("SELECT SEQ, ENTITY_NO " +
                                       "  FROM ENTITY_NEW_INFO " +
                                       " WHERE SEQ = {0} " +
                                       "   AND FARM_SEQ = {1} " +
                                       "   AND FLAG = 'Y' " +
                                       "   AND ACTIVE_FLAG = 'Y' ", parameters.entity_seq, parameters.farm_seq);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        dataReader.Read();
                        sEntityNo = dataReader.GetString(1);
                    }
                    else
                    {
                        isError = true;

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SEARCH_NOTEXIST_ENTITY,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SEARCH_NOTEXIST_ENTITY),
                            data = string.Empty
                        };
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
                    _mClassDatabase.TransBegin();

                    string sMemo = parameters.memo;
                    sMemo = sMemo.Replace("'", "''");
                    sMemo = sMemo.Replace(@"\", @"\\");

                    // 등록과 수정을 구분해서 처리한다
                    // 등록시에는 사용자 계정정보를 등록한다
                    if (parameters.breed_seq == 0)
                    {
                        sQuery = "INSERT INTO BREED_HISTORY " +
                                 " (FARM_SEQ, ENTITY_SEQ, ENTITY_NO, BREED_TYPE, BREED_DATE, BREED_DUE_DATE, ADDED_METHOD, ADDED_DUE_DATE1, MEMO, USER_ID) " +
                                 " VALUES ( ";
                        sQuery += parameters.farm_seq + ", ";
                        sQuery += parameters.entity_seq + ", ";
                        sQuery += "'" + sEntityNo + "', ";
                        sQuery += "'E', ";
                        sQuery += "'" + parameters.estrus_date + "', ";
                        if (string.IsNullOrEmpty(parameters.estrus_due_date)) sQuery += "NULL, ";
                        else sQuery += "'" + parameters.estrus_due_date + "', ";
                        sQuery += parameters.inseminate_code + ", ";
                        if (string.IsNullOrEmpty(parameters.due_date)) sQuery += "NULL, ";
                        else sQuery += "'" + parameters.due_date + "', ";
                        if (string.IsNullOrEmpty(parameters.memo)) sQuery += "NULL, ";
                        else sQuery += "N'" + sMemo + "', ";
                        sQuery += "'" + parameters.uid + "' ";
                        sQuery += " )";
                    }
                    else
                    {
                        sQuery = "UPDATE BREED_HISTORY SET ";
                        sQuery += "BREED_DATE = '" + parameters.estrus_date + "', ";
                        if (string.IsNullOrEmpty(parameters.estrus_due_date)) sQuery += "BREED_DUE_DATE = NULL, ";
                        else sQuery += "BREED_DUE_DATE = '" + parameters.estrus_due_date + "', ";
                        sQuery += "ADDED_METHOD = " + parameters.inseminate_code + ", ";
                        if (string.IsNullOrEmpty(parameters.due_date)) sQuery += "ADDED_DUE_DATE1 = NULL, ";
                        else sQuery += "ADDED_DUE_DATE1 = '" + parameters.due_date + "', ";
                        if (string.IsNullOrEmpty(parameters.memo)) sQuery += "MEMO = NULL ";
                        else sQuery += "MEMO = N'" + sMemo + "'";
                        sQuery += string.Format(" WHERE SEQ = {0}", parameters.breed_seq);
                    }

                    int count = _mClassDatabase.QueryExecute(sQuery);

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
                    }
                }

                // 마지막 번식이력 정보로 개체정보의 임신상태를 수정한다
                // 발정 : 미임신
                // 수정 : 미임신
                // 감정 : 임신 => 임신 / 그 외 : 미임신
                // 건유 : 임신 => 임신 / 그 외 : 미임신
                // 분만 : 미임신
                string sLastBreedType = string.Empty;
                string sLastBreedDate = string.Empty;
                string sLastTextValue = string.Empty;

                if (!isError)
                {
                    // 마지막 번식이력 정보 추출
                    sQuery = string.Format("SELECT TOP 1 BREED_TYPE, BREED_DATE, BREED_TEXT_VALUE " +
                                           "  FROM BREED_HISTORY " +
                                           " WHERE FARM_SEQ = {0} " +
                                           "   AND ENTITY_SEQ = {1} " +
                                           "   AND FLAG = 'Y' " +
                                           " ORDER BY BREED_DATE DESC, SEQ DESC ", parameters.farm_seq, parameters.entity_seq);

                    if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                    {
                        if (dataReader.HasRows)
                        {
                            dataReader.Read();
                            sLastBreedType = dataReader.GetString(0);
                            sLastBreedDate = dataReader.GetDateTime(1).ToString("yyyy-MM-dd");
                            sLastTextValue = _mClassDatabase.GetSafeString(dataReader, 2);
                        }

                        dataReader.Close();
                        dataReader.Dispose();
                    }
                    else
                    {
                        isError = true;
                        _mClassDatabase.TransRollback();

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
                    int nPregnancyCode = 2;

                    switch (sLastBreedType)
                    {
                        case "E": nPregnancyCode = 2; break;
                        case "I": nPregnancyCode = 2; break;
                        case "A": nPregnancyCode = sLastTextValue == "Y" ? 1 : 2; break;
                        case "D": nPregnancyCode = sLastTextValue == "Y" ? 1 : 2; break;
                        case "C": nPregnancyCode = 2; break;
                    }

                    // 개체의 임신상태를 수정한다
                    sQuery = string.Format("UPDATE ENTITY_NEW_INFO SET PREGNANCY_CODE = {1} WHERE SEQ = {0}", parameters.entity_seq, nPregnancyCode);

                    int count = _mClassDatabase.QueryExecute(sQuery);

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
                isError = true;
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

        public ClassResponse.RES_RESULT SetInseminate(ClassRequest.REQ_INSEMINNATE parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "History", "inseminate");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START SetInseminate  ==========", sModuleName);
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

                if (string.IsNullOrEmpty(parameters.uid))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_UID,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_UID),
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

                if (parameters.entity_seq < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_ENTITY_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_ENTITY_SEQ),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (string.IsNullOrEmpty(parameters.inseminate_date))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_INSEMINATE_DATE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_INSEMINATE_DATE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (parameters.inseminate_code < 0)
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

            try
            {
                string sEntityNo = string.Empty;

                string sQuery = string.Format("SELECT SEQ, ENTITY_NO " +
                                       "  FROM ENTITY_NEW_INFO " +
                                       " WHERE SEQ = {0} " +
                                       "   AND FARM_SEQ = {1} " +
                                       "   AND FLAG = 'Y' " +
                                       "   AND ACTIVE_FLAG = 'Y' ", parameters.entity_seq, parameters.farm_seq);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        dataReader.Read();
                        sEntityNo = dataReader.GetString(1);
                    }
                    else
                    {
                        isError = true;

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SEARCH_NOTEXIST_ENTITY,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SEARCH_NOTEXIST_ENTITY),
                            data = string.Empty
                        };
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

                // 정액번호가 문자열로 들어오기 때문에 코드 값을 추출한다
                // 만일 등록되어 있지 않은 경우에는 등록한 후 코드 값을 추출한다
                int nSemenSeq = 0;

                if (!isError && !string.IsNullOrEmpty(parameters.semen_no))
                {
                    sQuery = string.Format("SELECT SEQ FROM FARM_SEMEN " +
                                           " WHERE FARM_SEQ = {0} " +
                                           "   AND SEMEN_NO = '{1}' " +
                                           "   AND FLAG = 'Y' ", parameters.farm_seq, parameters.semen_no);

                    if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                    {
                        if (dataReader.HasRows)
                        {
                            dataReader.Read();
                            nSemenSeq = dataReader.GetInt32(0);
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

                    if (!isError && nSemenSeq == 0)
                    {
                        sQuery = string.Format("INSERT INTO FARM_SEMEN (FARM_SEQ, SEMEN_NO) " +
                                               " OUTPUT INSERTED.SEQ VALUES ({0}, '{1}')", parameters.farm_seq, parameters.semen_no);

                        nSemenSeq = _mClassDatabase.QueryExecuteScalar(sQuery);

                        if (nSemenSeq < 1)
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
                }

                if (!isError)
                {
                    _mClassDatabase.TransBegin();

                    string sMemo = parameters.memo;
                    sMemo = sMemo.Replace("'", "''");
                    sMemo = sMemo.Replace(@"\", @"\\");

                    // 등록과 수정을 구분해서 처리한다
                    // 등록시에는 사용자 계정정보를 등록한다
                    if (parameters.breed_seq == 0)
                    {
                        sQuery = "INSERT INTO BREED_HISTORY " +
                                 " (FARM_SEQ, ENTITY_SEQ, ENTITY_NO, BREED_TYPE, BREED_DATE, BREED_METHOD, BREED_INT_VALUE1, BREED_INT_VALUE2, ADDED_METHOD, ADDED_DUE_DATE1, MEMO, USER_ID) " +
                                 " VALUES ( ";
                        sQuery += parameters.farm_seq + ", ";
                        sQuery += parameters.entity_seq + ", ";
                        sQuery += "'" + sEntityNo + "', ";
                        sQuery += "'I', ";
                        sQuery += "'" + parameters.inseminate_date + "', ";
                        sQuery += parameters.inseminate_code + ", ";
                        sQuery += parameters.inseminate_count + ", ";
                        sQuery += nSemenSeq + ", ";
                        sQuery += parameters.appraisal_code + ", ";
                        if (string.IsNullOrEmpty(parameters.due_date)) sQuery += "NULL, ";
                        else sQuery += "'" + parameters.due_date + "', ";
                        if (string.IsNullOrEmpty(parameters.memo)) sQuery += "NULL, ";
                        else sQuery += "N'" + sMemo + "', ";
                        sQuery += "'" + parameters.uid + "' ";
                        sQuery += " )";
                    }
                    else
                    {
                        sQuery = "UPDATE BREED_HISTORY SET ";
                        sQuery += "BREED_DATE = '" + parameters.inseminate_date + "', ";
                        sQuery += "BREED_METHOD = " + parameters.inseminate_code + ", ";
                        sQuery += "BREED_INT_VALUE1 = " + parameters.inseminate_count + ", ";
                        sQuery += "BREED_INT_VALUE2 = " + nSemenSeq + ", ";
                        sQuery += "ADDED_METHOD = " + parameters.appraisal_code + ", ";
                        if (string.IsNullOrEmpty(parameters.due_date)) sQuery += "ADDED_DUE_DATE1 = NULL, ";
                        else sQuery += "ADDED_DUE_DATE1 = '" + parameters.due_date + "', ";
                        if (string.IsNullOrEmpty(parameters.memo)) sQuery += "MEMO = NULL ";
                        else sQuery += "MEMO = N'" + sMemo + "'";
                        sQuery += string.Format(" WHERE SEQ = {0}", parameters.breed_seq);
                    }

                    int count = _mClassDatabase.QueryExecute(sQuery);

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
                    }
                }

                // 마지막 번식이력 정보로 개체정보의 임신상태를 수정한다
                // 발정 : 미임신
                // 수정 : 미임신
                // 감정 : 임신 => 임신 / 그 외 : 미임신
                // 건유 : 임신 => 임신 / 그 외 : 미임신
                // 분만 : 미임신
                string sLastBreedType = string.Empty;
                string sLastBreedDate = string.Empty;
                string sLastTextValue = string.Empty;

                if (!isError)
                {
                    // 마지막 번식이력 정보 추출
                    sQuery = string.Format("SELECT TOP 1 BREED_TYPE, BREED_DATE, BREED_TEXT_VALUE " +
                                           "  FROM BREED_HISTORY " +
                                           " WHERE FARM_SEQ = {0} " +
                                           "   AND ENTITY_SEQ = {1} " +
                                           "   AND FLAG = 'Y' " +
                                           " ORDER BY BREED_DATE DESC, SEQ DESC ", parameters.farm_seq, parameters.entity_seq);

                    if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                    {
                        if (dataReader.HasRows)
                        {
                            dataReader.Read();
                            sLastBreedType = dataReader.GetString(0);
                            sLastBreedDate = dataReader.GetDateTime(1).ToString("yyyy-MM-dd");
                            sLastTextValue = _mClassDatabase.GetSafeString(dataReader, 2);
                        }

                        dataReader.Close();
                        dataReader.Dispose();
                    }
                    else
                    {
                        isError = true;
                        _mClassDatabase.TransRollback();

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
                    int nPregnancyCode = 2;

                    switch (sLastBreedType)
                    {
                        case "E": nPregnancyCode = 2; break;
                        case "I": nPregnancyCode = 2; break;
                        case "A": nPregnancyCode = sLastTextValue == "Y" ? 1 : 2; break;
                        case "D": nPregnancyCode = sLastTextValue == "Y" ? 1 : 2; break;
                        case "C": nPregnancyCode = 2; break;
                    }

                    // 개체의 임신상태를 수정한다
                    sQuery = string.Format("UPDATE ENTITY_NEW_INFO SET PREGNANCY_CODE = {1} WHERE SEQ = {0}", parameters.entity_seq, nPregnancyCode);

                    int count = _mClassDatabase.QueryExecute(sQuery);

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

        public ClassResponse.RES_RESULT SetAppraisal(ClassRequest.REQ_APPRAISAL parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "History", "appraisal");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START SetAppraisal  ==========", sModuleName);
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

                if (string.IsNullOrEmpty(parameters.uid))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_UID,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_UID),
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

                if (parameters.entity_seq < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_ENTITY_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_ENTITY_SEQ),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (string.IsNullOrEmpty(parameters.appraisal_date))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_APPRAISAL_DATE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_APPRAISAL_DATE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (string.IsNullOrEmpty(parameters.pregnancy_flag))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_PREGNANCY_FLAG,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_PREGNANCY_FLAG),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }
                else if (parameters.pregnancy_flag != "Y" && parameters.pregnancy_flag != "N" && parameters.pregnancy_flag != "E")
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_PREGNANCY_FLAG,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_PREGNANCY_FLAG),
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

            try
            {
                string sEntityNo = string.Empty;

                string sQuery = string.Format("SELECT SEQ, ENTITY_NO " +
                           "  FROM ENTITY_NEW_INFO " +
                           " WHERE SEQ = {0} " +
                           "   AND FARM_SEQ = {1} " +
                           "   AND FLAG = 'Y' " +
                           "   AND ACTIVE_FLAG = 'Y' ", parameters.entity_seq, parameters.farm_seq);
                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        dataReader.Read();
                        sEntityNo = dataReader.GetString(1);
                    }
                    else
                    {
                        isError = true;

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SEARCH_NOTEXIST_ENTITY,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SEARCH_NOTEXIST_ENTITY),
                            data = string.Empty
                        };
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
                    _mClassDatabase.TransBegin();

                    string sMemo = parameters.memo;
                    sMemo = sMemo.Replace("'", "''");
                    sMemo = sMemo.Replace(@"\", @"\\");

                    // 등록과 수정을 구분해서 처리한다
                    // 등록시에는 사용자 계정정보를 등록한다
                    if (parameters.breed_seq == 0)
                    {
                        sQuery = "INSERT INTO BREED_HISTORY " +
                                 " (FARM_SEQ, ENTITY_SEQ, ENTITY_NO, BREED_TYPE, BREED_DATE, BREED_METHOD, BREED_TEXT_VALUE, ADDED_METHOD, ADDED_DATE, ADDED_DUE_DATE1, ADDED_DUE_DATE2, MEMO, USER_ID) " +
                                 " VALUES ( ";
                        sQuery += parameters.farm_seq + ", ";
                        sQuery += parameters.entity_seq + ", ";
                        sQuery += "'" + sEntityNo + "', ";
                        sQuery += "'A', ";
                        sQuery += "'" + parameters.appraisal_date + "', ";
                        sQuery += parameters.appraisal_code + ", ";
                        sQuery += "'" + parameters.pregnancy_flag + "', ";
                        sQuery += parameters.inseminate_code + ", ";
                        if (string.IsNullOrEmpty(parameters.inseminate_date)) sQuery += "NULL, ";
                        else sQuery += "'" + parameters.inseminate_date + "', ";
                        if (string.IsNullOrEmpty(parameters.calve_due_date)) sQuery += "NULL, ";
                        else sQuery += "'" + parameters.calve_due_date + "', ";
                        if (string.IsNullOrEmpty(parameters.dryup_due_date)) sQuery += "NULL, ";
                        else sQuery += "'" + parameters.dryup_due_date + "', ";
                        if (string.IsNullOrEmpty(parameters.memo)) sQuery += "NULL, ";
                        else sQuery += "N'" + sMemo + "', ";
                        sQuery += "'" + parameters.uid + "' ";
                        sQuery += " )";
                    }
                    else
                    {
                        sQuery = "UPDATE BREED_HISTORY SET ";
                        sQuery += "BREED_DATE = '" + parameters.appraisal_date + "', ";
                        sQuery += "BREED_METHOD = " + parameters.appraisal_code + ", ";
                        sQuery += "BREED_TEXT_VALUE = '" + parameters.pregnancy_flag + "', ";
                        sQuery += "ADDED_METHOD = " + parameters.inseminate_code + ", ";
                        if (string.IsNullOrEmpty(parameters.inseminate_date)) sQuery += "ADDED_DATE = NULL, ";
                        else sQuery += "ADDED_DATE = '" + parameters.inseminate_date + "', ";
                        if (string.IsNullOrEmpty(parameters.calve_due_date)) sQuery += "ADDED_DUE_DATE1 = NULL, ";
                        else sQuery += "ADDED_DUE_DATE1 = '" + parameters.calve_due_date + "', ";
                        if (string.IsNullOrEmpty(parameters.dryup_due_date)) sQuery += "ADDED_DUE_DATE2 = NULL, ";
                        else sQuery += "ADDED_DUE_DATE2 = '" + parameters.dryup_due_date + "', ";
                        if (string.IsNullOrEmpty(parameters.memo)) sQuery += "MEMO = NULL ";
                        else sQuery += "MEMO = N'" + sMemo + "'";
                        sQuery += string.Format(" WHERE SEQ = {0}", parameters.breed_seq);
                    }

                    int count = _mClassDatabase.QueryExecute(sQuery);

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
                    }
                }

                // 마지막 번식이력 정보로 개체정보의 임신상태를 수정한다
                // 발정 : 미임신
                // 수정 : 미임신
                // 감정 : 임신 => 임신 / 그 외 : 미임신
                // 건유 : 임신 => 임신 / 그 외 : 미임신
                // 분만 : 미임신
                string sLastBreedType = string.Empty;
                string sLastBreedDate = string.Empty;
                string sLastTextValue = string.Empty;

                if (!isError)
                {
                    // 마지막 번식이력 정보 추출
                    sQuery = string.Format("SELECT TOP 1 BREED_TYPE, BREED_DATE, BREED_TEXT_VALUE " +
                                           "  FROM BREED_HISTORY " +
                                           " WHERE FARM_SEQ = {0} " +
                                           "   AND ENTITY_SEQ = {1} " +
                                           "   AND FLAG = 'Y' " +
                                           " ORDER BY BREED_DATE DESC, SEQ DESC ", parameters.farm_seq, parameters.entity_seq);

                    if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                    {
                        if (dataReader.HasRows)
                        {
                            dataReader.Read();
                            sLastBreedType = dataReader.GetString(0);
                            sLastBreedDate = dataReader.GetDateTime(1).ToString("yyyy-MM-dd");
                            sLastTextValue = _mClassDatabase.GetSafeString(dataReader, 2);
                        }

                        dataReader.Close();
                        dataReader.Dispose();
                    }
                    else
                    {
                        isError = true;
                        _mClassDatabase.TransRollback();

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
                    int nPregnancyCode = 2;

                    switch (sLastBreedType)
                    {
                        case "E": nPregnancyCode = 2; break;
                        case "I": nPregnancyCode = 2; break;
                        case "A": nPregnancyCode = sLastTextValue == "Y" ? 1 : 2; break;
                        case "D": nPregnancyCode = sLastTextValue == "Y" ? 1 : 2; break;
                        case "C": nPregnancyCode = 2; break;
                    }

                    // 개체의 임신상태를 수정한다
                    sQuery = string.Format("UPDATE ENTITY_NEW_INFO SET PREGNANCY_CODE = {1} WHERE SEQ = {0}", parameters.entity_seq, nPregnancyCode);

                    int count = _mClassDatabase.QueryExecute(sQuery);

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
                isError = true;
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

        public ClassResponse.RES_RESULT SetDryup(ClassRequest.REQ_DRYUP parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "History", "dryup");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START SetDryup  ==========", sModuleName);
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

                if (string.IsNullOrEmpty(parameters.uid))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_UID,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_UID),
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

                if (parameters.entity_seq < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_ENTITY_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_ENTITY_SEQ),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (string.IsNullOrEmpty(parameters.dryup_date))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_DRYUP_DATE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_DRYUP_DATE),
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

            try
            {
                string sEntityNo = string.Empty;

                string sQuery = string.Format("SELECT SEQ, ENTITY_NO " +
                           "  FROM ENTITY_NEW_INFO " +
                           " WHERE SEQ = {0} " +
                           "   AND FARM_SEQ = {1} " +
                           "   AND FLAG = 'Y' " +
                           "   AND ACTIVE_FLAG = 'Y' ", parameters.entity_seq, parameters.farm_seq);
                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        dataReader.Read();
                        sEntityNo = dataReader.GetString(1);
                    }
                    else
                    {
                        isError = true;

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SEARCH_NOTEXIST_ENTITY,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SEARCH_NOTEXIST_ENTITY),
                            data = string.Empty
                        };
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
                    _mClassDatabase.TransBegin();

                    string sMemo = parameters.memo;
                    sMemo = sMemo.Replace("'", "''");
                    sMemo = sMemo.Replace(@"\", @"\\");

                    // 등록과 수정을 구분해서 처리한다
                    // 등록시에는 사용자 계정정보를 등록한다
                    if (parameters.breed_seq == 0)
                    {
                        sQuery = "INSERT INTO BREED_HISTORY " +
                                 " (FARM_SEQ, ENTITY_SEQ, ENTITY_NO, BREED_TYPE, BREED_DATE, BREED_TEXT_VALUE, ADDED_METHOD, ADDED_DATE, ADDED_DUE_DATE1, MEMO, USER_ID) " +
                                 " VALUES ( ";
                        sQuery += parameters.farm_seq + ", ";
                        sQuery += parameters.entity_seq + ", ";
                        sQuery += "'" + sEntityNo + "', ";
                        sQuery += "'D', ";
                        sQuery += "'" + parameters.dryup_date + "', ";
                        if (string.IsNullOrEmpty(parameters.pregnancy_flag)) sQuery += "NULL, ";
                        else sQuery += "'" + parameters.pregnancy_flag + "', ";
                        sQuery += parameters.inseminate_code + ", ";
                        if (string.IsNullOrEmpty(parameters.inseminate_date)) sQuery += "NULL, ";
                        else sQuery += "'" + parameters.inseminate_date + "', ";
                        if (string.IsNullOrEmpty(parameters.calve_due_date)) sQuery += "NULL, ";
                        else sQuery += "'" + parameters.calve_due_date + "', ";
                        if (string.IsNullOrEmpty(parameters.memo)) sQuery += "NULL, ";
                        else sQuery += "N'" + sMemo + "', ";
                        sQuery += "'" + parameters.uid + "' ";
                        sQuery += " )";
                    }
                    else
                    {
                        sQuery = "UPDATE BREED_HISTORY SET ";
                        sQuery += "BREED_DATE = '" + parameters.dryup_date + "', ";
                        if (string.IsNullOrEmpty(parameters.pregnancy_flag)) sQuery += "BREED_TEXT_VALUE = NULL, ";
                        else sQuery += "BREED_TEXT_VALUE = '" + parameters.pregnancy_flag + "', ";
                        sQuery += "ADDED_METHOD = " + parameters.inseminate_code + ", ";
                        if (string.IsNullOrEmpty(parameters.inseminate_date)) sQuery += "ADDED_DATE = NULL, ";
                        else sQuery += "ADDED_DATE = '" + parameters.inseminate_date + "', ";
                        if (string.IsNullOrEmpty(parameters.calve_due_date)) sQuery += "ADDED_DUE_DATE1 = NULL, ";
                        else sQuery += "ADDED_DUE_DATE1 = '" + parameters.calve_due_date + "', ";
                        if (string.IsNullOrEmpty(parameters.memo)) sQuery += "MEMO = NULL ";
                        else sQuery += "MEMO = N'" + sMemo + "'";
                        sQuery += string.Format(" WHERE SEQ = {0}", parameters.breed_seq);
                    }

                    int count = _mClassDatabase.QueryExecute(sQuery);

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
                    }
                }

                // 마지막 번식이력 정보로 개체정보의 임신상태를 수정한다
                // 발정 : 미임신
                // 수정 : 미임신
                // 감정 : 임신 => 임신 / 그 외 : 미임신
                // 건유 : 임신 => 임신 / 그 외 : 미임신
                // 분만 : 미임신
                string sLastBreedType = string.Empty;
                string sLastBreedDate = string.Empty;
                string sLastTextValue = string.Empty;

                if (!isError)
                {
                    // 마지막 번식이력 정보 추출
                    sQuery = string.Format("SELECT TOP 1 BREED_TYPE, BREED_DATE, BREED_TEXT_VALUE " +
                                           "  FROM BREED_HISTORY " +
                                           " WHERE FARM_SEQ = {0} " +
                                           "   AND ENTITY_SEQ = {1} " +
                                           "   AND FLAG = 'Y' " +
                                           " ORDER BY BREED_DATE DESC, SEQ DESC ", parameters.farm_seq, parameters.entity_seq);

                    if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                    {
                        if (dataReader.HasRows)
                        {
                            dataReader.Read();
                            sLastBreedType = dataReader.GetString(0);
                            sLastBreedDate = dataReader.GetDateTime(1).ToString("yyyy-MM-dd");
                            sLastTextValue = _mClassDatabase.GetSafeString(dataReader, 2);
                        }

                        dataReader.Close();
                        dataReader.Dispose();
                    }
                    else
                    {
                        isError = true;
                        _mClassDatabase.TransRollback();

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
                    int nPregnancyCode = 2;

                    switch (sLastBreedType)
                    {
                        case "E": nPregnancyCode = 2; break;
                        case "I": nPregnancyCode = 2; break;
                        case "A": nPregnancyCode = sLastTextValue == "Y" ? 1 : 2; break;
                        case "D": nPregnancyCode = sLastTextValue == "Y" ? 1 : 2; break;
                        case "C": nPregnancyCode = 2; break;
                    }

                    // 개체의 임신상태를 수정한다
                    sQuery = string.Format("UPDATE ENTITY_NEW_INFO SET PREGNANCY_CODE = {1} WHERE SEQ = {0}", parameters.entity_seq, nPregnancyCode);

                    int count = _mClassDatabase.QueryExecute(sQuery);

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
                    }
                }

                int nEntityKind = 4;

                if (!isError)
                {
                    // 개체분류는 건유일 이후의 내역중에서 분만 또는 건유의 최근 번식이력을 추출한다
                    // 번식이력이 분만인 경우에는 착유우로 변경하고 그 외에는 건유우로 변경한다
                    sQuery = string.Format("SELECT TOP 1 BREED_TYPE, BREED_DATE " +
                                           "  FROM (SELECT SEQ, BREED_TYPE, BREED_DATE " +
                                           "          FROM BREED_HISTORY " +
                                           "         WHERE FARM_SEQ = {0} " +
                                           "           AND ENTITY_SEQ = {1} " +
                                           "           AND BREED_DATE > '{2}' " +
                                           "           AND FLAG = 'Y') A " +
                                           " WHERE BREED_TYPE = 'C' " +
                                           "    OR BREED_TYPE = 'D' " +
                                           " ORDER BY BREED_DATE DESC, SEQ DESC ", parameters.farm_seq, parameters.entity_seq, parameters.dryup_date);

                    if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                    {
                        if (dataReader.HasRows)
                        {
                            dataReader.Read();

                            if (dataReader.GetString(0) == "C") nEntityKind = 3;
                        }

                        dataReader.Close();
                        dataReader.Dispose();
                    }
                    else
                    {
                        isError = true;
                        _mClassDatabase.TransRollback();

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
                    sQuery = string.Format("UPDATE ENTITY_NEW_INFO SET ENTITY_KIND = {1} WHERE SEQ = {0}", parameters.entity_seq, nEntityKind);

                    int count = _mClassDatabase.QueryExecute(sQuery);

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
                isError = true;
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

        public ClassResponse.RES_RESULT SetCalve(ClassRequest.REQ_CALVE parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "History", "calve");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START SetCalve  ==========", sModuleName);
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

                if (string.IsNullOrEmpty(parameters.uid))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_UID,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_UID),
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

                if (parameters.entity_seq < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_ENTITY_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_ENTITY_SEQ),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (string.IsNullOrEmpty(parameters.calve_date))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_CALVE_DATE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_CALVE_DATE),
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

            try
            {
                string sEntityNo = string.Empty;
                int nEntityType = 0;

                string sQuery = string.Format("SELECT SEQ, ENTITY_NO, ENTITY_TYPE " +
                           "  FROM ENTITY_NEW_INFO " +
                           " WHERE SEQ = {0} " +
                           "   AND FARM_SEQ = {1} " +
                           "   AND FLAG = 'Y' " +
                           "   AND ACTIVE_FLAG = 'Y' ", parameters.entity_seq, parameters.farm_seq);
                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        dataReader.Read();

                        sEntityNo = dataReader.GetString(1);
                        nEntityType = dataReader.GetInt32(2);
                    }
                    else
                    {
                        isError = true;

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SEARCH_NOTEXIST_ENTITY,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SEARCH_NOTEXIST_ENTITY),
                            data = string.Empty
                        };
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

                // 분만등록인 경우에는 등록일자 기준으로 이전 분만이력과 이후 분만이력을 추출하고
                // 분만수정인 경우에는 등록일자 기준으로 이전 분만이력과 이후 분만이력을 추출하고
                //                    수정일자 기준으로 이전 분만이력과 이후 분만이력을 추출한다
                string sBreedDate = string.Empty;
                Tuple<int, string> paramPreCalveInfo = new Tuple<int, string>(0, string.Empty);
                Tuple<int, string> paramPosCalveInfo = new Tuple<int, string>(0, string.Empty);
                Tuple<int, string> regPreCalveInfo = new Tuple<int, string>(0, string.Empty);
                Tuple<int, string> regPosCalveInfo = new Tuple<int, string>(0, string.Empty);

                // 파라미터 기준으로 이전, 이후 분만일자를 추출한다
                // 수정인 경우에는 자기 자신의 데이타를 제외하고 처리한다
                if (!isError)
                {
                    if (parameters.breed_seq == 0)
                    {
                        sQuery = string.Format("SELECT 'PRE' AS DATA_TYPE, SEQ, BREED_DATE " +
                                               "  FROM (SELECT TOP 1 SEQ, BREED_DATE " +
                                               "          FROM BREED_HISTORY " +
                                               "         WHERE FARM_SEQ = {0} " +
                                               "           AND ENTITY_SEQ = {1} " +
                                               "           AND BREED_TYPE = 'C' " +
                                               "           AND BREED_DATE < '{2}' " +
                                               "           AND FLAG = 'Y' " +
                                               "         ORDER BY BREED_DATE DESC, SEQ DESC) A " +
                                               "UNION " +
                                               "SELECT 'POS' AS DATA_TYPE, SEQ, BREED_DATE " +
                                               "  FROM (SELECT TOP 1 SEQ, BREED_DATE " +
                                               "          FROM BREED_HISTORY " +
                                               "         WHERE FARM_SEQ = {0} " +
                                               "           AND ENTITY_SEQ = {1} " +
                                               "           AND BREED_TYPE = 'C' " +
                                               "           AND BREED_DATE > '{2}' " +
                                               "           AND FLAG = 'Y' " +
                                               "         ORDER BY BREED_DATE) A ", parameters.farm_seq, parameters.entity_seq, parameters.calve_date);
                    }
                    else
                    {
                        sQuery = string.Format("SELECT 'PRE' AS DATA_TYPE, SEQ, BREED_DATE " +
                                               "  FROM (SELECT TOP 1 SEQ, BREED_DATE " +
                                               "          FROM BREED_HISTORY " +
                                               "         WHERE FARM_SEQ = {0} " +
                                               "           AND ENTITY_SEQ = {1} " +
                                               "           AND SEQ <> {3} " +
                                               "           AND BREED_TYPE = 'C' " +
                                               "           AND BREED_DATE < '{2}' " +
                                               "           AND FLAG = 'Y' " +
                                               "         ORDER BY BREED_DATE DESC, SEQ DESC) A " +
                                               "UNION " +
                                               "SELECT 'POS' AS DATA_TYPE, SEQ, BREED_DATE " +
                                               "  FROM (SELECT TOP 1 SEQ, BREED_DATE " +
                                               "          FROM BREED_HISTORY " +
                                               "         WHERE FARM_SEQ = {0} " +
                                               "           AND ENTITY_SEQ = {1} " +
                                               "           AND SEQ <> {3} " +
                                               "           AND BREED_TYPE = 'C' " +
                                               "           AND BREED_DATE > '{2}' " +
                                               "           AND FLAG = 'Y' " +
                                               "         ORDER BY BREED_DATE) A ", parameters.farm_seq, parameters.entity_seq, parameters.calve_date, parameters.breed_seq);
                    }

                    if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                string sType = dataReader.GetString(0);

                                if (sType == "PRE")
                                {
                                    paramPreCalveInfo = new Tuple<int, string>(_mClassDatabase.GetSafeInteger(dataReader, 1), _mClassDatabase.GetSafeDateTime(dataReader, 2));
                                }
                                else if (sType == "POS")
                                {
                                    paramPosCalveInfo = new Tuple<int, string>(_mClassDatabase.GetSafeInteger(dataReader, 1), _mClassDatabase.GetSafeDateTime(dataReader, 2));
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
                }

                // 수정인 경우에만 현재 등록된 분만일자 이전 / 이후의 데이타를 추출한다
                if (!isError && parameters.breed_seq > 0)
                {
                    sQuery = string.Format("SELECT BREED_DATE " +
                                           "  FROM BREED_HISTORY " +
                                           " WHERE SEQ = {0} ", parameters.breed_seq);

                    if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                    {
                        if (dataReader.HasRows)
                        {
                            dataReader.Read();

                            sBreedDate = dataReader.GetDateTime(0).ToString("yyyy-MM-dd HH:mm:ss");
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

                // 수정인 경우에만 현재 등록된 분만일자 이전 / 이후의 데이타를 추출한다
                if (!isError && parameters.breed_seq > 0)
                {
                    sQuery = string.Format("SELECT 'PRE' AS DATA_TYPE, SEQ, BREED_DATE " +
                                           "  FROM (SELECT TOP 1 SEQ, BREED_DATE " +
                                           "          FROM BREED_HISTORY " +
                                           "         WHERE FARM_SEQ = {0} " +
                                           "           AND ENTITY_SEQ = {1} " +
                                           "           AND BREED_TYPE = 'C' " +
                                           "           AND BREED_DATE < '{2}' " +
                                           "           AND FLAG = 'Y' " +
                                           "         ORDER BY BREED_DATE DESC, SEQ DESC) A " +
                                           "UNION " +
                                           "SELECT 'POS' AS DATA_TYPE, SEQ, BREED_DATE " +
                                           "  FROM (SELECT TOP 1 SEQ, BREED_DATE " +
                                           "          FROM BREED_HISTORY " +
                                           "         WHERE FARM_SEQ = {0} " +
                                           "           AND ENTITY_SEQ = {1} " +
                                           "           AND BREED_TYPE = 'C' " +
                                           "           AND BREED_DATE > '{2}' " +
                                           "           AND FLAG = 'Y' " +
                                           "         ORDER BY BREED_DATE) A ", parameters.farm_seq, parameters.entity_seq, sBreedDate);

                    if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                string sType = dataReader.GetString(0);

                                if (sType == "PRE")
                                {
                                    regPreCalveInfo = new Tuple<int, string>(_mClassDatabase.GetSafeInteger(dataReader, 1), _mClassDatabase.GetSafeDateTime(dataReader, 2));
                                }
                                else if (sType == "POS")
                                {
                                    regPosCalveInfo = new Tuple<int, string>(_mClassDatabase.GetSafeInteger(dataReader, 1), _mClassDatabase.GetSafeDateTime(dataReader, 2));
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
                }

                if (!isError)
                {
                    _mClassDatabase.TransBegin();

                    string sMemo = parameters.memo;
                    sMemo = sMemo.Replace("'", "''");
                    sMemo = sMemo.Replace(@"\", @"\\");

                    // 분만간격을 계산한다
                    int nTerms = 0;

                    if (!string.IsNullOrEmpty(paramPreCalveInfo.Item2))
                    {
                        DateTime dtFrom = Convert.ToDateTime(paramPreCalveInfo.Item2);
                        DateTime dtTo = Convert.ToDateTime(parameters.calve_date);

                        TimeSpan timespan = new TimeSpan();
                        timespan = dtTo - dtFrom;

                        nTerms = (int)timespan.TotalDays;
                    }

                    // 등록과 수정을 구분해서 처리한다
                    // 등록시에는 사용자 계정정보를 등록한다
                    if (parameters.breed_seq == 0)
                    {
                        sQuery = "INSERT INTO BREED_HISTORY " +
                                 " (FARM_SEQ, ENTITY_SEQ, ENTITY_NO, BREED_TYPE, BREED_DATE, BREED_DUE_DATE, BREED_METHOD, BREED_INT_VALUE1, BREED_INT_VALUE2, BREED_TEXT_VALUE, MEMO, USER_ID) " +
                                 " VALUES ( ";
                        sQuery += parameters.farm_seq + ", ";
                        sQuery += parameters.entity_seq + ", ";
                        sQuery += "'" + sEntityNo + "', ";
                        sQuery += "'C', ";
                        sQuery += "'" + parameters.calve_date + "', ";
                        if (string.IsNullOrEmpty(parameters.due_date)) sQuery += "NULL, ";
                        else sQuery += "'" + parameters.due_date + "', ";
                        sQuery += parameters.calve_code + ", ";
                        sQuery += parameters.calve_count + ", ";
                        sQuery += nTerms + ", ";
                        if (string.IsNullOrEmpty(parameters.calve_flag)) sQuery += "NULL, ";
                        else sQuery += "'" + parameters.calve_flag + "', ";
                        if (string.IsNullOrEmpty(parameters.memo)) sQuery += "NULL, ";
                        else sQuery += "N'" + sMemo + "', ";
                        sQuery += "'" + parameters.uid + "' ";
                        sQuery += " )";
                    }
                    else
                    {
                        sQuery = "UPDATE BREED_HISTORY SET ";
                        sQuery += "BREED_DATE = '" + parameters.calve_date + "', ";
                        if (string.IsNullOrEmpty(parameters.due_date)) sQuery += "BREED_DUE_DATE = NULL, ";
                        else sQuery += "BREED_DUE_DATE = '" + parameters.due_date + "', ";
                        sQuery += "BREED_METHOD = " + parameters.calve_code + ", ";
                        sQuery += "BREED_INT_VALUE1 = " + parameters.calve_count + ", ";
                        sQuery += "BREED_INT_VALUE2 = " + nTerms + ", ";
                        if (string.IsNullOrEmpty(parameters.calve_flag)) sQuery += "BREED_TEXT_VALUE = NULL, ";
                        else sQuery += "BREED_TEXT_VALUE = '" + parameters.calve_flag + "', ";
                        if (string.IsNullOrEmpty(parameters.memo)) sQuery += "MEMO = NULL ";
                        else sQuery += "MEMO = N'" + sMemo + "'";
                        sQuery += string.Format(" WHERE SEQ = {0}", parameters.breed_seq);
                    }

                    int count = _mClassDatabase.QueryExecute(sQuery);

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
                    }
                }

                // 등록한 분만이력 뒤에 분만이력이 있는 경우에는 해당 이력의 분만 간격을 수정한다
                if (!isError && (int)paramPosCalveInfo.Item1 > 0)
                {
                    int nTerms = 0;

                    DateTime dtFrom = Convert.ToDateTime(parameters.calve_date);
                    DateTime dtTo = Convert.ToDateTime(paramPosCalveInfo.Item2);

                    TimeSpan timespan = new TimeSpan();
                    timespan = dtTo - dtFrom;
                    nTerms = (int)timespan.TotalDays;

                    sQuery = string.Format("UPDATE BREED_HISTORY SET BREED_INT_VALUE2 = {1} WHERE SEQ = {0} ", paramPosCalveInfo.Item1, nTerms);

                    int count = _mClassDatabase.QueryExecute(sQuery);

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
                    }
                }

                // 수정인 경우
                // 변경되는 분만일자가 등록된 분만일자 기준으로 이전 분만일자보다 이전이거나 이후 분만일자보다 이후이면
                // 등록된 분만일자 기준의 이전 분만일자와 이후 분만일자의 분만간격으로 이후 분만내역의 분만간격을 수정한다
                if (!isError && parameters.breed_seq > 0)
                {
                    // 이전 분만일자와 이후 분만일자 모두 있는 경우에만 수정한다
                    if (!string.IsNullOrEmpty(regPreCalveInfo.Item2) && !string.IsNullOrEmpty(regPosCalveInfo.Item2))
                    {
                        int nTerms = 0;

                        DateTime dtFrom = Convert.ToDateTime(regPreCalveInfo.Item2);
                        DateTime dtTo = Convert.ToDateTime(regPosCalveInfo.Item2);
                        DateTime dtCheck = Convert.ToDateTime(parameters.calve_date);

                        if (dtCheck < dtFrom || dtCheck > dtTo)
                        {
                            TimeSpan timespan = new TimeSpan();
                            timespan = dtTo - dtFrom;
                            nTerms = (int)timespan.TotalDays;

                            sQuery = string.Format("UPDATE BREED_HISTORY SET BREED_INT_VALUE2 = {1} WHERE SEQ = {0} ", regPosCalveInfo.Item1, nTerms);

                            int count = _mClassDatabase.QueryExecute(sQuery);

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
                            }
                        }
                    }
                }

                // 마지막 분만이력을 추출하여 수정되는 일자와 비교하여 산차를 처리한다
                string sLastCalveDate = string.Empty;
                int nLastCalveCount = 0;

                if (!isError)
                {
                    // 마지막 분만이력 추출
                    sQuery = string.Format("SELECT TOP 1 BREED_DATE, BREED_INT_VALUE1 " +
                                           "  FROM BREED_HISTORY " +
                                           " WHERE FARM_SEQ = {0} " +
                                           "   AND ENTITY_SEQ = {1} " +
                                           "   AND FLAG = 'Y' " +
                                           "   AND BREED_TYPE = 'C' " +
                                           " ORDER BY BREED_DATE DESC, SEQ DESC ", parameters.farm_seq, parameters.entity_seq);

                    if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                    {
                        if (dataReader.HasRows)
                        {
                            dataReader.Read();

                            sLastCalveDate = dataReader.GetDateTime(0).ToString("yyyy-MM-dd");
                            nLastCalveCount = dataReader.GetInt32(1);
                        }

                        dataReader.Close();
                        dataReader.Dispose();
                    }
                    else
                    {
                        isError = true;
                        _mClassDatabase.TransRollback();

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
                    int nCalveCount = 0;

                    if (Convert.ToDateTime(sLastCalveDate) <= Convert.ToDateTime(parameters.calve_date))
                        nCalveCount = parameters.calve_count;
                    else
                        nCalveCount = nLastCalveCount;

                    sQuery = string.Format("UPDATE ENTITY_NEW_INFO SET CALVE_FLAG = 'Y', CALVE_COUNT = {1} WHERE SEQ = {0}", parameters.entity_seq, nCalveCount);

                    int count = _mClassDatabase.QueryExecute(sQuery);

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
                    }
                }

                // 마지막 번식이력 정보로 개체정보의 임신상태를 수정한다
                // 발정 : 미임신
                // 수정 : 미임신
                // 감정 : 임신 => 임신 / 그 외 : 미임신
                // 건유 : 임신 => 임신 / 그 외 : 미임신
                // 분만 : 미임신
                string sLastBreedType = string.Empty;
                string sLastBreedDate = string.Empty;
                string sLastTextValue = string.Empty;

                if (!isError)
                {
                    // 마지막 번식이력 정보 추출
                    sQuery = string.Format("SELECT TOP 1 BREED_TYPE, BREED_DATE, BREED_TEXT_VALUE " +
                                           "  FROM BREED_HISTORY " +
                                           " WHERE FARM_SEQ = {0} " +
                                           "   AND ENTITY_SEQ = {1} " +
                                           "   AND FLAG = 'Y' " +
                                           " ORDER BY BREED_DATE DESC, SEQ DESC ", parameters.farm_seq, parameters.entity_seq);

                    if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                    {
                        if (dataReader.HasRows)
                        {
                            dataReader.Read();
                            sLastBreedType = dataReader.GetString(0);
                            sLastBreedDate = dataReader.GetDateTime(1).ToString("yyyy-MM-dd");
                            sLastTextValue = _mClassDatabase.GetSafeString(dataReader, 2);
                        }

                        dataReader.Close();
                        dataReader.Dispose();
                    }
                    else
                    {
                        isError = true;
                        _mClassDatabase.TransRollback();

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
                    int nPregnancyCode = 2;

                    switch (sLastBreedType)
                    {
                        case "E": nPregnancyCode = 2; break;
                        case "I": nPregnancyCode = 2; break;
                        case "A": nPregnancyCode = sLastTextValue == "Y" ? 1 : 2; break;
                        case "D": nPregnancyCode = sLastTextValue == "Y" ? 1 : 2; break;
                        case "C": nPregnancyCode = 2; break;
                    }

                    // 개체의 임신상태를 수정한다
                    sQuery = string.Format("UPDATE ENTITY_NEW_INFO SET PREGNANCY_CODE = {1} WHERE SEQ = {0}", parameters.entity_seq, nPregnancyCode);

                    int count = _mClassDatabase.QueryExecute(sQuery);

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
                    }
                }

                int nEntityKind = 3;

                // 한우인 경우에는 기본값을 번식우로 설정한다
                if (nEntityType == 1) nEntityKind = 1;

                if (!isError)
                {
                    // 개체분류는 분만일 이후에 건유일이 없는 경우 개체분류를 젖소는 착유우, 한우는 번식우로 변경한다
                    // 개체분류는 분만일 이후의 내역중에서 분만 또는 건유의 최근 번식이력을 추출한다
                    // 번식이력이 건유일인 경우에는 건유우로 변경하고 그 외에는 착유우로 변경한다
                    sQuery = string.Format("SELECT TOP 1 BREED_TYPE, BREED_DATE " +
                                           "  FROM (SELECT SEQ, BREED_TYPE, BREED_DATE " +
                                           "          FROM BREED_HISTORY " +
                                           "         WHERE FARM_SEQ = {0} " +
                                           "           AND ENTITY_SEQ = {1} " +
                                           "           AND BREED_DATE > '{2}' " +
                                           "           AND FLAG = 'Y') A " +
                                           " WHERE BREED_TYPE = 'C' " +
                                           "    OR BREED_TYPE = 'D' " +
                                           " ORDER BY BREED_DATE DESC, SEQ DESC ", parameters.farm_seq, parameters.entity_seq, parameters.calve_date);

                    if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                    {
                        if (dataReader.HasRows)
                        {
                            dataReader.Read();

                            if (dataReader.GetString(0) == "D") nEntityKind = 4;
                        }

                        dataReader.Close();
                        dataReader.Dispose();
                    }
                    else
                    {
                        isError = true;
                        _mClassDatabase.TransRollback();

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
                    sQuery = string.Format("UPDATE ENTITY_NEW_INFO SET ENTITY_KIND = {1} WHERE SEQ = {0}", parameters.entity_seq, nEntityKind);

                    int count = _mClassDatabase.QueryExecute(sQuery);

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
                isError = true;
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

        public ClassResponse.RES_RESULT GetHistoryMonthCount(ClassRequest.REQ_HISTORYMONTH parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "History", "month_count");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetHistoryMonthCount  ==========", sModuleName);
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

                string sQuery = string.Format("SELECT COUNT(CASE WHEN (BREED_TYPE = 'E' OR BREED_TYPE = 'C') " +
                           "                   AND BREED_DUE_DATE IS NOT NULL " +
                           "                   AND CONVERT(CHAR(10), A.BREED_DUE_DATE, 120) >= '{1}' " +
                           "                   AND CONVERT(CHAR(10), A.BREED_DUE_DATE, 120) <= '{2}' THEN 1 END) AS E_COUNT, " +
                           "       COUNT(CASE WHEN BREED_TYPE = 'E' AND ADDED_DUE_DATE1 IS NOT NULL " +
                           "                   AND CONVERT(CHAR(10), A.ADDED_DUE_DATE1, 120) >= '{1}' " +
                           "                   AND CONVERT(CHAR(10), A.ADDED_DUE_DATE1, 120) <= '{2}' THEN 1 END) AS I_COUNT, " +
                           "       COUNT(CASE WHEN BREED_TYPE = 'I' AND ADDED_DUE_DATE1 IS NOT NULL THEN 1 END) AS A_COUNT, " +
                           "       COUNT(CASE WHEN (BREED_TYPE = 'A' OR BREED_TYPE = 'D') " +
                           "                   AND ADDED_DUE_DATE1 IS NOT NULL " +
                           "                   AND CONVERT(CHAR(10), A.ADDED_DUE_DATE1, 120) >= '{1}' " +
                           "                   AND CONVERT(CHAR(10), A.ADDED_DUE_DATE1, 120) <= '{2}' THEN 1 END) AS C_COUNT, " +
                           "       COUNT(CASE WHEN BREED_TYPE = 'A' " +
                           "                   AND ADDED_DUE_DATE2 IS NOT NULL " +
                           "                   AND CONVERT(CHAR(10), A.ADDED_DUE_DATE2, 120) >= '{1}' " +
                           "                   AND CONVERT(CHAR(10), A.ADDED_DUE_DATE2, 120) <= '{2}' THEN 1 END) AS D_COUNT " +
                           "  FROM (SELECT A.BREED_TYPE, A.BREED_DUE_DATE, A.ADDED_DUE_DATE1, A.ADDED_DUE_DATE2 " +
                           "          FROM UDF_HISTORY_LASTDATA({0}) A " +
                           "          LEFT OUTER JOIN ENTITY_NEW_INFO B " +
                           "            ON A.ENTITY_SEQ = B.SEQ " +
                           "           AND B.FLAG = 'Y' " +
                           "           AND B.ACTIVE_FLAG = 'Y' " +
                           "         WHERE ((CONVERT(CHAR(10), A.BREED_DUE_DATE, 120) >= '{1}' " +
                           "           AND   CONVERT(CHAR(10), A.BREED_DUE_DATE, 120) <= '{2}') " +
                           "            OR  (CONVERT(CHAR(10), A.ADDED_DUE_DATE1, 120) >= '{1}' " +
                           "           AND   CONVERT(CHAR(10), A.ADDED_DUE_DATE1, 120) <= '{2}') " +
                           "            OR  (CONVERT(CHAR(10), A.ADDED_DUE_DATE2, 120) >= '{1}' " +
                           "           AND   CONVERT(CHAR(10), A.ADDED_DUE_DATE2, 120) <= '{2}')) " +
                           "           AND B.ENTITY_NO IS NOT NULL ) A ", parameters.farm_seq, sFrom, sTo);
                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        dataReader.Read();

                        countInfo.ed_count = dataReader.GetInt32(0);
                        countInfo.id_count = dataReader.GetInt32(1);
                        countInfo.ad_count = dataReader.GetInt32(2);
                        countInfo.dd_count = dataReader.GetInt32(4);
                        countInfo.cd_count = dataReader.GetInt32(3);
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

        public ClassResponse.RES_RESULT GetEstrusInfo(ClassRequest.REQ_BREEDINFO parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "History", "get_estrus");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetEstrusInfo  ==========", sModuleName);
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

                if (parameters.entity_seq < 0)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_ENTITY_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_ENTITY_SEQ),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (parameters.breed_seq < 0)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_BREED_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_BREED_SEQ),
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

            try
            {
                string sQuery = string.Format("SELECT BREED_DATE, BREED_DUE_DATE, ADDED_METHOD, ADDED_DUE_DATE1, MEMO " +
                                       "  FROM BREED_HISTORY " +
                                       " WHERE SEQ = {0} ", parameters.breed_seq);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        ClassStruct.ST_ESTRUS_INFO estrusInfo = new ClassStruct.ST_ESTRUS_INFO();

                        dataReader.Read();

                        estrusInfo.estrus_date = dataReader.GetDateTime(0).ToString("yyyy-MM-dd HH:mm:ss");
                        if (dataReader.IsDBNull(1)) estrusInfo.estrus_due_date = string.Empty;
                        else estrusInfo.estrus_due_date = dataReader.GetDateTime(1).ToString("yyyy-MM-dd");
                        estrusInfo.inseminate_code = dataReader.GetInt32(2);
                        if (dataReader.IsDBNull(3)) estrusInfo.inseminate_due_date = string.Empty;
                        else estrusInfo.inseminate_due_date = dataReader.GetDateTime(3).ToString("yyyy-MM-dd");
                        estrusInfo.memo = _mClassDatabase.GetSafeString(dataReader, 4);

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SUCCESS,
                            message = string.Empty,
                            data = JsonConvert.SerializeObject(estrusInfo)
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

        public ClassResponse.RES_RESULT GetInseminateInfo(ClassRequest.REQ_BREEDINFO parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "History", "get_inseminate");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetInseminateInfo  ==========", sModuleName);
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

                if (parameters.entity_seq < 0)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_ENTITY_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_ENTITY_SEQ),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (parameters.breed_seq < 0)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_BREED_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_BREED_SEQ),
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

            try
            {
                string sQuery = string.Format("SELECT A.BREED_DATE, A.BREED_METHOD, A.BREED_INT_VALUE1, A.BREED_INT_VALUE2, A.ADDED_METHOD, A.ADDED_DUE_DATE1, A.MEMO, B.SEMEN_NO " +
                                       "  FROM BREED_HISTORY A " +
                                       "  LEFT OUTER JOIN FARM_SEMEN B " +
                                       "    ON A.BREED_INT_VALUE2 = B.SEQ " +
                                       " WHERE A.SEQ = {0} ", parameters.breed_seq);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        ClassStruct.ST_INSEMINATE_INFO inseminateInfo = new ClassStruct.ST_INSEMINATE_INFO();

                        dataReader.Read();

                        inseminateInfo.inseminate_date = dataReader.GetDateTime(0).ToString("yyyy-MM-dd HH:mm:ss");
                        inseminateInfo.inseminate_code = dataReader.GetInt32(1);
                        inseminateInfo.inseminate_count = dataReader.GetInt32(2);
                        inseminateInfo.semen_seq = dataReader.GetInt32(3);
                        inseminateInfo.semen_no = _mClassDatabase.GetSafeString(dataReader, 7);
                        if (dataReader.IsDBNull(5)) inseminateInfo.appraisal_due_date = string.Empty;
                        else inseminateInfo.appraisal_due_date = dataReader.GetDateTime(5).ToString("yyyy-MM-dd");
                        inseminateInfo.appraisal_code = dataReader.GetInt32(4);
                        inseminateInfo.memo = _mClassDatabase.GetSafeString(dataReader, 6);

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SUCCESS,
                            message = string.Empty,
                            data = JsonConvert.SerializeObject(inseminateInfo)
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

        public ClassResponse.RES_RESULT GetAppraisalInfo(ClassRequest.REQ_BREEDINFO parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "History", "get_appraisal");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetAppraisalInfo  ==========", sModuleName);
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

                if (parameters.entity_seq < 0)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_ENTITY_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_ENTITY_SEQ),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (parameters.breed_seq < 0)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_BREED_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_BREED_SEQ),
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

            try
            {
                string sQuery = string.Format("SELECT BREED_DATE, BREED_METHOD, BREED_TEXT_VALUE, ADDED_METHOD, ADDED_DATE, ADDED_DUE_DATE1, ADDED_DUE_DATE2, MEMO " +
                                       "  FROM BREED_HISTORY " +
                                       " WHERE SEQ = {0} ", parameters.breed_seq);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        ClassStruct.ST_APPRAISAL_INFO appraisalInfo = new ClassStruct.ST_APPRAISAL_INFO();

                        dataReader.Read();

                        appraisalInfo.appraisal_date = dataReader.GetDateTime(0).ToString("yyyy-MM-dd HH:mm:ss");
                        appraisalInfo.pregnancy_flag = dataReader.GetString(2);
                        appraisalInfo.appraisal_code = dataReader.GetInt32(1);
                        if (dataReader.IsDBNull(5)) appraisalInfo.calve_due_date = string.Empty;
                        else appraisalInfo.calve_due_date = dataReader.GetDateTime(5).ToString("yyyy-MM-dd");
                        if (dataReader.IsDBNull(6)) appraisalInfo.dryup_due_date = string.Empty;
                        else appraisalInfo.dryup_due_date = dataReader.GetDateTime(6).ToString("yyyy-MM-dd");
                        if (dataReader.IsDBNull(4)) appraisalInfo.inseminate_date = string.Empty;
                        else appraisalInfo.inseminate_date = dataReader.GetDateTime(4).ToString("yyyy-MM-dd");
                        appraisalInfo.inseminate_code = dataReader.GetInt32(3);
                        appraisalInfo.memo = _mClassDatabase.GetSafeString(dataReader, 7);

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SUCCESS,
                            message = string.Empty,
                            data = JsonConvert.SerializeObject(appraisalInfo)
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

        public ClassResponse.RES_RESULT GetDryupInfo(ClassRequest.REQ_BREEDINFO parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "History", "get_dryup");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetDryupInfo  ==========", sModuleName);
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

                if (parameters.entity_seq < 0)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_ENTITY_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_ENTITY_SEQ),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (parameters.breed_seq < 0)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_BREED_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_BREED_SEQ),
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

            try
            {
                string sQuery = string.Format("SELECT BREED_DATE, BREED_TEXT_VALUE, ADDED_METHOD, ADDED_DATE, ADDED_DUE_DATE1, MEMO " +
                                       "  FROM BREED_HISTORY " +
                                       " WHERE SEQ = {0} ", parameters.breed_seq);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        ClassStruct.ST_DRYUP_INFO dryupInfo = new ClassStruct.ST_DRYUP_INFO();

                        dataReader.Read();

                        dryupInfo.dryup_date = dataReader.GetDateTime(0).ToString("yyyy-MM-dd HH:mm:ss");
                        dryupInfo.pregnancy_flag = _mClassDatabase.GetSafeString(dataReader, 1);
                        if (dataReader.IsDBNull(4))
                            dryupInfo.calve_due_date = string.Empty;
                        else
                            dryupInfo.calve_due_date = dataReader.GetDateTime(4).ToString("yyyy-MM-dd");
                        dryupInfo.inseminate_code = dataReader.GetInt32(2);
                        if (dataReader.IsDBNull(3))
                            dryupInfo.inseminate_date = string.Empty;
                        else
                            dryupInfo.inseminate_date = dataReader.GetDateTime(3).ToString("yyyy-MM-dd");
                        dryupInfo.memo = _mClassDatabase.GetSafeString(dataReader, 5);

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SUCCESS,
                            message = string.Empty,
                            data = JsonConvert.SerializeObject(dryupInfo)
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

        public ClassResponse.RES_RESULT GetCalveInfo(ClassRequest.REQ_BREEDINFO parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "History", "get_calve");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetCalveInfo  ==========", sModuleName);
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

                if (parameters.entity_seq < 0)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_ENTITY_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_ENTITY_SEQ),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (parameters.breed_seq < 0)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_BREED_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_BREED_SEQ),
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

            try
            {
                string sQuery = string.Format("SELECT BREED_DATE, BREED_DUE_DATE, BREED_METHOD, BREED_INT_VALUE1, BREED_INT_VALUE2, BREED_TEXT_VALUE, MEMO " +
                                       "  FROM BREED_HISTORY " +
                                       " WHERE SEQ = {0} ", parameters.breed_seq);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        ClassStruct.ST_CALVE_INFO calveInfo = new ClassStruct.ST_CALVE_INFO();

                        dataReader.Read();

                        calveInfo.calve_date = dataReader.GetDateTime(0).ToString("yyyy-MM-dd HH:mm:ss");
                        calveInfo.calve_flag = _mClassDatabase.GetSafeString(dataReader, 5);
                        calveInfo.calve_code = dataReader.GetInt32(2);
                        calveInfo.calve_count = _mClassDatabase.GetSafeInteger(dataReader, 3);
                        if (dataReader.IsDBNull(1)) calveInfo.estrus_due_date = string.Empty;
                        else calveInfo.estrus_due_date = dataReader.GetDateTime(1).ToString("yyyy-MM-dd");
                        calveInfo.memo = _mClassDatabase.GetSafeString(dataReader, 6);

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SUCCESS,
                            message = string.Empty,
                            data = JsonConvert.SerializeObject(calveInfo)
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

        public ClassResponse.RES_RESULT GetLastInseminate(ClassRequest.REQ_LAST_INSEMINATE parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "History", "last_inseminate");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetLastInseminate  ==========", sModuleName);
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

                if (parameters.entity_seq < 0)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_ENTITY_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_ENTITY_SEQ),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (string.IsNullOrEmpty(parameters.breed_date))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_BREED_DATE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_BREED_DATE),
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

            try
            {
                // 감정등록 시 등록한 추가정보를 추출한다
                // 마지막 번식정보가 수정이거나 감정인 경우에만 정보를 전달한다
                string sQuery = string.Format("SELECT TOP 1 BREED_TYPE, BREED_DATE, BREED_METHOD, ADDED_METHOD, ADDED_DATE " +
                                       "  FROM BREED_HISTORY " +
                                       " WHERE FARM_SEQ = {0} " +
                                       "   AND ENTITY_SEQ = {1} " +
                                       "   AND CONVERT(CHAR(10), BREED_DATE, 120) < '{2}' " +
                                       "   AND FLAG = 'Y' " +
                                       " ORDER BY BREED_DATE DESC, SEQ DESC ", parameters.farm_seq, parameters.entity_seq, parameters.breed_date);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        dataReader.Read();

                        string sBreedType = dataReader.GetString(0);

                        if (sBreedType == "I" || sBreedType == "A")
                        {
                            ClassStruct.ST_LAST_INSEMINATE lastInfo = new ClassStruct.ST_LAST_INSEMINATE();

                            if (sBreedType == "I")
                            {
                                lastInfo.inseminate_date = dataReader.GetDateTime(1).ToString("yyyy-MM-dd");
                                lastInfo.inseminate_code = dataReader.GetInt32(2);
                            }
                            else if (sBreedType == "A")
                            {
                                if (dataReader.IsDBNull(4))
                                    lastInfo.inseminate_date = string.Empty;
                                else
                                    lastInfo.inseminate_date = dataReader.GetDateTime(4).ToString("yyyy-MM-dd");

                                lastInfo.inseminate_code = dataReader.GetInt32(3);
                            }

                            response = new ClassResponse.RES_RESULT
                            {
                                result = ClassError.RESULT_SUCCESS,
                                message = string.Empty,
                                data = JsonConvert.SerializeObject(lastInfo)
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

        public ClassResponse.RES_RESULT SetDeleteHistory(ClassRequest.REQ_BREEDINFO parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "History", "delete");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START SetDeleteHistory  ==========", sModuleName);
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

                if (parameters.entity_seq < 0)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_ENTITY_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_ENTITY_SEQ),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (parameters.breed_seq < 0)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_BREED_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_BREED_SEQ),
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

            try
            {
                string sBreedCode = string.Empty;
                string sBreedDate = string.Empty;
                int nEntityType = 0;

                string sQuery = string.Format("SELECT A.BREED_TYPE, A.BREED_DATE, B.ENTITY_TYPE " +
                                       "  FROM BREED_HISTORY A " +
                                       "  LEFT OUTER JOIN ENTITY_NEW_INFO B " +
                                       "    ON A.ENTITY_SEQ = B.SEQ " +
                                       "   AND B.FLAG = 'Y' " +
                                       "   AND B.ACTIVE_FLAG = 'Y' " +
                                       " WHERE A.SEQ = {0} ", parameters.breed_seq);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        dataReader.Read();

                        sBreedCode = dataReader.GetString(0);
                        sBreedDate = dataReader.GetDateTime(1).ToString("yyyy-MM-dd");
                        nEntityType = _mClassDatabase.GetSafeInteger(dataReader, 2);
                    }
                    else
                    {
                        isError = true;

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
                    isError = true;

                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
                        data = string.Empty
                    };
                }

                // 분만이력이 아닌 경우에는 그냥 삭제하고 종료한다
                // 분만이력인 경우에는 분만간격을 재 계산해준다
                string sPreCalveDate = string.Empty;

                int nPosSeq = 0;
                string sPosCalveDate = string.Empty;

                if (!isError && sBreedCode == "C")
                {
                    // 분만간격 계산을 위해서 입력된 분만일자보다 작은 최근 분만일자를 추출한다
                    if (!isError)
                    {
                        sQuery = string.Format("SELECT TOP 1 BREED_DATE " +
                                                "  FROM BREED_HISTORY " +
                                                " WHERE SEQ <> {3} " +
                                                "   AND FARM_SEQ = {0} " +
                                                "   AND ENTITY_SEQ = {1} " +
                                                "   AND BREED_TYPE = 'C' " +
                                                "   AND CONVERT(CHAR(10), BREED_DATE, 120) < '{2}' " +
                                                "   AND FLAG = 'Y' " +
                                                " ORDER BY BREED_DATE DESC, SEQ DESC", parameters.farm_seq, parameters.entity_seq, sBreedDate, parameters.breed_seq);

                        if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                        {
                            if (dataReader.HasRows)
                            {
                                dataReader.Read();
                                sPreCalveDate = dataReader.GetDateTime(0).ToString("yyyy-MM-dd");
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

                    // 분만간격 재 계산을 위해서 분만일자보다 큰 최초 분만내역의 번식이력 번호와 분만일자를 추출한다
                    if (!isError)
                    {
                        sQuery = string.Format("SELECT TOP 1 SEQ, BREED_DATE " +
                                                "  FROM BREED_HISTORY " +
                                                " WHERE SEQ <> {3} " +
                                                "   AND FARM_SEQ = {0} " +
                                                "   AND ENTITY_SEQ = {1} " +
                                                "   AND BREED_TYPE = 'C' " +
                                                "   AND CONVERT(CHAR(10), BREED_DATE, 120) > '{2}' " +
                                                "   AND FLAG = 'Y' " +
                                                " ORDER BY BREED_DATE ", parameters.farm_seq, parameters.entity_seq, sBreedDate, parameters.breed_seq);

                        if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                        {
                            if (dataReader.HasRows)
                            {
                                dataReader.Read();

                                nPosSeq = dataReader.GetInt32(0);
                                sPosCalveDate = dataReader.GetDateTime(1).ToString("yyyy-MM-dd");
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
                }

                if (!isError)
                {
                    _mClassDatabase.TransBegin();

                    sQuery = string.Format("UPDATE BREED_HISTORY SET FLAG = 'N' WHERE SEQ = {0}", parameters.breed_seq);

                    int count = _mClassDatabase.QueryExecute(sQuery);

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
                    }
                }

                if (!isError && sBreedCode == "C" && nPosSeq > 0)
                {
                    int nTerms = 0;

                    if (!string.IsNullOrEmpty(sPreCalveDate))
                    {
                        DateTime dtFrom = Convert.ToDateTime(sPreCalveDate);
                        DateTime dtTo = Convert.ToDateTime(sPosCalveDate);

                        TimeSpan timespan = new TimeSpan();
                        timespan = dtTo - dtFrom;

                        nTerms = (int)timespan.TotalDays;
                    }

                    sQuery = string.Format("UPDATE BREED_HISTORY SET BREED_INT_VALUE2 = {1} WHERE SEQ = {0} ", nPosSeq, nTerms);

                    int count = _mClassDatabase.QueryExecute(sQuery);

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
                    }
                }

                // 마지막 번식이력 정보로 개체정보의 임신상태를 수정한다
                // 발정 : 미임신
                // 수정 : 미임신
                // 감정 : 임신 => 임신 / 그 외 : 미임신
                // 건유 : 임신구분 값에 따라서 처리한다
                // 분만 : 미임신
                string sLastBreedType = string.Empty;
                string sLastBreedDate = string.Empty;
                string sLastTextValue = string.Empty;

                if (!isError)
                {
                    // 마지막 번식이력 정보 추출
                    sQuery = string.Format("SELECT TOP 1 BREED_TYPE, BREED_DATE, BREED_TEXT_VALUE " +
                                           "  FROM BREED_HISTORY " +
                                           " WHERE FARM_SEQ = {1} " +
                                           "   AND ENTITY_SEQ = {2} " +
                                           "   AND FLAG = 'Y' " +
                                           " ORDER BY BREED_DATE DESC, SEQ DESC ", parameters.breed_seq, parameters.farm_seq, parameters.entity_seq);

                    if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                    {
                        if (dataReader.HasRows)
                        {
                            dataReader.Read();
                            sLastBreedType = dataReader.GetString(0);
                            sLastBreedDate = dataReader.GetDateTime(1).ToString("yyyy-MM-dd HH:mm:ss");
                            sLastTextValue = _mClassDatabase.GetSafeString(dataReader, 2);
                        }

                        dataReader.Close();
                        dataReader.Dispose();
                    }
                    else
                    {
                        isError = true;
                        _mClassDatabase.TransRollback();

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
                    int nPregnancyCode = 2;
                    int nEntityKind = 0;

                    switch (sLastBreedType)
                    {
                        case "E": nPregnancyCode = 2; break;
                        case "I": nPregnancyCode = 2; break;
                        case "A": nPregnancyCode = sLastTextValue == "Y" ? 1 : 2; break;
                        case "D": nPregnancyCode = sLastTextValue == "Y" ? 1 : 2; break;
                        case "C": nPregnancyCode = 2; break;
                    }

                    if (sLastBreedType == "D")
                    {
                        nEntityKind = 4;
                    }
                    else if (sLastBreedType == "C")
                    {
                        if (nEntityType == 1)
                            nEntityKind = 1;
                        else if (nEntityType == 2)
                            nEntityKind = 3;
                    }

                    // 개체의 임신상태를 변경한다
                    // 마지막 분만이력이 건유나 분만인 경우에만 개체분류를 변경한다
                    // 건유인 경우에는 건유우로
                    // 분만인 경우에는 젖소는 착유우로, 한우는 번식우로 변경한다
                    if (nEntityKind > 0)
                        sQuery = string.Format("UPDATE ENTITY_NEW_INFO SET PREGNANCY_CODE = {1}, ENTITY_KIND = {2} WHERE SEQ = {0}", parameters.entity_seq, nPregnancyCode, nEntityKind);
                    else
                        sQuery = string.Format("UPDATE ENTITY_NEW_INFO SET PREGNANCY_CODE = {1} WHERE SEQ = {0}", parameters.entity_seq, nPregnancyCode);

                    int count = _mClassDatabase.QueryExecute(sQuery);

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
                isError = true;
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

        public ClassResponse.RES_RESULT GetLastAppraisal(ClassRequest.REQ_LAST_INSEMINATE parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "History", "last_appraisal");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetLastAppraisal  ==========", sModuleName);
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

                if (parameters.entity_seq < 0)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_ENTITY_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_ENTITY_SEQ),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (string.IsNullOrEmpty(parameters.breed_date))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_BREED_DATE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_BREED_DATE),
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

            try
            {
                // 건유등록 시 등록한 추가정보를 추출한다
                // 감정이력인 경우에만 결과를 전달한다
                string sQuery = string.Format("SELECT TOP 1 BREED_TYPE, BREED_TEXT_VALUE, ADDED_METHOD, ADDED_DATE, ADDED_DUE_DATE1 " +
                                       "  FROM BREED_HISTORY " +
                                       " WHERE FARM_SEQ = {0} " +
                                       "   AND ENTITY_SEQ = {1} " +
                                       "   AND CONVERT(CHAR(10), BREED_DATE, 120) < '{2}' " +
                                       "   AND FLAG = 'Y' " +
                                       " ORDER BY BREED_DATE DESC, SEQ DESC ", parameters.farm_seq, parameters.entity_seq, parameters.breed_date);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        dataReader.Read();

                        string sBreedType = dataReader.GetString(0);

                        if (sBreedType == "A")
                        {
                            ClassStruct.ST_LAST_APPRAISAL lastInfo = new ClassStruct.ST_LAST_APPRAISAL
                            {
                                pregnancy_flag = _mClassDatabase.GetSafeString(dataReader, 1),
                                calve_due_date = dataReader.IsDBNull(4) ? string.Empty : dataReader.GetDateTime(4).ToString("yyyy-MM-dd"),
                                inseminate_date = dataReader.IsDBNull(3) ? string.Empty : dataReader.GetDateTime(3).ToString("yyyy-MM-dd"),
                                inseminate_code = dataReader.GetInt32(2)
                            };

                            response = new ClassResponse.RES_RESULT
                            {
                                result = ClassError.RESULT_SUCCESS,
                                message = string.Empty,
                                data = JsonConvert.SerializeObject(lastInfo)
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
    }
}
