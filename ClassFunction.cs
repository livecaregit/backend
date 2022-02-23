using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace LC_Service
{
    public class ClassFunction
    {
        public bool CheckTableExist(int pSeq, out bool pExist)
        {
            pExist = false;
            ClassOLEDB database = new ClassOLEDB();

            bool isError = false;
            OleDbDataReader dataReader = null;

            try
            {
                if (!database.GetConnectionState())
                {
                    if (!database.OpenDatabase())
                    {
                        isError = true;
                    }
                }

                if (!isError)
                {

                    string sName = string.Format("TEMP_ENTITY_{0}", pSeq);
                    string sQuery = string.Format("SELECT TABLE_CATALOG " +
                                                  "  FROM INFORMATION_SCHEMA.TABLES " +
                                                  " WHERE TABLE_NAME = '{0}' " +
                                                  "   AND TABLE_TYPE = 'BASE TABLE'", sName);

                    if (database.QueryOpen(sQuery, ref dataReader))
                    {
                        pExist = dataReader.HasRows;

                        dataReader.Close();
                        dataReader.Dispose();
                    }
                    else
                    {
                        isError = true;
                    }
                }
            }
            catch
            {
                isError = true;
            }

            return !isError;
        }

        public bool CreateTable(int pSeq)
        {
            ClassOLEDB database = new ClassOLEDB();

            bool isError = false;

            try
            {
                if (!database.GetConnectionState())
                {
                    if (!database.OpenDatabase())
                    {
                        isError = true;
                    }
                }

                if (!isError)
                {
                    string sName = string.Format("TEMP_ENTITY_{0}", pSeq);
                    string sQuery = string.Empty;

#if DEBUG
                    sQuery = "USE [LIVECARE] " + Environment.NewLine;
#else
                    sQuery = "USE [LIVECARE] " + Environment.NewLine;
#endif
                    sQuery += string.Format("SET ANSI_NULLS ON " + Environment.NewLine +
                                            "SET QUOTED_IDENTIFIER ON " + Environment.NewLine +
                                            "SET ANSI_PADDING ON " + Environment.NewLine +
                                            "CREATE TABLE [dbo].[{0}]( " + Environment.NewLine +
                                            " [SEQ] [int] IDENTITY(1,1) NOT NULL, " + Environment.NewLine +
                                            " [FARM_SEQ] [int] NOT NULL, " + Environment.NewLine +
                                            " [COORDINATOR_ID] [nvarchar](15) NULL, " + Environment.NewLine +
                                            " [STAR_NO] [char](1) NULL, " + Environment.NewLine +
                                            " [HUB_NO] [int] NULL, " + Environment.NewLine +
                                            " [TAG_ID] [nvarchar](50) NOT NULL, " + Environment.NewLine +
                                            " [CHECK_YEAR] [int] NOT NULL, " + Environment.NewLine +
                                            " [CHECK_MONTH] [int] NOT NULL, " + Environment.NewLine +
                                            " [CHECK_WEEK] [int] NOT NULL, " + Environment.NewLine +
                                            " [CHECK_DAY] [int] NOT NULL, " + Environment.NewLine +
                                            " [CHECK_HOUR] [int] NOT NULL, " + Environment.NewLine +
                                            " [CHECK_MINUTE] [int] NULL, " + Environment.NewLine +
                                            " [TEMPERATURE] [nvarchar](10) NOT NULL, " + Environment.NewLine +
                                            " [BATTERY] [nvarchar](10) NOT NULL, " + Environment.NewLine +
                                            " [NORMAL_FLAG] [char](1) NOT NULL CONSTRAINT [DF_{0}_NORMAL_FLAG]  DEFAULT ('Y'), " + Environment.NewLine +
                                            " [CREATE_DATE] [datetime] NOT NULL CONSTRAINT [DF_{0}_CREATE_DATE]  DEFAULT (getdate()), " + Environment.NewLine +
                                            " [FLAG] [char](1) NOT NULL CONSTRAINT [DF_{0}_FLAG]  DEFAULT ('Y'), " + Environment.NewLine +
                                            " [RSSI] [int] NULL DEFAULT ((0)), " + Environment.NewLine +
                                            " [SNR] [int] NULL DEFAULT ((0)), " + Environment.NewLine +
                                            " [SENSOR_X] [int] NULL DEFAULT ((0)), " + Environment.NewLine +
                                            " [SENSOR_Y] [int] NULL DEFAULT ((0)), " + Environment.NewLine +
                                            " [SENSOR_Z] [int] NULL DEFAULT ((0)), " + Environment.NewLine +
                                            " [SENSOR_VALUE] [int] NULL DEFAULT ((0)), " + Environment.NewLine +
                                            " [INT_COUNT] [int] NULL DEFAULT ((0)), " + Environment.NewLine +
                                            " [INT_TIME] [float] NULL DEFAULT ((0.0)), " + Environment.NewLine +
                                            " [ANGLE_X] [float] NULL DEFAULT ((0.0)), " + Environment.NewLine +
                                            " [ANGLE_Y] [float] NULL DEFAULT ((0.0)), " + Environment.NewLine +
                                            " [ANGLE_Z] [float] NULL DEFAULT ((0.0)), " + Environment.NewLine +
                                            " [HS_STATUS] [int] NULL, " + Environment.NewLine +
                                            " [GW_LOC] [nvarchar](30) NULL, " + Environment.NewLine +
                                            " [GW_ID] [nvarchar](30) NULL, " + Environment.NewLine +
                                            " [PH_VALUE] [float] NULL DEFAULT ((0.0)), " + Environment.NewLine +
                                            " [ANGULAR_X] [int] NULL DEFAULT ((0)), " + Environment.NewLine +
                                            " [ANGULAR_Y] [int] NULL DEFAULT ((0)), " + Environment.NewLine +
                                            " [ANGULAR_Z] [int] NULL DEFAULT ((0)), " + Environment.NewLine +
                                            " [SENSOR_START_X] [int] NULL DEFAULT ((0)), " + Environment.NewLine +
                                            " [SENSOR_START_Y] [int] NULL DEFAULT ((0)), " + Environment.NewLine +
                                            " [SENSOR_START_Z] [int] NULL DEFAULT ((0)), " + Environment.NewLine +
                                            " [ANGULAR_START_X] [int] NULL DEFAULT ((0)), " + Environment.NewLine +
                                            " [ANGULAR_START_Y] [int] NULL DEFAULT ((0)), " + Environment.NewLine +
                                            " [ANGULAR_START_Z] [int] NULL DEFAULT ((0)), " + Environment.NewLine +
                                            " [SENSOR_END_X] [int] NULL DEFAULT ((0)), " + Environment.NewLine +
                                            " [SENSOR_END_Y] [int] NULL DEFAULT ((0)), " + Environment.NewLine +
                                            " [SENSOR_END_Z] [int] NULL DEFAULT ((0)), " + Environment.NewLine +
                                            " [ANGULAR_END_X] [int] NULL DEFAULT ((0)), " + Environment.NewLine +
                                            " [ANGULAR_END_Y] [int] NULL DEFAULT ((0)), " + Environment.NewLine +
                                            " [ANGULAR_END_Z] [int] NULL DEFAULT ((0)), " + Environment.NewLine +
                                            " [EC_VALUE] [float] NULL, " + Environment.NewLine +
                                            " [EC_TDC] [float] NULL, " + Environment.NewLine +
                                            " CONSTRAINT [PK_{0}] PRIMARY KEY CLUSTERED  " + Environment.NewLine +
                                            "( " + Environment.NewLine +
                                            "	[SEQ] ASC " + Environment.NewLine +
                                            ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] " + Environment.NewLine +
                                            ") ON [PRIMARY] " + Environment.NewLine +
                                            "CREATE NONCLUSTERED INDEX [IDX_CHECK_DAY] ON [dbo].[{0}] " + Environment.NewLine +
                                            "( " + Environment.NewLine +
                                            "	[CHECK_DAY] ASC " + Environment.NewLine +
                                            ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] " + Environment.NewLine +
                                            "CREATE NONCLUSTERED INDEX [IDX_CHECK_HOUR] ON [dbo].[{0}] " + Environment.NewLine +
                                            "( " + Environment.NewLine +
                                            "	[CHECK_HOUR] ASC " + Environment.NewLine +
                                            ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] " + Environment.NewLine +
                                            "CREATE NONCLUSTERED INDEX [IDX_CHECK_MONTH] ON [dbo].[{0}] " + Environment.NewLine +
                                            "( " + Environment.NewLine +
                                            "	[CHECK_MONTH] ASC " + Environment.NewLine +
                                            ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] " + Environment.NewLine +
                                            "CREATE NONCLUSTERED INDEX [IDX_CHECK_WEEK] ON [dbo].[{0}] " + Environment.NewLine +
                                            "( " + Environment.NewLine +
                                            "	[CHECK_WEEK] ASC " + Environment.NewLine +
                                            ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] " + Environment.NewLine +
                                            "CREATE NONCLUSTERED INDEX [IDX_CHECK_YEAR] ON [dbo].[{0}] " + Environment.NewLine +
                                            "( " + Environment.NewLine +
                                            "	[CHECK_YEAR] ASC " + Environment.NewLine +
                                            ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] " + Environment.NewLine +
                                            "CREATE NONCLUSTERED INDEX [IDX_CREATE_DATE] ON [dbo].[{0}] " + Environment.NewLine +
                                            "( " + Environment.NewLine +
                                            "	[CREATE_DATE] ASC " + Environment.NewLine +
                                            ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] " + Environment.NewLine +
                                            "CREATE NONCLUSTERED INDEX [IDX_FARM_SEQ] ON [dbo].[{0}] " + Environment.NewLine +
                                            "( " + Environment.NewLine +
                                            "	[FARM_SEQ] ASC " + Environment.NewLine +
                                            ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] " + Environment.NewLine +
                                            "CREATE NONCLUSTERED INDEX [IDX_FLAG] ON [dbo].[{0}] " + Environment.NewLine +
                                            "( " + Environment.NewLine +
                                            "	[FLAG] ASC " + Environment.NewLine +
                                            ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] " + Environment.NewLine +
                                            "CREATE NONCLUSTERED INDEX [IDX_NORMAL_FLAG] ON [dbo].[{0}] " + Environment.NewLine +
                                            "( " + Environment.NewLine +
                                            "	[NORMAL_FLAG] ASC " + Environment.NewLine +
                                            ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] " + Environment.NewLine +
                                            "CREATE NONCLUSTERED INDEX [IDX_TAG_ID] ON [dbo].[{0}] " + Environment.NewLine +
                                            "( " + Environment.NewLine +
                                            "	[TAG_ID] ASC " + Environment.NewLine +
                                            ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] ", sName);

                    int count = database.QueryExecute(sQuery);
                }
            }
            catch
            {
                isError = true;
            }

            return !isError;
        }

        public Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }

        public string GetAppVersion(string pType)
        {
            string sVersion = string.Empty;
            ClassOLEDB database = new ClassOLEDB();

            bool isError = false;
            OleDbDataReader dataReader = null;

            try
            {
                if (!database.GetConnectionState())
                {
                    if (!database.OpenDatabase())
                    {
                        isError = true;
                    }
                }

                if (!isError)
                {
                    string sQuery = string.Format("SELECT TOP 1 APP_VERSION FROM APP_INFO WHERE FLAG = 'Y' AND APP_DIV = '{0}' ORDER BY SEQ DESC ", pType);

                    if (database.QueryOpen(sQuery, ref dataReader))
                    {
                        if (dataReader.HasRows)
                        {
                            dataReader.Read();
                            sVersion = dataReader.GetString(0);
                        }

                        dataReader.Close();
                        dataReader.Dispose();
                    }
                }
            }
            catch
            {
                sVersion = string.Empty;
            }
            finally
            {
                database.CloseDatabase();
            }

            return sVersion;
        }

        public bool GetFarmTimeDifference(int pSEQ, out int pHour, out int pMinute)
        {
            // 기본값은 한국시간을 설정한다
            pHour = 9;
            pMinute = 0;
            ClassOLEDB database = new ClassOLEDB();

            bool isError = false;
            OleDbDataReader dataReader = null;

            try
            {
                if (!database.GetConnectionState())
                {
                    if (!database.OpenDatabase())
                    {
                        isError = true;
                    }
                }

                if (!isError)
                {
                    string sQuery = string.Format("SELECT FARM_HOUR, FARM_MINUTE FROM FARM_INFO WHERE FLAG = 'Y' AND SEQ = {0} ", pSEQ);

                    if (database.QueryOpen(sQuery, ref dataReader))
                    {
                        if (dataReader.HasRows)
                        {
                            dataReader.Read();
                            pHour = dataReader.GetInt32(0);
                            pMinute = dataReader.GetInt32(1);
                        }

                        dataReader.Close();
                        dataReader.Dispose();
                    }
                }
            }
            catch
            {
                isError = true;
            }
            finally
            {
                database.CloseDatabase();
            }

            return !isError;
        }

        public bool GetFarmTempSetting(int pSEQ, out double pMin, out double pMax)
        {
            pMin = 0.0;
            pMax = 100.0;
            ClassOLEDB database = new ClassOLEDB();

            bool isError = false;
            OleDbDataReader dataReader = null;

            try
            {
                if (!database.GetConnectionState())
                {
                    if (!database.OpenDatabase())
                    {
                        isError = true;
                    }
                }

                if (!isError)
                {
                    string sQuery = string.Format("SELECT TEMP_MIN, TEMP_MAX FROM FARM_INFO WHERE FLAG = 'Y' AND SEQ = {0} ", pSEQ);

                    if (database.QueryOpen(sQuery, ref dataReader))
                    {
                        if (dataReader.HasRows)
                        {
                            dataReader.Read();
                            pMin = dataReader.GetDouble(0);
                            pMax = dataReader.GetDouble(1);
                        }

                        dataReader.Close();
                        dataReader.Dispose();
                    }
                }
            }
            catch
            {
                isError = true;
            }
            finally
            {
                database.CloseDatabase();
            }

            return !isError;
        }

        public void ParsingDeviceData(ClassStruct.ST_LORATAG_FULLINFO pTagInfo, string pPayload, out ClassStruct.ST_SENSOR_SETTING pSetting, out ClassStruct.ST_SENSOR_DATA[] pSensor)
        {
            pSetting = new ClassStruct.ST_SENSOR_SETTING();
            pSensor = new ClassStruct.ST_SENSOR_DATA[5];

            if (string.IsNullOrEmpty(pPayload)) return;

            try
            {
                string sHEX;

                // 태그 버전에 따라서 처리한다
                if (pTagInfo.tag_version == "1.1")
                {
                    // 패킷은 아래와 같이 구성된다
                    // 온도(2) + 데이타(6) + 온도(2) + 데이타(6) + 온도(2) + 데이타(6) + 온도(2) + 데이타(6) + 온도(2) + 데이타(6) + 
                    // 배터리(1) + 수집주기(1) + ATH(2) + ADT(1) + ITH(2) + IDT(1) + RSSI(2) + SNR(1)

                    // 온도 : 측정온도 * 10에 대한 HEX 값
                    // 데이타는 인터럽트가 발생된 경우와 발생되지 않은 경우로 구분된다
                    //   구분은 데이타의 첫 바이트를 ASCII 코드로 "I"에 해당하면 인터럽트 발생 데이타이고 아니면 센서 데이타이다
                    //     센서 데이타 : 0 ~ 4000으로 보전된 센서값의 HEX 값 (X, Y, Z 각각 2BYTE)
                    //     인터럽트 데이타 : 구분자(1) + 카운트(1) + 지속시간(2) + Vector(2)
                    //       구분자 : "I" 에 대한 ASCII HEX 값
                    //       카운트 : 회수에 대한 HEX 값
                    //       지속시간 : 초단위 지속시간에 대한 HEX 값
                    //       Vector : vector * 10 한 결과의 정수의 HEX 값
                    // 배터리 : 배터리 * 10의 HEX 값
                    // 수집주기 : 분단위로 환산된 값의 HEX 값
                    // ACT (Active Threath Hold) : ACT의 HEX 값
                    // ADT (Active Duration Time) : 100msec 단위의 곱하기 값의 HEX 값 (만일 1초인 경우에는 ADT는 10)
                    // ICT (Inactive Threath Hold) : ICT의 HEX 값
                    // IDT (Inactive Duration Time) : 100msec 단위 값의 HEX 값
                    // RSSI : Signed 2 Byte HEX
                    // SNR : Signed 1 Byte HEX

                    for (int i = 0; i < 5; i++)
                    {
                        sHEX = pPayload.Substring(i * 16, 4);

                        // 양수, 음수를 구분해서 처리한다 (0x5000 보다 크면 영하온도)
                        int nTemp = int.Parse(sHEX, System.Globalization.NumberStyles.HexNumber);

                        if (nTemp <= 20480)
                        {
                            pSensor[i].TEMP_VALUE = nTemp / 10.0;
                        }
                        else
                        {
                            uint uValue = Convert.ToUInt32(sHEX, 16);

                            uValue *= 100;
                            uValue |= 0xffff0000;

                            double dTemp = unchecked((int)uValue) / 1000.0;
                            pSensor[i].TEMP_VALUE = Convert.ToDouble(dTemp.ToString("F1"));
                        }

                        // 데이타 포맷이 센서 데이타인지 인터럽트 데이타인지 구분한다
                        sHEX = pPayload.Substring(i * 16 + 4, 12);

                        if (sHEX.Substring(0, 2).ToUpper() == "49")
                        {
                            // 인터럽트 데이타
                            string sCount = sHEX.Substring(2, 2);
                            string sTime = sHEX.Substring(4, 4);
                            string sVector = sHEX.Substring(8, 4);

                            int nCount = int.Parse(sCount, System.Globalization.NumberStyles.HexNumber);
                            double dTime = int.Parse(sTime, System.Globalization.NumberStyles.HexNumber) / 10.0;
                            double dVector = int.Parse(sVector, System.Globalization.NumberStyles.HexNumber) / 10.0;

                            pSensor[i].DATA_TYPE = "I";
                            pSensor[i].INT_COUNTER = nCount;
                            pSensor[i].INT_TIME = dTime;
                            pSensor[i].SENSOR_VALUE = Convert.ToInt32(dVector);
                            pSensor[i].SENSOR_DATA = new Tuple<int, int, int>(0, 0, 0);
                            pSensor[i].ANGLE_DATA = new Tuple<double, double, double>(0.0, 0.0, 0.0);
                        }
                        else
                        {
                            // 센서 데이타 (0 ~ 4096으로 보정된 값의 HEX 값)
                            string sXSensor = sHEX.Substring(0, 4);
                            string sYSensor = sHEX.Substring(4, 4);
                            string sZSensor = sHEX.Substring(8, 4);

                            int nXSensor = int.Parse(sXSensor, System.Globalization.NumberStyles.HexNumber) - 2048;
                            int nYSensor = int.Parse(sYSensor, System.Globalization.NumberStyles.HexNumber) - 2048;
                            int nZSensor = int.Parse(sZSensor, System.Globalization.NumberStyles.HexNumber) - 2048;
                            double dVector = Convert.ToDouble(Math.Sqrt(Math.Pow(nXSensor, 2) + Math.Pow(nYSensor, 2) + Math.Pow(nZSensor, 2)).ToString("F1"));

                            double dX = Convert.ToDouble(nXSensor);
                            double dY = Convert.ToDouble(nYSensor);
                            double dZ = Convert.ToDouble(nZSensor);

                            double angleX = Math.Atan(dX / (Math.Sqrt(Math.Pow(dY, 2) + Math.Pow(dZ, 2)))) * (180 / Math.PI);
                            double angleY = Math.Atan(dY / (Math.Sqrt(Math.Pow(dX, 2) + Math.Pow(dZ, 2)))) * (180 / Math.PI);
                            double angleZ = Math.Atan(dZ / (Math.Sqrt(Math.Pow(dX, 2) + Math.Pow(dY, 2)))) * (180 / Math.PI);

                            if (double.IsNaN(angleX)) angleX = 0.0;
                            if (double.IsNaN(angleY)) angleY = 0.0;
                            if (double.IsNaN(angleZ)) angleZ = 0.0;

                            pSensor[i].DATA_TYPE = "S";
                            pSensor[i].INT_COUNTER = 0;
                            pSensor[i].INT_TIME = 0;
                            pSensor[i].SENSOR_VALUE = Convert.ToInt32(dVector);
                            pSensor[i].SENSOR_DATA = new Tuple<int, int, int>(nXSensor, nYSensor, nZSensor);
                            pSensor[i].ANGLE_DATA = new Tuple<double, double, double>(angleX, angleY, angleZ);
                        }
                    }

                    // 배터리
                    sHEX = pPayload.Substring(80, 2);
                    pSetting.BATTERY = int.Parse(sHEX, System.Globalization.NumberStyles.HexNumber) / 10.0;
                    // Hall Sensor (기본값 -1로 설정)
                    pSetting.HALL_SENSOR = -1;

                    // 수집주기
                    sHEX = pPayload.Substring(82, 2);
                    pSetting.INTERVAL = int.Parse(sHEX, System.Globalization.NumberStyles.HexNumber) * 60;

                    // ATH
                    sHEX = pPayload.Substring(84, 4);
                    pSetting.ATH = int.Parse(sHEX, System.Globalization.NumberStyles.HexNumber);

                    // ADT (ADT는 100msec 단위 값 : 10 이면 1초)
                    sHEX = pPayload.Substring(88, 2);
                    pSetting.ADT = int.Parse(sHEX, System.Globalization.NumberStyles.HexNumber);

                    // ITH
                    sHEX = pPayload.Substring(90, 4);
                    pSetting.ITH = int.Parse(sHEX, System.Globalization.NumberStyles.HexNumber);

                    // IDT (IDT는 초단위 값)
                    sHEX = pPayload.Substring(94, 2);
                    pSetting.IDT = int.Parse(sHEX, System.Globalization.NumberStyles.HexNumber);

                    // RSSI (Signed 2Byte)
                    sHEX = pPayload.Substring(96, 4);
                    pSetting.RSSI = Convert.ToInt16(sHEX, 16);

                    // SNR (Signed 1Byte)
                    sHEX = pPayload.Substring(100, 2);
                    pSetting.SNR = (int)Convert.ToSByte(sHEX, 16);
                }
                else if (pTagInfo.tag_version == "1.2")
                {
                    // 패킷은 아래와 같이 구성된다
                    // 온도(2) + 구분자(1) + 데이타(6) + 온도(2) + 구분자(1) + 데이타(6) + 온도(2) + 구분자(1) + 데이타(6) + 온도(2) + 구분자(1) + 데이타(6) + 온도(2) + 구분자(1) + 데이타(6) + 
                    // 배터리(1) + 수집주기(1) + ATH(1) + RSSI(2) + SNR(1)

                    // 온도 : 측정온도 * 10에 대한 HEX 값
                    // 구분자 : 인터럽트 발생시 대문자 "I"에 대한 ASCII HEX 값 (0x49)
                    //          인터럽트 미발생시 대문자 "N"에 대한 ASCII HEX 값 (0x4E)
                    // 데이타 : 0 ~ 4000으로 보정된 센서값의 HEX 값 (X, Y, Z 각각 2BYTE)
                    // 배터리 : 배터리 * 10의 HEX 값
                    // 수집주기 : 분단위로 환산된 값의 HEX 값
                    // ATH (Active Threath Hold) : ATH / 10의 HEX 값
                    // RSSI : Signed 2 Byte HEX
                    // SNR : Signed 1 Byte HEX

                    for (int i = 0; i < 5; i++)
                    {
                        sHEX = pPayload.Substring(i * 18, 4);

                        // 양수, 음수를 구분해서 처리한다 (0x5000 보다 크면 영하온도)
                        int nTemp = int.Parse(sHEX, System.Globalization.NumberStyles.HexNumber);

                        if (nTemp <= 20480)
                        {
                            pSensor[i].TEMP_VALUE = nTemp / 10.0;
                        }
                        else
                        {
                            uint uValue = Convert.ToUInt32(sHEX, 16);

                            uValue *= 100;
                            uValue |= 0xffff0000;

                            double dTemp = unchecked((int)uValue) / 1000.0;
                            pSensor[i].TEMP_VALUE = Convert.ToDouble(dTemp.ToString("F1"));
                        }

                        sHEX = pPayload.Substring(i * 18 + 4, 14);

                        string sType = sHEX.Substring(0, 2);
                        string sXSensor = sHEX.Substring(2, 4);
                        string sYSensor = sHEX.Substring(6, 4);
                        string sZSensor = sHEX.Substring(10, 4);

                        int nXSensor = int.Parse(sXSensor, System.Globalization.NumberStyles.HexNumber) - 2048;
                        int nYSensor = int.Parse(sYSensor, System.Globalization.NumberStyles.HexNumber) - 2048;
                        int nZSensor = int.Parse(sZSensor, System.Globalization.NumberStyles.HexNumber) - 2048;
                        double dVector = Convert.ToDouble(Math.Sqrt(Math.Pow(nXSensor, 2) + Math.Pow(nYSensor, 2) + Math.Pow(nZSensor, 2)).ToString("F1"));

                        double dX = Convert.ToDouble(nXSensor);
                        double dY = Convert.ToDouble(nYSensor);
                        double dZ = Convert.ToDouble(nZSensor);

                        double angleX = Math.Atan(dX / (Math.Sqrt(Math.Pow(dY, 2) + Math.Pow(dZ, 2)))) * (180 / Math.PI);
                        double angleY = Math.Atan(dY / (Math.Sqrt(Math.Pow(dX, 2) + Math.Pow(dZ, 2)))) * (180 / Math.PI);
                        double angleZ = Math.Atan(dZ / (Math.Sqrt(Math.Pow(dX, 2) + Math.Pow(dY, 2)))) * (180 / Math.PI);

                        if (double.IsNaN(angleX)) angleX = 0.0;
                        if (double.IsNaN(angleY)) angleY = 0.0;
                        if (double.IsNaN(angleZ)) angleZ = 0.0;

                        pSensor[i].DATA_TYPE = "S";
                        pSensor[i].INT_COUNTER = sType == "49" ? 1 : 0;
                        pSensor[i].INT_TIME = 0;
                        pSensor[i].SENSOR_VALUE = Convert.ToInt32(dVector);
                        pSensor[i].SENSOR_DATA = new Tuple<int, int, int>(nXSensor, nYSensor, nZSensor);
                        pSensor[i].ANGLE_DATA = new Tuple<double, double, double>(angleX, angleY, angleZ);
                    }

                    // 배터리
                    sHEX = pPayload.Substring(90, 2);
                    pSetting.BATTERY = int.Parse(sHEX, System.Globalization.NumberStyles.HexNumber) / 10.0;
                    // Hall Sensor (기본값 -1로 설정)
                    pSetting.HALL_SENSOR = -1;

                    // 수집주기
                    sHEX = pPayload.Substring(92, 2);
                    pSetting.INTERVAL = int.Parse(sHEX, System.Globalization.NumberStyles.HexNumber) * 60;

                    // ATH
                    sHEX = pPayload.Substring(94, 2);
                    pSetting.ATH = int.Parse(sHEX, System.Globalization.NumberStyles.HexNumber) * 10;

                    // ADT (1.2 버전에서는 받지 않는다)
                    pSetting.ADT = 0;

                    // ITH (1.2 버전에서는 받지 않는다)
                    pSetting.ITH = 0;

                    // IDT ((1.2 버전에서는 받지 않는다))
                    pSetting.IDT = 0;

                    // RSSI (Signed 2Byte)
                    sHEX = pPayload.Substring(96, 4);
                    pSetting.RSSI = Convert.ToInt16(sHEX, 16);

                    // SNR (Signed 1Byte)
                    sHEX = pPayload.Substring(100, 2);
                    pSetting.SNR = (int)Convert.ToSByte(sHEX, 16);
                }
                else if (pTagInfo.tag_version == "1.3")
                {
                    // 패킷은 아래와 같이 구성된다
                    // 온도(2) + 구분자(1) + 데이타(6) + 온도(2) + 구분자(1) + 데이타(6) + 온도(2) + 구분자(1) + 데이타(6) + 온도(2) + 구분자(1) + 데이타(6) + 온도(2) + 구분자(1) + 데이타(6) + 
                    // 배터리(1) + 수집주기(1) + ATH(1) + RSSI(2) + SNR(1)

                    // 온도 : 측정온도 * 10에 대한 HEX 값
                    // 구분자 : 인터럽트 발생 횟수 (0 : 미발생)
                    // 데이타 : 0 ~ 4000으로 보정된 센서값의 HEX 값 (X, Y, Z 각각 2BYTE)
                    // 배터리 : 배터리 * 10의 HEX 값
                    // 수집주기 : 분단위로 환산된 값의 HEX 값
                    // ATH (Active Threath Hold) : ATH / 10의 HEX 값
                    // RSSI : Signed 2 Byte HEX
                    // SNR : Signed 1 Byte HEX

                    for (int i = 0; i < 5; i++)
                    {
                        sHEX = pPayload.Substring(i * 18, 4);

                        // 양수, 음수를 구분해서 처리한다 (0x5000 보다 크면 영하온도)
                        int nTemp = int.Parse(sHEX, System.Globalization.NumberStyles.HexNumber);

                        if (nTemp <= 20480)
                        {
                            pSensor[i].TEMP_VALUE = nTemp / 10.0;
                        }
                        else
                        {
                            uint uValue = Convert.ToUInt32(sHEX, 16);

                            uValue *= 100;
                            uValue |= 0xffff0000;

                            double dTemp = unchecked((int)uValue) / 1000.0;
                            pSensor[i].TEMP_VALUE = Convert.ToDouble(dTemp.ToString("F1"));
                        }

                        sHEX = pPayload.Substring(i * 18 + 4, 14);

                        string sType = sHEX.Substring(0, 2);
                        string sXSensor = sHEX.Substring(2, 4);
                        string sYSensor = sHEX.Substring(6, 4);
                        string sZSensor = sHEX.Substring(10, 4);

                        int nXSensor = int.Parse(sXSensor, System.Globalization.NumberStyles.HexNumber) - 2048;
                        int nYSensor = int.Parse(sYSensor, System.Globalization.NumberStyles.HexNumber) - 2048;
                        int nZSensor = int.Parse(sZSensor, System.Globalization.NumberStyles.HexNumber) - 2048;
                        double dVector = Convert.ToDouble(Math.Sqrt(Math.Pow(nXSensor, 2) + Math.Pow(nYSensor, 2) + Math.Pow(nZSensor, 2)).ToString("F1"));

                        double dX = Convert.ToDouble(nXSensor);
                        double dY = Convert.ToDouble(nYSensor);
                        double dZ = Convert.ToDouble(nZSensor);

                        double angleX = Math.Atan(dX / (Math.Sqrt(Math.Pow(dY, 2) + Math.Pow(dZ, 2)))) * (180 / Math.PI);
                        double angleY = Math.Atan(dY / (Math.Sqrt(Math.Pow(dX, 2) + Math.Pow(dZ, 2)))) * (180 / Math.PI);
                        double angleZ = Math.Atan(dZ / (Math.Sqrt(Math.Pow(dX, 2) + Math.Pow(dY, 2)))) * (180 / Math.PI);

                        if (double.IsNaN(angleX)) angleX = 0.0;
                        if (double.IsNaN(angleY)) angleY = 0.0;
                        if (double.IsNaN(angleZ)) angleZ = 0.0;

                        pSensor[i].DATA_TYPE = "S";
                        pSensor[i].INT_COUNTER = int.Parse(sType, System.Globalization.NumberStyles.HexNumber);
                        pSensor[i].INT_TIME = 0;
                        pSensor[i].SENSOR_VALUE = Convert.ToInt32(dVector);
                        pSensor[i].SENSOR_DATA = new Tuple<int, int, int>(nXSensor, nYSensor, nZSensor);
                        pSensor[i].ANGLE_DATA = new Tuple<double, double, double>(angleX, angleY, angleZ);
                    }

                    // 배터리
                    sHEX = pPayload.Substring(90, 2);
                    pSetting.BATTERY = int.Parse(sHEX, System.Globalization.NumberStyles.HexNumber) / 10.0;
                    // Hall Sensor (기본값 -1로 설정)
                    pSetting.HALL_SENSOR = -1;

                    // 수집주기
                    sHEX = pPayload.Substring(92, 2);
                    pSetting.INTERVAL = int.Parse(sHEX, System.Globalization.NumberStyles.HexNumber) * 60;

                    // ATH
                    sHEX = pPayload.Substring(94, 2);
                    pSetting.ATH = int.Parse(sHEX, System.Globalization.NumberStyles.HexNumber) * 10;

                    // ADT (1.2 버전에서는 받지 않는다)
                    pSetting.ADT = 0;

                    // ITH (1.2 버전에서는 받지 않는다)
                    pSetting.ITH = 0;

                    // IDT ((1.2 버전에서는 받지 않는다))
                    pSetting.IDT = 0;

                    // RSSI (Signed 2Byte)
                    sHEX = pPayload.Substring(96, 4);
                    pSetting.RSSI = Convert.ToInt16(sHEX, 16);

                    // SNR (Signed 1Byte)
                    sHEX = pPayload.Substring(100, 2);
                    pSetting.SNR = (int)Convert.ToSByte(sHEX, 16);
                }
                else if (pTagInfo.tag_version == "1.4")
                {
                    // 패킷은 아래와 같이 구성된다
                    // 온도(2) + 구분자(1) + 데이타(6) + 온도(2) + 구분자(1) + 데이타(6) + 온도(2) + 구분자(1) + 데이타(6) + 온도(2) + 구분자(1) + 데이타(6) + 온도(2) + 구분자(1) + 데이타(6) + 
                    // 배터리(1) + 수집주기(1) + ATH(1) + RSSI(2) + SNR(1)

                    // 온도 : 측정온도 * 10에 대한 HEX 값
                    // 구분자 : 인터럽트 발생 횟수 (0 : 미발생)
                    // 데이타 : 0 ~ 4000으로 보정된 센서값의 HEX 값 (X, Y, Z 각각 2BYTE)
                    // 배터리 정보에 Hall Sensor 값이 포함되어 들어온다
                    // 배터리 값이 100 보다 큰 경우 : Hall Sensor ON / 배터리는 배터리 * 10 + 100의 HEX 값
                    // 배터리 값이 100 보다 작은 경우 : Hall Sensor OFF / 배터리는 배터리 * 10의 HEX 값
                    // 수집주기 : 분단위로 환산된 값의 HEX 값
                    // ATH (Active Threath Hold) : ATH / 10의 HEX 값
                    // RSSI : Signed 2 Byte HEX
                    // SNR : Signed 1 Byte HEX

                    for (int i = 0; i < 5; i++)
                    {
                        sHEX = pPayload.Substring(i * 18, 4);

                        // 양수, 음수를 구분해서 처리한다 (0x5000 보다 크면 영하온도)
                        int nTemp = int.Parse(sHEX, System.Globalization.NumberStyles.HexNumber);

                        if (nTemp <= 20480)
                        {
                            pSensor[i].TEMP_VALUE = nTemp / 10.0;
                        }
                        else
                        {
                            uint uValue = Convert.ToUInt32(sHEX, 16);

                            uValue *= 100;
                            uValue |= 0xffff0000;

                            double dTemp = unchecked((int)uValue) / 1000.0;
                            pSensor[i].TEMP_VALUE = Convert.ToDouble(dTemp.ToString("F1"));
                        }

                        sHEX = pPayload.Substring(i * 18 + 4, 14);

                        string sType = sHEX.Substring(0, 2);
                        string sXSensor = sHEX.Substring(2, 4);
                        string sYSensor = sHEX.Substring(6, 4);
                        string sZSensor = sHEX.Substring(10, 4);

                        int nXSensor = int.Parse(sXSensor, System.Globalization.NumberStyles.HexNumber) - 2048;
                        int nYSensor = int.Parse(sYSensor, System.Globalization.NumberStyles.HexNumber) - 2048;
                        int nZSensor = int.Parse(sZSensor, System.Globalization.NumberStyles.HexNumber) - 2048;
                        double dVector = Convert.ToDouble(Math.Sqrt(Math.Pow(nXSensor, 2) + Math.Pow(nYSensor, 2) + Math.Pow(nZSensor, 2)).ToString("F1"));

                        double dX = Convert.ToDouble(nXSensor);
                        double dY = Convert.ToDouble(nYSensor);
                        double dZ = Convert.ToDouble(nZSensor);

                        double angleX = Math.Atan(dX / (Math.Sqrt(Math.Pow(dY, 2) + Math.Pow(dZ, 2)))) * (180 / Math.PI);
                        double angleY = Math.Atan(dY / (Math.Sqrt(Math.Pow(dX, 2) + Math.Pow(dZ, 2)))) * (180 / Math.PI);
                        double angleZ = Math.Atan(dZ / (Math.Sqrt(Math.Pow(dX, 2) + Math.Pow(dY, 2)))) * (180 / Math.PI);

                        if (double.IsNaN(angleX)) angleX = 0.0;
                        if (double.IsNaN(angleY)) angleY = 0.0;
                        if (double.IsNaN(angleZ)) angleZ = 0.0;

                        pSensor[i].DATA_TYPE = "S";
                        pSensor[i].INT_COUNTER = int.Parse(sType, System.Globalization.NumberStyles.HexNumber);
                        pSensor[i].INT_TIME = 0;
                        pSensor[i].SENSOR_VALUE = Convert.ToInt32(dVector);
                        pSensor[i].SENSOR_DATA = new Tuple<int, int, int>(nXSensor, nYSensor, nZSensor);
                        pSensor[i].ANGLE_DATA = new Tuple<double, double, double>(angleX, angleY, angleZ);
                    }

                    // 배터리 & Hall Sensor
                    sHEX = pPayload.Substring(90, 2);
                    int nBattery = int.Parse(sHEX, System.Globalization.NumberStyles.HexNumber);

                    if (nBattery >= 100)
                    {
                        pSetting.HALL_SENSOR = 1;
                        nBattery -= 100;
                    }
                    else
                    {
                        pSetting.HALL_SENSOR = 0;
                    }

                    pSetting.BATTERY = nBattery / 10.0;

                    // 수집주기
                    sHEX = pPayload.Substring(92, 2);
                    pSetting.INTERVAL = int.Parse(sHEX, System.Globalization.NumberStyles.HexNumber) * 60;

                    // ATH
                    sHEX = pPayload.Substring(94, 2);
                    pSetting.ATH = int.Parse(sHEX, System.Globalization.NumberStyles.HexNumber) * 10;

                    // ADT (1.2 버전부터는 받지 않는다)
                    pSetting.ADT = 0;

                    // ITH (1.2 버전부터는 받지 않는다)
                    pSetting.ITH = 0;

                    // IDT ((1.2 버전부터는 받지 않는다))
                    pSetting.IDT = 0;

                    // RSSI (Signed 2Byte)
                    sHEX = pPayload.Substring(96, 4);
                    pSetting.RSSI = Convert.ToInt16(sHEX, 16);

                    // SNR (Signed 1Byte)
                    sHEX = pPayload.Substring(100, 2);
                    pSetting.SNR = (int)Convert.ToSByte(sHEX, 16);
                }
            }
            catch (Exception Exp)
            {
                ClassLog._mLogger.Error(Exp.Message);
            }
        }

        public void ParsingGemtekOldData(string pPayload, out ClassStruct.ST_SENSOR_SETTING pSetting, out ClassStruct.ST_SENSOR_DATA[] pSensor)
        {
            pSetting = new ClassStruct.ST_SENSOR_SETTING();
            pSensor = new ClassStruct.ST_SENSOR_DATA[5];

            if (string.IsNullOrEmpty(pPayload)) return;

            try
            {
                // 전체 패킷에서 온도 스트링 값을 처리한다
                for (int i = 0; i < 5; i++)
                {
                    // 온도정보 파싱
                    pSensor[i].TEMP_VALUE = Convert.ToDouble(HEXToString(pPayload.Substring(i * 18, 6))) / 10.0;

                    // 센서정보 파싱
                    char[] charArrayData = HEXToString(pPayload.Substring(i * 18 + 6, 12)).ToCharArray();

                    string sXSensor = string.Empty;
                    string sYSensor = string.Empty;
                    string sZSensor = string.Empty;

                    int nXSensor = 0;
                    int nYSensor = 0;
                    int nZSensor = 0;

                    for (int j = 0; j < charArrayData.Length; j++)
                    {
                        int nSensor = Convert.ToInt32(charArrayData[j]) - 33;

                        switch (j)
                        {
                            case 0: sXSensor = nSensor.ToString("D2"); break;
                            case 1: sXSensor += nSensor.ToString("D2"); break;
                            case 2: sYSensor = nSensor.ToString("D2"); break;
                            case 3: sYSensor += nSensor.ToString("D2"); break;
                            case 4: sZSensor = nSensor.ToString("D2"); break;
                            case 5: sZSensor += nSensor.ToString("D2"); break;
                        }
                    }

                    nXSensor = Convert.ToInt32(sXSensor) - 2048;
                    nYSensor = Convert.ToInt32(sYSensor) - 2048;
                    nZSensor = Convert.ToInt32(sZSensor) - 2048;
                    double dVector = Convert.ToDouble(Math.Sqrt(Math.Pow(nXSensor, 2) + Math.Pow(nYSensor, 2) + Math.Pow(nZSensor, 2)).ToString("F1"));

                    double dX = Convert.ToDouble(nXSensor);
                    double dY = Convert.ToDouble(nYSensor);
                    double dZ = Convert.ToDouble(nZSensor);

                    double angleX = Math.Atan(dX / (Math.Sqrt(Math.Pow(dY, 2) + Math.Pow(dZ, 2)))) * (180 / Math.PI);
                    double angleY = Math.Atan(dY / (Math.Sqrt(Math.Pow(dX, 2) + Math.Pow(dZ, 2)))) * (180 / Math.PI);
                    double angleZ = Math.Atan(dZ / (Math.Sqrt(Math.Pow(dX, 2) + Math.Pow(dY, 2)))) * (180 / Math.PI);

                    if (double.IsNaN(angleX)) angleX = 0.0;
                    if (double.IsNaN(angleY)) angleY = 0.0;
                    if (double.IsNaN(angleZ)) angleZ = 0.0;

                    pSensor[i].DATA_TYPE = "S";
                    pSensor[i].INT_COUNTER = 0;
                    pSensor[i].INT_TIME = 0;
                    pSensor[i].SENSOR_VALUE = Convert.ToInt32(dVector);
                    pSensor[i].SENSOR_DATA = new Tuple<int, int, int>(nXSensor, nYSensor, nZSensor);
                    pSensor[i].ANGLE_DATA = new Tuple<double, double, double>(angleX, angleY, angleZ);
                }

                pSetting.BATTERY = Convert.ToDouble(HEXToString(pPayload.Substring(90, 4))) / 10.0;
                pSetting.INTERVAL = Convert.ToInt32(HEXToString(pPayload.Substring(94)));
            }
            catch (Exception Exp)
            {
                ClassLog._mLogger.Error(Exp.Message);
            }
        }

        public string HEXToString(string pHEX)
        {
            string sConvert = string.Empty;

            for (int i = 0; i < pHEX.Length; i += 2)
            {
                string hexa = pHEX.Substring(i, 2);

                uint uAscii = System.Convert.ToUInt32(hexa, 16);
                char character = System.Convert.ToChar(uAscii);

                sConvert += character;
            }

            return sConvert;
        }
    }
}