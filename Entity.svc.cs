using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml;

namespace LC_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Entity" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Entity.svc or Entity.svc.cs at the Solution Explorer and start debugging.
    public class Entity : IEntity
    {
        private ClassOLEDB _mClassDatabase = new ClassOLEDB();
        private readonly ClassError _mClassError = new ClassError();
        private readonly ClassFunction _mClassFunction = new ClassFunction();

        ~Entity()
        {
            if (_mClassDatabase.GetConnectionState()) _mClassDatabase.CloseDatabase();
            _mClassDatabase = null;
        }

        public ClassResponse.RES_RESULT GetEntityList(ClassRequest.REQ_ENTITYLIST parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Entity", "entity_list");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetEntityList  ==========", sModuleName);
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

                if (parameters.search_type == "E")
                {
                    if (string.IsNullOrEmpty(parameters.entity_id) && parameters.entity_seq < 1)
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
                }

                if (!string.IsNullOrEmpty(parameters.order_field))
                {
                    if (parameters.order_field != "EID" && parameters.order_field != "CNT" && parameters.order_field != "EST")
                    {
                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_PARAM_ERROR_ORDER_FIELD,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_ORDER_FIELD),
                            data = string.Empty
                        };

                        ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                        return response;
                    }
                }

                if (!string.IsNullOrEmpty(parameters.order_field))
                {
                    if (string.IsNullOrEmpty(parameters.order_type))
                    {
                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_PARAM_ERROR_ORDER_TYPE,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_ORDER_TYPE),
                            data = string.Empty
                        };

                        ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                        return response;
                    }
                    else
                    {
                        if (parameters.order_type != "ASC" && parameters.order_type != "DESC")
                        {
                            response = new ClassResponse.RES_RESULT
                            {
                                result = ClassError.RESULT_PARAM_ERROR_ORDER_TYPE,
                                message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_ORDER_TYPE),
                                data = string.Empty
                            };

                            ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                            return response;
                        }
                    }
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

            List<ClassStruct.ST_ENTITYINFO> entityList = new List<ClassStruct.ST_ENTITYINFO>();

            try
            {
                // 기본값은 개체상태의 내림차순으로 기본값으로 한다
                // 정렬필드
                string sOrderField = string.Empty;
                switch (parameters.order_field)
                {
                    case "EID": sOrderField = "ENTITY_NO"; break;
                    case "CNT": sOrderField = "DRINK_COUNT"; break;
                    case "EST": sOrderField = "ENTITY_STATUS_ORDER"; break;
                    default: sOrderField = "ENTITY_STATUS_ORDER"; break;
                }

                // 정렬방식
                string sOrderType = "DESC";
                if (!string.IsNullOrEmpty(parameters.order_field)) sOrderType = parameters.order_type;

                // 페이지 인덱스
                int nFromIndex = (parameters.page_index - 1) * 20 + 1;
                int nToIndex = parameters.page_index * 20;

                // 개체 ID 정렬인 경우에는 2차 정렬을 등록순으로 정렬하고
                // 그 외에는 2차 정렬을 개체 ID로 정렬한다
                string sQuery = "SELECT A.SEQ, A.FARM_SEQ, A.ENTITY_NO, A.ENTITY_SEX, A.ENTITY_TYPE, A.DETAIL_TYPE, A.ENTITY_KIND, " +
                         "       A.BIRTH, A.BIRTH_MONTH, A.BIRTH_CODE, A.WEIGHT, A.SHIPMENT, A.SHIPMENT_REASON, A.ENTITY_PIN, " +
                         "       A.TAG_ID, A.IMAGE_URL, A.PREGNANCY_CODE, A.CALVE_FLAG, A.CALVE_COUNT, " +
                         "       A.ENTITY_STATUS, A.TEMP_MIN, A.TEMP_MAX, A.ENTITY_STATUS_ORDER, " +
                         "       A.DRINK_COUNT, A.FAVORITE_FLAG, A.INSEMINATE_FLAG, A.NOACTIVITY_FLAG, ";

                switch (parameters.lang_code)
                {
                    case "KR":
                        sQuery += "       B.CODE_NAME AS ENTITY_SEX_DISP, C.CODE_NAME AS ENTITY_TYPE_DISP, D.CODE_NAME AS DETAIL_TYPE_DISP, E.CODE_NAME AS ENTITY_KIND_DISP, ";
                        break;
                    case "JP":
                        sQuery += "       B.JP_VALUE AS ENTITY_SEX_DISP, C.JP_VALUE AS ENTITY_TYPE_DISP, D.JP_VALUE AS DETAIL_TYPE_DISP, E.JP_VALUE AS ENTITY_KIND_DISP, ";
                        break;
                    case "US":
                        sQuery += "       B.EN_VALUE AS ENTITY_SEX_DISP, C.EN_VALUE AS ENTITY_TYPE_DISP, D.EN_VALUE AS DETAIL_TYPE_DISP, E.EN_VALUE AS ENTITY_KIND_DISP, ";
                        break;
                    case "CN":
                        sQuery += "       B.ZH_VALUE AS ENTITY_SEX_DISP, C.ZH_VALUE AS ENTITY_TYPE_DISP, D.ZH_VALUE AS DETAIL_TYPE_DISP, E.ZH_VALUE AS ENTITY_KIND_DISP, ";
                        break;
                    case "PT":
                        sQuery += "       B.PT_VALUE AS ENTITY_SEX_DISP, C.PT_VALUE AS ENTITY_TYPE_DISP, D.PT_VALUE AS DETAIL_TYPE_DISP, E.PT_VALUE AS ENTITY_KIND_DISP, ";
                        break;
                    case "BR":
                        sQuery += "       B.PT_VALUE AS ENTITY_SEX_DISP, C.PT_VALUE AS ENTITY_TYPE_DISP, D.PT_VALUE AS DETAIL_TYPE_DISP, E.PT_VALUE AS ENTITY_KIND_DISP, ";
                        break;
                    default:
                        sQuery += "       B.EN_VALUE AS ENTITY_SEX_DISP, C.EN_VALUE AS ENTITY_TYPE_DISP, D.EN_VALUE AS DETAIL_TYPE_DISP, E.EN_VALUE AS ENTITY_KIND_DISP, ";
                        break;
                }

                sQuery += "        F.SIRE_NAME, F.SIRE_INFO, F.DAM_NAME, F.DAM_INFO, F.GS_NAME, F.GS_INFO, F.GGS_NAME, F.GGS_INFO ";

                if (parameters.order_field == "EID")
                    sQuery += string.Format("  FROM (SELECT ROW_NUMBER() OVER (ORDER BY {0} {1}, SEQ) AS ROW_NUM, ", sOrderField, sOrderType);
                else
                    sQuery += string.Format("  FROM (SELECT ROW_NUMBER() OVER (ORDER BY {0} {1}, ENTITY_NO) AS ROW_NUM, ", sOrderField, sOrderType);

                sQuery += string.Format("               SEQ, FARM_SEQ, ENTITY_NO, ENTITY_SEX, ENTITY_TYPE, DETAIL_TYPE, ENTITY_KIND, " +
                                        "               BIRTH, BIRTH_MONTH, BIRTH_CODE, WEIGHT, SHIPMENT, SHIPMENT_REASON, ENTITY_PIN, " +
                                        "               TAG_ID, IMAGE_URL, PREGNANCY_CODE, CALVE_FLAG, CALVE_COUNT, " +
                                        "               ENTITY_STATUS, TEMP_MIN, TEMP_MAX, ENTITY_STATUS_ORDER, " +
                                        "               DRINK_COUNT, FAVORITE_FLAG, INSEMINATE_FLAG, NOACTIVITY_FLAG " +
                                        "          FROM UDF_FARM_ENTITYLIST({0}, '{1}') " +
                                        "         WHERE SEQ > 0 ", parameters.farm_seq, parameters.uid);

                // 즐겨찾기 / 개체번호 검색 / 조건검색에 따라서 처리
                if (parameters.search_type == "F")
                {
                    sQuery += "           AND FAVORITE_FLAG = 'Y' ";
                }
                else if (parameters.search_type == "E")
                {
                    if (parameters.entity_seq > 0)
                        sQuery += string.Format("           AND SEQ = {0} ", parameters.entity_seq);
                    else if (!string.IsNullOrEmpty(parameters.entity_id))
                        sQuery += string.Format("           AND ENTITY_NO LIKE '%{0}%' ", parameters.entity_id);
                }
                else if (parameters.search_type == "C")
                {
                    // 건유소 / 착유소 구분
                    if (!string.IsNullOrEmpty(parameters.dryup_flag))
                    {
                        int nKind = 0;

                        if (parameters.dryup_flag == "Y") nKind = 4;
                        else nKind = 3;

                        sQuery += string.Format("           AND ENTITY_KIND = {0} ", nKind);
                    }

                    // 경산우 / 미경산우 구분
                    if (!string.IsNullOrEmpty(parameters.calve_flag))
                    {
                        sQuery += string.Format("           AND CALVE_FLAG = '{0}' ", parameters.calve_flag);
                    }

                    // 임신중 / 미임신중 구분
                    if (!string.IsNullOrEmpty(parameters.pregnancy_flag))
                    {
                        if (parameters.pregnancy_flag == "Y")
                            sQuery += "           AND PREGNANCY_CODE = 1 ";
                        else
                            sQuery += "           AND PREGNANCY_CODE = 2 ";
                    }

                    // 수정함 / 미수정 구분
                    if (!string.IsNullOrEmpty(parameters.breed_status))
                    {
                        sQuery += string.Format("           AND INSEMINATE_FLAG = '{0}' ", parameters.breed_status);
                    }

                    // 음수구분
                    //if (parameters.drink_flag == "Y")
                    //{
                    //    sQuery += String.Format("           AND DRINK_COUNT = {0} ", parameters.drink_count);
                    //}

                    // 음수구분
                    if (!string.IsNullOrEmpty(parameters.drink_flag))
                    {
                        //sQuery += String.Format("           AND DRINK_COUNT = {0} ", parameters.drink_count);

                        // 음수 없음, 있음으로 변경
                        if (parameters.drink_flag == "N")
                            sQuery += "           AND DRINK_COUNT = 0 ";
                        else if (parameters.drink_flag == "Y")
                            sQuery += "           AND DRINK_COUNT > 0 ";
                    }

                    // 개체상태 구분
                    if (!string.IsNullOrEmpty(parameters.search_level))
                    {
                        bool isFirst = true;
                        string[] arrayType = parameters.search_level.Split('|');

                        if (arrayType.Length > 0)
                        {
                            sQuery += "           AND ( ";

                            for (int i = 0; i < arrayType.Length; i++)
                            {
                                if (isFirst)
                                {
                                    isFirst = false;
                                    sQuery += string.Format(" ENTITY_STATUS = '{0}' ", arrayType[i]);
                                }
                                else
                                {
                                    sQuery += string.Format(" OR ENTITY_STATUS = '{0}' ", arrayType[i]);
                                }
                            }

                            sQuery += " ) ";
                        }
                    }
                }

                sQuery += string.Format("       ) A " +
                                        "  LEFT OUTER JOIN CODE_MST B " +
                                        "    ON A.ENTITY_SEX = B.CODE_NO " +
                                        "   AND B.CODE_DIV = '100' " +
                                        "   AND B.FLAG = 'Y' " +
                                        "  LEFT OUTER JOIN CODE_MST C " +
                                        "    ON A.ENTITY_TYPE = C.CODE_NO " +
                                        "   AND C.CODE_DIV = '110' " +
                                        "   AND C.FLAG = 'Y' " +
                                        "  LEFT OUTER JOIN CODE_MST D " +
                                        "    ON A.DETAIL_TYPE = D.CODE_NO " +
                                        "   AND D.CODE_DIV = '120' " +
                                        "   AND D.FLAG = 'Y' " +
                                        "  LEFT OUTER JOIN CODE_MST E " +
                                        "    ON A.ENTITY_KIND = E.CODE_NO " +
                                        "   AND E.CODE_DIV = '130' " +
                                        "   AND E.FLAG = 'Y' " +
                                        "  LEFT OUTER JOIN ENTITY_LINEAGE F " +
                                        "    ON A.SEQ = F.ENTITY_SEQ " +
                                        " WHERE A.ROW_NUM BETWEEN {0} AND {1} " +
                                        " ORDER BY A.ROW_NUM ", nFromIndex, nToIndex);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            ClassStruct.ST_ENTITYINFO entityInfo = new ClassStruct.ST_ENTITYINFO
                            {
                                entity_seq = dataReader.GetInt32(0),
                                entity_id = dataReader.GetString(2),
                                entity_sex = dataReader.GetInt32(3),
                                entity_sex_disp = _mClassDatabase.GetSafeString(dataReader, 27),
                                entity_type = dataReader.GetInt32(4),
                                entity_type_disp = _mClassDatabase.GetSafeString(dataReader, 28),
                                detail_type = dataReader.GetInt32(5),
                                detail_type_disp = _mClassDatabase.GetSafeString(dataReader, 29),
                                entity_kind = _mClassDatabase.GetSafeInteger(dataReader, 6),
                                entity_kind_disp = _mClassDatabase.GetSafeString(dataReader, 30),
                                birth_date = dataReader.IsDBNull(7) ? string.Empty : dataReader.GetDateTime(7).ToString("yyyy-MM-dd"),
                                birth_month = dataReader.GetInt32(8),
                                temp_min = dataReader.GetDouble(20).ToString("F2"),
                                temp_max = dataReader.GetDouble(21).ToString("F2"),
                                image_url = _mClassDatabase.GetSafeString(dataReader, 15),
                                warning_level = dataReader.GetString(19),
                                warning_level_disp = string.Empty,
                                drink_count = dataReader.GetInt32(23),
                                tag_id = _mClassDatabase.GetSafeString(dataReader, 14),
                                pregnancy_flag = dataReader.GetInt32(16) == 1 ? "Y" : "N",
                                entity_pin = _mClassDatabase.GetSafeString(dataReader, 13),
                                favorite_flag = dataReader.GetString(24),
                                shipment_date = dataReader.IsDBNull(11) ? string.Empty : dataReader.GetDateTime(11).ToString("yyyy-MM-dd"),
                                shipment_reason = _mClassDatabase.GetSafeString(dataReader, 12),
                                calve_flag = dataReader.GetString(17),
                                calve_count = _mClassDatabase.GetSafeInteger(dataReader, 18),
                                sire_name = _mClassDatabase.GetSafeString(dataReader, 31),
                                sire_info = _mClassDatabase.GetSafeString(dataReader, 32),
                                dam_name = _mClassDatabase.GetSafeString(dataReader, 33),
                                dam_info = _mClassDatabase.GetSafeString(dataReader, 34),
                                gs_name = _mClassDatabase.GetSafeString(dataReader, 35),
                                gs_info = _mClassDatabase.GetSafeString(dataReader, 36),
                                ggs_name = _mClassDatabase.GetSafeString(dataReader, 37),
                                ggs_info = _mClassDatabase.GetSafeString(dataReader, 38)
                            };

                            entityList.Add(entityInfo);
                        }

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SUCCESS,
                            message = string.Empty,
                            data = JsonConvert.SerializeObject(entityList)
                        };
                    }
                    else
                    {
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

            ClassLog._mLogger.InfoFormat("{0}  RESPONSE DATA  [데이타 전송완료]" + Environment.NewLine, sModuleName);
            return response;
            #endregion
        }

        public ClassResponse.RES_RESULT GetChartDonutInfo(ClassRequest.REQ_FARMENTITY parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Entity", "chart_donut");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetChartDonutInfo  ==========", sModuleName);
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

            ClassStruct.ST_CHART_DONUT donutInfo = new ClassStruct.ST_CHART_DONUT();

            try
            {
                // 농장의 UTC Time을 추출한다
                DateTime utcTime = DateTime.UtcNow;
                string sUtcTime = string.Empty;

                _mClassFunction.GetFarmTimeDifference(parameters.farm_seq, out int nDiffHour, out int nDiffMinute);

                utcTime = utcTime.AddHours(nDiffHour).AddMinutes(nDiffMinute);
                sUtcTime = utcTime.ToString("yyyy-MM-dd HH:mm:ss");

                double dMinTemp = 0.0d;
                double dMaxTemp = 100.0d;

                // 개체정보의 온도 설정 값 추출
                string sQuery = string.Format("SELECT TEMP_MIN, TEMP_MAX " +
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

                        dMinTemp = dataReader.GetDouble(0);
                        dMaxTemp = dataReader.GetDouble(1);
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
                    string sFromDate = utcTime.AddHours(-23).ToString("yyyy-MM-dd HH:00:00");
                    string sToDate = utcTime.ToString("yyyy-MM-dd HH:59:59");

                    int nLow = 0;
                    int nHigh = 0;
                    int nNormal = 0;

                    sQuery = string.Format("SELECT AVG_TEMPERATURE " +
                                           "  FROM TEMP_HOUR WITH (INDEX = IDX_ENTITY_CHECK_HOUR) " +
                                           " WHERE ENTITY_SEQ = {0} " +
                                           "   AND CHECK_DATE >= '{1}' " +
                                           "   AND CHECK_DATE <= '{2}' " +
                                           " ORDER BY CHECK_DATE ", parameters.entity_seq, sFromDate, sToDate);

                    if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                double dTemp = dataReader.GetDouble(0);

                                if (dTemp > dMaxTemp) nHigh++;
                                else if (dTemp < dMinTemp) nLow++;
                                else nNormal++;
                            }

                            donutInfo.high_count = nHigh;
                            donutInfo.normal_count = nNormal;
                            donutInfo.low_count = nLow;
                        }
                        else
                        {
                            donutInfo.high_count = 0;
                            donutInfo.normal_count = 0;
                            donutInfo.low_count = 0;
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
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SUCCESS,
                        message = string.Empty,
                        data = JsonConvert.SerializeObject(donutInfo)
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

        public ClassResponse.RES_RESULT GetChartLineInfo(ClassRequest.REQ_CHART_LINE parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Entity", "chart_line");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetChartLineInfo  ==========", sModuleName);
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

                if (string.IsNullOrEmpty(parameters.check_date))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_CHECK_DATE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_CHECK_DATE),
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

            List<ClassStruct.ST_CHART_LINE> lineList = new List<ClassStruct.ST_CHART_LINE>();
            //Dictionary<string, double> tempDictionary = new Dictionary<string, double>();

            try
            {
                // 농장의 UTC 시간을 이용하는 것이 아니고 파라미터로 받은 일자를 이용하도록 수정 (2018-01-24)
                DateTime dtCheckDate = Convert.ToDateTime(parameters.check_date);

                double dMinTemp = 0.0d;
                double dMaxTemp = 100.0d;

                // 개체정보의 온도 설정 값 추출
                string sQuery = string.Format("SELECT TEMP_MIN, TEMP_MAX " +
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

                        dMinTemp = dataReader.GetDouble(0);
                        dMaxTemp = dataReader.GetDouble(1);
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
                    string sFrom = dtCheckDate.AddHours(-23).ToString("yyyy-MM-dd HH:00:00");
                    string sTo = dtCheckDate.ToString("yyyy-MM-dd HH:00:00");

                    sQuery = string.Format("SELECT CHECK_DATE, AVG_TEMPERATURE " +
                                           "  FROM TEMP_HOUR WITH (INDEX = IDX_ENTITY_CHECK_HOUR) " +
                                           " WHERE ENTITY_SEQ = {0} " +
                                           "   AND CHECK_DATE >= '{1}' " +
                                           "   AND CHECK_DATE <= '{2}' " +
                                           " ORDER BY CHECK_DATE ", parameters.entity_seq, sFrom, sTo);

                    if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                ClassStruct.ST_CHART_LINE lineInfo = new ClassStruct.ST_CHART_LINE();

                                string sKey = dataReader.GetDateTime(0).ToString("yyyy-MM-dd HH:00:00");
                                double dTemp = dataReader.GetDouble(1);

                                string sNormalFlag = string.Empty;

                                if (dTemp < dMinTemp || dTemp > dMaxTemp)
                                {
                                    lineInfo.time_disp = sKey;
                                    lineInfo.time_value = dTemp;
                                    lineInfo.normal_flag = "N";
                                }
                                else
                                {
                                    lineInfo.time_disp = sKey;
                                    lineInfo.time_value = dTemp;
                                    lineInfo.normal_flag = "Y";
                                }

                                lineList.Add(lineInfo);

                                //if (!tempDictionary.ContainsKey(sKey)) tempDictionary.Add(sKey, dTemp);
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

                //if (!isError)
                //{
                //    for (int i = -23 ; i <= 0 ; i++)
                //    {
                //        double dTemp = 0.0;
                //        string sKey = dtCheckDate.AddHours(i).ToString("yyyy-MM-dd HH:00:00");

                //        ClassStruct.ST_CHARTLINE lineInfo = new ClassStruct.ST_CHARTLINE();

                //        if (tempDictionary.ContainsKey(sKey))
                //        {
                //            dTemp = tempDictionary[sKey];

                //            if (dTemp < dMinTemp || dTemp > dMaxTemp)
                //            {
                //                lineInfo.time_disp = sKey;
                //                lineInfo.time_value = dTemp;
                //                lineInfo.normal_flag = "N";
                //            }
                //            else
                //            {
                //                lineInfo.time_disp = sKey;
                //                lineInfo.time_value = dTemp;
                //                lineInfo.normal_flag = "Y";
                //            }
                //        }
                //        else
                //        {
                //            lineInfo.time_disp = sKey;
                //            lineInfo.time_value = 0.0;
                //            lineInfo.normal_flag = "N";
                //        }

                //        lineList.Add(lineInfo);
                //    }
                //}

                if (!isError)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SUCCESS,
                        message = string.Empty,
                        data = JsonConvert.SerializeObject(lineList)
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

        public ClassResponse.RES_RESULT GetChartLineInfoRange(ClassRequest.REQ_CHART_LINE_RANGE parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Entity", "chart_line_range");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetChartLineInfoRange  ==========", sModuleName);
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

                DateTime dtValue = DateTime.MinValue;

                if (string.IsNullOrEmpty(parameters.check_start_date) && !DateTime.TryParse(parameters.check_start_date, out dtValue))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_CHECK_DATE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_CHECK_DATE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (string.IsNullOrEmpty(parameters.check_end_date) && !DateTime.TryParse(parameters.check_end_date, out dtValue))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_CHECK_DATE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_CHECK_DATE),
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

            List<ClassStruct.ST_CHART_LINE> lineList = new List<ClassStruct.ST_CHART_LINE>();

            try
            {
                double dMinTemp = 0.0d;
                double dMaxTemp = 100.0d;

                // 개체정보의 온도 설정 값 추출
                string sQuery = string.Format("SELECT TEMP_MIN, TEMP_MAX " +
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

                        dMinTemp = dataReader.GetDouble(0);
                        dMaxTemp = dataReader.GetDouble(1);
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
                    sQuery = $@"
 SELECT CHECK_DATE, AVG_TEMPERATURE 
 FROM TEMP_HOUR WITH (INDEX = IDX_ENTITY_CHECK_HOUR) 
 WHERE ENTITY_SEQ = {parameters.entity_seq} 
 AND (CONVERT(CHAR(10), CHECK_DATE, 23) BETWEEN '{parameters.check_start_date}' AND '{parameters.check_end_date}')
 ORDER BY CHECK_DATE ";

                    if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                ClassStruct.ST_CHART_LINE lineInfo = new ClassStruct.ST_CHART_LINE();

                                string sKey = dataReader.GetDateTime(0).ToString("yyyy-MM-dd HH:00:00");
                                double dTemp = dataReader.GetDouble(1);

                                string sNormalFlag = string.Empty;

                                if (dTemp < dMinTemp || dTemp > dMaxTemp)
                                {
                                    lineInfo.time_disp = sKey;
                                    lineInfo.time_value = dTemp;
                                    lineInfo.normal_flag = "N";
                                }
                                else
                                {
                                    lineInfo.time_disp = sKey;
                                    lineInfo.time_value = dTemp;
                                    lineInfo.normal_flag = "Y";
                                }

                                lineList.Add(lineInfo);
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
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SUCCESS,
                        message = string.Empty,
                        data = JsonConvert.SerializeObject(lineList)
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

        public ClassResponse.RES_RESULT GetChartColorInfo(ClassRequest.REQ_CHART_COLOR parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Entity", "chart_color");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetChartColorInfo  ==========", sModuleName);
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

                if (string.IsNullOrEmpty(parameters.from_date))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_FROM_DATE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_FROM_DATE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (string.IsNullOrEmpty(parameters.to_date))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_TO_DATE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_TO_DATE),
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

            List<ClassStruct.ST_CHART_COLOR> colorList = new List<ClassStruct.ST_CHART_COLOR>();
            Dictionary<string, ClassStruct.ST_COLOR_INFO> colorDictionary = new Dictionary<string, ClassStruct.ST_COLOR_INFO>();

            try
            {
                ClassColor classColor = new ClassColor();

                double dMinTemp = 0.0d;
                double dMaxTemp = 100.0d;

                // 개체정보의 온도 설정 값 추출
                // 색상은 COLOR_MIN / COLOR_MAX 값을 사용하도록 수정 (2017-10-11)
                string sQuery = string.Format("SELECT COLOR_MIN, COLOR_MAX " +
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

                        dMinTemp = dataReader.GetDouble(0);
                        dMaxTemp = dataReader.GetDouble(1);
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
                    string sFromDate = string.Format("{0} 00:00:00", parameters.from_date);
                    string sToDate = string.Format("{0} 23:00:00", parameters.to_date);

                    //sQuery = String.Format("SELECT CHECK_DATE, ROUND(AVG_TEMPERATURE, 1) FROM TEMP_HOUR " +
                    //                        " WHERE CHECK_DATE >= '{2}' " +
                    //                        "   AND CHECK_DATE <= '{3}' " +
                    //                        "   AND FARM_SEQ = {0} " +
                    //                        "   AND ENTITY_SEQ = {1} " +
                    //                        " ORDER BY CHECK_DATE ",
                    //                        parameters.farm_seq, parameters.entity_seq, sFromDate, sToDate);

                    sQuery = string.Format("SELECT CHECK_DATE, ROUND(AVG_TEMPERATURE, 1) " +
                                           "  FROM TEMP_HOUR WITH (INDEX = IDX_ENTITY_CHECK_HOUR) " +
                                           " WHERE ENTITY_SEQ = {0} " +
                                           "   AND CHECK_DATE >= '{1}' " +
                                           "   AND CHECK_DATE <= '{2}' " +
                                           " ORDER BY CHECK_DATE ",
                                           parameters.entity_seq, sFromDate, sToDate);

                    if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                int index = 0;
                                double dTemp = dataReader.GetDouble(1);

                                if (dTemp <= dMinTemp) index = 0;
                                else if (dTemp >= dMaxTemp) index = classColor.GetColorArrayLength() - 1;
                                else index = (int)(dTemp * 10) - (int)(dMinTemp * 10);

                                ClassStruct.ST_COLOR_INFO colorInfo = new ClassStruct.ST_COLOR_INFO
                                {
                                    time_disp = dataReader.GetDateTime(0).ToString("yyyy-MM-dd HH:00:00"),
                                    time_value = classColor.GetColorString(index),
                                    normal_flag = dTemp > dMinTemp && dTemp < dMaxTemp ? "Y" : "N"
                                };

                                if (!colorDictionary.ContainsKey(colorInfo.time_disp)) colorDictionary.Add(colorInfo.time_disp, colorInfo);
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
                    // 검색기간을 입력받은 파라미터로 처리하도록 변경 (v1.2.3)
                    DateTime dtFrom = Convert.ToDateTime(parameters.from_date);
                    DateTime dtTo = Convert.ToDateTime(parameters.to_date);

                    TimeSpan timespan = dtTo - dtFrom;
                    int nDays = (int)timespan.TotalDays;

                    for (int day = 0; day <= nDays; day++)
                    {
                        ClassStruct.ST_CHART_COLOR chartColor = new ClassStruct.ST_CHART_COLOR();
                        List<ClassStruct.ST_COLOR_INFO> lineList = new List<ClassStruct.ST_COLOR_INFO>();

                        // 검색기간을 입력받은 파라미터로 처리하도록 변경 (v1.2.3)
                        DateTime sDate = dtFrom.AddDays(day);

                        for (int i = 0; i <= 23; i++)
                        {
                            string sCheckDate = string.Format("{0} {1}:00:00", sDate.ToString("yyyy-MM-dd"), i.ToString("D2"));
                            ClassStruct.ST_COLOR_INFO lineInfo = new ClassStruct.ST_COLOR_INFO();

                            // Dictionary와 비교해서 처리한다
                            if (colorDictionary.ContainsKey(sCheckDate))
                            {
                                lineInfo = colorDictionary[sCheckDate];
                            }
                            else
                            {
                                lineInfo.time_disp = sCheckDate;
                                lineInfo.time_value = "#FFFF00";
                                lineInfo.normal_flag = "N";
                            }

                            lineList.Add(lineInfo);
                        }

                        chartColor.day_disp = sDate.ToString("yyyy-MM-dd");
                        chartColor.day_value = lineList;

                        colorList.Add(chartColor);
                    }
                }

                if (!isError)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SUCCESS,
                        message = string.Empty,
                        data = JsonConvert.SerializeObject(colorList)
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

            if (response.result == ClassError.RESULT_SUCCESS)
                ClassLog._mLogger.InfoFormat("{0}  RESPONSE DATA  [데이타 전송완료]" + Environment.NewLine, sModuleName);
            else
                ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);

            return response;
            #endregion
        }

        public ClassResponse.RES_RESULT GetChartListInfo(ClassRequest.REQ_FARMPAGESEQ parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Entity", "chart_list");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetChartListInfo  ==========", sModuleName);
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

            List<ClassStruct.ST_CHART_LINE> lineList = new List<ClassStruct.ST_CHART_LINE>();

            try
            {
                double dMinTemp = 0.0d;
                double dMaxTemp = 100.0d;

                // 개체정보의 온도 설정 값 추출
                string sQuery = string.Format("SELECT TEMP_MIN, TEMP_MAX " +
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

                        dMinTemp = dataReader.GetDouble(0);
                        dMaxTemp = dataReader.GetDouble(1);
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
                    // 페이지 처리를 위해서 page_seq 파라미터 값을 기준으로 추출한다 (2018-06-19)
                    sQuery = string.Format("SELECT TOP 20 SEQ, CHECK_DATE, AVG_TEMPERATURE " +
                                           "  FROM TEMP_HOUR " +
                                           " WHERE FARM_SEQ = {0} " +
                                           "   AND ENTITY_SEQ = {1} ", parameters.farm_seq, parameters.entity_seq);
                    if (parameters.page_seq > 0) sQuery += string.Format("   AND SEQ < {0} ", parameters.page_seq);
                    sQuery += " ORDER BY CHECK_DATE DESC ";

                    if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                ClassStruct.ST_CHART_LINE lineInfo = new ClassStruct.ST_CHART_LINE
                                {
                                    seq = dataReader.GetInt32(0),
                                    time_disp = dataReader.GetDateTime(1).ToString("yyyy-MM-dd HH:00:00")
                                };
                                double dValue = Convert.ToDouble(dataReader.GetDouble(2).ToString("F2"));
                                lineInfo.time_value = dValue;
                                if (dValue < dMinTemp) lineInfo.normal_flag = "L";
                                else if (dValue > dMaxTemp) lineInfo.normal_flag = "H";
                                else lineInfo.normal_flag = "Y";

                                lineList.Add(lineInfo);
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
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SUCCESS,
                        message = string.Empty,
                        data = JsonConvert.SerializeObject(lineList)
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

        //public ClassResponse.RES_RESULT GetChartAlarmInfo(ClassRequest.REQ_FARMPAGESEQ parameters)
        //{
        //    string sModuleName = String.Format("[{0}][{1}]", "Entity", "chart_alarm");
        //    string requestData = JsonConvert.SerializeObject(parameters);

        //    ClassLog._mLogger.InfoFormat("{0}  ==========  START GetChartAlarmInfo  ==========", sModuleName);
        //    ClassLog._mLogger.InfoFormat("{0}  RECEIVE REQUEST DATA  [{1}]", sModuleName, requestData);

        //    ClassResponse.RES_RESULT response = new ClassResponse.RES_RESULT();
        //    string receiveData = String.Empty;

        //    #region Check Parameter Process
        //    try
        //    {
        //        if (String.IsNullOrEmpty(parameters.lang_code))
        //        {
        //            response = new ClassResponse.RES_RESULT
        //            {
        //                result = ClassError.RESULT_PARAM_ERROR_LANG_CODE,
        //                message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_LANG_CODE),
        //                data = String.Empty
        //            };

        //            ClassLog._mLogger.Info(String.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
        //            return response;
        //        }

        //        if (Array.IndexOf(ClassStruct.LANGUAGE_CODE, parameters.lang_code) < 0)
        //        {
        //            response = new ClassResponse.RES_RESULT
        //            {
        //                result = ClassError.RESULT_PARAM_ERROR_LANG_CODE,
        //                message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_LANG_CODE),
        //                data = String.Empty
        //            };

        //            ClassLog._mLogger.Info(String.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
        //            return response;
        //        }

        //        if (parameters.farm_seq < 1)
        //        {
        //            response = new ClassResponse.RES_RESULT
        //            {
        //                result = ClassError.RESULT_PARAM_ERROR_FARM_SEQ,
        //                message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_FARM_SEQ),
        //                data = String.Empty
        //            };

        //            ClassLog._mLogger.Info(String.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
        //            return response;
        //        }

        //        if (parameters.entity_seq < 0)
        //        {
        //            response = new ClassResponse.RES_RESULT
        //            {
        //                result = ClassError.RESULT_PARAM_ERROR_ENTITY_SEQ,
        //                message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_ENTITY_SEQ),
        //                data = String.Empty
        //            };

        //            ClassLog._mLogger.Info(String.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
        //            return response;
        //        }
        //    }
        //    catch
        //    {
        //        response = new ClassResponse.RES_RESULT
        //        {
        //            result = ClassError.RESULT_SYSTEM_ERROR_EXCEPTION,
        //            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_EXCEPTION),
        //            data = String.Empty
        //        };

        //        ClassLog._mLogger.Info(String.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
        //        return response;
        //    }
        //    #endregion

        //    #region Check Database Process
        //    if (!_mClassDatabase.GetConnectionState())
        //    {
        //        if (!_mClassDatabase.OpenDatabase())
        //        {
        //            response = new ClassResponse.RES_RESULT
        //            {
        //                result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
        //                message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
        //                data = String.Empty
        //            };

        //            ClassLog._mLogger.Info(String.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
        //            return response;
        //        }
        //    }
        //    #endregion

        //    #region Business Logic Process
        //    bool isError = false;
        //    string sQuery = String.Empty;

        //    OleDbDataReader dataReader = null;
        //    List<ClassStruct.ST_CHARTALARM> alarmList = new List<ClassStruct.ST_CHARTALARM>();

        //    try
        //    {
        //        double dMinTemp = 0.0d;
        //        double dMaxTemp = 100.0d;

        //        // 개체정보의 온도 설정 값 추출
        //        sQuery = String.Format("SELECT TEMP_MIN, TEMP_MAX " +
        //                               "  FROM ENTITY_NEW_INFO " +
        //                               " WHERE SEQ = {0} " +
        //                               "   AND FARM_SEQ = {1} " +
        //                               "   AND FLAG = 'Y' " +
        //                               "   AND ACTIVE_FLAG = 'Y' ", parameters.entity_seq, parameters.farm_seq);

        //        if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
        //        {
        //            if (dataReader.HasRows)
        //            {
        //                dataReader.Read();

        //                dMinTemp = dataReader.GetDouble(0);
        //                dMaxTemp = dataReader.GetDouble(1);
        //            }
        //            else
        //            {
        //                isError = true;

        //                response = new ClassResponse.RES_RESULT
        //                {
        //                    result = ClassError.RESULT_SEARCH_NOTEXIST_ENTITY,
        //                    message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SEARCH_NOTEXIST_ENTITY),
        //                    data = String.Empty
        //                };
        //            }

        //            dataReader.Close();
        //            dataReader.Dispose();
        //        }
        //        else
        //        {
        //            isError = true;

        //            response = new ClassResponse.RES_RESULT
        //            {
        //                result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
        //                message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
        //                data = String.Empty
        //            };
        //        }

        //        if (!isError)
        //        {
        //            sQuery = String.Format("SELECT TOP 20 SEQ, ALARM_LEVEL, CREATE_DATE " +
        //                                   "  FROM ALARM_DANGER " +
        //                                   " WHERE FARM_SEQ = {0} " +
        //                                   "   AND ENTITY_SEQ = {1} ", parameters.farm_seq, parameters.entity_seq);
        //            if (parameters.page_seq > 0) sQuery += String.Format("   AND SEQ < {0} ", parameters.page_seq);
        //            sQuery += " ORDER BY SEQ DESC ";

        //            if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
        //            {
        //                if (dataReader.HasRows)
        //                {
        //                    while (dataReader.Read())
        //                    {
        //                        ClassStruct.ST_CHARTALARM alarmInfo = new ClassStruct.ST_CHARTALARM();

        //                        int nAlarm = dataReader.GetInt32(1);
        //                        //int nLevel = 0;
        //                        string sAlarm = String.Empty;

        //                        // 알람구분 
        //                        // 1 : 체온상승관심 / 2 : 체온상승주의 / 3 : 체온상승경고 / 4 : 체온하락경고 / 5 : 활동량증가 / 6 : 발정 / 7 : 분만 / 8 : 질병

        //                        switch (parameters.lang_code)
        //                        {
        //                            case "KR":
        //                                {
        //                                    switch (nAlarm)
        //                                    {
        //                                        case 1: sAlarm = "체온상승관심"; break;
        //                                        case 2: sAlarm = "체온상승주의"; break;
        //                                        case 3: sAlarm = "체온상승경고"; break;
        //                                        case 4: sAlarm = "체온하락경고"; break;
        //                                        case 5: sAlarm = "활동량증가"; break;
        //                                        case 6: sAlarm = "발정"; break;
        //                                        case 7: sAlarm = "분만"; break;
        //                                        case 8: sAlarm = "질병"; break;
        //                                    }

        //                                    break;
        //                                }
        //                            case "JP":
        //                                {
        //                                    switch (nAlarm)
        //                                    {
        //                                        case 1: sAlarm = "体温上昇関心"; break;
        //                                        case 2: sAlarm = "体温上昇注意"; break;
        //                                        case 3: sAlarm = "体温上昇警告"; break;
        //                                        case 4: sAlarm = "体温下落警告"; break;
        //                                        case 5: sAlarm = "活動量の増加"; break;
        //                                        case 6: sAlarm = "発情"; break;
        //                                        case 7: sAlarm = "分娩"; break;
        //                                        case 8: sAlarm = "疾病"; break;
        //                                    }

        //                                    break;
        //                                }
        //                            case "US":
        //                                {
        //                                    switch (nAlarm)
        //                                    {
        //                                        case 1: sAlarm = "High-Advisory"; break;
        //                                        case 2: sAlarm = "High-Watch"; break;
        //                                        case 3: sAlarm = "High-Warning"; break;
        //                                        case 4: sAlarm = "Low-Warning"; break;
        //                                        case 5: sAlarm = "Increase-Activity"; break;
        //                                        case 6: sAlarm = "Estrus"; break;
        //                                        case 7: sAlarm = "Calving"; break;
        //                                        case 8: sAlarm = "Disease"; break;
        //                                    }

        //                                    break;
        //                                }
        //                            case "CN":
        //                                {
        //                                    switch (nAlarm)
        //                                    {
        //                                        case 1: sAlarm = "体温上升关注"; break;
        //                                        case 2: sAlarm = "体温上升注意"; break;
        //                                        case 3: sAlarm = "体温上升警告"; break;
        //                                        case 4: sAlarm = "体温下降警告"; break;
        //                                        case 5: sAlarm = "활동량증가"; break;
        //                                        case 6: sAlarm = "발정"; break;
        //                                        case 7: sAlarm = "분만"; break;
        //                                        case 8: sAlarm = "질병"; break;
        //                                    }

        //                                    break;
        //                                }
        //                            case "PT":
        //                                {
        //                                    switch (nAlarm)
        //                                    {
        //                                        case 1: sAlarm = "Atenção de alta"; break;
        //                                        case 2: sAlarm = "Cuidado de alta"; break;
        //                                        case 3: sAlarm = "Alerta de alta"; break;
        //                                        case 4: sAlarm = "Alerta de baixa"; break;
        //                                        case 5: sAlarm = "Aumentar atividade"; break;
        //                                        case 6: sAlarm = "Estro"; break;
        //                                        case 7: sAlarm = "Parto"; break;
        //                                        case 8: sAlarm = "Doenças"; break;
        //                                    }

        //                                    break;
        //                                }
        //                            case "BR":
        //                                {
        //                                    switch (nAlarm)
        //                                    {
        //                                        case 1: sAlarm = "Atenção de alta"; break;
        //                                        case 2: sAlarm = "Cuidado de alta"; break;
        //                                        case 3: sAlarm = "Alerta de alta"; break;
        //                                        case 4: sAlarm = "Alerta de baixa"; break;
        //                                        case 5: sAlarm = "Aumentar atividade"; break;
        //                                        case 6: sAlarm = "Estro"; break;
        //                                        case 7: sAlarm = "Parto"; break;
        //                                        case 8: sAlarm = "Doenças"; break;
        //                                    }

        //                                    break;
        //                                }
        //                            default:
        //                                {
        //                                    switch (nAlarm)
        //                                    {
        //                                        case 1: sAlarm = "High-Advisory"; break;
        //                                        case 2: sAlarm = "High-Watch"; break;
        //                                        case 3: sAlarm = "High-Warning"; break;
        //                                        case 4: sAlarm = "Low-Warning"; break;
        //                                        case 5: sAlarm = "Increase-Activity"; break;
        //                                        case 6: sAlarm = "Estrus"; break;
        //                                        case 7: sAlarm = "Calving"; break;
        //                                        case 8: sAlarm = "Disease"; break;
        //                                    }

        //                                    break;
        //                                }
        //                        }

        //                        alarmInfo.seq = dataReader.GetInt32(0);
        //                        alarmInfo.warning_time = dataReader.GetDateTime(2).ToString("yyyy-MM-dd HH:mm:ss");
        //                        alarmInfo.warning_level = nAlarm;
        //                        alarmInfo.warning_level_disp = sAlarm;

        //                        alarmList.Add(alarmInfo);
        //                    }
        //                }

        //                dataReader.Close();
        //                dataReader.Dispose();
        //            }
        //            else
        //            {
        //                isError = true;

        //                response = new ClassResponse.RES_RESULT
        //                {
        //                    result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
        //                    message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
        //                    data = String.Empty
        //                };
        //            }
        //        }

        //        if (!isError)
        //        {
        //            response = new ClassResponse.RES_RESULT
        //            {
        //                result = ClassError.RESULT_SUCCESS,
        //                message = String.Empty,
        //                data = JsonConvert.SerializeObject(alarmList)
        //            };
        //        }
        //    }
        //    catch
        //    {
        //        response = new ClassResponse.RES_RESULT
        //        {
        //            result = ClassError.RESULT_SYSTEM_ERROR_EXCEPTION,
        //            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_EXCEPTION),
        //            data = String.Empty
        //        };
        //    }
        //    finally
        //    {
        //        _mClassDatabase.CloseDatabase();
        //    }

        //    ClassLog._mLogger.Info(String.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
        //    return response;
        //    #endregion
        //}

        public ClassResponse.RES_RESULT SetInsertEntity(ClassRequest.REQ_ENTITYINSERT parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Entity", "insert");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START SetInsertEntity  ==========", sModuleName);
            ClassLog._mLogger.InfoFormat("{0}  RECEIVE REQUEST DATA  [{1}]", sModuleName, requestData);

            ClassResponse.RES_RESULT response = new ClassResponse.RES_RESULT();
            string receiveData = string.Empty;

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

                if (string.IsNullOrEmpty(parameters.entity_id))
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

                if (parameters.entity_sex < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_ENTITY_SEX,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_ENTITY_SEX),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (parameters.entity_type < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_ENTITY_TYPE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_ENTITY_TYPE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (parameters.detail_type < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_DETAIL_TYPE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_DETAIL_TYPE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (string.IsNullOrEmpty(parameters.entity_birth))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_ENTITY_BIRTH,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_ENTITY_BIRTH),
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
            int nSEQ = 0;
            bool isError = false;
            string sQuery = string.Empty;
            OleDbDataReader dataReader = null;

            try
            {
                // 농장의 UTC Time을 추출한다
                DateTime utcTime = DateTime.UtcNow;
                string sUtcTime = string.Empty;

                _mClassFunction.GetFarmTimeDifference(parameters.farm_seq, out int nDiffHour, out int nDiffMinute);

                utcTime = utcTime.AddHours(nDiffHour).AddMinutes(nDiffMinute);
                sUtcTime = utcTime.ToString("yyyy-MM-dd HH:mm:ss");

                _mClassFunction.GetFarmTempSetting(parameters.farm_seq, out double dTempMin, out double dTempMax);

                // 같은 농장에 같은 ENTITY_NO는 등록할 수 없도록 체크한다
                sQuery = string.Format("SELECT SEQ " +
                                       "  FROM ENTITY_NEW_INFO " +
                                       " WHERE FARM_SEQ = {0} " +
                                       "   AND ENTITY_NO = '{1}' " +
                                       "   AND FLAG = 'Y' ", parameters.farm_seq, parameters.entity_id);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        isError = true;

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_ERROR_ENTITY_NO_EXISTED,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_ERROR_ENTITY_NO_EXISTED),
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

                // 같은 ENTITY_PIN은 등록할 수 없도록 체크한다
                if (!isError && !string.IsNullOrEmpty(parameters.entity_pin))
                {
                    sQuery = string.Format("SELECT SEQ " +
                                           "  FROM ENTITY_NEW_INFO " +
                                           " WHERE ENTITY_PIN = '{0}' " +
                                           "   AND FLAG = 'Y' ", parameters.entity_pin);

                    if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                    {
                        if (dataReader.HasRows)
                        {
                            isError = true;

                            response = new ClassResponse.RES_RESULT
                            {
                                result = ClassError.RESULT_ERROR_ENTITY_PIN_EXISTED,
                                message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_ERROR_ENTITY_PIN_EXISTED),
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
                }

                if (!isError)
                {
                    _mClassDatabase.TransBegin();

                    // 월령 계산 및 등록일자 농장의 UTC 타임을 이용한다
                    int nDiffDays = 0;
                    int nBirthCode = 0;

                    // 월령구분을 계산한다
                    DateTime birthDate = Convert.ToDateTime(parameters.entity_birth);
                    TimeSpan timespan = Convert.ToDateTime(utcTime.ToString("yyyy-MM-dd")) - Convert.ToDateTime(birthDate.ToString("yyyy-MM-dd"));
                    nDiffDays = timespan.Days;

                    if (nDiffDays < 270)
                        nBirthCode = 3;
                    else if (nDiffDays >= 270 && nDiffDays <= 569)
                        nBirthCode = 1;
                    else
                        nBirthCode = 2;

                    string sImageURL = string.Empty;
                    if (!string.IsNullOrEmpty(parameters.image_name))
                    {
#if DEBUG
                        //sImageURL = String.Format("http://www.ulikekorea.com/LC_SERVICE/UploadImage/{0}", parameters.image_name);
                        sImageURL = String.Format("http://www.livecareworld.com/LIVECARE/LC_SERVICE/UploadImage/{0}", parameters.image_name);
#else
                        sImageURL = string.Format("http://www.livecareworld.com/LIVECARE/LC_SERVICE/UploadImage/{0}", parameters.image_name);
#endif
                    }

                    sQuery = "INSERT INTO ENTITY_NEW_INFO (FARM_SEQ, ENTITY_NO, ENTITY_SEX, ENTITY_TYPE, DETAIL_TYPE, ENTITY_KIND, BIRTH, BIRTH_MONTH, BIRTH_CODE, " +
                             "                             IMAGE_URL, TAG_ID, CREATE_DATE, ENTITY_PIN, SHIPMENT, SHIPMENT_REASON, TEMP_MIN, TEMP_MAX, CALVE_COUNT) " +
                             " OUTPUT INSERTED.SEQ VALUES ( ";
                    sQuery += parameters.farm_seq + ", ";
                    sQuery += "'" + parameters.entity_id + "', ";
                    sQuery += parameters.entity_sex + ", ";
                    sQuery += parameters.entity_type + ", ";
                    sQuery += parameters.detail_type + ", ";
                    sQuery += parameters.entity_kind + ", ";
                    sQuery += "CONVERT(DATETIME, '" + parameters.entity_birth + "'), ";
                    sQuery += "DATEDIFF(MONTH, CONVERT(DATETIME, '" + parameters.entity_birth + "'), CONVERT(DATETIME, '" + sUtcTime + "')), ";
                    sQuery += nBirthCode + ", ";
                    if (string.IsNullOrEmpty(parameters.image_name)) sQuery += "NULL, ";
                    else sQuery += "'" + sImageURL + "', ";
                    if (string.IsNullOrEmpty(parameters.tag_id)) sQuery += "NULL, ";
                    else sQuery += "'" + parameters.tag_id + "', ";
                    sQuery += "'" + sUtcTime + "', ";
                    if (string.IsNullOrEmpty(parameters.entity_pin)) sQuery += "NULL, ";
                    else sQuery += "'" + parameters.entity_pin + "', ";
                    if (string.IsNullOrEmpty(parameters.shipment)) sQuery += "NULL, ";
                    else sQuery += "'" + parameters.shipment + "', ";
                    if (string.IsNullOrEmpty(parameters.reason)) sQuery += "NULL, ";
                    else sQuery += "N'" + parameters.reason + "', ";
                    sQuery += dTempMin + ", ";
                    sQuery += dTempMax + ",";
                    sQuery += parameters.calve_count + ")";

                    nSEQ = _mClassDatabase.QueryExecuteScalar(sQuery);

                    if (nSEQ < 1)
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

                    // 혈통정보를 등록한다
                    // 모두 빈 값인 경우에는 등록하지 않는다
                    if (!isError)
                    {
                        if (!string.IsNullOrEmpty(parameters.sire_name) || !string.IsNullOrEmpty(parameters.sire_info) ||
                            !string.IsNullOrEmpty(parameters.dam_name) || !string.IsNullOrEmpty(parameters.dam_info) ||
                            !string.IsNullOrEmpty(parameters.gs_name) || !string.IsNullOrEmpty(parameters.gs_info) ||
                            !string.IsNullOrEmpty(parameters.gs_name) || !string.IsNullOrEmpty(parameters.ggs_info))
                        {
                            sQuery = "INSERT INTO ENTITY_LINEAGE (FARM_SEQ, ENTITY_SEQ, SIRE_NAME, SIRE_INFO, DAM_NAME, DAM_INFO, GS_NAME, GS_INFO, GGS_NAME, GGS_INFO) VALUES (";
                            sQuery += parameters.farm_seq + ", ";
                            sQuery += nSEQ + ", ";
                            if (string.IsNullOrEmpty(parameters.sire_name)) sQuery += "NULL , ";
                            else sQuery += "'" + parameters.sire_name + "', ";
                            if (string.IsNullOrEmpty(parameters.sire_info)) sQuery += "NULL , ";
                            else sQuery += "'" + parameters.sire_info + "', ";
                            if (string.IsNullOrEmpty(parameters.dam_name)) sQuery += "NULL , ";
                            else sQuery += "'" + parameters.dam_name + "', ";
                            if (string.IsNullOrEmpty(parameters.dam_info)) sQuery += "NULL , ";
                            else sQuery += "'" + parameters.dam_info + "', ";
                            if (string.IsNullOrEmpty(parameters.gs_name)) sQuery += "NULL , ";
                            else sQuery += "'" + parameters.gs_name + "', ";
                            if (string.IsNullOrEmpty(parameters.gs_info)) sQuery += "NULL , ";
                            else sQuery += "'" + parameters.gs_info + "', ";
                            if (string.IsNullOrEmpty(parameters.ggs_name)) sQuery += "NULL , ";
                            else sQuery += "'" + parameters.ggs_name + "', ";
                            if (string.IsNullOrEmpty(parameters.ggs_info)) sQuery += "NULL";
                            else sQuery += "'" + parameters.ggs_info + "'";
                            sQuery += ")";

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

                    if (!isError)
                    {
                        if (_mClassFunction.CheckTableExist(nSEQ, out bool isExist))
                        {
                            if (!isExist)
                            {
                                if (_mClassFunction.CreateTable(nSEQ))
                                {
                                    if (_mClassFunction.CheckTableExist(nSEQ, out isExist))
                                    {
                                        if (!isExist)
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
                            }
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

                    if (!isError && !string.IsNullOrEmpty(parameters.tag_id))
                    {
                        sQuery = string.Format("UPDATE TAG_INFO SET ENTITY_SEQ = {0} WHERE TAG_ID = '{1}'", nSEQ, parameters.tag_id);
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

        public ClassResponse.RES_RESULT SetUpdateEntity(ClassRequest.REQ_ENTITYUPDATE parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Entity", "update");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START SetUpdateEntity  ==========", sModuleName);
            ClassLog._mLogger.InfoFormat("{0}  RECEIVE REQUEST DATA  [{1}]", sModuleName, requestData);

            ClassResponse.RES_RESULT response = new ClassResponse.RES_RESULT();
            string receiveData = string.Empty;

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

                if (string.IsNullOrEmpty(parameters.entity_id))
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

                if (parameters.entity_sex < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_ENTITY_SEX,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_ENTITY_SEX),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (parameters.entity_type < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_ENTITY_TYPE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_ENTITY_TYPE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (parameters.detail_type < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_DETAIL_TYPE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_DETAIL_TYPE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (string.IsNullOrEmpty(parameters.entity_birth))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_ENTITY_BIRTH,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_ENTITY_BIRTH),
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
            int count = 0;
            bool isError = false;
            string sQuery = string.Empty;

            OleDbDataReader dataReader = null;

            string sTagID = string.Empty;

            try
            {
                // 농장의 UTC Time을 추출한다
                DateTime utcTime = DateTime.UtcNow;
                string sUtcTime = string.Empty;

                _mClassFunction.GetFarmTimeDifference(parameters.farm_seq, out int nDiffHour, out int nDiffMinute);

                utcTime = utcTime.AddHours(nDiffHour).AddMinutes(nDiffMinute);
                sUtcTime = utcTime.ToString("yyyy-MM-dd HH:mm:ss");

                // 신규 등록되는 태그가 사용중인 태그인지 체크한다
                //if (!String.IsNullOrEmpty(parameters.tag_id))
                //{
                //    sQuery = String.Format("SELECT SEQ " +
                //                           "  FROM ENTITY_NEW_INFO " +
                //                           " WHERE SEQ <> {0} " +
                //                           "  AND TAG_ID = '{1}' " +
                //                           "  AND FLAG = 'Y' " +
                //                           "  AND ACTIVE_FLAG = 'Y' ", parameters.entity_seq, parameters.tag_id);

                //    if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                //    {
                //        if (dataReader.HasRows)
                //        {
                //            isError = true;

                //            response = new ClassResponse.RES_RESULT
                //            {
                //                result = ClassError.RESULT_ERROR_TAG_ALREADY_USED,
                //                message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_ERROR_TAG_ALREADY_USED),
                //                data = String.Empty
                //            };
                //        }

                //        dataReader.Close();
                //        dataReader.Dispose();
                //    }
                //    else
                //    {
                //        isError = true;

                //        response = new ClassResponse.RES_RESULT
                //        {
                //            result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
                //            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
                //            data = String.Empty
                //        };
                //    }
                //}

                // 같은 농장에 같은 ENTITY_NO는 등록할 수 없도록 체크한다
                sQuery = string.Format("SELECT SEQ " +
                                       "  FROM ENTITY_NEW_INFO " +
                                       " WHERE SEQ <> {0} " +
                                       "   AND FARM_SEQ = {1} " +
                                       "   AND ENTITY_NO = '{2}' " +
                                       "   AND FLAG = 'Y' ", parameters.entity_seq, parameters.farm_seq, parameters.entity_id);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        isError = true;

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_ERROR_ENTITY_NO_EXISTED,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_ERROR_ENTITY_NO_EXISTED),
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

                // 기존에 등록된 태그 정보를 추출
                //if (!isError)
                //{
                //    sQuery = String.Format("SELECT TAG_ID " +
                //                           "  FROM ENTITY_NEW_INFO " +
                //                           " WHERE SEQ = {0} " +
                //                           "   AND FLAG = 'Y' " +
                //                           "   AND ACTIVE_FLAG = 'Y' ", parameters.entity_seq);

                //    if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                //    {
                //       if (dataReader.HasRows)
                //       {
                //           if (dataReader.HasRows)
                //           {
                //               dataReader.Read();
                //               sTagID = _mClassDatabase.GetSafeString(dataReader, 0);
                //           }
                //       }
                //       else
                //       {
                //            isError = true;

                //            response = new ClassResponse.RES_RESULT
                //            {
                //                result = ClassError.RESULT_SEARCH_NOTEXIST_ENTITY,
                //                message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SEARCH_NOTEXIST_ENTITY),
                //                data = String.Empty
                //            };
                //        }

                //        dataReader.Close();
                //        dataReader.Dispose();
                //    }
                //    else
                //    {
                //        isError = true;

                //        response = new ClassResponse.RES_RESULT
                //        {
                //            result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
                //            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
                //            data = String.Empty
                //        };
                //    }
                //}

                // 같은 ENTITY_PIN은 등록할 수 없도록 체크한다
                if (!isError && !string.IsNullOrEmpty(parameters.entity_pin))
                {
                    sQuery = string.Format("SELECT SEQ " +
                                           "  FROM ENTITY_NEW_INFO " +
                                           " WHERE SEQ <> {0} " +
                                           "   AND ENTITY_PIN = '{1}' " +
                                           "   AND FLAG = 'Y' ", parameters.entity_seq, parameters.entity_pin);

                    if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                    {
                        if (dataReader.HasRows)
                        {
                            isError = true;

                            response = new ClassResponse.RES_RESULT
                            {
                                result = ClassError.RESULT_ERROR_ENTITY_PIN_EXISTED,
                                message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_ERROR_ENTITY_PIN_EXISTED),
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
                }

                if (!isError)
                {
                    _mClassDatabase.TransBegin();

                    string sImageURL = string.Empty;
                    if (!string.IsNullOrEmpty(parameters.image_name))
                    {
#if DEBUG
                        //sImageURL = String.Format("http://www.ulikekorea.com/LC_SERVICE/UploadImage/{0}", parameters.image_name);
                        sImageURL = String.Format("http://www.livecareworld.com/LIVECARE/LC_SERVICE/UploadImage/{0}", parameters.image_name);
#else
                        sImageURL = string.Format("http://www.livecareworld.com/LIVECARE/LC_SERVICE/UploadImage/{0}", parameters.image_name);
#endif
                    }

                    // FARM_SEQ는 변경하지 않는다
                    sQuery = "UPDATE ENTITY_NEW_INFO SET ";
                    sQuery += "ENTITY_NO = N'" + parameters.entity_id + "', ";
                    sQuery += "ENTITY_SEX = " + parameters.entity_sex + ", ";
                    sQuery += "ENTITY_TYPE = " + parameters.entity_type + ", ";
                    sQuery += "DETAIL_TYPE = " + parameters.detail_type + ", ";
                    sQuery += "ENTITY_KIND = " + parameters.entity_kind + ", ";
                    sQuery += "BIRTH = CONVERT(DATETIME, '" + parameters.entity_birth + "'), ";
                    sQuery += "BIRTH_MONTH = DATEDIFF(MONTH, CONVERT(DATETIME, '" + parameters.entity_birth + "'), CONVERT(DATETIME, '" + sUtcTime + "')), ";
                    if (string.IsNullOrEmpty(parameters.image_name)) sQuery += "IMAGE_URL = NULL, ";
                    else sQuery += "IMAGE_URL = N'" + sImageURL + "', ";
                    //if (sTagID != parameters.tag_id)
                    //{
                    //    if (String.IsNullOrEmpty(parameters.tag_id)) sQuery += "TAG_ID = NULL, ";
                    //    else sQuery += "TAG_ID = '" + parameters.tag_id + "', ";
                    //}
                    if (string.IsNullOrEmpty(parameters.entity_pin)) sQuery += "ENTITY_PIN = NULL, ";
                    else sQuery += "ENTITY_PIN = N'" + parameters.entity_pin + "', ";
                    if (string.IsNullOrEmpty(parameters.shipment)) sQuery += "SHIPMENT = NULL, ";
                    else sQuery += "SHIPMENT = N'" + parameters.shipment + "', ";
                    if (string.IsNullOrEmpty(parameters.reason)) sQuery += "SHIPMENT_REASON = NULL, ";
                    else sQuery += "SHIPMENT_REASON = N'" + parameters.reason + "', ";
                    sQuery += "CALVE_COUNT = " + parameters.calve_count;
                    sQuery += string.Format(" WHERE SEQ = {0} AND FARM_SEQ = {1}", parameters.entity_seq, parameters.farm_seq);

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
                    }
                }

                // 혈통정보를 추출한다
                bool isExist = false;

                if (!isError)
                {
                    sQuery = string.Format("SELECT SEQ FROM ENTITY_LINEAGE WHERE ENTITY_SEQ = {0} ", parameters.entity_seq);

                    if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                    {
                        isExist = dataReader.HasRows;

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

                // 혈통정보를 수정한다
                // 등록되어 있지 않은 경우에는 등록한다
                if (!isError)
                {
                    if (!isExist)
                    {
                        if (!string.IsNullOrEmpty(parameters.sire_name) || !string.IsNullOrEmpty(parameters.sire_info) ||
                            !string.IsNullOrEmpty(parameters.dam_name) || !string.IsNullOrEmpty(parameters.dam_info) ||
                            !string.IsNullOrEmpty(parameters.gs_name) || !string.IsNullOrEmpty(parameters.gs_info) ||
                            !string.IsNullOrEmpty(parameters.gs_name) || !string.IsNullOrEmpty(parameters.ggs_info))
                        {
                            sQuery = "INSERT INTO ENTITY_LINEAGE (FARM_SEQ, ENTITY_SEQ, SIRE_NAME, SIRE_INFO, DAM_NAME, DAM_INFO, GS_NAME, GS_INFO, GGS_NAME, GGS_INFO) VALUES (";
                            sQuery += parameters.farm_seq + ", ";
                            sQuery += parameters.entity_seq + ", ";
                            if (string.IsNullOrEmpty(parameters.sire_name)) sQuery += "NULL , ";
                            else sQuery += "N'" + parameters.sire_name + "', ";
                            if (string.IsNullOrEmpty(parameters.sire_info)) sQuery += "NULL , ";
                            else sQuery += "N'" + parameters.sire_info + "', ";
                            if (string.IsNullOrEmpty(parameters.dam_name)) sQuery += "NULL , ";
                            else sQuery += "N'" + parameters.dam_name + "', ";
                            if (string.IsNullOrEmpty(parameters.dam_info)) sQuery += "NULL , ";
                            else sQuery += "N'" + parameters.dam_info + "', ";
                            if (string.IsNullOrEmpty(parameters.gs_name)) sQuery += "NULL , ";
                            else sQuery += "N'" + parameters.gs_name + "', ";
                            if (string.IsNullOrEmpty(parameters.gs_info)) sQuery += "NULL , ";
                            else sQuery += "N'" + parameters.gs_info + "', ";
                            if (string.IsNullOrEmpty(parameters.ggs_name)) sQuery += "NULL , ";
                            else sQuery += "N'" + parameters.ggs_name + "', ";
                            if (string.IsNullOrEmpty(parameters.ggs_info)) sQuery += "NULL";
                            else sQuery += "N'" + parameters.ggs_info + "'";
                            sQuery += ")";

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
                            }
                        }
                    }
                    else
                    {
                        sQuery = "UPDATE ENTITY_LINEAGE SET ";
                        if (string.IsNullOrEmpty(parameters.sire_name)) sQuery += "SIRE_NAME = NULL, ";
                        else sQuery += "SIRE_NAME = N'" + parameters.sire_name + "', ";
                        if (string.IsNullOrEmpty(parameters.sire_info)) sQuery += "SIRE_INFO = NULL, ";
                        else sQuery += "SIRE_INFO = N'" + parameters.sire_info + "', ";
                        if (string.IsNullOrEmpty(parameters.dam_name)) sQuery += "DAM_NAME = NULL, ";
                        else sQuery += "DAM_NAME = N'" + parameters.dam_name + "', ";
                        if (string.IsNullOrEmpty(parameters.dam_info)) sQuery += "DAM_INFO = NULL, ";
                        else sQuery += "DAM_INFO = N'" + parameters.dam_info + "', ";
                        if (string.IsNullOrEmpty(parameters.gs_name)) sQuery += "GS_NAME = NULL, ";
                        else sQuery += "GS_NAME = N'" + parameters.gs_name + "', ";
                        if (string.IsNullOrEmpty(parameters.gs_info)) sQuery += "GS_INFO = NULL, ";
                        else sQuery += "GS_INFO = N'" + parameters.gs_info + "', ";
                        if (string.IsNullOrEmpty(parameters.ggs_name)) sQuery += "GGS_NAME = NULL, ";
                        else sQuery += "GGS_NAME = N'" + parameters.ggs_name + "', ";
                        if (string.IsNullOrEmpty(parameters.ggs_info)) sQuery += "GGS_INFO = NULL ";
                        else sQuery += "GGS_INFO = N'" + parameters.ggs_info + "' ";
                        sQuery += string.Format(" WHERE ENTITY_SEQ = {0}", parameters.entity_seq);

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
                        }
                    }
                }

                // 기존에 등록된 태그를 미사용 상태로 변경한다
                //if (!isError)
                //{
                //    if (!String.IsNullOrEmpty(sTagID) && sTagID != parameters.tag_id)
                //    {
                //        sQuery = String.Format("UPDATE TAG_INFO SET ENTITY_SEQ = 0 WHERE TAG_ID = '{0}' ", sTagID);

                //        count = _mClassDatabase.QueryExecute(sQuery);

                //        if (count < 1)
                //        {
                //            isError = true;
                //            _mClassDatabase.TransRollback();

                //            response = new ClassResponse.RES_RESULT
                //            {
                //                result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
                //                message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
                //                data = String.Empty
                //            };
                //        }
                //    }
                //}

                // 신규 태그에 대해서 사용 상태로 변경한다
                //if (!isError)
                //{
                //    if (!String.IsNullOrEmpty(parameters.tag_id) && sTagID != parameters.tag_id)
                //    {
                //        sQuery = String.Format("UPDATE TAG_INFO SET ENTITY_SEQ = {0} WHERE TAG_ID = '{1}' ", parameters.entity_seq, parameters.tag_id);

                //        count = _mClassDatabase.QueryExecute(sQuery);

                //        if (count < 1)
                //        {
                //            isError = true;
                //            _mClassDatabase.TransRollback();

                //            response = new ClassResponse.RES_RESULT
                //            {
                //                result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
                //                message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
                //                data = String.Empty
                //            };
                //        }
                //    }
                //}

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

        public ClassResponse.RES_RESULT SetDeleteEntity(ClassRequest.REQ_FARMENTITY parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Entity", "delete");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START SetDeleteEntity  ==========", sModuleName);
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

                /// Demo농장의 개체의 경우 해당 삭제프로세스에서 제외 하도록 한다. 
                /// DEMO_TAG를 가지고 있으며 데이터수정작업이 진행되는 개체
                List<int> DemoEntity = new List<int>()
                {
                    25566
                    ,25575
                    ,25576
                    ,25577
                    ,25578
                    ,25579
                    ,25581
                    ,26115
                    ,41495
                    ,41496
                    ,41497
                    ,41519
                    ,41520
                    ,41521
                    ,41522
                    ,41523
                };

                if (parameters.entity_seq < 1 || DemoEntity.Contains(parameters.entity_seq))
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
                // 태그가 투여된 개체 : 개체정보만 비활성화 시킨다
                // 매핑 상태의 개체 : 개체를 비활성화 시키고 , 매핑된 태그정보를 투여예정 상태로 초기화 시킨다
                // 태그도 없고 매핑도 안된 개체 : 개체정보만 비활성화 시킨다

                // 태그정보를 추출한다
                int nSEQ = 0;
                string sTagID = string.Empty;

                //bool isExist = false;
                string sQuery = string.Format("SELECT A.SEQ, B.TAG_ID " +
                           "  FROM TAG_INFO A " +
                           "  LEFT OUTER JOIN ENTITY_NEW_INFO B " +
                           "    ON A.ENTITY_SEQ = B.SEQ " +
                           "   AND B.FLAG = 'Y' " +
                           "   AND B.ACTIVE_FLAG = 'Y' " +
                           " WHERE A.ENTITY_SEQ = {0} " +
                           "   AND A.FARM_SEQ = {1} " +
                           "   AND A.TAG_STATUS = 'I'" +
                           "   AND A.MATCH_DATE IS NOT NULL ", parameters.entity_seq, parameters.farm_seq);
                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        dataReader.Read();
                        nSEQ = dataReader.GetInt32(0);
                        sTagID = _mClassDatabase.GetSafeString(dataReader, 1);
                    }
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

                int count;
                if (!isError)
                {
                    if (string.IsNullOrEmpty(sTagID) && nSEQ > 0)
                    {
                        // 태그 정보를 초기화한다
                        sQuery = string.Format("UPDATE TAG_INFO " +
                                               "   SET FARM_SEQ = 0, " +
                                               "       ENTITY_SEQ = 0, " +
                                               "       MATCH_DATE = NULL " +
                                               " WHERE SEQ = {0} ", nSEQ);

                        count = _mClassDatabase.QueryExecute(sQuery);

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
                }

                if (!isError)
                {
                    // 개체 삭제를 비활성화로만 처리한다 (2019-07-31)
                    sQuery = string.Format("UPDATE ENTITY_NEW_INFO SET ACTIVE_FLAG = 'N' " +
                                           " WHERE SEQ = {0} " +
                                           "   AND FARM_SEQ = {1}", parameters.entity_seq, parameters.farm_seq);

                    count = _mClassDatabase.QueryExecute(sQuery);

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

                //// 태그정보를 추출한다
                //string sTagID = String.Empty;

                //sQuery = String.Format("SELECT TAG_ID " +
                //                       "  FROM ENTITY_NEW_INFO " +
                //                       " WHERE SEQ = {0} " +
                //                       "   AND FARM_SEQ = {1}", parameters.entity_seq, parameters.farm_seq);

                //if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                //{
                //    if (dataReader.HasRows)
                //    {
                //        dataReader.Read();
                //        sTagID = _mClassDatabase.GetSafeString(dataReader, 0);
                //    }
                //    else
                //    {
                //        isError = true;

                //        response = new ClassResponse.RES_RESULT
                //        {
                //            result = ClassError.RESULT_SEARCH_NOTEXIST_ENTITY,
                //            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SEARCH_NOTEXIST_ENTITY),
                //            data = String.Empty
                //        };
                //    }

                //    dataReader.Close();
                //    dataReader.Dispose();
                //}
                //else
                //{
                //    isError = true;

                //    response = new ClassResponse.RES_RESULT
                //    {
                //        result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
                //        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
                //        data = String.Empty
                //    };
                //}

                //if (!isError)
                //{
                //    _mClassDatabase.TransBegin();

                //    sQuery = String.Format("UPDATE ENTITY_NEW_INFO SET TAG_ID = NULL, FLAG = 'N', ACTIVE_FLAG = 'N' " +
                //                           " WHERE SEQ = {0} " +
                //                           "   AND FARM_SEQ = {1}", parameters.entity_seq, parameters.farm_seq);

                //    count = _mClassDatabase.QueryExecute(sQuery);

                //    if (count < 1)
                //    {
                //        isError = true;
                //        _mClassDatabase.TransRollback();

                //        response = new ClassResponse.RES_RESULT
                //        {
                //            result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
                //            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
                //            data = String.Empty
                //        };
                //    }
                //}

                //if (!isError && !String.IsNullOrEmpty(sTagID))
                //{
                //    sQuery = String.Format("UPDATE TAG_INFO SET ENTITY_SEQ = 0 " +
                //                           " WHERE FARM_SEQ = {0} " +
                //                           "   AND TAG_ID = '{1}' ", parameters.farm_seq, sTagID);

                //    count = _mClassDatabase.QueryExecute(sQuery);

                //    // 없는 태그를 삭제하는 경우가 있어서
                //    if (count < 0)
                //    {
                //        isError = true;
                //        _mClassDatabase.TransRollback();

                //        response = new ClassResponse.RES_RESULT
                //        {
                //            result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
                //            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
                //            data = String.Empty
                //        };
                //    }
                //}

                //// Henry (2017-01-17) : 복제로 사용하고 있는 상태에서는 삭제할 수 없기 때문에 제외한다
                //if (!isError)
                //{
                //    string sTableName = String.Format("TEMP_ENTITY_{0}", parameters.entity_seq);
                //    sQuery = String.Format("SELECT COUNT(SEQ) FROM {0} WHERE FLAG = 'Y' ", sTableName);

                //    if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                //    {
                //        if (dataReader.HasRows)
                //        {
                //            dataReader.Read();
                //            if (dataReader.GetInt32(0) > 0) isExist = true;
                //        }

                //        dataReader.Close();
                //        dataReader.Dispose();
                //    }
                //    else
                //    {
                //        isError = true;
                //        _mClassDatabase.TransRollback();

                //        response = new ClassResponse.RES_RESULT
                //        {
                //            result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
                //            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
                //            data = String.Empty
                //        };
                //    }
                //}

                //if (!isError && !isExist)
                //{
                //    string sTableName = String.Format("TEMP_ENTITY_{0}", parameters.entity_seq);
                //    sQuery = String.Format("DROP TABLE [dbo].[{0}];", sTableName);

                //    count = _mClassDatabase.QueryExecute(sQuery);
                //}

                //if (!isError)
                //{
                //    _mClassDatabase.TransCommit();

                //    response = new ClassResponse.RES_RESULT
                //    {
                //        result = ClassError.RESULT_SUCCESS,
                //        message = String.Empty,
                //        data = String.Empty
                //    };
                //}
            }
            catch
            {
                //_mClassDatabase.TransRollback();

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

        public ClassResponse.RES_RESULT GetPushAlarmList(ClassRequest.REQ_ALARM_LIST parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Entity", "alarm_list");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetPushAlarmList  ==========", sModuleName);
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

            List<ClassStruct.ST_PUSHINFO> pushList = new List<ClassStruct.ST_PUSHINFO>();

            try
            {
                // 페이지 인덱스
                int nFromIndex = (parameters.page_index - 1) * 20 + 1;
                int nToIndex = parameters.page_index * 20;

                // 알람시간을 CREATE_DATE가 아니라 농장시간이 SEND_DATE로 처리하도록 수정 (2021-05-28)
                string sQuery = string.Format("SELECT ENTITY_SEQ, PUSH_TYPE, MSG_TYPE, SEND_DATE, START_DATE, END_DATE, PEAK_DATE, ENTITY_NO " +
                                       "  FROM (SELECT ROW_NUMBER() OVER (ORDER BY A.SEND_DATE DESC) AS ROW_NUM, " +
                                       "               A.ENTITY_SEQ, A.PUSH_TYPE, A.MSG_TYPE, A.SEND_DATE, A.START_DATE, A.END_DATE, A.PEAK_DATE, B.ENTITY_NO " +
                                       "          FROM PUSH_HISTORY A " +
                                       "          LEFT OUTER JOIN ENTITY_NEW_INFO B " +
                                       "            ON A.ENTITY_SEQ = B.SEQ " +
                                       "         WHERE A.FARM_SEQ = {0} " +
                                       "           AND A.FLAG = 'Y' " +
                                       "           AND B.FLAG = 'Y' " +
                                       "           AND B.ACTIVE_FLAG = 'Y' ", parameters.farm_seq);
                //sQuery = String.Format("SELECT ENTITY_SEQ, PUSH_TYPE, MSG_TYPE, CREATE_DATE, START_DATE, END_DATE, PEAK_DATE, ENTITY_NO " +
                //                       "  FROM (SELECT ROW_NUMBER() OVER (ORDER BY A.CREATE_DATE DESC) AS ROW_NUM, " +
                //                       "               A.ENTITY_SEQ, A.PUSH_TYPE, A.MSG_TYPE, A.CREATE_DATE, A.START_DATE, A.END_DATE, A.PEAK_DATE, B.ENTITY_NO " +
                //                       "          FROM PUSH_HISTORY A " +
                //                       "          LEFT OUTER JOIN ENTITY_NEW_INFO B " +
                //                       "            ON A.ENTITY_SEQ = B.SEQ " +
                //                       "         WHERE A.FARM_SEQ = {0} " +
                //                       "           AND A.FLAG = 'Y' " +
                //                       "           AND B.FLAG = 'Y' " +
                //                       "           AND B.ACTIVE_FLAG = 'Y' ", parameters.farm_seq);
                if (!string.IsNullOrEmpty(parameters.entity_id)) sQuery += string.Format("           AND B.ENTITY_NO LIKE '%{0}%' ", parameters.entity_id);
                if (!string.IsNullOrEmpty(parameters.alarm_code))
                {
                    string[] arrayAlarmCode = parameters.alarm_code.Split('|');

                    if (arrayAlarmCode.Length == 1)
                    {
                        if (parameters.alarm_code == "DA")
                        {
                            sQuery += string.Format("           AND A.PUSH_TYPE = '{0}' ", parameters.alarm_code);
                        }
                        else
                        {
                            sQuery += string.Format("           AND A.MSG_TYPE = '{0}' ", parameters.alarm_code);
                        }
                    }
                    else if (arrayAlarmCode.Length > 1)
                    {
                        sQuery += "           AND ( ";
                        for (int i = 0; i < arrayAlarmCode.Length; i++)
                        {
                            // 예정일알람은 PUSH_TYPE과 비교해야 한다
                            if (arrayAlarmCode[i] == "DA")
                            {
                                if (i == 0)
                                    sQuery += string.Format("                 A.PUSH_TYPE = '{0}' ", arrayAlarmCode[i]);
                                else
                                    sQuery += string.Format("             OR  A.PUSH_TYPE = '{0}' ", arrayAlarmCode[i]);
                            }
                            else
                            {
                                if (i == 0)
                                    sQuery += string.Format("                 A.MSG_TYPE = '{0}' ", arrayAlarmCode[i]);
                                else
                                    sQuery += string.Format("             OR  A.MSG_TYPE = '{0}' ", arrayAlarmCode[i]);
                            }
                        }
                        sQuery += "               ) ";
                    }
                }
                sQuery += string.Format("       ) A " +
                                        " WHERE A.ROW_NUM BETWEEN {0} AND {1} ", nFromIndex, nToIndex);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            ClassStruct.ST_PUSHINFO pushInfo = new ClassStruct.ST_PUSHINFO
                            {
                                entity_seq = dataReader.GetInt32(0),
                                entity_id = _mClassDatabase.GetSafeString(dataReader, 7),
                                alarm_code = dataReader.GetString(2),
                                alarm_date = dataReader.GetDateTime(3).ToString("yyyy-MM-dd HH:mm:ss")
                            };

                            // 추가정보 처리
                            string sAlarmType = string.Empty;
                            string sAddTitle = string.Empty;
                            string sAddDate = string.Empty;

                            switch (parameters.lang_code)
                            {
                                case "KR":
                                    {
                                        switch (pushInfo.alarm_code)
                                        {
                                            case "EA": sAlarmType = "발정징후"; break;
                                            case "CA": sAlarmType = "분만징후"; break;
                                            case "HA": sAlarmType = "체온상승"; break;
                                            case "LA": sAlarmType = "체온하락"; break;
                                            case "AI": sAlarmType = "외부활동증가"; break;
                                            case "ON": sAlarmType = "외부활동감소"; break;
                                            case "AN": sAlarmType = "내부활동저하"; break;
                                            case "KN": sAlarmType = "음수없음"; break;
                                            case "ED": sAlarmType = "발정예정일"; break;
                                            case "ID": sAlarmType = "수정예정일"; break;
                                            case "AD": sAlarmType = "임신감정예정일"; break;
                                            case "DD": sAlarmType = "건유예정일"; break;
                                            case "CD": sAlarmType = "분만예정일"; break;
                                            case "UA": sAlarmType = "사용자알람"; break;
                                            case "SC": sAlarmType = "캡슐뱉음"; break;
                                        }
                                        break;
                                    }
                                case "JP":
                                    {
                                        switch (pushInfo.alarm_code)
                                        {
                                            case "EA": sAlarmType = "発情兆候"; break;
                                            case "CA": sAlarmType = "分娩兆候"; break;
                                            case "HA": sAlarmType = "体温上昇"; break;
                                            case "LA": sAlarmType = "体温下落"; break;
                                            case "AI": sAlarmType = "外部活動増加"; break;
                                            case "ON": sAlarmType = "外部活動減少"; break;
                                            case "AN": sAlarmType = "内部活動低下"; break;
                                            case "KN": sAlarmType = "飮水なし"; break;
                                            case "ED": sAlarmType = "発情予定日"; break;
                                            case "ID": sAlarmType = "授精予定日"; break;
                                            case "AD": sAlarmType = "妊娠鑑定予定日"; break;
                                            case "DD": sAlarmType = "乾乳予定日"; break;
                                            case "CD": sAlarmType = "分娩予定日"; break;
                                            case "UA": sAlarmType = "ユーザーアラーム"; break;
                                            case "SC": sAlarmType = "カプセル吐き出し"; break;
                                        }
                                        break;
                                    }
                                case "US":
                                    {
                                        switch (pushInfo.alarm_code)
                                        {
                                            case "EA": sAlarmType = "Estrus"; break;
                                            case "CA": sAlarmType = "Calving"; break;
                                            case "HA": sAlarmType = "High-Temp."; break;
                                            case "LA": sAlarmType = "Low-Temp."; break;
                                            case "AI": sAlarmType = "High-Act."; break;
                                            case "ON": sAlarmType = "Low-Act."; break;
                                            case "AN": sAlarmType = "Rumen Low-Act."; break;
                                            case "KN": sAlarmType = "Low drinking"; break;
                                            case "ED": sAlarmType = "Estimated estrus date"; break;
                                            case "ID": sAlarmType = "Scheduled insemination date"; break;
                                            case "AD": sAlarmType = "Scheduled diagnosis date"; break;
                                            case "DD": sAlarmType = "Scheduled dry off date"; break;
                                            case "CD": sAlarmType = "Estimated calving date"; break;
                                            case "UA": sAlarmType = "User Alarm"; break;
                                            case "SC": sAlarmType = "spitting out capsules"; break;
                                        }
                                        break;
                                    }
                                case "PT":
                                    {
                                        switch (pushInfo.alarm_code)
                                        {
                                            case "EA": sAlarmType = "Estro"; break;
                                            case "CA": sAlarmType = "Parto"; break;
                                            case "HA": sAlarmType = "Temp. alta"; break;
                                            case "LA": sAlarmType = "Temp. baixa"; break;
                                            case "AI": sAlarmType = "Aumento na ativiade"; break;
                                            case "ON": sAlarmType = "Redução na ativiade"; break;
                                            case "AN": sAlarmType = "Redução de atividade do rúmen"; break;
                                            case "KN": sAlarmType = "Bebendo baixo"; break;
                                            case "ED": sAlarmType = "D-prevista de Estro"; break;
                                            case "ID": sAlarmType = "D-agendada de Inseminação"; break;
                                            case "AD": sAlarmType = "D-agendada de Diagnóstico"; break;
                                            case "DD": sAlarmType = "D-agendada de Secagem"; break;
                                            case "CD": sAlarmType = "D-prevista de Parto"; break;
                                            case "UA": sAlarmType = "Alarme do usuário"; break;
                                            case "SC": sAlarmType = "cuspindo cápsulas"; break;
                                        }
                                        break;
                                    }
                                case "BR":
                                    {
                                        switch (pushInfo.alarm_code)
                                        {
                                            case "EA": sAlarmType = "Estro"; break;
                                            case "CA": sAlarmType = "Parto"; break;
                                            case "HA": sAlarmType = "Temp. alta"; break;
                                            case "LA": sAlarmType = "Temp. baixa"; break;
                                            case "AI": sAlarmType = "Aumento na ativiade"; break;
                                            case "ON": sAlarmType = "Redução na ativiade"; break;
                                            case "AN": sAlarmType = "Redução de atividade do rúmen"; break;
                                            case "KN": sAlarmType = "Bebendo baixo"; break;
                                            case "ED": sAlarmType = "D-prevista de Estro"; break;
                                            case "ID": sAlarmType = "D-agendada de Inseminação"; break;
                                            case "AD": sAlarmType = "D-agendada de Diagnóstico"; break;
                                            case "DD": sAlarmType = "D-agendada de Secagem"; break;
                                            case "CD": sAlarmType = "D-prevista de Parto"; break;
                                            case "UA": sAlarmType = "Alarme do usuário"; break;
                                            case "SC": sAlarmType = "cuspindo cápsulas"; break;
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        switch (pushInfo.alarm_code)
                                        {
                                            case "EA": sAlarmType = "Estrus"; break;
                                            case "CA": sAlarmType = "Calving"; break;
                                            case "HA": sAlarmType = "High-Temp."; break;
                                            case "LA": sAlarmType = "Low-Temp."; break;
                                            case "AI": sAlarmType = "High-Act."; break;
                                            case "ON": sAlarmType = "Low-Act."; break;
                                            case "AN": sAlarmType = "Rumen Low-Act."; break;
                                            case "KN": sAlarmType = "Low drinking"; break;
                                            case "ED": sAlarmType = "Estimated estrus date"; break;
                                            case "ID": sAlarmType = "Scheduled insemination date"; break;
                                            case "AD": sAlarmType = "Scheduled diagnosis date"; break;
                                            case "DD": sAlarmType = "Scheduled dry off date"; break;
                                            case "CD": sAlarmType = "Estimated calving date"; break;
                                            case "UA": sAlarmType = "User Alarm"; break;
                                        }
                                        break;
                                    }
                            }

                            if (pushInfo.alarm_code == "EA")
                            {
                                string sDay = string.Empty;
                                string sHour = string.Empty;

                                string sStartDate = _mClassDatabase.GetSafeDateTime(dataReader, 4);
                                string sEndDate = _mClassDatabase.GetSafeDateTime(dataReader, 5);
                                string sPeakDate = _mClassDatabase.GetSafeDateTime(dataReader, 6);

                                switch (parameters.lang_code)
                                {
                                    case "KR": sAddTitle = "권장수정시기"; sDay = "일"; sHour = "시"; break;
                                    case "JP": sAddTitle = "推奨授精時期"; sDay = "日"; sHour = "時"; break;
                                    case "US": sAddTitle = "Suggested time AI"; sDay = "Day"; sHour = "hr"; break;
                                    case "CN": sAddTitle = "Suggested time AI"; sDay = "Day"; sHour = "hr"; break;
                                    case "PT": sAddTitle = "Tempo sugerido de IA"; sDay = "Dia"; sHour = "hr"; break;
                                    case "BR": sAddTitle = "Tempo sugerido de IA"; sDay = "Dia"; sHour = "hr"; break;
                                    default: sAddTitle = "Suggested time AI"; sDay = "Day"; sHour = "hr"; break;
                                }

                                if (!string.IsNullOrEmpty(sEndDate) && !string.IsNullOrEmpty(sPeakDate))
                                {
                                    int nEndDay = Convert.ToDateTime(sEndDate).Day;
                                    int nEndHour = Convert.ToDateTime(sEndDate).Hour;
                                    int nPeakHour = Convert.ToDateTime(sPeakDate).Hour;

                                    switch (parameters.lang_code)
                                    {
                                        case "KR": sAddDate = string.Format("{0:D2}{3} {1:D2}~{2:D2}{4}", nEndDay, nEndHour, nPeakHour, sDay, sHour); break;
                                        case "JP": sAddDate = string.Format("{0:D2}{3} {1:D2}~{2:D2}{4}", nEndDay, nEndHour, nPeakHour, sDay, sHour); break;
                                        case "US": sAddDate = string.Format("{3}{0:D2} {1:D2}~{2:D2}{4}", nEndDay, nEndHour, nPeakHour, sDay, sHour); break;
                                        case "CN": sAddDate = string.Format("{3}{0:D2} {1:D2}~{2:D2}{4}", nEndDay, nEndHour, nPeakHour, sDay, sHour); break;
                                        case "PT": sAddDate = string.Format("{3}{0:D2} {1:D2}~{2:D2}{4}", nEndDay, nEndHour, nPeakHour, sDay, sHour); break;
                                        case "BR": sAddDate = string.Format("{3}{0:D2} {1:D2}~{2:D2}{4}", nEndDay, nEndHour, nPeakHour, sDay, sHour); break;
                                        default: sAddDate = string.Format("{3}{0:D2} {1:D2}~{2:D2}{4}", nEndDay, nEndHour, nPeakHour, sDay, sHour); break;
                                    }
                                }
                            }
                            else if (pushInfo.alarm_code == "CA")
                            {
                                string sDay = string.Empty;

                                string sStartDate = _mClassDatabase.GetSafeDateTime(dataReader, 4);
                                string sEndDate = _mClassDatabase.GetSafeDateTime(dataReader, 5);
                                string sPeakDate = _mClassDatabase.GetSafeDateTime(dataReader, 6);

                                switch (parameters.lang_code)
                                {
                                    case "KR": sAddTitle = "분만예정시기"; sDay = "일"; break;
                                    case "JP": sAddTitle = "分娩予定時期"; sDay = "日"; break;
                                    case "US": sAddTitle = "Estimated calving time"; sDay = "Day"; break;
                                    case "CN": sAddTitle = "Estimated calving time"; sDay = "Day"; break;
                                    case "PT": sAddTitle = "Tempo estimado de Parto"; sDay = "Dia"; break;
                                    case "BR": sAddTitle = "Tempo estimado de Parto"; sDay = "Dia"; break;
                                    default: sAddTitle = "Estimated calving time"; sDay = "Day"; break;
                                }

                                if (!string.IsNullOrEmpty(sPeakDate))
                                {
                                    string sPeak = string.Empty;
                                    int nHour = Convert.ToDateTime(sPeakDate).Hour;

                                    if (nHour >= 0 && nHour <= 5)
                                    {
                                        switch (parameters.lang_code)
                                        {
                                            case "KR": sPeak = "새벽"; break;
                                            case "JP": sPeak = "未明・明け方 0～6時"; break;
                                            case "US": sPeak = "Dawn"; break;
                                            case "CN": sPeak = "EDawn"; break;
                                            case "PT": sPeak = "Madrugada"; break;
                                            case "BR": sPeak = "Madrugada"; break;
                                            default: sPeak = "Dawn"; break;
                                        }
                                    }
                                    else if (nHour >= 6 && nHour <= 11)
                                    {
                                        switch (parameters.lang_code)
                                        {
                                            case "KR": sPeak = "오전"; break;
                                            case "JP": sPeak = "午前 6～12時"; break;
                                            case "US": sPeak = "AM"; break;
                                            case "CN": sPeak = "AM"; break;
                                            case "PT": sPeak = "AM"; break;
                                            case "BR": sPeak = "AM"; break;
                                            default: sPeak = "AM"; break;
                                        }
                                    }
                                    else if (nHour >= 12 && nHour <= 17)
                                    {
                                        switch (parameters.lang_code)
                                        {
                                            case "KR": sPeak = "오후"; break;
                                            case "JP": sPeak = "午後 12～18時"; break;
                                            case "US": sPeak = "PM"; break;
                                            case "CN": sPeak = "PM"; break;
                                            case "PT": sPeak = "PM"; break;
                                            case "BR": sPeak = "PM"; break;
                                            default: sPeak = "PM"; break;
                                        }
                                    }
                                    else if (nHour >= 18 && nHour <= 23)
                                    {
                                        switch (parameters.lang_code)
                                        {
                                            case "KR": sPeak = "저녁"; break;
                                            case "JP": sPeak = "夕方・夜 18～0時"; break;
                                            case "US": sPeak = "Evening"; break;
                                            case "CN": sPeak = "Evening"; break;
                                            case "PT": sPeak = "Noite"; break;
                                            case "BR": sPeak = "Noite"; break;
                                            default: sPeak = "Evening"; break;
                                        }
                                    }

                                    switch (parameters.lang_code)
                                    {
                                        case "KR": sAddDate = string.Format("{0:D2}{1} {2}", Convert.ToDateTime(sPeakDate).Day, sDay, sPeak); break;
                                        case "JP": sAddDate = string.Format("{0:D2}{1} {2}", Convert.ToDateTime(sPeakDate).Day, sDay, sPeak); break;
                                        case "US": sAddDate = string.Format("{1}{0:D2} {2}", Convert.ToDateTime(sPeakDate).Day, sDay, sPeak); break;
                                        case "CN": sAddDate = string.Format("{1}{0:D2} {2}", Convert.ToDateTime(sPeakDate).Day, sDay, sPeak); break;
                                        case "PT": sAddDate = string.Format("{1}{0:D2} {2}", Convert.ToDateTime(sPeakDate).Day, sDay, sPeak); break;
                                        case "BR": sAddDate = string.Format("{1}{0:D2} {2}", Convert.ToDateTime(sPeakDate).Day, sDay, sPeak); break;
                                        default: sAddDate = string.Format("{1}{0:D2} {2}", Convert.ToDateTime(sPeakDate).Day, sDay, sPeak); break;
                                    }
                                }
                            }
                            else if (pushInfo.alarm_code == "HA")
                            {
                                switch (parameters.lang_code)
                                {
                                    case "KR":
                                        {
                                            sAddTitle = "백신 또는 스트레스로 인한 체온상승이 의심됩니다";
                                            sAddDate = string.Empty;
                                            break;
                                        }
                                    case "JP":
                                        {
                                            sAddTitle = "ワクチンまたはストレスによる体温上昇が疑われます";
                                            sAddDate = string.Empty;
                                            break;
                                        }
                                    case "US":
                                        {
                                            sAddTitle = "Possibly due to vaccines or stress";
                                            sAddDate = string.Empty;
                                            break;
                                        }
                                    case "CN":
                                        {
                                            sAddTitle = "Possibly due to vaccines or stress";
                                            sAddDate = string.Empty;
                                            break;
                                        }
                                    case "PT":
                                        {
                                            sAddTitle = "Por acaso de vacinações ou estresse";
                                            sAddDate = string.Empty;
                                            break;
                                        }
                                    case "BR":
                                        {
                                            sAddTitle = "Por acaso de vacinações ou estresse";
                                            sAddDate = string.Empty;
                                            break;
                                        }
                                    default:
                                        {
                                            sAddTitle = "Possibly due to vaccines or stress";
                                            sAddDate = string.Empty;
                                            break;
                                        }
                                }
                            }
                            else if (pushInfo.alarm_code == "LA")
                            {
                                switch (parameters.lang_code)
                                {
                                    case "KR":
                                        {
                                            sAddTitle = "잦은 음수 또는 소화기 장애로 인한 체온하락이 의심됩니다";
                                            sAddDate = string.Empty;
                                            break;
                                        }
                                    case "JP":
                                        {
                                            sAddTitle = "頻繁な飮水または消化器障害による体温下落が疑われます";
                                            sAddDate = string.Empty;
                                            break;
                                        }
                                    case "US":
                                        {
                                            sAddTitle = "Possibly due to excessive drinking or digestive disorder";
                                            sAddDate = string.Empty;
                                            break;
                                        }
                                    case "CN":
                                        {
                                            sAddTitle = "Possibly due to excessive drinking or digestive disorder";
                                            sAddDate = string.Empty;
                                            break;
                                        }
                                    case "PT":
                                        {
                                            sAddTitle = "Por acaso de bebido demais ou distúrbio digestivo";
                                            sAddDate = string.Empty;
                                            break;
                                        }
                                    case "BR":
                                        {
                                            sAddTitle = "Por acaso de bebido demais ou distúrbio digestivo";
                                            sAddDate = string.Empty;
                                            break;
                                        }
                                    default:
                                        {
                                            sAddTitle = "Possibly due to excessive drinking or digestive disorder";
                                            sAddDate = string.Empty;
                                            break;
                                        }
                                }
                            }
                            else if (pushInfo.alarm_code == "SC")
                            {
                                switch (parameters.lang_code)
                                {
                                    case "KR": sAddTitle = "캡슐 뱉음 현상이 의심됩니다. 개체의 상태를 확인하세요"; break;
                                    case "JP": sAddTitle = "カプセルの吐出現象が疑われます。オブジェクトの状態を確認してください"; break;
                                    case "US": sAddTitle = "Capsule spitting is suspected. Check the state of a cow"; break;
                                    case "PT": sAddTitle = "Suspeita-se de cuspir cápsula. Verifique o estado de uma vaca"; break;
                                }
                                sAddDate = string.Empty;
                            }

                            pushInfo.alarm_type = sAlarmType;
                            pushInfo.add_title = sAddTitle;
                            pushInfo.add_date = sAddDate;

                            pushList.Add(pushInfo);
                        }

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SUCCESS,
                            message = string.Empty,
                            data = JsonConvert.SerializeObject(pushList)
                        };
                    }
                    else
                    {
                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SEARCH_NOTEXIST_ALARM,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SEARCH_NOTEXIST_ALARM),
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

        public ClassResponse.RES_RESULT GetEntityPregnancy(ClassRequest.REQ_FARMENTITY parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Entity", "pregnancy");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetEntityPregnancy  ==========", sModuleName);
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

            ClassStruct.ST_PREGNANCY pregnancyInfo = new ClassStruct.ST_PREGNANCY();

            try
            {
                string sQuery = string.Format("SELECT PREGNANCY_CODE " +
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
                        pregnancyInfo.pregnancy_flag = dataReader.GetInt32(0) == 1 ? "Y" : "N";

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SUCCESS,
                            message = string.Empty,
                            data = JsonConvert.SerializeObject(pregnancyInfo)
                        };
                    }
                    else
                    {
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

        public ClassResponse.RES_RESULT GetEntityHistoryList(ClassRequest.REQ_ENTITYHISTORY parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Entity", "history_list");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetEntityHistoryList  ==========", sModuleName);
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

            List<ClassStruct.ST_ENTITYHISTORY_INFO> historyList = new List<ClassStruct.ST_ENTITYHISTORY_INFO>();

            try
            {
                string sQuery;

                switch (parameters.lang_code)
                {
                    case "KR":
                        sQuery = "SELECT A.SEQ, A.BREED_DATE, A.BREED_TYPE, A.BREED_METHOD, A.BREED_INT_VALUE2, A.BREED_TEXT_VALUE, B.SEMEN_NO, C.CODE_NAME ";
                        break;
                    case "JP":
                        sQuery = "SELECT A.SEQ, A.BREED_DATE, A.BREED_TYPE, A.BREED_METHOD, A.BREED_INT_VALUE2, A.BREED_TEXT_VALUE, B.SEMEN_NO, C.JP_VALUE ";
                        break;
                    case "US":
                        sQuery = "SELECT A.SEQ, A.BREED_DATE, A.BREED_TYPE, A.BREED_METHOD, A.BREED_INT_VALUE2, A.BREED_TEXT_VALUE, B.SEMEN_NO, C.EN_VALUE ";
                        break;
                    case "CN":
                        sQuery = "SELECT A.SEQ, A.BREED_DATE, A.BREED_TYPE, A.BREED_METHOD, A.BREED_INT_VALUE2, A.BREED_TEXT_VALUE, B.SEMEN_NO, C.ZH_VALUE ";
                        break;
                    case "PT":
                        sQuery = "SELECT A.SEQ, A.BREED_DATE, A.BREED_TYPE, A.BREED_METHOD, A.BREED_INT_VALUE2, A.BREED_TEXT_VALUE, B.SEMEN_NO, C.PT_VALUE ";
                        break;
                    case "BR":
                        sQuery = "SELECT A.SEQ, A.BREED_DATE, A.BREED_TYPE, A.BREED_METHOD, A.BREED_INT_VALUE2, A.BREED_TEXT_VALUE, B.SEMEN_NO, C.PT_VALUE ";
                        break;
                    default:
                        sQuery = "SELECT A.SEQ, A.BREED_DATE, A.BREED_TYPE, A.BREED_METHOD, A.BREED_INT_VALUE2, A.BREED_TEXT_VALUE, B.SEMEN_NO, C.EN_VALUE ";
                        break;
                }

                sQuery += string.Format("  FROM BREED_HISTORY A " +
                                        "  LEFT OUTER JOIN FARM_SEMEN B " +
                                        "    ON A.BREED_TYPE = 'I' " +
                                        "   AND A.BREED_INT_VALUE2 = B.SEQ " +
                                        "   AND B.FLAG = 'Y' " +
                                        "  LEFT OUTER JOIN CODE_MST C " +
                                        "    ON A.BREED_TYPE = 'C' " +
                                        "   AND A.BREED_METHOD = C.CODE_NO " +
                                        "   AND C.CODE_DIV = '160' " +
                                        "   AND C.FLAG = 'Y' " +
                                        " WHERE A.FARM_SEQ = {0} " +
                                        "   AND A.ENTITY_SEQ = {1} " +
                                        "   AND A.FLAG = 'Y' ", parameters.farm_seq, parameters.entity_seq);
                if (parameters.search_year > 0) sQuery += string.Format("   AND DATEPART(YEAR, A.BREED_DATE) = {0}", parameters.search_year);
                if (!string.IsNullOrEmpty(parameters.search_flag))
                {
                    string[] arrayFlag = parameters.search_flag.Split('|');

                    string sSearch = string.Empty;
                    foreach (string flag in arrayFlag)
                    {
                        if (string.IsNullOrEmpty(sSearch))
                            sSearch = "'" + flag + "'";
                        else
                            sSearch += ", '" + flag + "'";
                    }

                    sQuery += string.Format("   AND BREED_TYPE IN ({0}) ", sSearch);
                }
                sQuery += " ORDER BY A.BREED_DATE DESC, A.SEQ DESC ";

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            string sBreedCode = dataReader.GetString(2);
                            ClassStruct.ST_ENTITYHISTORY_INFO breedInfo = new ClassStruct.ST_ENTITYHISTORY_INFO();

                            if (sBreedCode == "E")
                            {
                                breedInfo.breed_seq = dataReader.GetInt32(0);
                                breedInfo.breed_date = dataReader.GetDateTime(1).ToString("yyyy-MM-dd");
                                breedInfo.breed_type = dataReader.GetString(2);
                                breedInfo.pregnancy_flag = string.Empty;
                                breedInfo.calve_terms = 0;
                                breedInfo.calve_flag = string.Empty;
                                breedInfo.calve_code = 0;
                                breedInfo.calve_code_disp = string.Empty;
                                breedInfo.semen_no = string.Empty;

                                historyList.Add(breedInfo);
                            }
                            else if (sBreedCode == "I")
                            {
                                breedInfo.breed_seq = dataReader.GetInt32(0);
                                breedInfo.breed_date = dataReader.GetDateTime(1).ToString("yyyy-MM-dd");
                                breedInfo.breed_type = dataReader.GetString(2);
                                breedInfo.pregnancy_flag = string.Empty;
                                breedInfo.calve_terms = 0;
                                breedInfo.calve_flag = string.Empty;
                                breedInfo.calve_code = 0;
                                breedInfo.calve_code_disp = string.Empty;
                                breedInfo.semen_no = _mClassDatabase.GetSafeString(dataReader, 6);

                                historyList.Add(breedInfo);
                            }
                            else if (sBreedCode == "A")
                            {
                                breedInfo.breed_seq = dataReader.GetInt32(0);
                                breedInfo.breed_date = dataReader.GetDateTime(1).ToString("yyyy-MM-dd");
                                breedInfo.breed_type = dataReader.GetString(2);
                                breedInfo.pregnancy_flag = dataReader.GetString(5);
                                breedInfo.calve_terms = 0;
                                breedInfo.calve_flag = string.Empty;
                                breedInfo.calve_code = 0;
                                breedInfo.calve_code_disp = string.Empty;
                                breedInfo.semen_no = string.Empty;

                                historyList.Add(breedInfo);
                            }
                            else if (sBreedCode == "D")
                            {
                                breedInfo.breed_seq = dataReader.GetInt32(0);
                                breedInfo.breed_date = dataReader.GetDateTime(1).ToString("yyyy-MM-dd");
                                breedInfo.breed_type = dataReader.GetString(2);
                                breedInfo.pregnancy_flag = string.Empty;
                                breedInfo.calve_terms = 0;
                                breedInfo.calve_flag = string.Empty;
                                breedInfo.calve_code = 0;
                                breedInfo.calve_code_disp = string.Empty;
                                breedInfo.semen_no = string.Empty;

                                historyList.Add(breedInfo);
                            }
                            else if (sBreedCode == "C")
                            {
                                breedInfo.breed_seq = dataReader.GetInt32(0);
                                breedInfo.breed_date = dataReader.GetDateTime(1).ToString("yyyy-MM-dd");
                                breedInfo.breed_type = dataReader.GetString(2);
                                breedInfo.pregnancy_flag = string.Empty;
                                breedInfo.calve_terms = dataReader.GetInt32(4);
                                breedInfo.calve_flag = _mClassDatabase.GetSafeString(dataReader, 5);
                                breedInfo.calve_code = dataReader.GetInt32(3);
                                breedInfo.calve_code_disp = _mClassDatabase.GetSafeString(dataReader, 7);
                                breedInfo.semen_no = string.Empty;

                                historyList.Add(breedInfo);
                            }
                        }

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
            ClassLog._mLogger.InfoFormat("{0}  RESPONSE DATA  [데이타 전송완료]" + Environment.NewLine, sModuleName);
            return response;
            #endregion
        }

        public ClassResponse.RES_RESULT GetEntityInfo(ClassRequest.REQ_ENTITYINFO parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Entity", "entity_info");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetEntityInfo  ==========", sModuleName);
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

                if (string.IsNullOrEmpty(parameters.entity_no))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_ENTITY_NO,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_ENTITY_NO),
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
                string sQuery = "SELECT A.SEQ, A.ENTITY_NO, A.ENTITY_SEX, A.ENTITY_TYPE, A.DETAIL_TYPE, A.ENTITY_KIND, A.BIRTH, A.BIRTH_MONTH, " +
                         "       A.PREGNANCY_CODE, A.TEMP_MIN, A.TEMP_MAX, A.IMAGE_URL, A.TAG_ID, A.ENTITY_PIN, A.TAG_COUNT, A.INJECT_COUNT, " +
                         "       B.ADDED_DUE_DATE1 AS CALVE_DUE_DATE, ISNULL(DATEDIFF(DAY, GETDATE(), B.ADDED_DUE_DATE1), 0) AS CALVE_DUE_REMAIN, ";

                switch (parameters.lang_code)
                {
                    case "KR":
                        sQuery += "       C.CODE_NAME AS ENTITY_SEX_DISP, D.CODE_NAME AS ENTITY_TYPE_DISP, E.CODE_NAME AS DETAIL_TYPE_DISP, F.CODE_NAME AS ENTITY_KIND_DISP ";
                        break;
                    case "JP":
                        sQuery += "       C.JP_VALUE AS ENTITY_SEX_DISP, D.JP_VALUE AS ENTITY_TYPE_DISP, E.JP_VALUE AS DETAIL_TYPE_DISP, F.JP_VALUE AS ENTITY_KIND_DISP ";
                        break;
                    case "US":
                        sQuery += "       C.EN_VALUE AS ENTITY_SEX_DISP, D.EN_VALUE AS ENTITY_TYPE_DISP, E.EN_VALUE AS DETAIL_TYPE_DISP, F.EN_VALUE AS ENTITY_KIND_DISP ";
                        break;
                    case "CN":
                        sQuery += "       C.ZH_VALUE AS ENTITY_SEX_DISP, D.ZH_VALUE AS ENTITY_TYPE_DISP, E.ZH_VALUE AS DETAIL_TYPE_DISP, F.ZH_VALUE AS ENTITY_KIND_DISP ";
                        break;
                    case "PT":
                        sQuery += "       C.PT_VALUE AS ENTITY_SEX_DISP, D.PT_VALUE AS ENTITY_TYPE_DISP, E.PT_VALUE AS DETAIL_TYPE_DISP, F.PT_VALUE AS ENTITY_KIND_DISP ";
                        break;
                    case "BR":
                        sQuery += "       C.PT_VALUE AS ENTITY_SEX_DISP, D.PT_VALUE AS ENTITY_TYPE_DISP, E.PT_VALUE AS DETAIL_TYPE_DISP, F.PT_VALUE AS ENTITY_KIND_DISP ";
                        break;
                    default:
                        sQuery += "       C.EN_VALUE AS ENTITY_SEX_DISP, D.EN_VALUE AS ENTITY_TYPE_DISP, E.EN_VALUE AS DETAIL_TYPE_DISP, F.EN_VALUE AS ENTITY_KIND_DISP ";
                        break;
                }

                sQuery += string.Format("  FROM ENTITY_NEW_INFO A " +
                                        "  LEFT OUTER JOIN UDF_HISTORY_LASTDATA(2) B " +
                                        "    ON A.SEQ = B.ENTITY_SEQ " +
                                        "   AND (B.BREED_TYPE = 'A' OR B.BREED_TYPE = 'D') " +
                                        "  LEFT OUTER JOIN CODE_MST C " +
                                        "    ON A.ENTITY_SEX = C.CODE_NO " +
                                        "   AND C.CODE_DIV = '100' " +
                                        "   AND C.FLAG = 'Y' " +
                                        "  LEFT OUTER JOIN CODE_MST D " +
                                        "    ON A.ENTITY_TYPE = D.CODE_NO " +
                                        "   AND D.CODE_DIV = '110' " +
                                        "   AND D.FLAG = 'Y' " +
                                        "  LEFT OUTER JOIN CODE_MST E " +
                                        "    ON A.DETAIL_TYPE = E.CODE_NO " +
                                        "   AND E.CODE_DIV = '120' " +
                                        "   AND E.FLAG = 'Y' " +
                                        "  LEFT OUTER JOIN CODE_MST F " +
                                        "    ON A.ENTITY_KIND = F.CODE_NO " +
                                        "   AND F.CODE_DIV = '130' " +
                                        "   AND F.FLAG = 'Y' " +
                                        " WHERE A.FARM_SEQ = {0} " +
                                        "   AND A.ENTITY_NO = '{1}' " +
                                        "   AND A.ACTIVE_FLAG = 'Y' " +
                                        "   AND A.FLAG = 'Y' ", parameters.farm_seq, parameters.entity_no);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        dataReader.Read();

                        ClassStruct.ST_LORA_ENTITY entityInfo = new ClassStruct.ST_LORA_ENTITY
                        {
                            entity_seq = dataReader.GetInt32(0),
                            entity_id = dataReader.GetString(1),
                            entity_sex = dataReader.GetInt32(2),
                            entity_sex_disp = _mClassDatabase.GetSafeString(dataReader, 18),
                            entity_type = dataReader.GetInt32(3),
                            entity_type_disp = _mClassDatabase.GetSafeString(dataReader, 19),
                            detail_type = dataReader.GetInt32(4),
                            detail_type_disp = _mClassDatabase.GetSafeString(dataReader, 20),
                            entity_kind = _mClassDatabase.GetSafeInteger(dataReader, 5),
                            entity_kind_disp = _mClassDatabase.GetSafeString(dataReader, 21),
                            birth_date = dataReader.IsDBNull(6) ? string.Empty : dataReader.GetDateTime(6).ToString("yyyy-MM-dd"),
                            birth_month = dataReader.GetInt32(7),
                            pregnancy_flag = dataReader.GetInt32(8) == 1 ? "Y" : "N",
                            temp_min = dataReader.GetDouble(9).ToString("F2"),
                            temp_max = dataReader.GetDouble(10).ToString("F2"),
                            image_url = _mClassDatabase.GetSafeString(dataReader, 11),
                            tag_id = _mClassDatabase.GetSafeString(dataReader, 12),
                            tag_count = dataReader.GetInt32(14),
                            inject_count = dataReader.GetInt32(15),
                            entity_pin = _mClassDatabase.GetSafeString(dataReader, 13)
                        };
                        if (dataReader.IsDBNull(16))
                        {
                            entityInfo.calve_due_date = string.Empty;
                            entityInfo.calve_due_remain = 0;
                        }
                        else
                        {
                            entityInfo.calve_due_date = dataReader.GetDateTime(16).ToString("yyyy-MM-dd");
                            entityInfo.calve_due_remain = dataReader.GetInt32(17);
                        }

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SUCCESS,
                            message = string.Empty,
                            data = JsonConvert.SerializeObject(entityInfo)
                        };
                    }
                    else
                    {
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

        public ClassResponse.RES_RESULT SetFavoriteInsert(ClassRequest.REQ_FAVORITE parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Entity", "favorite_insert");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START SetFavoriteInsert  ==========", sModuleName);
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
            bool isExist = false;
            OleDbDataReader dataReader = null;

            try
            {
                // 이미 즐겨찾기에 등록된 경우에는 그냥 성공을 리턴한다
                string sQuery = string.Format("SELECT SEQ FROM ENTITY_FAVORITE WHERE ENTITY_SEQ = {0} AND USER_ID = '{1}' AND FLAG = 'Y' ", parameters.entity_seq, parameters.uid);
                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    isExist = dataReader.HasRows;

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
                    if (isExist)
                    {
                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SUCCESS,
                            message = string.Empty,
                            data = string.Empty
                        };
                    }
                    else
                    {
                        sQuery = string.Format("INSERT INTO ENTITY_FAVORITE (USER_ID, FARM_SEQ, ENTITY_SEQ) VALUES ('{0}', {1}, {2}) ", parameters.uid, parameters.farm_seq, parameters.entity_seq);
                        int count = _mClassDatabase.QueryExecute(sQuery);

                        if (count > 0)
                        {
                            response = new ClassResponse.RES_RESULT
                            {
                                result = ClassError.RESULT_SUCCESS,
                                message = string.Empty,
                                data = string.Empty
                            };
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

        public ClassResponse.RES_RESULT SetFavoriteDelete(ClassRequest.REQ_FAVORITE parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Entity", "favorite_delete");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START SetFavoriteDelete  ==========", sModuleName);
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
            try
            {
                string sQuery = string.Format("UPDATE ENTITY_FAVORITE SET FLAG = 'N' " +
                                       " WHERE USER_ID = '{0}' " +
                                       "   AND FARM_SEQ = {1} " +
                                       "   AND ENTITY_SEQ = {2} " +
                                       "   AND FLAG = 'Y' ", parameters.uid, parameters.farm_seq, parameters.entity_seq);
                int count = _mClassDatabase.QueryExecute(sQuery);

                if (count > 0)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SUCCESS,
                        message = string.Empty,
                        data = string.Empty
                    };
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

        public ClassResponse.RES_RESULT GetDashboardList(ClassRequest.REQ_UID parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Entity", "dashboard");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetDashboardList  ==========", sModuleName);
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

            List<ClassStruct.ST_DASHBOARD> dashboardList = new List<ClassStruct.ST_DASHBOARD>();

            try
            {
                string sQuery = string.Format("WITH ENTITY_LIST AS ( " +
                                       "SELECT A.SEQ, A.FARM_SEQ, A.ENTITY_NO, A.ENTITY_SEX, A.ENTITY_TYPE, A.DETAIL_TYPE, A.ENTITY_KIND,  " +
                                       "       A.BIRTH, A.BIRTH_MONTH, A.BIRTH_CODE, A.WEIGHT, A.SHIPMENT, A.SHIPMENT_REASON, A.ENTITY_PIN, " +
                                       "       A.TAG_ID, A.IMAGE_URL, A.PREGNANCY_CODE, A.CALVE_FLAG, ISNULL(A.CALVE_COUNT, -1) AS CALVE_COUNT, " +
                                       "       A.ENTITY_STATUS, A.TEMP_MIN, A.TEMP_MAX, " +
                                       "	   CASE WHEN A.ENTITY_STATUS = 'NM' THEN 1 " +
                                       "            WHEN A.ENTITY_STATUS = 'EA' THEN 2 " +
                                       "            WHEN A.ENTITY_STATUS = 'CA' THEN 3 " +
                                       "            WHEN A.ENTITY_STATUS = 'HA' THEN 4 " +
                                       "            WHEN A.ENTITY_STATUS = 'LA' THEN 5 " +
                                       "        END AS ENTITY_STATUS_ORDER, " +
                                       "       B.NAME AS FARM_NAME, ISNULL(C.DRINK_COUNT, 0) AS DRINK_COUNT, ISNULL(D.FAVORITE_FLAG, 'N') AS FAVORITE_FLAG " +
                                       "  FROM ENTITY_NEW_INFO A " +
                                       "  LEFT OUTER JOIN FARM_INFO B " +
                                       "    ON A.FARM_SEQ = B.SEQ " +
                                       "  LEFT OUTER JOIN  " +
                                       "       (SELECT A.FARM_SEQ, A.ENTITY_SEQ, COUNT(A.SEQ) AS DRINK_COUNT " +
                                       "          FROM ENTITY_DISTINCTION A WITH (INDEX = IDX_DATE_FARM_ENTITY) " +
                                       "          LEFT OUTER JOIN UDF_FARM_UTCDATE(-23) B " +
                                       "            ON A.FARM_SEQ = B.SEQ " +
                                       "         WHERE A.CHECK_DATE >= B.FROM_DATE " +
                                       "           AND A.CHECK_DATE <= B.TO_DATE " +
                                       "           AND A.CHECK_TYPE = 'D' " +
                                       "           AND A.FLAG = 'Y' " +
                                       "         GROUP BY A.FARM_SEQ, A.ENTITY_SEQ) C " +
                                       "    ON A.FARM_SEQ = C.FARM_SEQ " +
                                       "   AND A.SEQ = C.ENTITY_SEQ " +
                                       "  LEFT OUTER JOIN " +
                                       "       (SELECT FARM_SEQ, ENTITY_SEQ, 'Y' AS FAVORITE_FLAG " +
                                       "          FROM ENTITY_FAVORITE " +
                                       "         WHERE USER_ID = '{0}' " +
                                       "           AND FLAG = 'Y') D " +
                                       "    ON A.FARM_SEQ = D.FARM_SEQ " +
                                       "   AND A.SEQ = D.ENTITY_SEQ " +
                                       " WHERE A.FARM_SEQ IN (SELECT SEQ FROM UDF_USER_FARMLIST('{0}')) " +
                                       "   AND A.FLAG = 'Y' " +
                                       "   AND A.ACTIVE_FLAG = 'Y' " +
                                       "   AND (A.ENTITY_STATUS = 'EA' OR A.ENTITY_STATUS = 'CA' OR A.ENTITY_STATUS = 'HA' OR A.ENTITY_STATUS = 'LA') " +
                                       ") " +
                                       "SELECT A.SEQ, A.FARM_SEQ, A.ENTITY_NO, A.ENTITY_SEX, A.ENTITY_TYPE, A.DETAIL_TYPE, A.ENTITY_KIND,  " +
                                       "       A.BIRTH, A.BIRTH_MONTH, A.BIRTH_CODE, A.WEIGHT, A.SHIPMENT, A.SHIPMENT_REASON, A.ENTITY_PIN, " +
                                       "       A.TAG_ID, A.IMAGE_URL, A.PREGNANCY_CODE, A.CALVE_FLAG, A.CALVE_COUNT, A.ENTITY_STATUS, " +
                                       "       A.TEMP_MIN, A.TEMP_MAX, A.ENTITY_STATUS_ORDER, A.FARM_NAME, A.DRINK_COUNT, A.FAVORITE_FLAG, ", parameters.uid);

                switch (parameters.lang_code)
                {
                    case "KR":
                        sQuery += "       B.CODE_NAME AS ENTITY_SEX_DISP, C.CODE_NAME AS ENTITY_TYPE_DISP, D.CODE_NAME AS DETAIL_TYPE_DISP, E.CODE_NAME AS ENTITY_KIND_DISP ";
                        break;
                    case "JP":
                        sQuery += "       B.JP_VALUE AS ENTITY_SEX_DISP, C.JP_VALUE AS ENTITY_TYPE_DISP, D.JP_VALUE AS DETAIL_TYPE_DISP, E.JP_VALUE AS ENTITY_KIND_DISP ";
                        break;
                    case "US":
                        sQuery += "       B.EN_VALUE AS ENTITY_SEX_DISP, C.EN_VALUE AS ENTITY_TYPE_DISP, D.EN_VALUE AS DETAIL_TYPE_DISP, E.EN_VALUE AS ENTITY_KIND_DISP ";
                        break;
                    case "CN":
                        sQuery += "       B.ZH_VALUE AS ENTITY_SEX_DISP, C.ZH_VALUE AS ENTITY_TYPE_DISP, D.ZH_VALUE AS DETAIL_TYPE_DISP, E.ZH_VALUE AS ENTITY_KIND_DISP ";
                        break;
                    case "PT":
                        sQuery += "       B.PT_VALUE AS ENTITY_SEX_DISP, C.PT_VALUE AS ENTITY_TYPE_DISP, D.PT_VALUE AS DETAIL_TYPE_DISP, E.PT_VALUE AS ENTITY_KIND_DISP ";
                        break;
                    case "BR":
                        sQuery += "       B.PT_VALUE AS ENTITY_SEX_DISP, C.PT_VALUE AS ENTITY_TYPE_DISP, D.PT_VALUE AS DETAIL_TYPE_DISP, E.PT_VALUE AS ENTITY_KIND_DISP ";
                        break;
                    default:
                        sQuery += "       B.EN_VALUE AS ENTITY_SEX_DISP, C.EN_VALUE AS ENTITY_TYPE_DISP, D.EN_VALUE AS DETAIL_TYPE_DISP, E.EN_VALUE AS ENTITY_KIND_DISP ";
                        break;
                }

                sQuery += "  FROM ENTITY_LIST A " +
                          "  LEFT OUTER JOIN CODE_MST B " +
                          "    ON A.ENTITY_SEX = B.CODE_NO " +
                          "   AND B.CODE_DIV = '100' " +
                          "   AND B.FLAG = 'Y' " +
                          "  LEFT OUTER JOIN CODE_MST C " +
                          "    ON A.ENTITY_TYPE = C.CODE_NO " +
                          "   AND C.CODE_DIV = '110' " +
                          "   AND C.FLAG = 'Y' " +
                          "  LEFT OUTER JOIN CODE_MST D " +
                          "    ON A.DETAIL_TYPE = D.CODE_NO " +
                          "   AND D.CODE_DIV = '120' " +
                          "   AND D.FLAG = 'Y' " +
                          "  LEFT OUTER JOIN CODE_MST E " +
                          "    ON A.ENTITY_KIND = E.CODE_NO " +
                          "   AND E.CODE_DIV = '130' " +
                          "   AND E.FLAG = 'Y' " +
                          " ORDER BY A.ENTITY_STATUS_ORDER DESC ";

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            ClassStruct.ST_DASHBOARD dashboardInfo = new ClassStruct.ST_DASHBOARD
                            {
                                farm_seq = dataReader.GetInt32(1),
                                farm_name = _mClassDatabase.GetSafeString(dataReader, 23),
                                entity_seq = dataReader.GetInt32(0),
                                entity_id = dataReader.GetString(2),
                                entity_sex = dataReader.GetInt32(3),
                                entity_sex_disp = _mClassDatabase.GetSafeString(dataReader, 26),
                                entity_type = dataReader.GetInt32(4),
                                entity_type_disp = _mClassDatabase.GetSafeString(dataReader, 27),
                                detail_type = dataReader.GetInt32(5),
                                detail_type_disp = _mClassDatabase.GetSafeString(dataReader, 28),
                                entity_kind = _mClassDatabase.GetSafeInteger(dataReader, 6),
                                entity_kind_disp = _mClassDatabase.GetSafeString(dataReader, 29),
                                birth_date = dataReader.IsDBNull(7) ? string.Empty : dataReader.GetDateTime(7).ToString("yyyy-MM-dd"),
                                birth_month = dataReader.GetInt32(8),
                                temp_min = dataReader.GetDouble(20).ToString("F2"),
                                temp_max = dataReader.GetDouble(21).ToString("F2"),
                                image_url = _mClassDatabase.GetSafeString(dataReader, 15),
                                warning_level = dataReader.GetString(19),
                                drink_count = dataReader.GetInt32(24),
                                tag_id = _mClassDatabase.GetSafeString(dataReader, 14),
                                pregnancy_flag = dataReader.GetInt32(16) == 1 ? "Y" : "N",
                                entity_pin = _mClassDatabase.GetSafeString(dataReader, 13),
                                favorite_flag = dataReader.GetString(25)
                            };
                            switch (parameters.lang_code)
                            {
                                case "KR":
                                    {
                                        if (dashboardInfo.warning_level == "EA") dashboardInfo.warning_level_disp = "발정";
                                        else if (dashboardInfo.warning_level == "CA") dashboardInfo.warning_level_disp = "분만";
                                        else if (dashboardInfo.warning_level == "HA") dashboardInfo.warning_level_disp = "체온상승";
                                        else if (dashboardInfo.warning_level == "LA") dashboardInfo.warning_level_disp = "체온하락";
                                        break;
                                    }
                                case "JP":
                                    {
                                        if (dashboardInfo.warning_level == "EA") dashboardInfo.warning_level_disp = "発情";
                                        else if (dashboardInfo.warning_level == "CA") dashboardInfo.warning_level_disp = "分娩";
                                        else if (dashboardInfo.warning_level == "HA") dashboardInfo.warning_level_disp = "体温上昇";
                                        else if (dashboardInfo.warning_level == "LA") dashboardInfo.warning_level_disp = "体温下落";
                                        break;
                                    }
                                case "US":
                                    {
                                        if (dashboardInfo.warning_level == "EA") dashboardInfo.warning_level_disp = "Estrus";
                                        else if (dashboardInfo.warning_level == "CA") dashboardInfo.warning_level_disp = "Calving";
                                        else if (dashboardInfo.warning_level == "HA") dashboardInfo.warning_level_disp = "High-Temp.";
                                        else if (dashboardInfo.warning_level == "LA") dashboardInfo.warning_level_disp = "Low-Temp.";
                                        break;
                                    }
                                case "CN":
                                    {
                                        if (dashboardInfo.warning_level == "EA") dashboardInfo.warning_level_disp = "発情";
                                        else if (dashboardInfo.warning_level == "CA") dashboardInfo.warning_level_disp = "分娩";
                                        else if (dashboardInfo.warning_level == "HA") dashboardInfo.warning_level_disp = "体温上昇";
                                        else if (dashboardInfo.warning_level == "LA") dashboardInfo.warning_level_disp = "体温下落";
                                        break;
                                    }
                                case "PT":
                                    {
                                        if (dashboardInfo.warning_level == "EA") dashboardInfo.warning_level_disp = "Estro";
                                        else if (dashboardInfo.warning_level == "CA") dashboardInfo.warning_level_disp = "Parto";
                                        else if (dashboardInfo.warning_level == "HA") dashboardInfo.warning_level_disp = "Temp. alta";
                                        else if (dashboardInfo.warning_level == "LA") dashboardInfo.warning_level_disp = "Temp. baixa";
                                        break;
                                    }
                                case "BR":
                                    {
                                        if (dashboardInfo.warning_level == "EA") dashboardInfo.warning_level_disp = "Estro";
                                        else if (dashboardInfo.warning_level == "CA") dashboardInfo.warning_level_disp = "Parto";
                                        else if (dashboardInfo.warning_level == "HA") dashboardInfo.warning_level_disp = "Temp. alta";
                                        else if (dashboardInfo.warning_level == "LA") dashboardInfo.warning_level_disp = "Temp. baixa";
                                        break;
                                    }
                                default:
                                    {
                                        if (dashboardInfo.warning_level == "EA") dashboardInfo.warning_level_disp = "Estrus";
                                        else if (dashboardInfo.warning_level == "CA") dashboardInfo.warning_level_disp = "Calving";
                                        else if (dashboardInfo.warning_level == "HA") dashboardInfo.warning_level_disp = "High-Temp.";
                                        else if (dashboardInfo.warning_level == "LA") dashboardInfo.warning_level_disp = "Low-Temp.";
                                        break;
                                    }
                            }

                            dashboardList.Add(dashboardInfo);
                        }

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SUCCESS,
                            message = string.Empty,
                            data = JsonConvert.SerializeObject(dashboardList)
                        };
                    }
                    else
                    {
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
            ClassLog._mLogger.InfoFormat("{0}  RESPONSE DATA  [데이타 전송완료]" + Environment.NewLine, sModuleName);
            return response;
            #endregion
        }

        public ClassResponse.RES_RESULT GetHistoryDetailInfo(ClassRequest.REQ_FARMENTITY parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Entity", "history_info");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetHistoryDetailInfo  ==========", sModuleName);
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
                // 농장의 UTC Time을 추출한다
                DateTime utcTime = DateTime.UtcNow;
                string sUtcTime = string.Empty;

                _mClassFunction.GetFarmTimeDifference(parameters.farm_seq, out int nDiffHour, out int nDiffMinute);

                utcTime = utcTime.AddHours(nDiffHour).AddMinutes(nDiffMinute);
                sUtcTime = utcTime.ToString("yyyy-MM-dd HH:mm:ss");

                ClassStruct.ST_HISTORY_DETAIL historyDetail = new ClassStruct.ST_HISTORY_DETAIL();

                string sQuery;

                // 개체정보의 체크 (임신상태 / 산차, 개체분류를 추출한다)
                switch (parameters.lang_code)
                {
                    case "KR": sQuery = "SELECT A.SEQ, A.ENTITY_KIND, A.PREGNANCY_CODE, ISNULL(A.CALVE_COUNT, -1), B.CODE_NAME AS ENTITY_KIND_DISP, C.CODE_NAME AS PREGNANCY_CODE_DISP "; break;
                    case "JP": sQuery = "SELECT A.SEQ, A.ENTITY_KIND, A.PREGNANCY_CODE, ISNULL(A.CALVE_COUNT, -1), B.JP_VALUE AS ENTITY_KIND_DISP, C.JP_VALUE AS PREGNANCY_CODE_DISP "; break;
                    case "US": sQuery = "SELECT A.SEQ, A.ENTITY_KIND, A.PREGNANCY_CODE, ISNULL(A.CALVE_COUNT, -1), B.EN_VALUE AS ENTITY_KIND_DISP, C.EN_VALUE AS PREGNANCY_CODE_DISP "; break;
                    case "CN": sQuery = "SELECT A.SEQ, A.ENTITY_KIND, A.PREGNANCY_CODE, ISNULL(A.CALVE_COUNT, -1), B.ZH_VALUE AS ENTITY_KIND_DISP, C.ZH_VALUE AS PREGNANCY_CODE_DISP "; break;
                    case "PT": sQuery = "SELECT A.SEQ, A.ENTITY_KIND, A.PREGNANCY_CODE, ISNULL(A.CALVE_COUNT, -1), B.PT_VALUE AS ENTITY_KIND_DISP, C.PT_VALUE AS PREGNANCY_CODE_DISP "; break;
                    case "BR": sQuery = "SELECT A.SEQ, A.ENTITY_KIND, A.PREGNANCY_CODE, ISNULL(A.CALVE_COUNT, -1), B.PT_VALUE AS ENTITY_KIND_DISP, C.PT_VALUE AS PREGNANCY_CODE_DISP "; break;
                    default: sQuery = "SELECT A.SEQ, A.ENTITY_KIND, A.PREGNANCY_CODE, ISNULL(A.CALVE_COUNT, -1), B.EN_VALUE AS ENTITY_KIND_DISP, C.EN_VALUE AS PREGNANCY_CODE_DISP "; break;
                }

                sQuery += string.Format("  FROM ENTITY_NEW_INFO A" +
                                        "  LEFT OUTER JOIN CODE_MST B " +
                                        "    ON B.CODE_DIV = '130' " +
                                        "   AND A.ENTITY_KIND = B.CODE_NO " +
                                        "   AND B.FLAG = 'Y' " +
                                        "  LEFT OUTER JOIN CODE_MST C " +
                                        "    ON C.CODE_DIV = '170' " +
                                        "   AND A.PREGNANCY_CODE = C.CODE_NO " +
                                        "   AND C.FLAG = 'Y' " +
                                        " WHERE A.SEQ = {0} " +
                                        "   AND A.FARM_SEQ = {1} " +
                                        "   AND A.FLAG = 'Y'" +
                                       "   AND A.ACTIVE_FLAG = 'Y' ", parameters.entity_seq, parameters.farm_seq);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        dataReader.Read();

                        historyDetail.entity_kind = dataReader.GetInt32(1);
                        historyDetail.entity_kind_disp = _mClassDatabase.GetSafeString(dataReader, 4);
                        historyDetail.pregnancy_code = dataReader.GetInt32(2);
                        historyDetail.pregnancy_code_disp = _mClassDatabase.GetSafeString(dataReader, 5);
                        historyDetail.calve_count = _mClassDatabase.GetSafeInteger(dataReader, 3);
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

                // 마지막 번식일정에 따라서 처리한다
                // 발정 : 마지막 분만일이 있으면 -> 분만 후
                //        마지막 분만일이 없으면 -> 빈 값
                // 수정 : 수정 후
                // 감정 : 분만예정일이 있으면 -> 분만까지
                //        분만예정일이 없고 마지막 분만일이 있으면 -> 분만 후
                //        분만예정일이 없고 마지막 분만일이 없으면 -> 빈 값
                // 건유 : 분만예정일이 있으면 -> 분만까지
                // 분만 : 분만 후
                if (!isError)
                {
                    sQuery = string.Format("SELECT RESULT_CODE, RESULT_DATE, RESULT_DAY, LAST_CALVE_DATE " +
                                           "  FROM (SELECT BREED_TYPE, BREED_DATE, ADDED_DUE_DATE1, LAST_CALVE_DATE, TO_DATE, " +
                                           "               CASE BREED_TYPE " +
                                           "                    WHEN 'E' THEN " +
                                           "                         CASE WHEN LAST_CALVE_DATE IS NOT NULL THEN 'AC' " +
                                           "                              ELSE NULL " +
                                           "                          END " +
                                           "                    WHEN 'I' THEN 'AI' " +
                                           "                    WHEN 'A' THEN " +
                                           "                         CASE WHEN ADDED_DUE_DATE1 IS NOT NULL THEN 'BC' " +
                                           "                              WHEN ADDED_DUE_DATE1 IS NULL AND BREED_TEXT_VALUE = 'N' AND LAST_CALVE_DATE IS NOT NULL THEN 'AC' " +
                                           "                              ELSE NULL " +
                                           "                          END " +
                                           "                    WHEN 'D' THEN " +
                                           "                         CASE WHEN ADDED_DUE_DATE1 IS NOT NULL THEN 'BC' " +
                                           "                              ELSE NULL " +
                                           "                          END " +
                                           "                    WHEN 'C' THEN 'AC' " +
                                           "                END RESULT_CODE, " +
                                           "               CASE BREED_TYPE " +
                                           "                    WHEN 'E' THEN " +
                                           "                         CASE WHEN LAST_CALVE_DATE IS NOT NULL THEN CONVERT(CHAR(10), LAST_CALVE_DATE, 23) " +
                                           "                              ELSE NULL " +
                                           "                          END " +
                                           "                    WHEN 'I' THEN CONVERT(CHAR(10), BREED_DATE, 23) " +
                                           "                    WHEN 'A' THEN " +
                                           "                         CASE WHEN ADDED_DUE_DATE1 IS NOT NULL THEN CONVERT(CHAR(10), ADDED_DUE_DATE1, 23) " +
                                           "                              WHEN ADDED_DUE_DATE1 IS NULL AND BREED_TEXT_VALUE = 'N' AND LAST_CALVE_DATE IS NOT NULL " +
                                           "                              THEN CONVERT(CHAR(10), LAST_CALVE_DATE, 23) " +
                                           "                              ELSE NULL " +
                                           "                          END " +
                                           "                    WHEN 'D' THEN " +
                                           "                         CASE WHEN ADDED_DUE_DATE1 IS NOT NULL THEN CONVERT(CHAR(10), ADDED_DUE_DATE1, 23) " +
                                           "                              ELSE NULL " +
                                           "                          END " +
                                           "                    WHEN 'C' THEN CONVERT(CHAR(10), BREED_DATE, 23) " +
                                           "                END RESULT_DATE, " +
                                           "               CASE BREED_TYPE " +
                                           "                    WHEN 'E' THEN " +
                                           "                         CASE WHEN LAST_CALVE_DATE IS NOT NULL THEN DATEDIFF(DAY, LAST_CALVE_DATE, TO_DATE) " +
                                           "                              ELSE 999999 " +
                                           "                          END " +
                                           "                    WHEN 'I' THEN DATEDIFF(DAY, BREED_DATE, TO_DATE) " +
                                           "                    WHEN 'A' THEN " +
                                           "                         CASE WHEN ADDED_DUE_DATE1 IS NOT NULL THEN DATEDIFF(DAY, TO_DATE, ADDED_DUE_DATE1) " +
                                           "                              WHEN ADDED_DUE_DATE1 IS NULL AND BREED_TEXT_VALUE = 'N' AND LAST_CALVE_DATE IS NOT NULL " +
                                           "                              THEN DATEDIFF(DAY, LAST_CALVE_DATE, TO_DATE) " +
                                           "                              ELSE 99999 " +
                                           "                          END " +
                                           "                    WHEN 'D' THEN " +
                                           "                         CASE WHEN ADDED_DUE_DATE1 IS NOT NULL THEN DATEDIFF(DAY, TO_DATE, ADDED_DUE_DATE1) " +
                                           "                              ELSE 99999 " +
                                           "                          END " +
                                           "                    WHEN 'C' THEN DATEDIFF(DAY, BREED_DATE, TO_DATE) " +
                                           "                    ELSE 999999 " +
                                           "                END RESULT_DAY " +
                                           "          FROM (SELECT A.BREED_TYPE, A.BREED_DATE, A.ADDED_DUE_DATE1, A.BREED_TEXT_VALUE, B.LAST_CALVE_DATE, C.TO_DATE " +
                                           "                  FROM UDF_HISTORY_LASTDATA({0}) A " +
                                           "                  LEFT OUTER JOIN UDF_HISTORY_LASTINFO({0}) B " +
                                           "                    ON A.ENTITY_SEQ = B.ENTITY_SEQ " +
                                           "                  LEFT OUTER JOIN UDF_FARM_UTCDATE(0) C " +
                                           "                    ON C.SEQ = {0} " +
                                           "                 WHERE A.FARM_SEQ = {0} " +
                                           "                   AND A.ENTITY_SEQ = {1} " +
                                           "               ) BREED_LIST " +
                                           "       ) RESULT_LIST ", parameters.farm_seq, parameters.entity_seq);

                    if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                    {
                        if (dataReader.HasRows)
                        {
                            dataReader.Read();

                            if (dataReader.IsDBNull(0))
                            {
                                historyDetail.breed_code = string.Empty;
                                historyDetail.breed_title = string.Empty;
                                historyDetail.breed_day = 0;
                                if (dataReader.IsDBNull(3)) historyDetail.last_calve_date = string.Empty;
                                else historyDetail.last_calve_date = dataReader.GetDateTime(3).ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                historyDetail.breed_code = dataReader.GetString(0);
                                historyDetail.breed_title = string.Empty;
                                historyDetail.breed_day = dataReader.GetInt32(2);

                                if (dataReader.IsDBNull(3)) historyDetail.last_calve_date = string.Empty;
                                else historyDetail.last_calve_date = dataReader.GetDateTime(3).ToString("yyyy-MM-dd");
                            }

                            dataReader.Close();
                            dataReader.Dispose();
                        }
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
                    string sBreedTitle = string.Empty;

                    switch (parameters.lang_code)
                    {
                        case "KR":
                            {
                                switch (historyDetail.breed_code)
                                {
                                    case "AI": sBreedTitle = "수정 후"; break;
                                    case "BC": sBreedTitle = "분만예정일"; break;
                                    case "AC": sBreedTitle = "분만 후"; break;
                                }

                                break;
                            }
                        case "JP":
                            {
                                switch (historyDetail.breed_code)
                                {
                                    case "AI": sBreedTitle = "授精後"; break;
                                    case "BC": sBreedTitle = "分娩予定日"; break;
                                    case "AC": sBreedTitle = "分娩後"; break;
                                }

                                break;
                            }
                        case "US":
                            {
                                switch (historyDetail.breed_code)
                                {
                                    case "AI": sBreedTitle = "Insemination"; break;
                                    case "BC": sBreedTitle = "Estimated calving date"; break;
                                    case "AC": sBreedTitle = "Calving"; break;
                                }

                                break;
                            }
                        case "CN":
                            {
                                switch (historyDetail.breed_code)
                                {
                                    case "AI": sBreedTitle = "Insemination"; break;
                                    case "BC": sBreedTitle = "Estimated calving date"; break;
                                    case "AC": sBreedTitle = "Calving"; break;
                                }

                                break;
                            }
                        case "PT":
                            {
                                switch (historyDetail.breed_code)
                                {
                                    case "AI": sBreedTitle = "Inseminação"; break;
                                    case "BC": sBreedTitle = "D-prevista de Parto"; break;
                                    case "AC": sBreedTitle = "Parto"; break;
                                }

                                break;
                            }
                        case "BR":
                            {
                                switch (historyDetail.breed_code)
                                {
                                    case "AI": sBreedTitle = "Inseminação"; break;
                                    case "BC": sBreedTitle = "D-prevista de Parto"; break;
                                    case "AC": sBreedTitle = "Parto"; break;
                                }

                                break;
                            }
                        default:
                            {
                                switch (historyDetail.breed_code)
                                {
                                    case "AI": sBreedTitle = "Insemination"; break;
                                    case "BC": sBreedTitle = "Estimated calving date"; break;
                                    case "AC": sBreedTitle = "Calving"; break;
                                }

                                break;
                            }
                    }

                    historyDetail.breed_title = sBreedTitle;
                }

                if (!isError)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SUCCESS,
                        message = string.Empty,
                        data = JsonConvert.SerializeObject(historyDetail)
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

            ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
            return response;
            #endregion
        }

        public ClassResponse.RES_RESULT GetEntityStatistics(ClassRequest.REQ_FARMSEQ parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Entity", "statistics");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetEntityStatistics  ==========", sModuleName);
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

            try
            {
                ClassStruct.ST_STATISTICS_INFO stStatistics = new ClassStruct.ST_STATISTICS_INFO();
                ClassStruct.ST_STATISTICS_DISPLAY stDisplay = new ClassStruct.ST_STATISTICS_DISPLAY();
                ClassStruct.ST_STATISTICS_PREGNANCY stPregnancy = new ClassStruct.ST_STATISTICS_PREGNANCY();

                // 임신구분 통계 및 개체분류 통계
                string sQuery = string.Format("SELECT A.CODE_NO, A.CODE_NAME, A.JP_VALUE, A.EN_VALUE, A.ZH_VALUE, A.PT_VALUE, " +
                                       "       ISNULL(B.PREGNANCY_COUNTER, 0) AS PREGNANCY_COUNTER, " +
                                       "       ISNULL(C.NOPREGNANCY_COUNTER, 0) AS NOPREGNANCY_COUNTER " +
                                       "  FROM CODE_MST A " +
                                       "  LEFT OUTER JOIN " +
                                       "       (SELECT ENTITY_KIND, PREGNANCY_CODE, COUNT(SEQ) AS PREGNANCY_COUNTER " +
                                       "          FROM ENTITY_NEW_INFO " +
                                       "         WHERE FARM_SEQ = {0} " +
                                       "           AND FLAG = 'Y' " +
                                       "           AND ACTIVE_FLAG = 'Y' " +
                                       "           AND PREGNANCY_CODE = 1 " +
                                       "         GROUP BY ENTITY_KIND, PREGNANCY_CODE) B " +
                                       "    ON A.CODE_NO = B.ENTITY_KIND " +
                                       "  LEFT OUTER JOIN " +
                                       "       (SELECT ENTITY_KIND, PREGNANCY_CODE, COUNT(SEQ) AS NOPREGNANCY_COUNTER " +
                                       "          FROM ENTITY_NEW_INFO " +
                                       "         WHERE FARM_SEQ = {0} " +
                                       "           AND FLAG = 'Y' " +
                                       "           AND ACTIVE_FLAG = 'Y' " +
                                       "           AND PREGNANCY_CODE = 2 " +
                                       "         GROUP BY ENTITY_KIND, PREGNANCY_CODE) C " +
                                       "    ON A.CODE_NO = C.ENTITY_KIND " +
                                       " WHERE A.CODE_DIV = '130' ", parameters.farm_seq);
                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        List<string> displayName = new List<string>();
                        List<int> kindCouter = new List<int>();
                        List<int> pregnancyCounter = new List<int>();
                        List<int> nopregnancyCounter = new List<int>();

                        while (dataReader.Read())
                        {
                            switch (parameters.lang_code)
                            {
                                case "KR": displayName.Add(dataReader.GetString(1)); break;
                                case "JP": displayName.Add(dataReader.GetString(2)); break;
                                case "US": displayName.Add(dataReader.GetString(3)); break;
                                case "CN": displayName.Add(dataReader.GetString(4)); break;
                                case "PT": displayName.Add(dataReader.GetString(5)); break;
                                case "BR": displayName.Add(dataReader.GetString(5)); break;
                                default: displayName.Add(dataReader.GetString(3)); break;
                            }

                            pregnancyCounter.Add(dataReader.GetInt32(6));
                            nopregnancyCounter.Add(dataReader.GetInt32(7));
                            kindCouter.Add(dataReader.GetInt32(6) + dataReader.GetInt32(7));
                        }

                        stPregnancy.display_name = JsonConvert.SerializeObject(displayName, Newtonsoft.Json.Formatting.None);
                        stPregnancy.pregnancy_count = JsonConvert.SerializeObject(pregnancyCounter, Newtonsoft.Json.Formatting.None);
                        stPregnancy.nopregnancy_count = JsonConvert.SerializeObject(nopregnancyCounter, Newtonsoft.Json.Formatting.None);

                        stDisplay.display_name = JsonConvert.SerializeObject(displayName, Newtonsoft.Json.Formatting.None);
                        stDisplay.display_count = JsonConvert.SerializeObject(kindCouter, Newtonsoft.Json.Formatting.None);

                        stStatistics.pregnancy_info = stPregnancy;
                        stStatistics.kind_info = stDisplay;
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

                // 경산우구분 통계
                if (!isError)
                {
                    sQuery = string.Format("SELECT CALVE_FLAG, COUNT(SEQ) AS CALVE_COUNT " +
                                           "  FROM (SELECT SEQ, ENTITY_KIND, PREGNANCY_CODE, CALVE_FLAG " +
                                           "          FROM ENTITY_NEW_INFO " +
                                           "         WHERE FARM_SEQ = {0} " +
                                           "           AND FLAG = 'Y' " +
                                           "           AND ACTIVE_FLAG = 'Y') A " +
                                           "         GROUP BY CALVE_FLAG ", parameters.farm_seq);

                    if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                    {
                        if (dataReader.HasRows)
                        {
                            // 경산우 구분은 코드가 없기 때문에 따로 처리한다
                            Dictionary<string, int> calveList = new Dictionary<string, int>
                            {
                                { "Y", 0 },
                                { "N", 0 }
                            };

                            while (dataReader.Read())
                            {
                                calveList[dataReader.GetString(0)] = dataReader.GetInt32(1);
                            }

                            List<string> displayName = new List<string>();
                            List<int> displayCount = new List<int>();

                            string sKey = string.Empty;
                            string sName = string.Empty;

                            sKey = "Y";
                            switch (parameters.lang_code)
                            {
                                case "KR": sName = "경산우"; break;
                                case "JP": sName = "経産牛"; break;
                                case "US": sName = "Cow"; break;
                                case "CN": sName = "経産牛"; break;
                                case "PT": sName = "Vaca"; break;
                                case "BR": sName = "Vaca"; break;
                                default: sName = "Cow"; break;
                            }

                            displayName.Add(sName);
                            displayCount.Add(calveList[sKey]);

                            sKey = "N";
                            switch (parameters.lang_code)
                            {
                                case "KR": sName = "미경산우"; break;
                                case "JP": sName = "未経産牛"; break;
                                case "US": sName = "Heifer"; break;
                                case "CN": sName = "未経産牛"; break;
                                case "PT": sName = "Novilha"; break;
                                case "BR": sName = "Novilha"; break;
                                default: sName = "Heifer"; break;
                            }

                            displayName.Add(sName);
                            displayCount.Add(calveList[sKey]);

                            stDisplay.display_name = JsonConvert.SerializeObject(displayName, Newtonsoft.Json.Formatting.None);
                            stDisplay.display_count = JsonConvert.SerializeObject(displayCount, Newtonsoft.Json.Formatting.None);

                            stStatistics.calve_info = stDisplay;
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

                // 이번 달 통계
                string sFromDate = DateTime.Now.ToString("yyyy-MM-01");
                string sToDate = string.Format("{0}-{1:D2}", DateTime.Now.ToString("yyyy-MM"), DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

                if (!isError)
                {
                    sQuery = string.Format("SELECT A.ESTRUS_PRE_COUNTER, B.INSEMINATE_PRE_COUNTER, C.APPRAISAL_PRE_COUNTER, D.CALVE_PRE_COUNTER, E.DRYUP_PRE_COUNTER " +
                                           "  FROM (SELECT COUNT(A.SEQ) AS ESTRUS_PRE_COUNTER " +
                                           "          FROM UDF_HISTORY_LASTDATA({0}) A " +
                                           "          LEFT OUTER JOIN ENTITY_NEW_INFO B " +
                                           "		    ON A.ENTITY_SEQ = B.SEQ " +
                                           "		   AND B.FLAG = 'Y' " +
                                           "		   AND B.ACTIVE_FLAG = 'Y' " +
                                           "         WHERE (A.BREED_TYPE = 'E' OR A.BREED_TYPE = 'C') " +
                                           "		   AND A.FARM_SEQ = {0} " +
                                           "           AND A.FLAG = 'Y' " +
                                           "           AND CONVERT(CHAR(10), A.BREED_DUE_DATE, 23) >= '{1}' " +
                                           "           AND CONVERT(CHAR(10), A.BREED_DUE_DATE, 23) <= '{2}' " +
                                           "           AND B.ENTITY_NO IS NOT NULL) A, " +
                                           "       (SELECT COUNT(A.SEQ) AS INSEMINATE_PRE_COUNTER " +
                                           "          FROM UDF_HISTORY_LASTDATA({0}) A " +
                                           "          LEFT OUTER JOIN ENTITY_NEW_INFO B " +
                                           "		    ON A.ENTITY_SEQ = B.SEQ " +
                                           "		   AND B.FLAG = 'Y' " +
                                           "		   AND B.ACTIVE_FLAG = 'Y' " +
                                           "         WHERE A.BREED_TYPE = 'E' " +
                                           "		   AND A.FARM_SEQ = {0} " +
                                           "           AND A.FLAG = 'Y' " +
                                           "           AND CONVERT(CHAR(10), A.ADDED_DUE_DATE1, 23) >= '{1}' " +
                                           "           AND CONVERT(CHAR(10), A.ADDED_DUE_DATE1, 23) <= '{2}' " +
                                           "           AND B.ENTITY_NO IS NOT NULL) B, " +
                                           "       (SELECT COUNT(A.SEQ) AS APPRAISAL_PRE_COUNTER " +
                                           "          FROM UDF_HISTORY_LASTDATA({0}) A " +
                                           "          LEFT OUTER JOIN ENTITY_NEW_INFO B " +
                                           "		    ON A.ENTITY_SEQ = B.SEQ " +
                                           "		   AND B.FLAG = 'Y' " +
                                           "		   AND B.ACTIVE_FLAG = 'Y' " +
                                           "         WHERE A.BREED_TYPE = 'I' " +
                                           "		   AND A.FARM_SEQ = {0} " +
                                           "           AND A.FLAG = 'Y' " +
                                           "           AND CONVERT(CHAR(10), A.ADDED_DUE_DATE1, 23) >= '{1}' " +
                                           "           AND CONVERT(CHAR(10), A.ADDED_DUE_DATE1, 23) <= '{2}' " +
                                           "           AND B.ENTITY_NO IS NOT NULL) C, " +
                                           "       (SELECT COUNT(A.SEQ) AS CALVE_PRE_COUNTER " +
                                           "          FROM UDF_HISTORY_LASTDATA({0}) A " +
                                           "          LEFT OUTER JOIN ENTITY_NEW_INFO B " +
                                           "		    ON A.ENTITY_SEQ = B.SEQ " +
                                           "		   AND B.FLAG = 'Y' " +
                                           "		   AND B.ACTIVE_FLAG = 'Y' " +
                                           "         WHERE (A.BREED_TYPE = 'A' OR A.BREED_TYPE = 'D') " +
                                           "		   AND A.FARM_SEQ = {0} " +
                                           "           AND A.FLAG = 'Y' " +
                                           "           AND CONVERT(CHAR(10), A.ADDED_DUE_DATE1, 23) >= '{1}' " +
                                           "           AND CONVERT(CHAR(10), A.ADDED_DUE_DATE1, 23) <= '{2}' " +
                                           "           AND B.ENTITY_NO IS NOT NULL) D, " +
                                           "       (SELECT COUNT(A.SEQ) AS DRYUP_PRE_COUNTER " +
                                           "          FROM UDF_HISTORY_LASTDATA({0}) A " +
                                           "          LEFT OUTER JOIN ENTITY_NEW_INFO B " +
                                           "		    ON A.ENTITY_SEQ = B.SEQ " +
                                           "		   AND B.FLAG = 'Y' " +
                                           "		   AND B.ACTIVE_FLAG = 'Y' " +
                                           "         WHERE A.BREED_TYPE = 'A' " +
                                           "		   AND A.FARM_SEQ = {0} " +
                                           "           AND A.FLAG = 'Y' " +
                                           "           AND CONVERT(CHAR(10), A.ADDED_DUE_DATE2, 23) >= '{1}' " +
                                           "           AND CONVERT(CHAR(10), A.ADDED_DUE_DATE2, 23) <= '{2}' " +
                                           "           AND B.ENTITY_NO IS NOT NULL) E ", parameters.farm_seq, sFromDate, sToDate);

                    if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                    {
                        if (dataReader.HasRows)
                        {
                            List<string> displayName = new List<string>();
                            List<int> displayCount = new List<int>();

                            dataReader.Read();

                            string sName = string.Empty;

                            // 발정예정
                            switch (parameters.lang_code)
                            {
                                case "KR": sName = "발정예정"; break;
                                case "JP": sName = "発情予定"; break;
                                case "US": sName = "S-ESTRUS"; break;
                                case "CN": sName = "发情预期"; break;
                                case "PT": sName = "ESTRO-AG"; break;
                                case "BR": sName = "ESTRO-AG"; break;
                                default: sName = "S-ESTRUS"; break;
                            }

                            displayName.Add(sName);
                            displayCount.Add(dataReader.GetInt32(0));

                            // 수정예정
                            switch (parameters.lang_code)
                            {
                                case "KR": sName = "수정예정"; break;
                                case "JP": sName = "授精予定"; break;
                                case "US": sName = "S-AI"; break;
                                case "CN": sName = "授精预期"; break;
                                case "PT": sName = "IA-AG"; break;
                                case "BR": sName = "IA-AG"; break;
                                default: sName = "S-AI"; break;
                            }

                            displayName.Add(sName);
                            displayCount.Add(dataReader.GetInt32(1));

                            // 감정예정
                            switch (parameters.lang_code)
                            {
                                case "KR": sName = "감정예정"; break;
                                case "JP": sName = "鑑定予定"; break;
                                case "US": sName = "S-DIAGNOS"; break;
                                case "CN": sName = "鉴定预期"; break;
                                case "PT": sName = "DIAGNÓ-AG"; break;
                                case "BR": sName = "DIAGNÓ-AG"; break;
                                default: sName = "S-DIAGNOS"; break;
                            }

                            displayName.Add(sName);
                            displayCount.Add(dataReader.GetInt32(2));

                            // 분만예정
                            switch (parameters.lang_code)
                            {
                                case "KR": sName = "분만예정"; break;
                                case "JP": sName = "分娩予定"; break;
                                case "US": sName = "S-CALVING"; break;
                                case "CN": sName = "分娩预期"; break;
                                case "PT": sName = "PARTO-AG"; break;
                                case "BR": sName = "PARTO-AG"; break;
                                default: sName = "S-CALVING"; break;
                            }

                            displayName.Add(sName);
                            displayCount.Add(dataReader.GetInt32(3));

                            // 건유예정
                            switch (parameters.lang_code)
                            {
                                case "KR": sName = "건유예정"; break;
                                case "JP": sName = "乾乳予定"; break;
                                case "US": sName = "S-DRY UP"; break;
                                case "CN": sName = "干奶预期"; break;
                                case "PT": sName = "SECAGEM-AG"; break;
                                case "BR": sName = "SECAGEM-AG"; break;
                                default: sName = "S-DRY UP"; break;
                            }

                            displayName.Add(sName);
                            displayCount.Add(dataReader.GetInt32(4));

                            stDisplay.display_name = JsonConvert.SerializeObject(displayName, Newtonsoft.Json.Formatting.None);
                            stDisplay.display_count = JsonConvert.SerializeObject(displayCount, Newtonsoft.Json.Formatting.None);

                            stStatistics.schedule_pre_info = stDisplay;
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

                // 다음 달 통계
                sFromDate = DateTime.Now.AddMonths(1).ToString("yyyy-MM-01");
                sToDate = string.Format("{0}-{1:D2}", DateTime.Now.AddMonths(1).ToString("yyyy-MM"), DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.AddMonths(1).Month));

                if (!isError)
                {
                    sQuery = string.Format("SELECT A.ESTRUS_PRE_COUNTER, B.INSEMINATE_PRE_COUNTER, C.APPRAISAL_PRE_COUNTER, D.CALVE_PRE_COUNTER, E.DRYUP_PRE_COUNTER " +
                                           "  FROM (SELECT COUNT(A.SEQ) AS ESTRUS_PRE_COUNTER " +
                                           "          FROM UDF_HISTORY_LASTDATA({0}) A " +
                                           "          LEFT OUTER JOIN ENTITY_NEW_INFO B " +
                                           "		    ON A.ENTITY_SEQ = B.SEQ " +
                                           "		   AND B.FLAG = 'Y' " +
                                           "		   AND B.ACTIVE_FLAG = 'Y' " +
                                           "         WHERE (A.BREED_TYPE = 'E' OR A.BREED_TYPE = 'C') " +
                                           "		   AND A.FARM_SEQ = {0} " +
                                           "           AND A.FLAG = 'Y' " +
                                           "           AND CONVERT(CHAR(10), A.BREED_DUE_DATE, 23) >= '{1}' " +
                                           "           AND CONVERT(CHAR(10), A.BREED_DUE_DATE, 23) <= '{2}' " +
                                           "           AND B.ENTITY_NO IS NOT NULL) A, " +
                                           "       (SELECT COUNT(A.SEQ) AS INSEMINATE_PRE_COUNTER " +
                                           "          FROM UDF_HISTORY_LASTDATA({0}) A " +
                                           "          LEFT OUTER JOIN ENTITY_NEW_INFO B " +
                                           "		    ON A.ENTITY_SEQ = B.SEQ " +
                                           "		   AND B.FLAG = 'Y' " +
                                           "		   AND B.ACTIVE_FLAG = 'Y' " +
                                           "         WHERE A.BREED_TYPE = 'E' " +
                                           "		   AND A.FARM_SEQ = {0} " +
                                           "           AND A.FLAG = 'Y' " +
                                           "           AND CONVERT(CHAR(10), A.ADDED_DUE_DATE1, 23) >= '{1}' " +
                                           "           AND CONVERT(CHAR(10), A.ADDED_DUE_DATE1, 23) <= '{2}' " +
                                           "           AND B.ENTITY_NO IS NOT NULL) B, " +
                                           "       (SELECT COUNT(A.SEQ) AS APPRAISAL_PRE_COUNTER " +
                                           "          FROM UDF_HISTORY_LASTDATA({0}) A " +
                                           "          LEFT OUTER JOIN ENTITY_NEW_INFO B " +
                                           "		    ON A.ENTITY_SEQ = B.SEQ " +
                                           "		   AND B.FLAG = 'Y' " +
                                           "		   AND B.ACTIVE_FLAG = 'Y' " +
                                           "         WHERE A.BREED_TYPE = 'I' " +
                                           "		   AND A.FARM_SEQ = {0} " +
                                           "           AND A.FLAG = 'Y' " +
                                           "           AND CONVERT(CHAR(10), A.ADDED_DUE_DATE1, 23) >= '{1}' " +
                                           "           AND CONVERT(CHAR(10), A.ADDED_DUE_DATE1, 23) <= '{2}' " +
                                           "           AND B.ENTITY_NO IS NOT NULL) C, " +
                                           "       (SELECT COUNT(A.SEQ) AS CALVE_PRE_COUNTER " +
                                           "          FROM UDF_HISTORY_LASTDATA({0}) A " +
                                           "          LEFT OUTER JOIN ENTITY_NEW_INFO B " +
                                           "		    ON A.ENTITY_SEQ = B.SEQ " +
                                           "		   AND B.FLAG = 'Y' " +
                                           "		   AND B.ACTIVE_FLAG = 'Y' " +
                                           "         WHERE (A.BREED_TYPE = 'A' OR A.BREED_TYPE = 'D') " +
                                           "		   AND A.FARM_SEQ = {0} " +
                                           "           AND A.FLAG = 'Y' " +
                                           "           AND CONVERT(CHAR(10), A.ADDED_DUE_DATE1, 23) >= '{1}' " +
                                           "           AND CONVERT(CHAR(10), A.ADDED_DUE_DATE1, 23) <= '{2}' " +
                                           "           AND B.ENTITY_NO IS NOT NULL) D, " +
                                           "       (SELECT COUNT(A.SEQ) AS DRYUP_PRE_COUNTER " +
                                           "          FROM UDF_HISTORY_LASTDATA({0}) A " +
                                           "          LEFT OUTER JOIN ENTITY_NEW_INFO B " +
                                           "		    ON A.ENTITY_SEQ = B.SEQ " +
                                           "		   AND B.FLAG = 'Y' " +
                                           "		   AND B.ACTIVE_FLAG = 'Y' " +
                                           "         WHERE A.BREED_TYPE = 'A' " +
                                           "		   AND A.FARM_SEQ = {0} " +
                                           "           AND A.FLAG = 'Y' " +
                                           "           AND CONVERT(CHAR(10), A.ADDED_DUE_DATE2, 23) >= '{1}' " +
                                           "           AND CONVERT(CHAR(10), A.ADDED_DUE_DATE2, 23) <= '{2}' " +
                                           "           AND B.ENTITY_NO IS NOT NULL) E ", parameters.farm_seq, sFromDate, sToDate);

                    if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                    {
                        if (dataReader.HasRows)
                        {
                            List<string> displayName = new List<string>();
                            List<int> displayCount = new List<int>();

                            dataReader.Read();

                            string sName = string.Empty;

                            // 발정예정
                            switch (parameters.lang_code)
                            {
                                case "KR": sName = "발정예정"; break;
                                case "JP": sName = "発情予定"; break;
                                case "US": sName = "S-ESTRUS"; break;
                                case "CN": sName = "发情预期"; break;
                                case "PT": sName = "ESTRO-AG"; break;
                                case "BR": sName = "ESTRO-AG"; break;
                                default: sName = "S-ESTRUS"; break;
                            }

                            displayName.Add(sName);
                            displayCount.Add(dataReader.GetInt32(0));

                            // 수정예정
                            switch (parameters.lang_code)
                            {
                                case "KR": sName = "수정예정"; break;
                                case "JP": sName = "授精予定"; break;
                                case "US": sName = "S-AI"; break;
                                case "CN": sName = "授精预期"; break;
                                case "PT": sName = "IA-AG"; break;
                                case "BR": sName = "IA-AG"; break;
                                default: sName = "S-AI"; break;
                            }

                            displayName.Add(sName);
                            displayCount.Add(dataReader.GetInt32(1));

                            // 감정예정
                            switch (parameters.lang_code)
                            {
                                case "KR": sName = "감정예정"; break;
                                case "JP": sName = "鑑定予定"; break;
                                case "US": sName = "S-DIAGNOS"; break;
                                case "CN": sName = "鉴定预期"; break;
                                case "PT": sName = "DIAGNÓ-AG"; break;
                                case "BR": sName = "DIAGNÓ-AG"; break;
                                default: sName = "S-DIAGNOS"; break;
                            }

                            displayName.Add(sName);
                            displayCount.Add(dataReader.GetInt32(2));

                            // 분만예정
                            switch (parameters.lang_code)
                            {
                                case "KR": sName = "분만예정"; break;
                                case "JP": sName = "分娩予定"; break;
                                case "US": sName = "S-CALVING"; break;
                                case "CN": sName = "分娩预期"; break;
                                case "PT": sName = "PARTO-AG"; break;
                                case "BR": sName = "PARTO-AG"; break;
                                default: sName = "S-CALVING"; break;
                            }

                            displayName.Add(sName);
                            displayCount.Add(dataReader.GetInt32(3));

                            // 건유예정
                            switch (parameters.lang_code)
                            {
                                case "KR": sName = "건유예정"; break;
                                case "JP": sName = "乾乳予定"; break;
                                case "US": sName = "S-DRY UP"; break;
                                case "CN": sName = "干奶预期"; break;
                                case "PT": sName = "SECAGEM-AG"; break;
                                case "BR": sName = "SECAGEM-AG"; break;
                                default: sName = "S-DRY UP"; break;
                            }

                            displayName.Add(sName);
                            displayCount.Add(dataReader.GetInt32(4));

                            stDisplay.display_name = JsonConvert.SerializeObject(displayName, Newtonsoft.Json.Formatting.None);
                            stDisplay.display_count = JsonConvert.SerializeObject(displayCount, Newtonsoft.Json.Formatting.None);

                            stStatistics.schedule_pos_info = stDisplay;
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

                // 일정 통계
                if (!isError)
                {
                    sFromDate = DateTime.Now.ToString("yyyy-MM-01");
                    sToDate = string.Format("{0}-{1:D2}", DateTime.Now.ToString("yyyy-MM"), DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

                    sQuery = string.Format("SELECT BREED_TYPE, ORDER_SEQ, COUNT(SEQ) AS BREED_COUNT " +
                                           "  FROM (SELECT A.SEQ, A.BREED_TYPE, " +
                                           "               CASE WHEN A.BREED_TYPE = 'E' THEN 1 " +
                                           "                    WHEN A.BREED_TYPE = 'I' THEN 2 " +
                                           "                    WHEN A.BREED_TYPE = 'A' THEN 3 " +
                                           "                    WHEN A.BREED_TYPE = 'C' THEN 4 " +
                                           "                    WHEN A.BREED_TYPE = 'D' THEN 5 " +
                                           "                END ORDER_SEQ " +
                                           "          FROM BREED_HISTORY A " +
                                           "          LEFT OUTER JOIN ENTITY_NEW_INFO B " +
                                           "            ON A.ENTITY_SEQ = B.SEQ " +
                                           "           AND B.FLAG = 'Y' " +
                                           "           AND B.ACTIVE_FLAG = 'Y' " +
                                           "         WHERE A.FARM_SEQ = {0} " +
                                           "           AND CONVERT(CHAR(10), A.BREED_DATE, 23) >= '{1}' " +
                                           "           AND CONVERT(CHAR(10), A.BREED_DATE, 23) <= '{2}' " +
                                           "           AND A.FLAG = 'Y' " +
                                           "           AND B.ENTITY_NO IS NOT NULL) A " +
                                           " GROUP BY A.BREED_TYPE, A.ORDER_SEQ " +
                                           " ORDER BY ORDER_SEQ", parameters.farm_seq, sFromDate, sToDate);

                    if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                    {
                        if (dataReader.HasRows)
                        {
                            List<string> displayName = new List<string>();
                            List<int> displayCount = new List<int>();

                            string sName = string.Empty;

                            switch (parameters.lang_code)
                            {
                                case "KR": sName = "발정"; break;
                                case "JP": sName = "発情"; break;
                                case "US": sName = "Estrus"; break;
                                case "CN": sName = "发情"; break;
                                case "PT": sName = "Estro"; break;
                                case "BR": sName = "Estro"; break;
                                default: sName = "Estrus"; break;
                            }

                            displayName.Add(sName);
                            displayCount.Add(0);

                            switch (parameters.lang_code)
                            {
                                case "KR": sName = "수정"; break;
                                case "JP": sName = "授精"; break;
                                case "US": sName = "Insemination"; break;
                                case "CN": sName = "授精"; break;
                                case "PT": sName = "Inseminação"; break;
                                case "BR": sName = "Inseminação"; break;
                                default: sName = "Insemination"; break;
                            }

                            displayName.Add(sName);
                            displayCount.Add(0);

                            switch (parameters.lang_code)
                            {
                                case "KR": sName = "감정"; break;
                                case "JP": sName = "鑑定"; break;
                                case "US": sName = "Diagnosis"; break;
                                case "CN": sName = "鉴定"; break;
                                case "PT": sName = "Diganóstico"; break;
                                case "BR": sName = "Diganóstico"; break;
                                default: sName = "Diagnosis"; break;
                            }

                            displayName.Add(sName);
                            displayCount.Add(0);

                            switch (parameters.lang_code)
                            {
                                case "KR": sName = "분만"; break;
                                case "JP": sName = "分娩"; break;
                                case "US": sName = "Calving"; break;
                                case "CN": sName = "分娩"; break;
                                case "PT": sName = "Parto"; break;
                                case "BR": sName = "Parto"; break;
                                default: sName = "Calving"; break;
                            }

                            displayName.Add(sName);
                            displayCount.Add(0);

                            switch (parameters.lang_code)
                            {
                                case "KR": sName = "건유"; break;
                                case "JP": sName = "乾乳"; break;
                                case "US": sName = "Dry up"; break;
                                case "CN": sName = "干奶"; break;
                                case "PT": sName = "Secagem"; break;
                                case "BR": sName = "Secagem"; break;
                                default: sName = "Dry up"; break;
                            }

                            displayName.Add(sName);
                            displayCount.Add(0);

                            while (dataReader.Read())
                            {
                                displayCount[dataReader.GetInt32(1) - 1] = dataReader.GetInt32(2);
                            }

                            stDisplay.display_name = JsonConvert.SerializeObject(displayName, Newtonsoft.Json.Formatting.None);
                            stDisplay.display_count = JsonConvert.SerializeObject(displayCount, Newtonsoft.Json.Formatting.None);

                            stStatistics.breed_info = stDisplay;
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
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SUCCESS,
                        message = string.Empty,
                        data = JsonConvert.SerializeObject(stStatistics)
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

        public ClassResponse.RES_RESULT GetChartActivity(ClassRequest.REQ_CHART_LINE parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Entity", "chart_activity");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetChartActivity  ==========", sModuleName);
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

                if (string.IsNullOrEmpty(parameters.check_date))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_CHECK_DATE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_CHECK_DATE),
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

            List<ClassStruct.ST_CHART_ACTIVITY> activityList = new List<ClassStruct.ST_CHART_ACTIVITY>();

            try
            {
                // 활동량 지표를 레오테크인 경우에는 1500으로 설정하고 나머지는 2000으로 설정하기 위해서 TAG_KIND 정보를 추출한다 (2021-03-15)
                string sTagKind = string.Empty;

                string sQuery = string.Format("SELECT B.TAG_KIND " +
                                       "  FROM ENTITY_NEW_INFO A " +
                                       "  LEFT OUTER JOIN TAG_INFO B " +
                                       "    ON A.TAG_ID = B.TAG_ID " +
                                       " WHERE A.SEQ = {0} " +
                                       "   AND A.ACTIVE_FLAG = 'Y' " +
                                       "   AND A.FLAG = 'Y' " +
                                       "   AND B.FLAG = 'Y' ", parameters.entity_seq);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        dataReader.Read();
                        sTagKind = dataReader.GetString(0);
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
                    // 농장의 UTC 시간을 이용하는 것이 아니고 파라미터로 받은 일자를 이용하도록 수정 (2018-01-24)
                    DateTime dtCheckDate = Convert.ToDateTime(parameters.check_date);

                    // 활동량 지표를 레오테크인 경우에는 1500으로 설정하고 나머지는 2000으로 설정한다 (2021-03-15)
                    int nLimit = 2000;
                    if (sTagKind == "L") nLimit = 1500;

                    // 기존 10분단위 리스트에서 시간단위 리스트로 변경 (2021-06-24)
                    // 시간단위 리스트에서 시간당 MAX 값을 이용해서 추출한다

                    //string sStart = dtCheckDate.AddHours(-30).ToString("yyyy-MM-dd HH:mm:00");
                    //string sFrom = dtCheckDate.AddHours(-24).ToString("yyyy-MM-dd HH:mm:00");
                    //string sTo = dtCheckDate.ToString("yyyy-MM-dd HH:mm:59");
                    string sStart = dtCheckDate.AddHours(-30).ToString("yyyy-MM-dd HH:00:00");
                    string sFrom = dtCheckDate.AddHours(-24).ToString("yyyy-MM-dd HH:00:00");
                    string sTo = dtCheckDate.AddHours(1).ToString("yyyy-MM-dd HH:00:00");
                    string sTableName = string.Format("TEMP_ENTITY_{0}", parameters.entity_seq);

                    //sQuery = String.Format("WITH RESULT AS ( " +
                    //                       "SELECT CREATE_DATE, CHECK_YEAR, CHECK_MONTH, CHECK_DAY, CHECK_HOUR, MINUTE_INDEX, " +
                    //                       "       TEMPERATURE, BATTERY, RSSI, SNR, SENSOR_VALUE, " +
                    //                       "       CASE WHEN SENSOR_VALUE < {4} THEN 0 " +
                    //                       "            WHEN SENSOR_VALUE >= {4} THEN SENSOR_VALUE / 1000.0 " +
                    //                       "        END AS SENSOR_INDEX " +
                    //                       " FROM (SELECT CREATE_DATE, CHECK_YEAR, CHECK_MONTH, CHECK_DAY, CHECK_HOUR, MINUTE_INDEX, " +
                    //                       "              TEMPERATURE, BATTERY, RSSI, SNR, SENSOR_VALUE, " +
                    //                       "              ROW_NUMBER() " +
                    //                       "              OVER (PARTITION BY A.CHECK_YEAR, A.CHECK_MONTH, A.CHECK_DAY, A.CHECK_HOUR, A.MINUTE_INDEX " +
                    //                       "	                ORDER BY A.SENSOR_VALUE DESC) AS ROW_NUM " +
                    //                       "        FROM (SELECT A.CREATE_DATE, A.CHECK_YEAR, A.CHECK_MONTH, A.CHECK_DAY, A.CHECK_HOUR, " +
                    //                       "                     CASE WHEN A.CHECK_MINUTE BETWEEN 0 AND 9 THEN 0 " +
                    //                       "                          WHEN A.CHECK_MINUTE BETWEEN 10 AND 19 THEN 1 " +
                    //                       "                          WHEN A.CHECK_MINUTE BETWEEN 20 AND 29 THEN 2 " +
                    //                       "                          WHEN A.CHECK_MINUTE BETWEEN 30 AND 39 THEN 3 " +
                    //                       "                          WHEN A.CHECK_MINUTE BETWEEN 40 AND 49 THEN 4 " +
                    //                       "                          WHEN A.CHECK_MINUTE BETWEEN 50 AND 59 THEN 5 " +
                    //                       "                     END AS MINUTE_INDEX, " +
                    //                       "                     A.TEMPERATURE, A.BATTERY, A.RSSI, A.SNR, A.SENSOR_VALUE " +
                    //                       "                FROM {0} A " +
                    //                       "               WHERE A.CREATE_DATE >= '{1}' " +
                    //                       "                 AND A.CREATE_DATE <= '{3}' " +
                    //                       "             ) A " +
                    //                       "      ) LIST " +
                    //                       " WHERE ROW_NUM = 1) " +
                    //                       "SELECT A.CREATE_DATE, A.CHECK_YEAR, A.CHECK_MONTH, A.CHECK_DAY, A.CHECK_HOUR, A.MINUTE_INDEX, " +
                    //                       "       A.TEMPERATURE, A.BATTERY, A.RSSI, A.SNR, ISNULL(A.SENSOR_VALUE, 0), ISNULL(A.SENSOR_INDEX, 0), " +
                    //                       "       MOVING_AVG = (SELECT ISNULL(AVG(SENSOR_VALUE), 0) " +
                    //                       "                       FROM RESULT B " +
                    //                       "                      WHERE B.CREATE_DATE >= DATEADD(MINUTE, -360, A.CREATE_DATE) " +
                    //                       "                        AND B.CREATE_DATE <= A.CREATE_DATE) / 1000.0 " +
                    //                       "  FROM RESULT A " +
                    //                       " WHERE A.CREATE_DATE >= '{2}' " +
                    //                       "   AND A.CREATE_DATE <= '{3}' " +
                    //                       " ORDER BY A.CREATE_DATE ", sTableName, sStart, sFrom, sTo, nLimit);

                    sQuery = string.Format("WITH TEMP_LIST AS ( " +
                                           "SELECT CHECK_YEAR, CHECK_MONTH, CHECK_DAY, CHECK_HOUR, ISNULL(MAX(SENSOR_VALUE), 0) AS MAX_SENSOR_VALUE, " +
                                           "       CONVERT(DATETIME, CONCAT(CONVERT(NVARCHAR, CHECK_YEAR), '-', CONVERT(NVARCHAR, CHECK_MONTH), '-', CONVERT(NVARCHAR, CHECK_DAY), ' ', CONVERT(NVARCHAR, CHECK_HOUR), ':00:00')) AS CREATE_DATE " +
                                           "  FROM {0} " +
                                           " WHERE CREATE_DATE >= '{1}' " +
                                           "   AND CREATE_DATE < '{3}' " +
                                           " GROUP BY CHECK_YEAR, CHECK_MONTH, CHECK_DAY, CHECK_HOUR " +
                                           ") " +
                                           "SELECT A.CREATE_DATE, " +
                                           "       CASE WHEN A.MAX_SENSOR_VALUE < {4} THEN 0 " +
                                           "            ELSE A.MAX_SENSOR_VALUE / 1000.0 " +
                                           "        END SENSOR_VALUE, " +
                                           "		MOVING_AVG = (SELECT ISNULL(AVG(B.MAX_SENSOR_VALUE), 0) " +
                                           "                        FROM TEMP_LIST B " +
                                           "                       WHERE B.CREATE_DATE >= DATEADD(HOUR, -4, A.CREATE_DATE) " +
                                           "                         AND B.CREATE_DATE <= A.CREATE_DATE) / 1000.0 " +
                                           "  FROM TEMP_LIST A " +
                                           " WHERE A.CREATE_DATE >= '{2}' " +
                                           "   AND A.CREATE_DATE < '{3}'", sTableName, sStart, sFrom, sTo, nLimit);

                    if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                //string sKey = String.Format("{0}{1}0:00",
                                //    dataReader.GetDateTime(0).ToString("yyyy-MM-dd HH:mm:ss").Substring(0, 14), dataReader.GetInt32(5));
                                //double dActivity = Convert.ToDouble(dataReader.GetDecimal(11).ToString("F1"));
                                //double dAverage = Convert.ToDouble(dataReader.GetDecimal(12).ToString("F1"));

                                string sKey = dataReader.GetDateTime(0).ToString("yyyy-MM-dd HH:00:00");
                                double dActivity = Convert.ToDouble(dataReader.GetDecimal(1).ToString("F1"));
                                double dAverage = Convert.ToDouble(dataReader.GetDecimal(2).ToString("F1"));

                                ClassStruct.ST_CHART_ACTIVITY activityInfo = new ClassStruct.ST_CHART_ACTIVITY
                                {
                                    time_disp = sKey,
                                    activity_value = dActivity,
                                    avg_value = dAverage
                                };

                                activityList.Add(activityInfo);
                            }
                        }

                        dataReader.Close();
                        dataReader.Dispose();

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SUCCESS,
                            message = string.Empty,
                            data = JsonConvert.SerializeObject(activityList)
                        };
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

        public ClassResponse.RES_RESULT GetChartActivityRange(ClassRequest.REQ_CHART_LINE_RANGE parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Entity", "chart_activity_range");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetChartActivityRange  ==========", sModuleName);
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

                DateTime dtValue = DateTime.MinValue;

                if (string.IsNullOrEmpty(parameters.check_start_date) && !DateTime.TryParse(parameters.check_start_date, out dtValue))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_CHECK_DATE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_CHECK_DATE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (string.IsNullOrEmpty(parameters.check_end_date) && !DateTime.TryParse(parameters.check_end_date, out dtValue))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_CHECK_DATE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_CHECK_DATE),
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

            List<ClassStruct.ST_CHART_ACTIVITY> activityList = new List<ClassStruct.ST_CHART_ACTIVITY>();

            try
            {
                // 활동량 지표를 레오테크인 경우에는 1500으로 설정하고 나머지는 2000으로 설정하기 위해서 TAG_KIND 정보를 추출한다 (2021-03-15)
                string sTagKind = string.Empty;

                string sQuery = string.Format("SELECT B.TAG_KIND " +
                                       "  FROM ENTITY_NEW_INFO A " +
                                       "  LEFT OUTER JOIN TAG_INFO B " +
                                       "    ON A.TAG_ID = B.TAG_ID " +
                                       " WHERE A.SEQ = {0} " +
                                       "   AND A.ACTIVE_FLAG = 'Y' " +
                                       "   AND A.FLAG = 'Y' " +
                                       "   AND B.FLAG = 'Y' ", parameters.entity_seq);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        dataReader.Read();
                        sTagKind = dataReader.GetString(0);
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
                    // 활동량 지표를 레오테크인 경우에는 1500으로 설정하고 나머지는 2000으로 설정한다 (2021-03-15)
                    int nLimit = 2000;
                    if (sTagKind == "L") nLimit = 1500;
                    string sTableName = string.Format("TEMP_ENTITY_{0}", parameters.entity_seq);

                    //                   sQuery = $@"
                    //WITH TEMP_LIST AS (
                    //SELECT CHECK_YEAR, CHECK_MONTH, CHECK_DAY, CHECK_HOUR, ISNULL(MAX(SENSOR_VALUE), 0) AS MAX_SENSOR_VALUE, 

                    //CONVERT(DATETIME, CONCAT(CONVERT(NVARCHAR, CHECK_YEAR), '-', CONVERT(NVARCHAR, CHECK_MONTH), '-', CONVERT(NVARCHAR, CHECK_DAY), ' ', CONVERT(NVARCHAR, CHECK_HOUR), ':00:00')) AS CREATE_DATE 
                    //FROM {sTableName} 
                    //WHERE (CONVERT(CHAR(10), CREATE_DATE, 23) BETWEEN '{parameters.check_start_date}' AND '{parameters.check_end_date}')
                    //GROUP BY CHECK_YEAR, CHECK_MONTH, CHECK_DAY, CHECK_HOUR 
                    //) 
                    //SELECT A.CREATE_DATE, 
                    //CASE WHEN A.MAX_SENSOR_VALUE < {nLimit} THEN 0 
                    //ELSE A.MAX_SENSOR_VALUE / 1000.0 
                    //END SENSOR_VALUE, 
                    //MOVING_AVG = (SELECT ISNULL(AVG(B.MAX_SENSOR_VALUE), 0) 
                    //FROM TEMP_LIST B 
                    //WHERE B.CREATE_DATE >= DATEADD(HOUR, -4, A.CREATE_DATE) 
                    //AND B.CREATE_DATE <= A.CREATE_DATE) / 1000.0 
                    //FROM TEMP_LIST A 
                    //WHERE (CONVERT(CHAR(10), A.CREATE_DATE, 23) BETWEEN '{parameters.check_start_date}' AND '{parameters.check_end_date}')";
                    
                    DateTime dtStartCheckDate = Convert.ToDateTime(parameters.check_start_date);
                    DateTime dtEndCheckDate = Convert.ToDateTime(parameters.check_end_date);

                    string sStart = dtStartCheckDate.AddHours(-6).ToString("yyyy-MM-dd HH:00:00");
                    string sFrom = dtStartCheckDate.ToString("yyyy-MM-dd HH:00:00");
                    string sTo = dtEndCheckDate.AddHours(1).ToString("yyyy-MM-dd HH:00:00");

                    sQuery = $@"
 WITH TEMP_LIST AS (
 SELECT CHECK_YEAR, CHECK_MONTH, CHECK_DAY, CHECK_HOUR, ISNULL(MAX(SENSOR_VALUE), 0) AS MAX_SENSOR_VALUE, 
 CONVERT(DATETIME, CONCAT(CONVERT(NVARCHAR, CHECK_YEAR), '-', CONVERT(NVARCHAR, CHECK_MONTH), '-', CONVERT(NVARCHAR, CHECK_DAY), ' ', CONVERT(NVARCHAR, CHECK_HOUR), ':00:00')) AS CREATE_DATE 
 FROM {sTableName} 
 WHERE CREATE_DATE >= '{sStart}' 
 AND CREATE_DATE < '{sTo}' 
 GROUP BY CHECK_YEAR, CHECK_MONTH, CHECK_DAY, CHECK_HOUR
 )
 SELECT A.CREATE_DATE, 
 CASE WHEN A.MAX_SENSOR_VALUE < {nLimit} THEN 0 
 ELSE A.MAX_SENSOR_VALUE / 1000.0
 END SENSOR_VALUE, 
 MOVING_AVG = (SELECT ISNULL(AVG(B.MAX_SENSOR_VALUE), 0) 
 FROM TEMP_LIST B 
 WHERE B.CREATE_DATE >= DATEADD(HOUR, -4, A.CREATE_DATE) 
 AND B.CREATE_DATE <= A.CREATE_DATE) / 1000.0 
 FROM TEMP_LIST A 
 WHERE A.CREATE_DATE >= '{sFrom}' 
 AND A.CREATE_DATE < '{sTo}'";

                    if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                string sKey = dataReader.GetDateTime(0).ToString("yyyy-MM-dd HH:00:00");
                                double dActivity = Convert.ToDouble(dataReader.GetDecimal(1).ToString("F1"));
                                double dAverage = Convert.ToDouble(dataReader.GetDecimal(2).ToString("F1"));

                                ClassStruct.ST_CHART_ACTIVITY activityInfo = new ClassStruct.ST_CHART_ACTIVITY
                                {
                                    time_disp = sKey,
                                    activity_value = dActivity,
                                    avg_value = dAverage
                                };

                                activityList.Add(activityInfo);
                            }
                        }

                        dataReader.Close();
                        dataReader.Dispose();

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SUCCESS,
                            message = string.Empty,
                            data = JsonConvert.SerializeObject(activityList)
                        };
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

        public ClassResponse.RES_RESULT GetInnerActivityStatus(ClassRequest.REQ_FARMENTITY parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Entity", "inner_status");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetInnerActivityStatus  ==========", sModuleName);
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
                string sQuery = string.Format("SELECT INNER_STATUS " +
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

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SUCCESS,
                            message = string.Empty,
                            data = dataReader.GetString(0)
                        };
                    }
                    else
                    {
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

        public ClassResponse.RES_RESULT GetPinInfo(ClassRequest.REQ_PIN_INFO parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Entity", "pin_info");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetPinInfo  ==========", sModuleName);
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

                if (string.IsNullOrEmpty(parameters.entity_pin))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_ENTITY_PIN,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_ENTITY_PIN),
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

            ClassStruct.ST_PIN_INFO pinInfo = new ClassStruct.ST_PIN_INFO();

            try
            {
                string sPrefix = "410";
                string sEntityID = string.Empty;

                if (parameters.entity_pin.Length == 15 || parameters.entity_pin.Substring(0, 3) == "410")
                    sEntityID = parameters.entity_pin;
                else
                    sEntityID = sPrefix + parameters.entity_pin;

                //string sKey = "04ECkvNd6RsuAmWNuj6KdemjSBD5YNkgWlv5Vd0KEfQkvLJcW21pqQviA1RcZZZyRInzTd5zVEZUOWiU%2FBSXfw%3D%3D";
                string sKey = "yfQSfxFXxu38KoWOZpdawhtBa2LbrsnMQA5elLShoQzVE8Qu2CNC9GoVCpA9YlsPT7UinwXFERzXa9xuXBAt9g%3D%3D";
                string sURL = string.Format("http://data.ekape.or.kr/openapi-data/service/user/mtrace/breeding/cattle?cattleNo={0}&ServiceKey={1}", sEntityID, sKey);

                WebClient client = new WebClient();
                client.Headers.Add("Content-Type", "application/xml");
                client.Encoding = Encoding.UTF8;
                string result = client.DownloadString(sURL);

                // XML Parsing
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(result);

                XmlNodeList xmlNodes = null;

                string sResultCode = string.Empty;
                string sREsultMsg = string.Empty;
                string sBirth = string.Empty;
                int nBirthMonth = 0;
                int nBirthCode = 0;
                string sCattleNo = string.Empty;
                string sTypeCode = string.Empty;
                string sTypeName = string.Empty;
                string sSexCode = string.Empty;
                string sSexName = string.Empty;

                // 결과 비교
                xmlNodes = xmlDoc.SelectNodes("//header");
                if (xmlNodes.Count == 0)
                {
                    isError = true;

                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SEARCH_NOTEXIST_ENTITY,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SEARCH_NOTEXIST_ENTITY),
                        data = string.Empty
                    };
                }

                if (!isError)
                {
                    sResultCode = xmlNodes[0].ChildNodes[0].InnerText;
                    sREsultMsg = xmlNodes[0].ChildNodes[1].InnerText;

                    if (sResultCode != "00")
                    {
                        isError = true;

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SEARCH_NOTEXIST_ENTITY,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SEARCH_NOTEXIST_ENTITY),
                            data = string.Empty
                        };
                    }
                }

                if (!isError)
                {
                    // 식별번호
                    xmlNodes = xmlDoc.SelectNodes("//cattleNo");
                    if (xmlNodes.Count == 0)
                        isError = true;
                    else
                        sCattleNo = xmlNodes[0].InnerText;

                    // 출생일자
                    xmlNodes = xmlDoc.SelectNodes("//birthYmd");
                    if (xmlNodes.Count == 0)
                        isError = true;
                    else
                        sBirth = xmlNodes[0].InnerText;

                    // 개체타입 코드
                    xmlNodes = xmlDoc.SelectNodes("//lsTypeCd");
                    if (xmlNodes.Count == 0)
                        isError = true;
                    else
                        sTypeCode = xmlNodes[0].InnerText;

                    // 개체타입 이름
                    xmlNodes = xmlDoc.SelectNodes("//lsTypeNm");
                    if (xmlNodes.Count == 0)
                        isError = true;
                    else
                        sTypeName = xmlNodes[0].InnerText;

                    // 개체성별 코드
                    xmlNodes = xmlDoc.SelectNodes("//sexCd");
                    if (xmlNodes.Count == 0)
                        isError = true;
                    else
                        sSexCode = xmlNodes[0].InnerText;

                    // 개체성별 이름
                    xmlNodes = xmlDoc.SelectNodes("//sexNm");
                    if (xmlNodes.Count == 0)
                        isError = true;
                    else
                        sSexName = xmlNodes[0].InnerText;

                    if (!string.IsNullOrEmpty(sBirth))
                    {
                        TimeSpan timespan = DateTime.Now - Convert.ToDateTime(sBirth);

                        int nDays = (int)timespan.TotalDays;
                        nBirthMonth = (int)(nDays / (365.25 / 12));

                        if (nDays < 270)
                            nBirthCode = 3;
                        else if (nDays >= 270 && nDays <= 569)
                            nBirthCode = 1;
                        else
                            nBirthCode = 2;
                    }
                }

                if (isError)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SEARCH_NOTEXIST_ENTITY,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SEARCH_NOTEXIST_ENTITY),
                        data = string.Empty
                    };
                }
                else
                {
                    pinInfo.entity_sex = Convert.ToInt32(sSexCode.Substring(sSexCode.Length - 1));
                    pinInfo.entity_type = Convert.ToInt32(sTypeCode.Substring(sTypeCode.Length - 1));
                    pinInfo.birth_day = sBirth;
                    pinInfo.birth_month = nBirthMonth;
                    pinInfo.birth_code = nBirthCode;

                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SUCCESS,
                        message = string.Empty,
                        data = JsonConvert.SerializeObject(pinInfo)
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

        public ClassResponse.RES_RESULT GetChartInterupt(ClassRequest.REQ_CHART_LINE parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Entity", "chart_interupt");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetChartInterupt  ==========", sModuleName);
            ClassLog._mLogger.InfoFormat("{0}  RECEIVE REQUEST DATA  [{1}]", sModuleName, requestData);

            ClassResponse.RES_RESULT response = new ClassResponse.RES_RESULT();
            string receiveData = string.Empty;

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

                if (string.IsNullOrEmpty(parameters.check_date))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_CHECK_DATE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_CHECK_DATE),
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
            string sQuery = string.Empty;

            OleDbDataReader dataReader = null;
            ClassStruct.ST_CHART_INTERUPT interuptList = new ClassStruct.ST_CHART_INTERUPT();
            List<ClassStruct.ST_INTERUPT_INFO> infoList = new List<ClassStruct.ST_INTERUPT_INFO>();

            Dictionary<string, double> tempDictionary = new Dictionary<string, double>();

            try
            {
                string sTableName = string.Format("TEMP_ENTITY_{0}", parameters.entity_seq);
                DateTime dtCheckDate = Convert.ToDateTime(parameters.check_date);

                string sFrom = dtCheckDate.AddDays(-30).ToString("yyyy-MM-dd");
                string sTo = dtCheckDate.ToString("yyyy-MM-dd");

                sQuery = string.Format("SELECT CHECK_YEAR, CHECK_MONTH, CHECK_DAY, SUM(INT_COUNT) AS SUM_COUNT " +
                                       "  FROM {0} " +
                                       " WHERE CONVERT(CHAR(10), CREATE_DATE, 23) >= '{1}' " +
                                       "   AND CONVERT(CHAR(10), CREATE_DATE, 23) <= '{2}' " +
                                       " GROUP BY CHECK_YEAR, CHECK_MONTH, CHECK_DAY " +
                                       " ORDER BY CHECK_YEAR, CHECK_MONTH, CHECK_DAY ", sTableName, sFrom, sTo);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            ClassStruct.ST_INTERUPT_INFO interuptInfo = new ClassStruct.ST_INTERUPT_INFO
                            {
                                time_disp = string.Format("{0}-{1:D2}-{2:D2}", dataReader.GetInt32(0), dataReader.GetInt32(1), dataReader.GetInt32(2)),
                                interupt_value = dataReader.GetInt32(3)
                            };

                            infoList.Add(interuptInfo);
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
                    // 데이타가 없을 때에는 Average 추출 시 오류가 발생된다
                    if (infoList.Count > 0)
                    {
                        interuptList.int_average = (int)infoList.Average(x => x.interupt_value);
                        interuptList.int_list = infoList;
                    }
                    else
                    {
                        interuptList.int_average = 0;
                        interuptList.int_list = infoList;
                    }

                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SUCCESS,
                        message = string.Empty,
                        data = JsonConvert.SerializeObject(interuptList)
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

        public ClassResponse.RES_RESULT GetChartInteruptRange(ClassRequest.REQ_CHART_LINE_RANGE parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Entity", "chart_interupt_range");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetChartInteruptRange  ==========", sModuleName);
            ClassLog._mLogger.InfoFormat("{0}  RECEIVE REQUEST DATA  [{1}]", sModuleName, requestData);

            ClassResponse.RES_RESULT response = new ClassResponse.RES_RESULT();
            string receiveData = string.Empty;

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
                
                
                DateTime dtValue = DateTime.MinValue;

                if (string.IsNullOrEmpty(parameters.check_start_date) && !DateTime.TryParse(parameters.check_start_date, out dtValue))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_CHECK_DATE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_CHECK_DATE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (string.IsNullOrEmpty(parameters.check_end_date) && !DateTime.TryParse(parameters.check_end_date, out dtValue))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_CHECK_DATE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_CHECK_DATE),
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
            string sQuery = string.Empty;

            OleDbDataReader dataReader = null;
            ClassStruct.ST_CHART_INTERUPT interuptList = new ClassStruct.ST_CHART_INTERUPT();
            List<ClassStruct.ST_INTERUPT_INFO> infoList = new List<ClassStruct.ST_INTERUPT_INFO>();

            Dictionary<string, double> tempDictionary = new Dictionary<string, double>();

            try
            {
                string sTableName = string.Format("TEMP_ENTITY_{0}", parameters.entity_seq);

                sQuery = $@"
 SELECT CHECK_YEAR, CHECK_MONTH, CHECK_DAY, SUM(INT_COUNT) AS SUM_COUNT 
 FROM {sTableName} 
 WHERE (CONVERT(CHAR(10), CREATE_DATE, 23) BETWEEN '{parameters.check_start_date}' AND '{parameters.check_end_date}')
 GROUP BY CHECK_YEAR, CHECK_MONTH, CHECK_DAY 
 ORDER BY CHECK_YEAR, CHECK_MONTH, CHECK_DAY 
";

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            ClassStruct.ST_INTERUPT_INFO interuptInfo = new ClassStruct.ST_INTERUPT_INFO
                            {
                                time_disp = string.Format("{0}-{1:D2}-{2:D2}", dataReader.GetInt32(0), dataReader.GetInt32(1), dataReader.GetInt32(2)),
                                interupt_value = dataReader.GetInt32(3)
                            };

                            infoList.Add(interuptInfo);
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
                    // 데이타가 없을 때에는 Average 추출 시 오류가 발생된다
                    if (infoList.Count > 0)
                    {
                        interuptList.int_average = (int)infoList.Average(x => x.interupt_value);
                        interuptList.int_list = infoList;
                    }
                    else
                    {
                        interuptList.int_average = 0;
                        interuptList.int_list = infoList;
                    }

                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SUCCESS,
                        message = string.Empty,
                        data = JsonConvert.SerializeObject(interuptList)
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
