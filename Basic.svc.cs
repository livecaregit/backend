using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LC_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Basic" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Basic.svc or Basic.svc.cs at the Solution Explorer and start debugging.
    public class Basic : IBasic
    {
        private ClassOLEDB _mClassDatabase = new ClassOLEDB();
        private readonly ClassError _mClassError = new ClassError();
        private readonly ClassFunction _mClassFunction = new ClassFunction();

        ~Basic()
        {
            if (_mClassDatabase.GetConnectionState()) _mClassDatabase.CloseDatabase();
            _mClassDatabase = null;
        }

        public ClassResponse.RES_RESULT GetDatabaseInfo()
        {
            ClassResponse.RES_RESULT response = new ClassResponse.RES_RESULT();
            ClassStruct.ST_DATABASE stDatabase = new ClassStruct.ST_DATABASE();

#if DEBUG
            //stDatabase.db_server = "livecare-testdb.cdh4z4yvbsq3.ap-northeast-2.rds.amazonaws.com";
            stDatabase.db_server = "db.livecareworld.com";
            stDatabase.db_name = "LIVECARE";
            stDatabase.db_user = "lcservice";
            stDatabase.db_password = "!lcservice!";
#else
            //stDatabase.db_server = "livecaredb.cdh4z4yvbsq3.ap-northeast-2.rds.amazonaws.com";
            stDatabase.db_server = "db.livecareworld.com";
            stDatabase.db_name = "LIVECARE";
            stDatabase.db_user = "lcservice";
            stDatabase.db_password = "!lcservice!";
#endif

            response = new ClassResponse.RES_RESULT
            {
                result = ClassError.RESULT_SUCCESS,
                message = string.Empty,
                data = JsonConvert.SerializeObject(stDatabase)
            };

            return response;
        }

        public ClassResponse.RES_RESULT SetLogin(ClassRequest.REQ_LOGIN parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Basic", "login");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START SetLogin  ==========", sModuleName);
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

                if (string.IsNullOrEmpty(parameters.pwd))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_PWD,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_PWD),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (string.IsNullOrEmpty(parameters.device_type))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_DEVICE_TYPE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_DEVICE_TYPE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (string.IsNullOrEmpty(parameters.model))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_MODEL,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_MODEL),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (string.IsNullOrEmpty(parameters.serial))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_SERIAL,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_SERIAL),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }
                
                if (string.IsNullOrEmpty(parameters.app_version))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_APP_VERSION,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_APP_VERSION),
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

            ClassStruct.ST_USERINFO userInfo = new ClassStruct.ST_USERINFO();
            OleDbDataReader dataReader = null;

            try
            {
                int nFarmSeq = 0;
                string sPassword = string.Empty;
                string sUserName = string.Empty;
                string sUserType = string.Empty;
                string sUserKind = string.Empty;
                string sModel = string.Empty;
                string sSerial = string.Empty;
                string sDeviceType = string.Empty;
                string sDeviceOs = string.Empty;
                string sProfile = string.Empty;

                string sQuery = string.Format("SELECT PWD, NAME, TYPE, KIND, FARM_SEQ, DEVICE_MODEL, DEVICE_SERIAL, DEVICE_OS, PROFILE_URL, DEVICE_TYPE " +
                           "  FROM USER_INFO " +
                           " WHERE FLAG = 'Y' " +
                           "   AND USER_ID = '{0}' ", parameters.uid.Replace("'", "''"));
                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        dataReader.Read();

                        sPassword = dataReader.GetString(0);
                        sUserName = dataReader.GetString(1);
                        sUserType = dataReader.GetString(2);
                        sUserKind = dataReader.GetString(3);
                        nFarmSeq = dataReader.GetInt32(4);
                        sModel = _mClassDatabase.GetSafeString(dataReader, 5);
                        sSerial = _mClassDatabase.GetSafeString(dataReader, 6);
                        sDeviceOs = _mClassDatabase.GetSafeString(dataReader, 7);
                        sProfile = _mClassDatabase.GetSafeString(dataReader, 8);
                        sDeviceType = _mClassDatabase.GetSafeString(dataReader, 9);
                    }
                    else
                    {
                        isError = true;

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_ERROR_MEMBER_NOTMATCHED,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_ERROR_MEMBER_NOTMATCHED),
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

                // 비밀번호 체크
                if (!isError)
                {
                    if (parameters.pwd != sPassword)
                    {
                        isError = true;

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_ERROR_PASSWORD_NOTMATCHED,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_ERROR_PASSWORD_NOTMATCHED),
                            data = string.Empty
                        };
                    }
                }

                // 버젼 체크
                if (!isError)
                {
                    string sAppVersion = _mClassFunction.GetAppVersion(parameters.device_type);

                    if (string.IsNullOrEmpty(sAppVersion))
                    {
                        isError = true;

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_ERROR_APPINFO_FAILED,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_ERROR_APPINFO_FAILED),
                            data = string.Empty
                        };
                    }
                    else
                    {
                        Version dbVersion = Version.Parse(sAppVersion);
                        Version appVersion = Version.Parse(parameters.app_version);

                        // 2017-06-09 DB 버전이 앱 버전보다 같거나 낮으면 성공
                        if (dbVersion > appVersion)
                        {
                            isError = true;

                            response = new ClassResponse.RES_RESULT
                            {
                                result = ClassError.RESULT_ERROR_APPUPDATE_NEEDED,
                                message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_ERROR_APPUPDATE_NEEDED),
                                data = string.Empty
                            };

                            // 버전업이 필요한 경우
                            //   안드로이드 단말은 디바이스를 먼저 초기화시킨다 - DEVICE_SERIAL 쳬계가 변경되었기 때문에 (2020-11-03)
                            //   아이폰 단말은 DEVICE_OS 버전이 '13.0.0' 보다 작은 경우에는 OS 업데이트 필요하다는 메세지로 전송

                            //if (sDeviceType == "A")
                            //{
                            //    sQuery = String.Format("UPDATE USER_INFO " +
                            //                           "   SET DEVICE_MODEL = NULL, " +
                            //                           "       DEVICE_SERIAL = NULL, " +
                            //                           "       DEVICE_OS = NULL " +
                            //                           " WHERE USER_ID = '{0}' ", parameters.uid.Replace("'", "''"));

                            //    ClassLog._mLogger.DebugFormat("query : {0}", sQuery);

                            //    int count = _mClassDatabase.QueryExecute(sQuery);

                            //    ClassLog._mLogger.DebugFormat("Result : {0}", count);

                            //    // 업데이트 메세지는 전송한다
                            //    isError = true;

                            //    response = new ClassResponse.RES_RESULT
                            //    {
                            //        result = ClassError.RESULT_ERROR_APPUPDATE_NEEDED,
                            //        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_ERROR_APPUPDATE_NEEDED),
                            //        data = String.Empty
                            //    };
                            //}
                            //else if (sDeviceType == "I")
                            //{
                            //    Version osVersion = new Version(sDeviceOs);
                            //    Version basicVersion = new Version("13.0.0");

                            //    if (osVersion < basicVersion)
                            //    {
                            //        isError = true;

                            //        response = new ClassResponse.RES_RESULT
                            //        {
                            //            result = ClassError.RESULT_ERROR_OSUPDATE_NEEDED,
                            //            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_ERROR_OSUPDATE_NEEDED),
                            //            data = String.Empty
                            //        };
                            //    }
                            //    else
                            //    {
                            //        isError = true;

                            //        response = new ClassResponse.RES_RESULT
                            //        {
                            //            result = ClassError.RESULT_ERROR_APPUPDATE_NEEDED,
                            //            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_ERROR_APPUPDATE_NEEDED),
                            //            data = String.Empty
                            //        };
                            //    }
                            //}
                        }
                    }
                }

                // 사용자인 경우에는 디바이스를 체크한다
                // Device ID 값 비교때문에 오류가 발생하여 우선은 체크하지 않고 무조건 수정하도록 한다 (2020-12-08)
                if (!isError && sUserKind == "U")
                {
                    // 단말기가 등록되지 않은 경우에는 최초 등록으로 비교하지 않으며
                    // 단말기가 등록된 경우에는 사용중이던 단말기인지 체크한다
                    //if (!String.IsNullOrEmpty(sModel) || !String.IsNullOrEmpty(sSerial))
                    //{
                    //    if (parameters.model != sModel || parameters.serial != sSerial)
                    //    {
                    //        // 단만기 정보가 틀린 경우에는 RESULT_NOTMATCH 에러를 리턴한다
                    //        isError = true;

                    //        response = new ClassResponse.RES_RESULT
                    //        {
                    //            result = ClassError.RESULT_ERROR_DEVICE_NOTMATCHED,
                    //            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_ERROR_DEVICE_NOTMATCHED),
                    //            data = String.Empty
                    //        };
                    //    }
                    //}
                }

                if (!isError)
                {
                    //if (sUserType == "U")
                    //{
                    //    // 사용자인 경우 최초 단말기 정보가 없었으면 단말기 정보와 앱 버젼을 수정하고
                    //    // 단말기 정보가 있는 경우에는 앱 버젼만 수정한다
                    //    if (String.IsNullOrEmpty(sModel) && String.IsNullOrEmpty(sSerial))
                    //    {
                    //        sQuery = "UPDATE USER_INFO SET ";
                    //        sQuery += " DEVICE_TYPE = '" + parameters.device_type + "', ";
                    //        sQuery += " DEVICE_MODEL = '" + parameters.model + "', ";
                    //        sQuery += " DEVICE_SERIAL = '" + parameters.serial + "', ";
                    //        if (String.IsNullOrEmpty(parameters.os_version)) sQuery += " DEVICE_OS = NULL, ";
                    //        else sQuery += " DEVICE_OS = '" + parameters.os_version + "', ";
                    //        if (String.IsNullOrEmpty(parameters.user_token)) sQuery += " USER_TOKEN = NULL, ";
                    //        else sQuery += " USER_TOKEN = '" + parameters.user_token + "', ";
                    //        sQuery += " APP_VERSION = '" + parameters.app_version + "', ";
                    //        sQuery += " LANG_CODE = '" + parameters.lang_code + "'";
                    //    }
                    //    else
                    //    {
                    //        // 모델명은 수정한다 (2019-12-26)
                    //        sQuery = "UPDATE USER_INFO SET ";
                    //        if (String.IsNullOrEmpty(parameters.os_version)) sQuery += " DEVICE_OS = NULL, ";
                    //        else sQuery += " DEVICE_OS = '" + parameters.os_version + "', ";
                    //        if (String.IsNullOrEmpty(parameters.user_token)) sQuery += " USER_TOKEN = NULL, ";
                    //        else sQuery += " USER_TOKEN = '" + parameters.user_token + "', ";
                    //        sQuery += " APP_VERSION = '" + parameters.app_version + "', ";
                    //        sQuery += " LANG_CODE = '" + parameters.lang_code + "'";
                    //    }
                    //}
                    //else if (sUserType == "A")
                    //{
                    //    // 관리자인 경우에는 디바이스 타입 / 사용자 토큰 / 앱 버젼 정보를 수정한다
                    //    sQuery = "UPDATE USER_INFO SET ";
                    //    sQuery += " DEVICE_TYPE = '" + parameters.device_type + "', ";
                    //    sQuery += " DEVICE_MODEL = '" + parameters.model + "', ";
                    //    sQuery += " DEVICE_SERIAL = '" + parameters.serial + "', ";
                    //    if (String.IsNullOrEmpty(parameters.os_version)) sQuery += " DEVICE_OS = NULL, ";
                    //    else sQuery += " DEVICE_OS = '" + parameters.os_version + "', ";
                    //    if (String.IsNullOrEmpty(parameters.user_token)) sQuery += " USER_TOKEN = NULL, ";
                    //    else sQuery += " USER_TOKEN = '" + parameters.user_token + "', ";
                    //    sQuery += " APP_VERSION = '" + parameters.app_version + "', ";
                    //    sQuery += " LANG_CODE = '" + parameters.lang_code + "' ";
                    //}

                    // Device ID 오류로 인해서 우선은 무조건 수정하도록 한다 (2020-12-08)
                    sQuery = "UPDATE USER_INFO SET ";
                    sQuery += " DEVICE_TYPE = '" + parameters.device_type + "', ";
                    sQuery += " DEVICE_MODEL = '" + parameters.model + "', ";
                    sQuery += " DEVICE_SERIAL = '" + parameters.serial + "', ";
                    if (string.IsNullOrEmpty(parameters.os_version)) sQuery += " DEVICE_OS = NULL, ";
                    else sQuery += " DEVICE_OS = '" + parameters.os_version + "', ";
                    if (string.IsNullOrEmpty(parameters.user_token)) sQuery += " USER_TOKEN = NULL, ";
                    else sQuery += " USER_TOKEN = '" + parameters.user_token + "', ";
                    sQuery += " APP_VERSION = '" + parameters.app_version + "', ";
                    sQuery += " LANG_CODE = '" + parameters.lang_code + "' ";
                    

                    sQuery += string.Format(" WHERE USER_ID = '{0}'", parameters.uid.Replace("'", "''"));

                    int count = _mClassDatabase.QueryExecute(sQuery);

                    if (count > 0)
                    {
                        userInfo.user_name = sUserName;
                        userInfo.user_type = sUserType;
                        userInfo.user_kind = sUserKind;
                        userInfo.farm_seq = nFarmSeq;
                        userInfo.profile_url = sProfile;

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SUCCESS,
                            message = string.Empty,
                            data = JsonConvert.SerializeObject(userInfo)
                        };
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

        public ClassResponse.RES_RESULT GetCodeList(ClassRequest.REQ_CODELIST parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Basic", "code_list");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetCodeList  ==========", sModuleName);
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

                if (string.IsNullOrEmpty(parameters.code_div))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_CODE_DIV,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_CODE_DIV),
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
            List<ClassStruct.ST_CODEINFO> codeList = new List<ClassStruct.ST_CODEINFO>();

            try
            {
                string sQuery;
                switch (parameters.lang_code)
                {
                    case "KR": sQuery = "SELECT CODE_DIV, CODE_NO, DIV_NAME, CODE_NAME "; break;
                    case "JP": sQuery = "SELECT CODE_DIV, CODE_NO, DIV_NAME, JP_VALUE "; break;
                    case "US": sQuery = "SELECT CODE_DIV, CODE_NO, DIV_NAME, EN_VALUE "; break;
                    case "CN": sQuery = "SELECT CODE_DIV, CODE_NO, DIV_NAME, ZH_VALUE "; break;
                    case "PT": sQuery = "SELECT CODE_DIV, CODE_NO, DIV_NAME, PT_VALUE "; break;
                    case "BR": sQuery = "SELECT CODE_DIV, CODE_NO, DIV_NAME, PT_VALUE "; break;
                    default: sQuery = "SELECT CODE_DIV, CODE_NO, DIV_NAME, EN_VALUE "; break;
                }

                sQuery += string.Format("  FROM CODE_MST " +
                                        " WHERE FLAG = 'Y' " +
                                        "   AND CODE_DIV = '{0}' ", parameters.code_div);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        while(dataReader.Read())
                        {
                            string sValue = _mClassDatabase.GetSafeString(dataReader, 3);

                            if (!string.IsNullOrEmpty(sValue))
                            {
                                ClassStruct.ST_CODEINFO codeInfo = new ClassStruct.ST_CODEINFO
                                {
                                    code_div = dataReader.GetString(0),
                                    div_name = dataReader.GetString(2),
                                    code_no = dataReader.GetInt32(1),
                                    code_name = sValue
                                };

                                codeList.Add(codeInfo);
                            }
                        }

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SUCCESS,
                            message = string.Empty,
                            data = JsonConvert.SerializeObject(codeList)
                        };
                    }
                    else
                    {
                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SEARCH_NOTEXIST_CODE,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SEARCH_NOTEXIST_CODE),
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

        public ClassResponse.RES_RESULT GetFarmList(ClassRequest.REQ_FARMLIST parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Basic", "farm_list");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetFarmList  ==========", sModuleName);
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
            List<ClassStruct.ST_FARMINFO> farmList = new List<ClassStruct.ST_FARMINFO>();

            try
            {
                string sQuery;

                // 사용자도 여러 농장으로 설정될 수 있도록 수정
                // 사용자인 경우에는 해당 농장정보만 추출한다
                switch (parameters.lang_code)
                {
                    case "KR": sQuery = "SELECT A.SEQ, A.NAME AS FARM_NAME, B.OWNER, B.TEL_NO, B.ADDRESS, B.ENTITY_TYPE, C.CODE_NAME"; break;
                    case "JP": sQuery = "SELECT A.SEQ, A.NAME AS FARM_NAME, B.OWNER, B.TEL_NO, B.ADDRESS, B.ENTITY_TYPE, C.JP_VALUE"; break;
                    case "US": sQuery = "SELECT A.SEQ, A.NAME AS FARM_NAME, B.OWNER, B.TEL_NO, B.ADDRESS, B.ENTITY_TYPE, C.EN_VALUE"; break;
                    case "CN": sQuery = "SELECT A.SEQ, A.NAME AS FARM_NAME, B.OWNER, B.TEL_NO, B.ADDRESS, B.ENTITY_TYPE, C.ZH_VALUE"; break;
                    case "PT": sQuery = "SELECT A.SEQ, A.NAME AS FARM_NAME, B.OWNER, B.TEL_NO, B.ADDRESS, B.ENTITY_TYPE, C.PT_VALUE"; break;
                    case "BR": sQuery = "SELECT A.SEQ, A.NAME AS FARM_NAME, B.OWNER, B.TEL_NO, B.ADDRESS, B.ENTITY_TYPE, C.PT_VALUE"; break;
                    default: sQuery = "SELECT A.SEQ, A.NAME AS FARM_NAME, B.OWNER, B.TEL_NO, B.ADDRESS, B.ENTITY_TYPE, C.EN_VALUE"; break;
                }

                sQuery += string.Format("  FROM UDF_USER_FARMLIST('{0}') A " +
                                        "  LEFT OUTER JOIN FARM_INFO B " +
                                        "    ON A.SEQ = B.SEQ " +
                                        "  LEFT OUTER JOIN CODE_MST C " +
                                        "    ON B.ENTITY_TYPE = C.CODE_NO " +
                                        "   AND C.CODE_DIV = '110' " +
                                        "   AND C.FLAG = 'Y' " +
                                        " WHERE B.FLAG = 'Y' ", parameters.uid);
                if (parameters.entity_type > 0) sQuery += string.Format("   AND B.ENTITY_TYPE = {0} ", parameters.entity_type);
                if (parameters.nation_code > 0) sQuery += string.Format("   AND B.NATION_CODE = {0} ", parameters.nation_code);
                if (parameters.region_code > 0) sQuery += string.Format("   AND B.REGION_CODE = {0} ", parameters.region_code);
                if (parameters.partner_code > 0) sQuery += string.Format("   AND B.PARTNER_CODE = {0} ", parameters.partner_code);
                if (parameters.branch_code > 0) sQuery += string.Format("   AND B.BRANCH_CODE = {0} ", parameters.branch_code);
                if (!string.IsNullOrEmpty(parameters.address)) sQuery += string.Format("   AND B.ADDRESS LIKE '%{0}%' ", parameters.address);
                if (!string.IsNullOrEmpty(parameters.farm_name)) sQuery += string.Format("   AND A.NAME LIKE '%{0}%' ", parameters.farm_name);
                sQuery += " ORDER BY A.NAME";

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            ClassStruct.ST_FARMINFO farmInfo = new ClassStruct.ST_FARMINFO
                            {
                                farm_seq = dataReader.GetInt32(0),
                                farm_name = dataReader.GetString(1),
                                owner = _mClassDatabase.GetSafeString(dataReader, 2),
                                tel_no = _mClassDatabase.GetSafeString(dataReader, 3),
                                address = _mClassDatabase.GetSafeString(dataReader, 4),
                                entity_type = dataReader.GetInt32(5),
                                entity_type_disp = _mClassDatabase.GetSafeString(dataReader, 6)
                            };

                            farmList.Add(farmInfo);
                        }

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SUCCESS,
                            message = string.Empty,
                            data = JsonConvert.SerializeObject(farmList)
                        };
                    }
                    else
                    {
                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SEARCH_NOTEXIST_FARM,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SEARCH_NOTEXIST_FARM),
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

        public ClassResponse.RES_RESULT GetTagList(ClassRequest.REQ_FARMSEQ parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Basic", "tag_list");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetTagList  ==========", sModuleName);
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
            List<ClassStruct.ST_TAGINFO> tagList = new List<ClassStruct.ST_TAGINFO>();

            try
            {
                string sQuery = string.Format("SELECT A.SEQ, A.TAG_ID, A.MAKE_DATE, B.NAME AS FARM_NAME " +
                                       "  FROM TAG_INFO A " +
                                       "  LEFT OUTER JOIN FARM_INFO B " +
                                       "    ON A.FARM_SEQ = B.SEQ " +
                                       "   AND B.FLAG = 'Y' " +
                                       " WHERE A.FARM_SEQ = {0} " +
                                       "   AND A.ENTITY_SEQ = 0 " +
                                       "   AND A.FLAG = 'Y'", parameters.farm_seq);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            ClassStruct.ST_TAGINFO tagInfo = new ClassStruct.ST_TAGINFO
                            {
                                farm_name = dataReader.GetString(3),
                                tag_id = _mClassDatabase.GetSafeString(dataReader, 1),
                                make_date = dataReader.IsDBNull(2) ? string.Empty : dataReader.GetDateTime(2).ToString("yyyy-MM-dd")
                            };

                            tagList.Add(tagInfo);
                        }

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SUCCESS,
                            message = string.Empty,
                            data = JsonConvert.SerializeObject(tagList)
                        };
                    }
                    else
                    {
                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SEARCH_NOTEXIST_TAG,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SEARCH_NOTEXIST_TAG),
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

        public ClassResponse.RES_RESULT GetSemenList(ClassRequest.REQ_FARMSEQ parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Basic", "semen_list");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetSemenList  ==========", sModuleName);
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
            List<ClassStruct.ST_SEMENINFO> semenList = new List<ClassStruct.ST_SEMENINFO>();

            try
            {
                // 정액번호를 FARM_SEMEN 테이블이 아니고
                // 번식이력 테이블인 BREED_HISTORY 테이블에서 최근 사용순으로 보내준다
                //sQuery = String.Format("SELECT SEQ, SEMEN_NO " +
                //                       "  FROM FARM_SEMEN " +
                //                       " WHERE FARM_SEQ = {0} " +
                //                       "   AND FLAG = 'Y' ", parameters.farm_seq);
                string sQuery = string.Format("SELECT A.SEMEN_SEQ, B.SEMEN_NO " +
                                       "  FROM (SELECT SEQ, BREED_DATE, SEMEN_SEQ " +
                                       "          FROM (SELECT ROW_NUMBER() OVER (PARTITION BY SEMEN_SEQ ORDER BY BREED_DATE DESC, SEQ DESC) AS ROW_NUM, " +
                                       "                       SEQ, BREED_DATE, SEMEN_SEQ " +
                                       "                  FROM (SELECT SEQ, BREED_DATE, BREED_INT_VALUE2 AS SEMEN_SEQ " +
                                       "                          FROM BREED_HISTORY " +
                                       "                         WHERE FARM_SEQ = {0} " +
                                       "                           AND BREED_TYPE = 'I' " +
                                       "                           AND FLAG = 'Y' " +
                                       "                           AND BREED_INT_VALUE2 > 0 " +
                                       "                       ) A " +
                                       "               ) B " +
                                       "         WHERE ROW_NUM = 1 " +
                                       "       ) A " +
                                       "  LEFT OUTER JOIN FARM_SEMEN B " +
                                       "    ON A.SEMEN_SEQ = B.SEQ " +
                                       "   AND B.FLAG = 'Y' " +
                                       " WHERE B.SEMEN_NO IS NOT NULL " +
                                       " ORDER BY A.BREED_DATE DESC, A.SEQ DESC ", parameters.farm_seq);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            ClassStruct.ST_SEMENINFO semenInfo = new ClassStruct.ST_SEMENINFO
                            {
                                semen_seq = dataReader.GetInt32(0),
                                semen_no = dataReader.GetString(1)
                            };

                            semenList.Add(semenInfo);
                        }

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SUCCESS,
                            message = string.Empty,
                            data = JsonConvert.SerializeObject(semenList)
                        };
                    }
                    else
                    {
                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SEARCH_NOTEXIST_SEMEN,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SEARCH_NOTEXIST_SEMEN),
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

        public ClassResponse.RES_RESULT SetSemenInsert(ClassRequest.REQ_SEMENNO parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Basic", "semen_insert");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START SetSemenInsert  ==========", sModuleName);
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

                if (string.IsNullOrEmpty(parameters.semen_no))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_SEMEN_NO,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_SEMEN_NO),
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

                // 존재하는 SEMEN_NO 인지 체크
                string sQuery = string.Format("SELECT SEQ " +
                                       "  FROM FARM_SEMEN " +
                                       " WHERE FARM_SEQ = {0} " +
                                       "   AND SEMEN_NO = '{1}' " +
                                       "   AND FLAG = 'Y' ", parameters.farm_seq, parameters.semen_no);
                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        isError = true;

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_ERROR_SEMEN_EXISTED,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_ERROR_SEMEN_EXISTED),
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
                    sQuery = string.Format("INSERT INTO FARM_SEMEN (FARM_SEQ, SEMEN_NO, CREATE_DATE) VALUES ({0}, '{1}', '{2}')",
                        parameters.farm_seq, parameters.semen_no, sUtcTime);

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

        public ClassResponse.RES_RESULT SetSemenDelete(ClassRequest.REQ_SEMENSEQ parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Basic", "semen_delete");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START SetSemenDelete  ==========", sModuleName);
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

                if (parameters.semen_seq < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_SEMEN_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_SEMEN_SEQ),
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
                string sQuery = string.Format("UPDATE FARM_SEMEN SET FLAG = 'N' WHERE SEQ = {0} AND FARM_SEQ = {1}", parameters.semen_seq, parameters.farm_seq);

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

        public ClassResponse.RES_RESULT SetSemenSetting(ClassRequest.REQ_SEMENSEQ parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Basic", "semen_set");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START SetSemenSetting  ==========", sModuleName);
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

                if (parameters.semen_seq < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_SEMEN_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_SEMEN_SEQ),
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
                // 정액번호 확인
                string sQuery = string.Format("SELECT SEMEN_NO FROM FARM_SEMEN WHERE FARM_SEQ = {0} AND SEQ = {1} AND FLAG = 'Y' ", parameters.farm_seq, parameters.semen_seq);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (!dataReader.HasRows)
                    {
                        isError = true;

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SEARCH_NOTEXIST_SEMEN,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SEARCH_NOTEXIST_SEMEN),
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
                    sQuery = string.Format("UPDATE BREED_DEFAULT SET SEMEN_SEQ = {1} WHERE FARM_SEQ = {0} AND FLAG = 'Y' ", parameters.farm_seq, parameters.semen_seq);
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

        public ClassResponse.RES_RESULT UploadImage(string fileName, System.IO.Stream fileContents)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Basic", "upload");
            string requestData = string.Format("file name : {0}", fileName);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START UploadImage  ==========", sModuleName);
            ClassLog._mLogger.InfoFormat("{0}  RECEIVE REQUEST DATA  [{1}]", sModuleName, requestData);

            ClassResponse.RES_RESULT response = new ClassResponse.RES_RESULT();
            string receiveData = string.Empty;

            #region Check Parameter Process
            try
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_FILE_NAME,
                        message = string.Empty,
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
                    message = string.Empty,
                    data = string.Empty
                };

                ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                return response;
            }
            #endregion

            #region Check Database Process
            #endregion

            #region Business Logic Process
            try
            {
                string sFolder = string.Empty;

#if DEBUG
                sFolder = @"D:\www\www.ulikekorea.com\LC_SERVICE\UploadImage";
#else
                sFolder = @"E:\LIVECARE\LC_SERVICE\UploadImage";
#endif

                if (!Directory.Exists(sFolder)) Directory.CreateDirectory(sFolder);

                string sFilePath = Path.Combine(sFolder, fileName);
                ClassLog._mLogger.InfoFormat("{0}  SAVE FILE PATH  [path={1}]", sModuleName, sFilePath);

                using (FileStream filestream = new FileStream(sFilePath, FileMode.Create))
                {
                    fileContents.CopyTo(filestream);
                    fileContents.Close();
                }

                response = new ClassResponse.RES_RESULT
                {
                    result = ClassError.RESULT_SUCCESS,
                    message = string.Empty,
                    data = string.Empty
                };
            }
            catch
            {
                response = new ClassResponse.RES_RESULT
                {
                    result = ClassError.RESULT_SYSTEM_ERROR_EXCEPTION,
                    message = string.Empty,
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

        public ClassResponse.RES_RESULT UploadeImageFromBase64(ClassRequest.REQ_UPLOADIMAGE parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Basic", "upload_base64");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START UploadeImageFromBase64  ==========", sModuleName);
            ClassLog._mLogger.InfoFormat("{0}  RECEIVE REQUEST DATA  [image kind : {1}    image name : {2}    image type : {3}]", sModuleName, parameters.image_kind, parameters.image_name, parameters.image_type);

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

                if (string.IsNullOrEmpty(parameters.image_name))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_IMAGE_NAME,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_IMAGE_NAME),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (string.IsNullOrEmpty(parameters.image_type))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_IMAGE_TYPE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_IMAGE_TYPE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (string.IsNullOrEmpty(parameters.image_data))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_IMAGE_DATA,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_IMAGE_DATA),
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
            #endregion

            #region Business Logic Process
            try
            {
                // 저장 폴더는 image_kind 값에 따라서 구분해서 처리한다
                // Empty 또는 "E" : 개체 이미지
                // "C" : 치료 이미지
                // "P" : 프로필 이미지
                // "M" : 채팅 메세지 이미지
                string sFolder = string.Empty;

#if DEBUG
                switch (parameters.image_kind)
                {
                    case "C": sFolder = @"D:\www\www.ulikekorea.com\LC_SERVICE\CureImage"; break;
                    case "P": sFolder = @"D:\www\www.ulikekorea.com\LC_SERVICE\ProfileImage"; break;
                    case "M": sFolder = @"D:\www\www.ulikekorea.com\LC_SERVICE\MessageImage"; break;
                    default: sFolder = @"D:\www\www.ulikekorea.com\LC_SERVICE\UploadImage"; break;
                }
#else
                switch (parameters.image_kind)
                {
                    case "C": sFolder = @"E:\LIVECARE\LC_SERVICE\CureImage"; break;
                    case "P": sFolder = @"E:\LIVECARE\LC_SERVICE\ProfileImage"; break;
                    case "M": sFolder = @"E:\LIVECARE\LC_SERVICE\MessageImage"; break;
                    default: sFolder = @"E:\LIVECARE\LC_SERVICE\UploadImage"; break;
                }
#endif

                if (!Directory.Exists(sFolder)) Directory.CreateDirectory(sFolder);

                string sFilePath = Path.Combine(sFolder, parameters.image_name);
                ClassLog._mLogger.InfoFormat("{0}  SAVE FILE PATH  [path={1}]", sModuleName, sFilePath);

                Image image = _mClassFunction.Base64ToImage(parameters.image_data);
                image.Save(sFilePath);

                response = new ClassResponse.RES_RESULT
                {
                    result = ClassError.RESULT_SUCCESS,
                    message = string.Empty,
                    data = string.Empty
                };
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

        public ClassResponse.RES_RESULT SetUserUpdate(ClassRequest.REQ_USERUPDATE parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Basic", "user_update");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START SetUserUpdate  ==========", sModuleName);
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

                if (string.IsNullOrEmpty(parameters.device_type))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_DEVICE_TYPE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_DEVICE_TYPE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }
                else if (parameters.device_type != "I" && parameters.device_type != "A")
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_DEVICE_TYPE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_DEVICE_TYPE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (string.IsNullOrEmpty(parameters.user_token))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_USER_TOKEN,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_USER_TOKEN),
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
                string sQuery = string.Format("SELECT PWD, NAME, TYPE, FARM_SEQ " +
                           "  FROM USER_INFO " +
                           " WHERE FLAG = 'Y' " +
                           "   AND USER_ID = '{0}' ", parameters.uid);
                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (!dataReader.HasRows)
                    {
                        isError = true;

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SEARCH_NOTEXIST_MEMBER,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SEARCH_NOTEXIST_MEMBER),
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
                    sQuery = string.Format("UPDATE USER_INFO SET DEVICE_TYPE = '{1}', USER_TOKEN = '{2}' WHERE USER_ID = '{0}'", 
                        parameters.uid, parameters.device_type, parameters.user_token);

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

        public ClassResponse.RES_RESULT CheckAppVersion(ClassRequest.REQ_VERSION parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Basic", "version");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START CheckAppVersion  ==========", sModuleName);
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

                if (string.IsNullOrEmpty(parameters.device_type))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_DEVICE_TYPE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_DEVICE_TYPE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (string.IsNullOrEmpty(parameters.app_version))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_APP_VERSION,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_APP_VERSION),
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
                string sAppVersion = _mClassFunction.GetAppVersion(parameters.device_type);

                if (string.IsNullOrEmpty(sAppVersion))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_ERROR_APPINFO_FAILED,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_ERROR_APPINFO_FAILED),
                        data = string.Empty
                    };
                }
                else
                {
                    Version dbVersion = Version.Parse(sAppVersion);
                    Version appVersion = Version.Parse(parameters.app_version);

                    // 2017-06-09 DB 버전이 앱 버전보다 같거나 낮으면 성공
                    if (dbVersion <= appVersion)
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
                            result = ClassError.RESULT_ERROR_APPUPDATE_NEEDED,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_ERROR_APPUPDATE_NEEDED),
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

        public ClassResponse.RES_RESULT UploadFile(string fileName, System.IO.Stream fileContents)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Basic", "upload_file");
            string requestData = string.Format("file name : {0}", fileName);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START UploadFile  ==========", sModuleName);
            ClassLog._mLogger.InfoFormat("{0}  RECEIVE REQUEST DATA  [{1}]", sModuleName, requestData);

            ClassResponse.RES_RESULT response = new ClassResponse.RES_RESULT();
            string receiveData = string.Empty;

            #region Check Parameter Process
            try
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_FILE_NAME,
                        message = string.Empty,
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
                    message = string.Empty,
                    data = string.Empty
                };

                ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                return response;
            }
            #endregion

            #region Check Database Process
            #endregion

            #region Business Logic Process
            try
            {
                string sFolder = string.Empty;

#if DEBUG
                sFolder = @"D:\www\www.ulikekorea.com\LC_SERVICE\UploadFile";
#else
                sFolder = @"E:\LIVECARE\LC_SERVICE\UploadFile";
#endif

                if (!Directory.Exists(sFolder)) Directory.CreateDirectory(sFolder);

                string sFilePath = Path.Combine(sFolder, fileName);
                ClassLog._mLogger.InfoFormat("{0}  SAVE FILE PATH  [path={1}]", sModuleName, sFilePath);

                using (FileStream filestream = new FileStream(sFilePath, FileMode.Create))
                {
                    fileContents.CopyTo(filestream);
                    fileContents.Close();
                }

                response = new ClassResponse.RES_RESULT
                {
                    result = ClassError.RESULT_SUCCESS,
                    message = string.Empty,
                    data = string.Empty
                };
            }
            catch
            {
                response = new ClassResponse.RES_RESULT
                {
                    result = ClassError.RESULT_SYSTEM_ERROR_EXCEPTION,
                    message = string.Empty,
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

        public ClassResponse.RES_RESULT SetUserProfileInsert(ClassRequest.REQ_PROFILE parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Basic", "profile_insert");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START SetUserProfileInsert  ==========", sModuleName);
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

                if (string.IsNullOrEmpty(parameters.image_name))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_IMAGE_NAME,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_IMAGE_NAME),
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
                // 사용자 정보 추출
                string sQuery = string.Format("SELECT NAME " +
                                       "  FROM USER_INFO " +
                                       " WHERE FLAG = 'Y' " +
                                       "   AND USER_ID = '{0}' ", parameters.uid.Replace("'", "''"));

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (!dataReader.HasRows)
                    {
                        isError = true;

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_ERROR_MEMBER_NOTMATCHED,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_ERROR_MEMBER_NOTMATCHED),
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
                    string sImageURL = string.Empty;
#if DEBUG
                    //sImageURL = String.Format("http://www.ulikekorea.com/LC_SERVICE/ProfileImage/{0}", parameters.image_name);
                    sImageURL = String.Format("http://www.livecareworld.com/LIVECARE/LC_SERVICE/ProfileImage/{0}", parameters.image_name);
#else
                    sImageURL = string.Format("http://www.livecareworld.com/LIVECARE/LC_SERVICE/ProfileImage/{0}", parameters.image_name);
#endif

                    sQuery = string.Format("UPDATE USER_INFO SET PROFILE_URL = '{1}' WHERE USER_ID = '{0}' ", parameters.uid, sImageURL);
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

        public ClassResponse.RES_RESULT SetUserProfileDelete(ClassRequest.REQ_USER_ID parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Basic", "profile_delete");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START SetUserProfileDelete  ==========", sModuleName);
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
            bool isError = false;
            OleDbDataReader dataReader = null;

            try
            {
                // 사용자 정보 추출
                string sQuery = string.Format("SELECT NAME " +
                                       "  FROM USER_INFO " +
                                       " WHERE FLAG = 'Y' " +
                                       "   AND USER_ID = '{0}' ", parameters.uid.Replace("'", "''"));
                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (!dataReader.HasRows)
                    {
                        isError = true;

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_ERROR_MEMBER_NOTMATCHED,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_ERROR_MEMBER_NOTMATCHED),
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
                    sQuery = string.Format("UPDATE USER_INFO SET PROFILE_URL = NULL WHERE USER_ID = '{0}' ", parameters.uid);
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

        public ClassResponse.RES_RESULT GetChattingList(ClassRequest.REQ_CHATLIST parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Basic", "chat_list");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetChattingList  ==========", sModuleName);
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

            List<ClassStruct.ST_CHATINFO> chatList = new List<ClassStruct.ST_CHATINFO>();

            try
            {
                string sQuery = string.Format("SELECT A.SEQ, A.FARM_SEQ, A.USER_ID, A.MSG_TYPE, A.MESSAGE, A.CREATE_DATE, B.NAME AS USER_NAME, B.PROFILE_URL " +
                           "  FROM CHAT_MESSAGE A " +
                           "  LEFT OUTER JOIN USER_INFO B " +
                           "    ON A.USER_ID = B.USER_ID " +
                           " WHERE A.SEQ > {0} " +
                           "   AND A.FARM_SEQ = {1} " +
                           " ORDER BY A.SEQ ", parameters.msg_no, parameters.farm_seq);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            ClassStruct.ST_CHATINFO chatInfo = new ClassStruct.ST_CHATINFO
                            {
                                message_no = dataReader.GetInt32(0),
                                user_id = dataReader.GetString(2),
                                user_name = _mClassDatabase.GetSafeString(dataReader, 6),
                                user_profile = _mClassDatabase.GetSafeString(dataReader, 7),
                                message_time = dataReader.GetDateTime(5).ToString("yyyy-MM-dd HH:mm:ss"),
                                message_type = dataReader.GetString(3),
                                message_data = dataReader.GetString(4)
                            };

                            chatList.Add(chatInfo);
                        }

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SUCCESS,
                            message = string.Empty,
                            data = JsonConvert.SerializeObject(chatList)
                        };
                    }
                    else
                    {
                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SEARCH_NOTEXIST_CHAT,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SEARCH_NOTEXIST_CHAT),
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

            //ClassLog._mLogger.Info(String.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
            ClassLog._mLogger.InfoFormat("{0}  RESPONSE DATA  [데이타 전송완료]" + Environment.NewLine, sModuleName);
            return response;
            #endregion
        }

        public ClassResponse.RES_RESULT GetProfileImage(ClassRequest.REQ_UID parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Basic", "profile_get");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetProfileImage  ==========", sModuleName);
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

            try
            {
                string sQuery = string.Format("SELECT PROFILE_URL " +
                           "  FROM USER_INFO " +
                           " WHERE FLAG = 'Y' " +
                           "   AND USER_ID = '{0}' ", parameters.uid);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        dataReader.Read();
                        string sURL = _mClassDatabase.GetSafeString(dataReader, 0);

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SUCCESS,
                            message = string.Empty,
                            data = sURL
                        };
                    }
                    else
                    {
                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SUCCESS,
                            message = string.Empty,
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

        public ClassResponse.RES_RESULT GetWorkerList(ClassRequest.REQ_WORKERLIST parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Basic", "worker_list");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetWorkerList  ==========", sModuleName);
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

                if (parameters.dept_no < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_DEPT_NO,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_DEPT_NO),
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

            List<ClassStruct.ST_WORKERINFO> workerList = new List<ClassStruct.ST_WORKERINFO>();

            try
            {
                string sQuery = string.Format("SELECT SEQ, WORKER_NAME " +
                                       "  FROM WORKER_INFO" +
                                       " WHERE DEPT_NO = {0} " +
                                       "   AND FLAG = 'Y' ", parameters.dept_no);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            ClassStruct.ST_WORKERINFO workerInfo = new ClassStruct.ST_WORKERINFO
                            {
                                worker_seq = dataReader.GetInt32(0),
                                worker_name = dataReader.GetString(1)
                            };

                            workerList.Add(workerInfo);
                        }

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SUCCESS,
                            message = string.Empty,
                            data = JsonConvert.SerializeObject(workerList)
                        };
                    }
                    else
                    {
                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SEARCH_NOTEXIST_WORKER,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SEARCH_NOTEXIST_WORKER),
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

        public ClassResponse.RES_RESULT CheckExistNewNotice(ClassRequest.REQ_NOTICE_INFO parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Basic", "notice_check");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START CheckExistNewNotice  ==========", sModuleName);
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

            try
            {
                string sQuery = string.Format("SELECT TOP 1 SEQ " +
                           "  FROM NOTICE_USER WITH (INDEX = IDX_USER_ID) " +
                           " WHERE USER_ID = '{0}' " +
                           "   AND READ_FLAG = 'N' " +
                           "   AND (FARM_SEQ = 0 OR FARM_SEQ = {1}) ", parameters.uid, parameters.farm_seq);
                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    string sNewFlag = dataReader.HasRows ? "Y" : "N";

                    dataReader.Close();
                    dataReader.Dispose();

                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SUCCESS,
                        message = string.Empty,
                        data = sNewFlag
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

        public ClassResponse.RES_RESULT GetNoticeList(ClassRequest.REQ_NOTICE_INFO parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Basic", "notice_list");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetNoticeList  ==========", sModuleName);
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

            List<ClassStruct.ST_NOTICE_INFO> noticeList = new List<ClassStruct.ST_NOTICE_INFO>();

            try
            {
                string sQuery = string.Format("SELECT A.NOTICE_SEQ, A.READ_FLAG, B.NOTICE_DATE, B.NOTICE_TITLE, B.NOTICE_CONTENT " +
                                       "  FROM NOTICE_USER A " +
                                       "  LEFT OUTER JOIN NOTICE_INFO B " +
                                       "    ON A.NOTICE_SEQ = B.SEQ " +
                                       " WHERE A.USER_ID = '{0}' " +
                                       "   AND (A.FARM_SEQ = 0 OR A.FARM_SEQ = {1}) " +
                                       "   AND B.FLAG = 'Y' " +
                                       " ORDER BY B.NOTICE_DATE DESC ", parameters.uid, parameters.farm_seq);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            ClassStruct.ST_NOTICE_INFO noticeInfo = new ClassStruct.ST_NOTICE_INFO
                            {
                                notice_seq = dataReader.GetInt32(0),
                                notice_date = dataReader.GetDateTime(2).ToString("yyyy-MM-dd HH:mm:ss"),
                                title = dataReader.GetString(3),
                                contents = dataReader.GetString(4),
                                new_flag = dataReader.GetString(1) == "N" ? "Y" : "N"
                            };

                            noticeList.Add(noticeInfo);
                        }

                        dataReader.Close();
                        dataReader.Dispose();

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SUCCESS,
                            message = string.Empty,
                            data = JsonConvert.SerializeObject(noticeList)
                        };
                    }
                    else
                    {
                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SEARCH_NOTEXIST_NOTICE,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SEARCH_NOTEXIST_NOTICE),
                            data = string.Empty
                        };
                    }
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

        public ClassResponse.RES_RESULT GetNoticeDetailInfo(ClassRequest.REQ_NOTICE_DETAIL parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Basic", "notice_info");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetNoticeDetailInfo  ==========", sModuleName);
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

                if (parameters.notice_seq < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_NOTICE_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_NOTICE_SEQ),
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
            OleDbDataReader dataReader = null;

            ClassStruct.ST_NOTICE_DETAIL noticeInfo = new ClassStruct.ST_NOTICE_DETAIL();

            try
            {
                string sQuery = string.Format("SELECT SEQ, NOTICE_DATE, NOTICE_TITLE, NOTICE_CONTENT " +
                                       "  FROM NOTICE_INFO " +
                                       " WHERE SEQ = {0} ", parameters.notice_seq);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        dataReader.Read();

                        noticeInfo.notice_seq = dataReader.GetInt32(0);
                        noticeInfo.notice_date = dataReader.GetDateTime(1).ToString("yyyy-MM-dd HH:mm:ss");
                        noticeInfo.title = _mClassDatabase.GetSafeString(dataReader, 2);
                        noticeInfo.contents = dataReader.GetString(3);
                        
                        dataReader.Close();
                        dataReader.Dispose();
                    }
                    else
                    {
                        isError = true;

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SEARCH_NOTEXIST_NOTICE,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SEARCH_NOTEXIST_NOTICE),
                            data = string.Empty
                        };
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

                // 공지사항 읽은 처리를 한다
                if (!isError)
                {
                    sQuery = string.Format("UPDATE NOTICE_USER SET READ_FLAG = 'Y' " +
                                           " WHERE NOTICE_SEQ = {0} " +
                                           "   AND USER_ID = '{1}' ", parameters.notice_seq, parameters.uid);

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

                // 결과 전송
                if (!isError)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SUCCESS,
                        message = string.Empty,
                        data = JsonConvert.SerializeObject(noticeInfo)
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

        public ClassResponse.RES_RESULT SetUserPushSetting(ClassRequest.REQ_PUSH_REGIST parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Basic", "push_regist");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START SetUserPushSetting  ==========", sModuleName);
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

                if (string.IsNullOrEmpty(parameters.ha_flag))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_HA_FLAG,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_HA_FLAG),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (string.IsNullOrEmpty(parameters.la_flag))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_LA_FLAG,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_LA_FLAG),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (string.IsNullOrEmpty(parameters.aa_flag))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_AA_FLAG,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_AA_FLAG),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (string.IsNullOrEmpty(parameters.an_flag))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_AN_FLAG,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_AN_FLAG),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (string.IsNullOrEmpty(parameters.kn_flag))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_KN_FLAG,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_KN_FLAG),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                } 
                
                if (string.IsNullOrEmpty(parameters.id_flag))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_ID_FLAG,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_ID_FLAG),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (string.IsNullOrEmpty(parameters.ad_flag))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_AD_FLAG,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_AD_FLAG),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (string.IsNullOrEmpty(parameters.cd_flag))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_CD_FLAG,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_CD_FLAG),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (string.IsNullOrEmpty(parameters.dd_flag))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_DD_FLAG,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_DD_FLAG),
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
                string sQuery = string.Format("SELECT HA_FLAG, LA_FLAG, AI_FLAG, AN_FLAG, KN_FLAG, ID_FLAG, AD_FLAG, CD_FLAG, DD_FLAG  " +
                                       "  FROM USER_PUSH " +
                                       " WHERE USER_ID = '{0}' ", parameters.uid);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    isExist = dataReader.HasRows;
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

                // 없으면 새로 등록하고 있으면 수정한다
                if (!isError)
                {
                    if (isExist)
                    {
                        sQuery = string.Format("UPDATE USER_PUSH " +
                                               "   SET HA_FLAG = '{1}', " +
                                               "       LA_FLAG = '{2}', " +
                                               "       AI_FLAG = '{3}', " +
                                               "       AN_FLAG = '{4}', " +
                                               "       KN_FLAG = '{5}', " +
                                               "       ID_FLAG = '{6}', " +
                                               "       AD_FLAG = '{7}', " +
                                               "       CD_FLAG = '{8}', " +
                                               "       DD_FLAG = '{9}' " +
                                               " WHERE USER_ID = '{0}' ",
                                               parameters.uid, parameters.ha_flag, parameters.la_flag, parameters.aa_flag, parameters.an_flag, parameters.kn_flag,
                                               parameters.id_flag, parameters.ad_flag, parameters.cd_flag, parameters.dd_flag);
                    }
                    else
                    {
                        sQuery = string.Format("INSERT INTO USER_PUSH (USER_ID, HA_FLAG, LA_FALG, AI_FLAG, AN_FLAG, KN_FLAG, ID_FLAG, AD_FLAG, CD_FLAG, DD_FLAG) " +
                                               "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}') ",
                                               parameters.uid, parameters.ha_flag, parameters.la_flag, parameters.aa_flag, parameters.an_flag, parameters.kn_flag,
                                               parameters.id_flag, parameters.ad_flag, parameters.cd_flag, parameters.dd_flag);
                    }

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

        public ClassResponse.RES_RESULT GetUserPushSetting(ClassRequest.REQ_UID parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Basic", "push_regist");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetUserPushSetting  ==========", sModuleName);
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

            ClassStruct.ST_USER_PUSH userPush = new ClassStruct.ST_USER_PUSH();

            try
            {
                string sQuery = string.Format("SELECT HA_FLAG, LA_FLAG, AI_FLAG, AN_FLAG, KN_FLAG, ID_FLAG, AD_FLAG, CD_FLAG, DD_FLAG " +
                                       "  FROM USER_PUSH " +
                                       " WHERE USER_ID = '{0}' ", parameters.uid);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        dataReader.Read();

                        userPush.ha_flag = dataReader.GetString(0);
                        userPush.la_flag = dataReader.GetString(1);
                        userPush.aa_flag = dataReader.GetString(2);
                        userPush.an_flag = dataReader.GetString(3);
                        userPush.kn_flag = dataReader.GetString(4);
                        userPush.id_flag = dataReader.GetString(5);
                        userPush.ad_flag = dataReader.GetString(6);
                        userPush.cd_flag = dataReader.GetString(7);
                        userPush.dd_flag = dataReader.GetString(8);

                        dataReader.Close();
                        dataReader.Dispose();

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SUCCESS,
                            message = string.Empty,
                            data = JsonConvert.SerializeObject(userPush)
                        };
                    }
                    else
                    {
                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SEARCH_NOTEXIST_PUSH_SETTING,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SEARCH_NOTEXIST_PUSH_SETTING),
                            data = string.Empty
                        };
                    }
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

        public ClassResponse.RES_RESULT GetUserAlarmList(ClassRequest.REQ_FARMSEQ parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Basic", "useralarm_list");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetUserAlarmList  ==========", sModuleName);
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

            List<ClassStruct.ST_USER_ALARM> alarmList = new List<ClassStruct.ST_USER_ALARM>();

            try
            {
                // 사용자정의 알람내역을 추출한다
                string sQuery = string.Format("SELECT SEQ, ALARM_TITLE, BASE_DATE, BASE_TYPE, ALARM_DAY " +
                                       "  FROM USER_ALARM " +
                                       " WHERE FARM_SEQ = {0} " +
                                       "   AND FLAG = 'Y' ", parameters.farm_seq);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            ClassStruct.ST_USER_ALARM alarmInfo = new ClassStruct.ST_USER_ALARM
                            {
                                alarm_seq = dataReader.GetInt32(0),
                                alarm_title = dataReader.GetString(1),
                                base_date = dataReader.GetString(2),
                                base_type = dataReader.GetString(3),
                                alarm_day = dataReader.GetInt32(4)
                            };

                            alarmList.Add(alarmInfo);
                        }

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SUCCESS,
                            message = string.Empty,
                            data = JsonConvert.SerializeObject(alarmList)
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

        public ClassResponse.RES_RESULT SetUserAlarmInsert(ClassRequest.REQ_USER_ALARM_INSERT parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Basic", "useralarm_insert");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START SetUserAlarmInsert  ==========", sModuleName);
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

                if (string.IsNullOrEmpty(parameters.alarm_title))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_ALARM_TITLE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_ALARM_TITLE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (string.IsNullOrEmpty(parameters.base_date))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_BASE_DATE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_BASE_DATE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (string.IsNullOrEmpty(parameters.base_type))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_BASE_TYPE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_BASE_TYPE),
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
                string sQuery = string.Format("INSERT INTO USER_ALARM (FARM_SEQ, ALARM_TITLE, BASE_DATE, BASE_TYPE, ALARM_DAY) " +
                                       "VALUES ({0}, N'{1}', '{2}', '{3}', {4})",
                                       parameters.farm_seq, parameters.alarm_title, parameters.base_date, parameters.base_type, parameters.alarm_day);
                
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

        public ClassResponse.RES_RESULT SetUserAlarmUpdate(ClassRequest.REQ_USER_ALARM_UPDATE parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Basic", "useralarm_update");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START SetUserAlarmUpdate  ==========", sModuleName);
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

                if (parameters.alarm_seq < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_ALARM_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_ALARM_SEQ),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (string.IsNullOrEmpty(parameters.alarm_title))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_ALARM_TITLE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_ALARM_TITLE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (string.IsNullOrEmpty(parameters.base_date))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_BASE_DATE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_BASE_DATE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (string.IsNullOrEmpty(parameters.base_type))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_BASE_TYPE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_BASE_TYPE),
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
                string sQuery = string.Format("UPDATE USER_ALARM " +
                                       "   SET ALARM_TITLE = N'{2}', " +
                                       "       BASE_DATE = '{3}', " +
                                       "       BASE_TYPE = '{4}', " +
                                       "       ALARM_DAY = {5} " +
                                       " WHERE SEQ = {0} " +
                                       "   AND FARM_SEQ = {1} ",
                                       parameters.alarm_seq, parameters.farm_seq, parameters.alarm_title, parameters.base_date, parameters.base_type, parameters.alarm_day);

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

        public ClassResponse.RES_RESULT SetUserAlarmDelete(ClassRequest.REQ_USER_ALARM_DELETE parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Basic", "useralarm_update");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START SetUserAlarmUpdate  ==========", sModuleName);
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

                if (parameters.alarm_seq < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_ALARM_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_ALARM_SEQ),
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
                string sQuery = string.Format("UPDATE USER_ALARM " +
                                       "   SET FLAG = 'N' " +
                                       " WHERE SEQ = {0} " +
                                       "   AND FARM_SEQ = {1} ", parameters.alarm_seq, parameters.farm_seq);

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
    }
}
