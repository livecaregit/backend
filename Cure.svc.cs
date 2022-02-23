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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Cure" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Cure.svc or Cure.svc.cs at the Solution Explorer and start debugging.
    public class Cure : ICure
    {
        private ClassOLEDB _mClassDatabase = new ClassOLEDB();
        private readonly ClassError _mClassError = new ClassError();
        private readonly ClassFunction _mClassFunction = new ClassFunction();

        ~Cure()
        {
            if (_mClassDatabase.GetConnectionState()) _mClassDatabase.CloseDatabase();
            _mClassDatabase = null;
        }

        public ClassResponse.RES_RESULT GetDiagnoisList(ClassRequest.REQ_FARMSEQ parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Cure", "diagnosis_list");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetFarmDiagnoisList  ==========", sModuleName);
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

            List<ClassStruct.ST_DIAGNOSIS_INFO> diagnosisList = new List<ClassStruct.ST_DIAGNOSIS_INFO>();

            try
            {
                string sQuery = string.Format("SELECT SEQ, DIAGNOSIS_NAME " +
                           "  FROM FARM_DIAGNOSIS " +
                           " WHERE FLAG = 'Y' " +
                           "   AND FARM_SEQ = {0} ", parameters.farm_seq);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            ClassStruct.ST_DIAGNOSIS_INFO diagnosisInfo = new ClassStruct.ST_DIAGNOSIS_INFO
                            {
                                diagnosis_seq = dataReader.GetInt32(0),
                                diagnosis_name = dataReader.GetString(1)
                            };

                            diagnosisList.Add(diagnosisInfo);
                        }

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SUCCESS,
                            message = string.Empty,
                            data = JsonConvert.SerializeObject(diagnosisList)
                        };
                    }
                    else
                    {
                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SEARCH_NOTEXIST_DIAGNOSIS,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SEARCH_NOTEXIST_DIAGNOSIS),
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

        public ClassResponse.RES_RESULT SetDiagnosisInsert(ClassRequest.REQ_DIAGNOSIS_INFO parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Cure", "diagnosis_insert");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START SetFarmDiagnosisInsert  ==========", sModuleName);
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

                if (string.IsNullOrEmpty(parameters.diagnosis_name))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_DIAGNOSIS_NAME,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_DIAGNOSIS_NAME),
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
                // 농장의 UTC Time을 추출한다
                DateTime utcTime = DateTime.UtcNow;
                string sUtcTime = string.Empty;

                _mClassFunction.GetFarmTimeDifference(parameters.farm_seq, out int nDiffHour, out int nDiffMinute);

                utcTime = utcTime.AddHours(nDiffHour).AddMinutes(nDiffMinute);
                sUtcTime = utcTime.ToString("yyyy-MM-dd HH:mm:ss");

                string sName = parameters.diagnosis_name;
                sName = sName.Replace("'", "''");
                sName = sName.Replace(@"\", @"\\");

                string sQuery = string.Format("INSERT INTO FARM_DIAGNOSIS (FARM_SEQ, DIAGNOSIS_NAME, CREATE_DATE) VALUES ({0}, N'{1}', '{2}') ", parameters.farm_seq, sName, sUtcTime);
                int count = _mClassDatabase.QueryExecute(sQuery);

                if (count < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
                        data = string.Empty
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

        public ClassResponse.RES_RESULT SetDiagnosisUpdate(ClassRequest.REQ_DIAGNOSIS_INFO parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Cure", "diagnosis_update");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START SetFarmDiagnosisUpdate  ==========", sModuleName);
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

                if (parameters.diagnosis_seq < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_DIAGNOSIS_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_DIAGNOSIS_SEQ),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (string.IsNullOrEmpty(parameters.diagnosis_name))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_DIAGNOSIS_NAME,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_DIAGNOSIS_NAME),
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
                string sName = parameters.diagnosis_name;
                sName = sName.Replace("'", "''");
                sName = sName.Replace(@"\", @"\\");

                string sQuery = string.Format("UPDATE FARM_DIAGNOSIS SET DIAGNOSIS_NAME = N'{1}' WHERE SEQ = {0} ", parameters.diagnosis_seq, sName);
                int count = _mClassDatabase.QueryExecute(sQuery);

                if (count < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
                        data = string.Empty
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

        public ClassResponse.RES_RESULT SetDiagnosisDelete(ClassRequest.REQ_DIAGNOSIS_INFO parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Cure", "diagnosis_delete");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START SetFarmDiagnosisDelete  ==========", sModuleName);
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

                if (parameters.diagnosis_seq < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_DIAGNOSIS_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_DIAGNOSIS_SEQ),
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
                string sQuery = string.Format("UPDATE FARM_DIAGNOSIS SET FLAG = 'N' WHERE SEQ = {0} ", parameters.diagnosis_seq);
                int count = _mClassDatabase.QueryExecute(sQuery);

                if (count < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
                        data = string.Empty
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

        public ClassResponse.RES_RESULT GetPrescriptionList(ClassRequest.REQ_FARMSEQ parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Cure", "prescription_list");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetFarmPrescriptionList  ==========", sModuleName);
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

            List<ClassStruct.ST_PRESCRIPTION_INFO> prescriptionList = new List<ClassStruct.ST_PRESCRIPTION_INFO>();

            try
            {
                string sQuery = string.Format("SELECT SEQ, PRESCRIPTION_TYPE, PRESCRIPTION_NAME, INGREDIENT " +
                                       "  FROM FARM_PRESCRIPTION " +
                                       " WHERE FLAG = 'Y' " +
                                       "   AND FARM_SEQ = {0} ", parameters.farm_seq);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            ClassStruct.ST_PRESCRIPTION_INFO prescriptionInfo = new ClassStruct.ST_PRESCRIPTION_INFO
                            {
                                prescription_seq = dataReader.GetInt32(0),
                                prescription_type = _mClassDatabase.GetSafeString(dataReader, 1),
                                prescription_name = dataReader.GetString(2),
                                ingredient = _mClassDatabase.GetSafeString(dataReader, 3)
                            };

                            prescriptionList.Add(prescriptionInfo);
                        }

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SUCCESS,
                            message = string.Empty,
                            data = JsonConvert.SerializeObject(prescriptionList)
                        };
                    }
                    else
                    {
                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SEARCH_NOTEXIST_PRESCRIPTION,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SEARCH_NOTEXIST_PRESCRIPTION),
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

        public ClassResponse.RES_RESULT SetPrescriptionInsert(ClassRequest.REQ_PRESCRIPTION_INFO parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Cure", "prescription_insert");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START SetFarmPrescriptionInsert  ==========", sModuleName);
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

                if (string.IsNullOrEmpty(parameters.prescription_name))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_PRESCRIPTION_NAME,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_PRESCRIPTION_NAME),
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
                // 농장의 UTC Time을 추출한다
                DateTime utcTime = DateTime.UtcNow;
                string sUtcTime = string.Empty;

                _mClassFunction.GetFarmTimeDifference(parameters.farm_seq, out int nDiffHour, out int nDiffMinute);

                utcTime = utcTime.AddHours(nDiffHour).AddMinutes(nDiffMinute);
                sUtcTime = utcTime.ToString("yyyy-MM-dd HH:mm:ss");

                string sPrescriptionType = string.Empty;
                string sPrescriptionName = string.Empty;
                string sIngredient = string.Empty;

                if (!string.IsNullOrEmpty(parameters.prescription_type))
                {
                    sPrescriptionType = parameters.prescription_type;
                    sPrescriptionType = sPrescriptionType.Replace("'", "''");
                    sPrescriptionType = sPrescriptionType.Replace(@"\", @"\\");
                }

                sPrescriptionName = parameters.prescription_name;
                sPrescriptionName = sPrescriptionName.Replace("'", "''");
                sPrescriptionName = sPrescriptionName.Replace(@"\", @"\\");

                if (!string.IsNullOrEmpty(parameters.ingredient))
                {
                    sIngredient = parameters.ingredient;
                    sIngredient = sIngredient.Replace("'", "''");
                    sIngredient = sIngredient.Replace(@"\", @"\\");
                }

                string sQuery = "INSERT INTO FARM_PRESCRIPTION (FARM_SEQ, PRESCRIPTION_TYPE, PRESCRIPTION_NAME, INGREDIENT, CREATE_DATE) VALUES ( ";
                sQuery += string.Format("{0}, ", parameters.farm_seq);
                if (string.IsNullOrEmpty(sPrescriptionType)) sQuery += "NULL, ";
                else sQuery += string.Format("N'{0}', ", sPrescriptionType);
                sQuery += string.Format("N'{0}', ", sPrescriptionName);
                if (string.IsNullOrEmpty(sIngredient)) sQuery += "NULL, ";
                else sQuery += string.Format("N'{0}', ", sIngredient);
                sQuery += "'" + sUtcTime + "') ";

                int count = _mClassDatabase.QueryExecute(sQuery);

                if (count < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
                        data = string.Empty
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

        public ClassResponse.RES_RESULT SetPrescriptionUpdate(ClassRequest.REQ_PRESCRIPTION_INFO parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Cure", "prescription_update");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START SetFarmPrescriptionUpdate  ==========", sModuleName);
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

                if (parameters.prescription_seq < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_PRESCRIPTION_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_PRESCRIPTION_SEQ),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (string.IsNullOrEmpty(parameters.prescription_name))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_PRESCRIPTION_NAME,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_PRESCRIPTION_NAME),
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
                string sPrescriptionType = string.Empty;
                string sPrescriptionName = string.Empty;
                string sIngredient = string.Empty;

                if (!string.IsNullOrEmpty(parameters.prescription_type))
                {
                    sPrescriptionType = parameters.prescription_type;
                    sPrescriptionType = sPrescriptionType.Replace("'", "''");
                    sPrescriptionType = sPrescriptionType.Replace(@"\", @"\\");

                }

                sPrescriptionName = parameters.prescription_name;
                sPrescriptionName = sPrescriptionName.Replace("'", "''");
                sPrescriptionName = sPrescriptionName.Replace(@"\", @"\\");

                if (!string.IsNullOrEmpty(parameters.ingredient))
                {
                    sIngredient = parameters.ingredient;
                    sIngredient = sIngredient.Replace("'", "''");
                    sIngredient = sIngredient.Replace(@"\", @"\\");
                }

                string sQuery = "UPDATE FARM_PRESCRIPTION SET ";
                if (string.IsNullOrEmpty(sPrescriptionType)) sQuery += "PRESCRIPTION_TYPE = NULL, ";
                else sQuery += string.Format("PRESCRIPTION_TYPE = N'{0}', ", sPrescriptionType);
                sQuery += string.Format("PRESCRIPTION_NAME = N'{0}', ", sPrescriptionName);
                if (string.IsNullOrEmpty(sIngredient)) sQuery += "INGREDIENT = NULL ";
                else sQuery += string.Format("INGREDIENT = N'{0}' ", sIngredient);
                sQuery += string.Format(" WHERE SEQ = {0} ", parameters.prescription_seq);

                int count = _mClassDatabase.QueryExecute(sQuery);

                if (count < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
                        data = string.Empty
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

        public ClassResponse.RES_RESULT SetPrescriptionDelete(ClassRequest.REQ_PRESCRIPTION_INFO parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Cure", "prescription_delete");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START SetFarmPrescriptionDelete  ==========", sModuleName);
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

                if (parameters.prescription_seq < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_PRESCRIPTION_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_PRESCRIPTION_SEQ),
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
                string sQuery = string.Format("UPDATE FARM_PRESCRIPTION SET FLAG = 'N' WHERE SEQ = {0} ", parameters.prescription_seq);
                int count = _mClassDatabase.QueryExecute(sQuery);

                if (count < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_SYSTEM_ERROR_DATABASE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SYSTEM_ERROR_DATABASE),
                        data = string.Empty
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

        public ClassResponse.RES_RESULT GetCureList(ClassRequest.REQ_FARMENTITY parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Cure", "cure_list");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetCureList  ==========", sModuleName);
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

            List<ClassStruct.ST_CURE_LIST> cureList = new List<ClassStruct.ST_CURE_LIST>();

            try
            {
                // 개체정보의 체크
                string sQuery = string.Format("SELECT SEQ FROM ENTITY_NEW_INFO " +
                                       " WHERE SEQ = {0} AND FARM_SEQ = {1} AND FLAG = 'Y' AND ACTIVE_FLAG = 'Y' ", parameters.entity_seq, parameters.farm_seq);
                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (!dataReader.HasRows)
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


                // 치료이력 추출
                if (!isError)
                {
                    sQuery = string.Format("SELECT A.SEQ, A.FARM_SEQ, A.ENTITY_SEQ, A.DIAGNOSIS_SEQ, A.DIAGNOSIS_DATE, " +
                                           "       B.DIAGNOSIS_NAME, C.PRESCRIPTION_DATE, C.PRESCRIPTION_NAME, ISNULL(D.PRESCRIPTION_COUNT, 0) " +
                                           "  FROM CURE_HISTORY A " +
                                           "  LEFT OUTER JOIN FARM_DIAGNOSIS B " +
                                           "    ON A.DIAGNOSIS_SEQ = B.SEQ " +
                                           "   AND A.FARM_SEQ = B.FARM_SEQ " +
                                           "   AND B.FLAG = 'Y' " +
                                           "  LEFT OUTER JOIN (SELECT SEQ, FARM_SEQ, ENTITY_SEQ, CURE_SEQ, PRESCRIPTION_DATE, PRESCRIPTION_SEQ, MEMO, PRESCRIPTION_NAME " +
                                           "                     FROM (SELECT ROW_NUMBER() OVER (PARTITION BY CURE_SEQ ORDER BY A.SEQ) AS ROW_NUM,  " +
                                           "                                  A.SEQ, A.FARM_SEQ, A.ENTITY_SEQ, A.CURE_SEQ, A.PRESCRIPTION_DATE, A.PRESCRIPTION_SEQ, A.MEMO, " +
                                           "                   	           B.PRESCRIPTION_NAME " +
                                           "                             FROM CURE_PRESCRIPTION A " +
                                           "                             LEFT OUTER JOIN FARM_PRESCRIPTION B " +
                                           "                               ON A.PRESCRIPTION_SEQ = B.SEQ " +
                                           "                              AND A.FARM_SEQ = B.FARM_SEQ " +
                                           "                              AND B.FLAG = 'Y' " +
                                           "                            WHERE A.FLAG = 'Y' " +
                                           "                              AND A.FARM_SEQ = {0} " +
                                           "                              AND A.ENTITY_SEQ = {1}) RESULT " +
                                           "                    WHERE ROW_NUM = 1) C " +
                                           "    ON A.FARM_SEQ = C.FARM_SEQ " +
                                           "   AND A.ENTITY_SEQ = C.ENTITY_SEQ " +
                                           "   AND A.SEQ = C.CURE_SEQ " +
                                           "  LEFT OUTER JOIN (SELECT FARM_SEQ, ENTITY_SEQ, CURE_SEQ, ISNULL(COUNT(SEQ), 0) AS PRESCRIPTION_COUNT " +
                                           "                     FROM CURE_PRESCRIPTION " +
                                           "                    WHERE FLAG = 'Y' " +
                                           "                      AND FARM_SEQ = {0} " +
                                           "                      AND ENTITY_SEQ = {1} " +
                                           "                    GROUP BY FARM_SEQ, ENTITY_SEQ, CURE_SEQ) D " +
                                           "    ON A.FARM_SEQ = D.FARM_SEQ " +
                                           "   AND A.ENTITY_SEQ = D.ENTITY_SEQ " +
                                           "   AND A.SEQ = D.CURE_SEQ " +
                                           " WHERE A.FLAG = 'Y' " +
                                           "   AND A.FARM_SEQ = {0} " +
                                           "   AND A.ENTITY_SEQ = {1} " +
                                           " ORDER BY A.DIAGNOSIS_DATE DESC ", parameters.farm_seq, parameters.entity_seq);

                    if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                    {
                        if (dataReader.HasRows)
                        {
                            string sText = string.Empty;

                            switch (parameters.lang_code)
                            {
                                case "KR": sText = "외"; break;
                                case "JP": sText = "외"; break;
                                case "US": sText = "et"; break;
                                case "CN": sText = "외"; break;
                                default: sText = "외"; break;
                            }

                            while (dataReader.Read())
                            {
                                ClassStruct.ST_CURE_LIST cureInfo = new ClassStruct.ST_CURE_LIST
                                {
                                    seq = dataReader.GetInt32(0),
                                    diagnosis_seq = dataReader.GetInt32(3),
                                    diagnosis_name = _mClassDatabase.GetSafeString(dataReader, 5),
                                    diagnosis_date = dataReader.GetDateTime(4).ToString("yyyy-MM-dd"),
                                    prescription_date = _mClassDatabase.GetSafeDateTime(dataReader, 6)
                                };

                                int nCount = _mClassDatabase.GetSafeInteger(dataReader, 8);
                                string sName = _mClassDatabase.GetSafeString(dataReader, 7);

                                switch (nCount)
                                {
                                    case 0: cureInfo.prescription_name = string.Empty; break;
                                    case 1: cureInfo.prescription_name = sName; break;
                                    default: if (nCount > 1) cureInfo.prescription_name = string.Format("{0} ({1})", sName, nCount); break;
                                }

                                cureList.Add(cureInfo);
                            }

                            response = new ClassResponse.RES_RESULT
                            {
                                result = ClassError.RESULT_SUCCESS,
                                message = string.Empty,
                                data = JsonConvert.SerializeObject(cureList)
                            };
                        }
                        else
                        {
                            isError = true;

                            response = new ClassResponse.RES_RESULT
                            {
                                result = ClassError.RESULT_SEARCH_NOTEXIST_CURE,
                                message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SEARCH_NOTEXIST_CURE),
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

        public ClassResponse.RES_RESULT GetCureInfo(ClassRequest.REQ_CURE_INFO parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Cure", "cure_info");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START GetCureInfo  ==========", sModuleName);
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

                if (parameters.cure_seq < 0)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_CURE_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_CURE_SEQ),
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

            ClassStruct.ST_CURE_INFO cureInfo = new ClassStruct.ST_CURE_INFO();

            try
            {
                // 개체정보의 체크
                string sQuery = string.Format("SELECT SEQ FROM ENTITY_NEW_INFO " +
                                       " WHERE SEQ = {0} AND FARM_SEQ = {1} AND FLAG = 'Y' AND ACTIVE_FLAG = 'Y' ", parameters.entity_seq, parameters.farm_seq);
                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (!dataReader.HasRows)
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

                // 치료정보 추출
                if (!isError)
                {
                    sQuery = string.Format("SELECT A.SEQ, A.FARM_SEQ, A.ENTITY_SEQ, A.DIAGNOSIS_SEQ, A.DIAGNOSIS_DATE, B.DIAGNOSIS_NAME " +
                                           "  FROM CURE_HISTORY A " +
                                           "  LEFT OUTER JOIN FARM_DIAGNOSIS B " +
                                           "    ON A.DIAGNOSIS_SEQ = B.SEQ " +
                                           "   AND A.FARM_SEQ = B.FARM_SEQ " +
                                           "   AND B.FLAG = 'Y' " +
                                           " WHERE A.FLAG = 'Y' " +
                                           "   AND A.SEQ = {0} " +
                                           "   AND A.FARM_SEQ = {1} " +
                                           "   AND A.ENTITY_SEQ = {2}", parameters.cure_seq, parameters.farm_seq, parameters.entity_seq);

                    if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                    {
                        if (dataReader.HasRows)
                        {
                            dataReader.Read();

                            cureInfo.cure_seq = dataReader.GetInt32(0);
                            cureInfo.diagnosis_seq = dataReader.GetInt32(3);
                            cureInfo.diagnosis_name = _mClassDatabase.GetSafeString(dataReader, 5);
                            cureInfo.diagnosis_date = dataReader.GetDateTime(4).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            isError = true;

                            response = new ClassResponse.RES_RESULT
                            {
                                result = ClassError.RESULT_SEARCH_NOTEXIST_CURE,
                                message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SEARCH_NOTEXIST_CURE),
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

                // 처방정보 추출
                if (!isError)
                {
                    List<ClassStruct.ST_CURE_PRESCRIPTION> prescriptionList = new List<ClassStruct.ST_CURE_PRESCRIPTION>();

                    sQuery = string.Format("SELECT A.SEQ, A.FARM_SEQ, A.ENTITY_SEQ, A.CURE_SEQ, A.PRESCRIPTION_DATE, A.PRESCRIPTION_SEQ, A.MEMO, B.PRESCRIPTION_NAME " +
                                           "  FROM CURE_PRESCRIPTION A " +
                                           "  LEFT OUTER JOIN FARM_PRESCRIPTION B " +
                                           "    ON A.PRESCRIPTION_SEQ = B.SEQ " +
                                           "   AND A.FARM_SEQ = B.FARM_SEQ " +
                                           "   AND B.FLAG = 'Y' " +
                                           " WHERE A.FLAG = 'Y' " +
                                           "   AND A.FARM_SEQ = {0} " +
                                           "   AND A.ENTITY_SEQ = {1} " +
                                           "   AND A.CURE_SEQ = {2} ", parameters.farm_seq, parameters.entity_seq, parameters.cure_seq);

                    if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                ClassStruct.ST_CURE_PRESCRIPTION prescription = new ClassStruct.ST_CURE_PRESCRIPTION
                                {
                                    seq = dataReader.GetInt32(0),
                                    prescription_date = dataReader.GetDateTime(4).ToString("yyyy-MM-dd HH:mm:ss"),
                                    prescription_seq = dataReader.GetInt32(5),
                                    prescription_name = _mClassDatabase.GetSafeString(dataReader, 7),
                                    memo = _mClassDatabase.GetSafeString(dataReader, 6)
                                };

                                prescriptionList.Add(prescription);
                            }

                            cureInfo.prescription_list = prescriptionList;
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

                // 이미지정보 추출
                if (!isError)
                {
                    List<ClassStruct.ST_CURE_IMAGE> imageList = new List<ClassStruct.ST_CURE_IMAGE>();

                    sQuery = string.Format("SELECT SEQ, FARM_SEQ, ENTITY_SEQ, CURE_SEQ, IMAGE_URL " +
                                           "  FROM CURE_IMAGE " +
                                           " WHERE FLAG = 'Y' " +
                                           "   AND FARM_SEQ = {0} " +
                                           "   AND ENTITY_SEQ = {1} " +
                                           "   AND CURE_SEQ = {2} ", parameters.farm_seq, parameters.entity_seq, parameters.cure_seq);

                    if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                ClassStruct.ST_CURE_IMAGE imageInfo = new ClassStruct.ST_CURE_IMAGE
                                {
                                    seq = dataReader.GetInt32(0),
                                    image_url = dataReader.GetString(4)
                                };

                                imageList.Add(imageInfo);
                            }

                            cureInfo.image_list= imageList;
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
                        data = JsonConvert.SerializeObject(cureInfo)
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

        public ClassResponse.RES_RESULT SetCureInsert(ClassRequest.REQ_CURE_INSERT parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Cure", "cure_insert");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START SetCureInsert  ==========", sModuleName);
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

                if (string.IsNullOrEmpty(parameters.diagnosis_date))
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_DIAGNOSIS_DATE,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_DIAGNOSIS_DATE),
                        data = string.Empty
                    };

                    ClassLog._mLogger.Info(string.Format("{0}  RESPONSE DATA  [{1}]", sModuleName, JsonConvert.SerializeObject(response)) + Environment.NewLine);
                    return response;
                }

                if (parameters.diagnosis_seq < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_DIAGNOSIS_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_DIAGNOSIS_SEQ),
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

                // 개체정보의 체크
                sQuery = string.Format("SELECT SEQ FROM ENTITY_NEW_INFO " +
                                       " WHERE SEQ = {0} AND FARM_SEQ = {1} AND FLAG = 'Y' AND ACTIVE_FLAG = 'Y' ", parameters.entity_seq, parameters.farm_seq);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (!dataReader.HasRows)
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

                    // 치료 History 저장
                    sQuery = string.Format("INSERT INTO CURE_HISTORY (FARM_SEQ, ENTITY_SEQ, DIAGNOSIS_SEQ, DIAGNOSIS_DATE, CREATE_DATE) OUTPUT INSERTED.SEQ VALUES ({0}, {1}, {2}, '{3}', '{4}') ",
                        parameters.farm_seq, parameters.entity_seq, parameters.diagnosis_seq, parameters.diagnosis_date, sUtcTime);

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
                }

                // 처방전 내역을 저장한다
                if (!isError && parameters.prescription_list.Count > 0)
                {
                    foreach (ClassStruct.ST_CURE_PRESCRIPTION_PROC prescription in parameters.prescription_list)
                    {
                        // 등록 시에 flag 값을 비교한다
                        if (prescription.flag != "C")
                        {
                            isError = true;
                            _mClassDatabase.TransRollback();

                            response = new ClassResponse.RES_RESULT
                            {
                                result = ClassError.RESULT_PARAM_ERROR_FLAG,
                                message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_FLAG),
                                data = string.Empty
                            };

                            break;
                        }

                        string sMemo = prescription.memo;
                        sMemo = sMemo.Replace("'", "''");
                        sMemo = sMemo.Replace(@"\", @"\\");

                        sQuery = "INSERT INTO CURE_PRESCRIPTION (FARM_SEQ, ENTITY_SEQ, CURE_SEQ, PRESCRIPTION_DATE, PRESCRIPTION_SEQ, MEMO, CREATE_DATE) VALUES (";
                        sQuery += string.Format("{0}, ", parameters.farm_seq);
                        sQuery += string.Format("{0}, ", parameters.entity_seq);
                        sQuery += string.Format("{0}, ", nSEQ);
                        sQuery += string.Format("'{0}', ", prescription.prescription_date);
                        sQuery += string.Format("{0}, ", prescription.prescription_seq);
                        if (string.IsNullOrEmpty(prescription.memo)) sQuery += "NULL, ";
                        else sQuery += string.Format("N'{0}', ", sMemo);
                        sQuery += "'" + sUtcTime + "') ";

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

                            break;
                        }
                    }
                }

                // 이미지 내역을 저장한다
                if (!isError && parameters.image_list.Count > 0)
                {
                    foreach (ClassStruct.ST_CURE_IMAGE_PROC imageInfo in parameters.image_list)
                    {
                        // 등록 시에 flag 값을 비교한다
                        if (imageInfo.flag != "C")
                        {
                            isError = true;
                            _mClassDatabase.TransRollback();

                            response = new ClassResponse.RES_RESULT
                            {
                                result = ClassError.RESULT_PARAM_ERROR_FLAG,
                                message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_FLAG),
                                data = string.Empty
                            };

                            break;
                        }

                        string sImageURL = string.Empty;
#if DEBUG
                        //sImageURL = String.Format("http://www.ulikekorea.com/LC_SERVICE/CureImage/{0}", imageInfo.image_name);
                        sImageURL = String.Format("http://www.livecareworld.com/LIVECARE/LC_SERVICE/CureImage/{0}", imageInfo.image_name);
#else
                        sImageURL = string.Format("http://www.livecareworld.com/LIVECARE/LC_SERVICE/CureImage/{0}", imageInfo.image_name);
#endif

                        sQuery = string.Format("INSERT INTO CURE_IMAGE (FARM_SEQ, ENTITY_SEQ, CURE_SEQ, IMAGE_URL, CREATE_DATE) VALUES ({0}, {1}, {2}, '{3}', '{4}')",
                            parameters.farm_seq, parameters.entity_seq, nSEQ, sImageURL, sUtcTime);

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

        public ClassResponse.RES_RESULT SetCureUpdate(ClassRequest.REQ_CURE_UPDATE parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Cure", "cure_update");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START SetCureUpdate  ==========", sModuleName);
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

                if (parameters.cure_seq < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_CURE_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_CURE_SEQ),
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

            try
            {
                // 농장의 UTC Time을 추출한다
                DateTime utcTime = DateTime.UtcNow;
                string sUtcTime = string.Empty;

                _mClassFunction.GetFarmTimeDifference(parameters.farm_seq, out int nDiffHour, out int nDiffMinute);

                utcTime = utcTime.AddHours(nDiffHour).AddMinutes(nDiffMinute);
                sUtcTime = utcTime.ToString("yyyy-MM-dd HH:mm:ss");

                // 개체정보의 체크
                sQuery = string.Format("SELECT SEQ FROM ENTITY_NEW_INFO " +
                                       " WHERE SEQ = {0} AND FARM_SEQ = {1} AND FLAG = 'Y' AND ACTIVE_FLAG = 'Y' ", parameters.entity_seq, parameters.farm_seq);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (!dataReader.HasRows)
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

                //  치료이력 정보 조회
                sQuery = string.Format("SELECT SEQ FROM CURE_HISTORY " +
                                       " WHERE SEQ = {0} AND FARM_SEQ = {1} AND ENTITY_SEQ = {2} AND FLAG = 'Y' ",
                                       parameters.cure_seq, parameters.farm_seq, parameters.entity_seq);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (!dataReader.HasRows)
                    {
                        isError = true;

                        response = new ClassResponse.RES_RESULT
                        {
                            result = ClassError.RESULT_SEARCH_NOTEXIST_CURE,
                            message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_SEARCH_NOTEXIST_CURE),
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

                    // 치료 History 수정 - 변경된 경우에만 수정한다
                    if (!string.IsNullOrEmpty(parameters.diagnosis_date) || parameters.diagnosis_seq > 0)
                    {
                        sQuery = "UPDATE CURE_HISTORY SET ";
                        if (parameters.diagnosis_seq > 0)
                        {
                            sQuery += string.Format("DIAGNOSIS_SEQ = {0} ", parameters.diagnosis_seq);
                            if (!string.IsNullOrEmpty(parameters.diagnosis_date)) sQuery += ", ";
                        }
                        if (!string.IsNullOrEmpty(parameters.diagnosis_date)) sQuery += string.Format("DIAGNOSIS_DATE = '{0}' ", parameters.diagnosis_date);
                        sQuery += string.Format(" WHERE SEQ = {0} AND FARM_SEQ = {1} AND ENTITY_SEQ = {2} ", parameters.cure_seq, parameters.farm_seq, parameters.entity_seq);

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

                // 처방전 내역 수정
                if (!isError && parameters.prescription_list.Count > 0)
                {
                    foreach (ClassStruct.ST_CURE_PRESCRIPTION_PROC prescription in parameters.prescription_list)
                    {
                        if (prescription.flag != "C" && prescription.flag != "U" && prescription.flag != "D")
                        {
                            isError = true;
                            _mClassDatabase.TransRollback();

                            response = new ClassResponse.RES_RESULT
                            {
                                result = ClassError.RESULT_PARAM_ERROR_FLAG,
                                message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_FLAG),
                                data = string.Empty
                            };

                            break;
                        }

                        // flag 값에 따라서 처리한다
                        string sMemo = prescription.memo;
                        sMemo = sMemo.Replace("'", "''");
                        sMemo = sMemo.Replace(@"\", @"\\");

                        switch (prescription.flag)
                        {
                            case "C":
                                {
                                    sQuery = "INSERT INTO CURE_PRESCRIPTION (FARM_SEQ, ENTITY_SEQ, CURE_SEQ, PRESCRIPTION_DATE, PRESCRIPTION_SEQ, MEMO, CREATE_DATE) VALUES (";
                                    sQuery += string.Format("{0}, ", parameters.farm_seq);
                                    sQuery += string.Format("{0}, ", parameters.entity_seq);
                                    sQuery += string.Format("{0}, ", parameters.cure_seq);
                                    sQuery += string.Format("'{0}', ", prescription.prescription_date);
                                    sQuery += string.Format("{0}, ", prescription.prescription_seq);
                                    if (string.IsNullOrEmpty(prescription.memo)) sQuery += "NULL, ";
                                    else sQuery += string.Format("N'{0}', ", sMemo);
                                    sQuery += "'" + sUtcTime + "') ";

                                    break;
                                }
                            case "U":
                                {
                                    sQuery = "UPDATE CURE_PRESCRIPTION SET ";
                                    sQuery += string.Format("PRESCRIPTION_DATE = '{0}', ", prescription.prescription_date);
                                    sQuery += string.Format("PRESCRIPTION_SEQ = {0}, ", prescription.prescription_seq);
                                    if (string.IsNullOrEmpty(prescription.memo)) sQuery += "MEMO = NULL ";
                                    else sQuery += string.Format("MEMO = N'{0}' ", sMemo);
                                    sQuery += string.Format(" WHERE SEQ = {0} AND FARM_SEQ = {1} AND ENTITY_SEQ = {2} AND CURE_SEQ = {3} ",
                                        prescription.seq, parameters.farm_seq, parameters.entity_seq, parameters.cure_seq);

                                    break;
                                }
                            case "D":
                                {
                                    sQuery = string.Format("UPDATE CURE_PRESCRIPTION SET FLAG = 'N' " +
                                                           " WHERE SEQ = {0} AND FARM_SEQ = {1} AND ENTITY_SEQ = {2} AND CURE_SEQ = {3} ",
                                                           prescription.seq, parameters.farm_seq, parameters.entity_seq, parameters.cure_seq);

                                    break;
                                }
                        }

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

                // 이미지 내역을 저장한다
                if (!isError && parameters.image_list.Count > 0)
                {
                    foreach (ClassStruct.ST_CURE_IMAGE_PROC imageInfo in parameters.image_list)
                    {
                        // 등록 시에 flag 값을 비교한다
                        if (imageInfo.flag != "C" && imageInfo.flag != "D")
                        {
                            isError = true;
                            _mClassDatabase.TransRollback();

                            response = new ClassResponse.RES_RESULT
                            {
                                result = ClassError.RESULT_PARAM_ERROR_FLAG,
                                message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_FLAG),
                                data = string.Empty
                            };

                            break;
                        }

                        string sImageURL = string.Empty;
#if DEBUG
                        //sImageURL = String.Format("http://www.ulikekorea.com/LC_SERVICE/CureImage/{0}", imageInfo.image_name);
                        sImageURL = String.Format("http://www.livecareworld.com/LIVECARE/LC_SERVICE/CureImage/{0}", imageInfo.image_name);
#else
                        sImageURL = string.Format("http://www.livecareworld.com/LIVECARE/LC_SERVICE/CureImage/{0}", imageInfo.image_name);
#endif
                        switch (imageInfo.flag)
                        {
                            case "C":
                                {
                                    sQuery = string.Format("INSERT INTO CURE_IMAGE (FARM_SEQ, ENTITY_SEQ, CURE_SEQ, IMAGE_URL, CREATE_DATE) VALUES ({0}, {1}, {2}, '{3}', '{4}')",
                                        parameters.farm_seq, parameters.entity_seq, parameters.cure_seq, sImageURL, sUtcTime);

                                    break;
                                }
                            case "D":
                                {
                                    sQuery = string.Format("UPDATE CURE_IMAGE SET FLAG = 'N' " +
                                                           " WHERE SEQ = {0} AND FARM_SEQ = {1} AND ENTITY_SEQ = {2} AND CURE_SEQ = {3} ",
                                                           imageInfo.seq, parameters.farm_seq, parameters.entity_seq, parameters.cure_seq);

                                    break;
                                }
                        }

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

        public ClassResponse.RES_RESULT SetCureDelete(ClassRequest.REQ_CURE_INFO parameters)
        {
            string sModuleName = string.Format("[{0}][{1}]", "Cure", "cure_delete");
            string requestData = JsonConvert.SerializeObject(parameters);

            ClassLog._mLogger.InfoFormat("{0}  ==========  START SetCureDelete  ==========", sModuleName);
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

                if (parameters.cure_seq < 1)
                {
                    response = new ClassResponse.RES_RESULT
                    {
                        result = ClassError.RESULT_PARAM_ERROR_CURE_SEQ,
                        message = _mClassError.GetErrorMessage(parameters.lang_code, ClassError.RESULT_PARAM_ERROR_CURE_SEQ),
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
            bool isExistPrescription = false;
            bool isExistImage = false;
            OleDbDataReader dataReader = null;

            try
            {
                // 개체정보의 체크
                string sQuery = string.Format("SELECT SEQ FROM ENTITY_NEW_INFO " +
                                       " WHERE SEQ = {0} AND FARM_SEQ = {1} AND FLAG = 'Y' AND ACTIVE_FLAG = 'Y' ", parameters.entity_seq, parameters.farm_seq);
                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    if (!dataReader.HasRows)
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

                // 치료의 처방 내역이 있는지 체크
                sQuery = string.Format("SELECT SEQ FROM CURE_PRESCRIPTION " +
                                       " WHERE FARM_SEQ = {0} AND ENTITY_SEQ = {1} AND CURE_SEQ = {2} AND FLAG = 'Y' ", parameters.farm_seq, parameters.entity_seq, parameters.cure_seq);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    isExistPrescription = dataReader.HasRows;

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

                // 치료의 이미지 내역이 있는지 체크
                sQuery = string.Format("SELECT SEQ FROM CURE_IMAGE " +
                                       " WHERE FARM_SEQ = {0} AND ENTITY_SEQ = {1} AND CURE_SEQ = {2} AND FLAG = 'Y' ", parameters.farm_seq, parameters.entity_seq, parameters.cure_seq);

                if (_mClassDatabase.QueryOpen(sQuery, ref dataReader))
                {
                    isExistImage = dataReader.HasRows;

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

                int count;
                // 치료 History 삭제
                if (!isError)
                {
                    _mClassDatabase.TransBegin();

                    sQuery = string.Format("UPDATE CURE_HISTORY SET FLAG = 'N' " +
                                           " WHERE SEQ = {0} AND FARM_SEQ = {1} AND ENTITY_SEQ = {2} ", parameters.cure_seq, parameters.farm_seq, parameters.entity_seq);
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

                // 처방전 내역 삭제
                if (!isError && isExistPrescription)
                {
                    sQuery = string.Format("UPDATE CURE_PRESCRIPTION SET FLAG = 'N' " +
                                           " WHERE FARM_SEQ = {0} AND ENTITY_SEQ = {1} AND CURE_SEQ = {2} ", parameters.farm_seq, parameters.entity_seq, parameters.cure_seq);
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

                // 이미지 내역을 저장한다
                if (!isError && isExistImage)
                {
                    sQuery = string.Format("UPDATE CURE_IMAGE SET FLAG = 'N' " +
                                           " WHERE FARM_SEQ = {0} AND ENTITY_SEQ = {1} AND CURE_SEQ = {2} ", parameters.farm_seq, parameters.entity_seq, parameters.cure_seq);
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
    }
}
