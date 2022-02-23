using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LC_Service
{
    public class ClassError
    {
        public const int RESULT_SUCCESS = 0;

        public const int RESULT_PARAM_ERROR_UID = 1001;
        public const int RESULT_PARAM_ERROR_PWD = 1002;
        public const int RESULT_PARAM_ERROR_DEVICE_TYPE = 1003;
        public const int RESULT_PARAM_ERROR_MODEL = 1004;
        public const int RESULT_PARAM_ERROR_SERIAL = 1005;
        public const int RESULT_PARAM_ERROR_USER_TOKEN = 1006;
        public const int RESULT_PARAM_ERROR_APP_VERSION = 1007;
        public const int RESULT_PARAM_ERROR_CODE_DIV = 1008;
        public const int RESULT_PARAM_ERROR_SEMEN_NO = 1009;
        public const int RESULT_PARAM_ERROR_SEMEN_SEQ = 1010;
        public const int RESULT_PARAM_ERROR_IMAGE_NAME = 1011;
        public const int RESULT_PARAM_ERROR_IMAGE_TYPE = 1012;
        public const int RESULT_PARAM_ERROR_IMAGE_DATA = 1013;
        public const int RESULT_PARAM_ERROR_FILE_NAME = 1014;
        public const int RESULT_PARAM_ERROR_FARM_SEQ = 1015;
        public const int RESULT_PARAM_ERROR_SEARCH_YEAR = 1016;
        public const int RESULT_PARAM_ERROR_SEARCH_MONTH = 1017;
        public const int RESULT_PARAM_ERROR_SEARCH_DATE = 1018;
        public const int RESULT_PARAM_ERROR_PAGE_INDEX = 1019;
        public const int RESULT_PARAM_ERROR_ENTITY_SEQ = 1020;
        public const int RESULT_PARAM_ERROR_SEARCH_TYPE = 1021;
        public const int RESULT_PARAM_ERROR_SEARCH_COUNT = 1022;
        public const int RESULT_PARAM_ERROR_SEARCH_LEVEL = 1023;
        public const int RESULT_PARAM_ERROR_PREGNANCY_FLAG = 1024;
        public const int RESULT_PARAM_ERROR_ORDER_FIELD = 1025;
        public const int RESULT_PARAM_ERROR_ORDER_TYPE = 1026;
        public const int RESULT_PARAM_ERROR_ENTITY_ID = 1027;
        public const int RESULT_PARAM_ERROR_ENTITY_SEX = 1028;
        public const int RESULT_PARAM_ERROR_ENTITY_TYPE = 1029;
        public const int RESULT_PARAM_ERROR_DETAIL_TYPE = 1030;
        public const int RESULT_PARAM_ERROR_ENTITY_BIRTH = 1031;
        public const int RESULT_PARAM_ERROR_CODE_NO = 1032;
        public const int RESULT_PARAM_ERROR_CALVE_COUNT = 1033;
        public const int RESULT_PARAM_ERROR_ESTRUS_DATE = 1034;
        public const int RESULT_PARAM_ERROR_INSEMINATE_CODE = 1035;
        public const int RESULT_PARAM_ERROR_DUE_DATE = 1036;
        public const int RESULT_PARAM_ERROR_INSEMINATE_COUNT = 1037;
        public const int RESULT_PARAM_ERROR_INSEMINATE_DATE = 1038;
        public const int RESULT_PARAM_ERROR_APPRAISAL_CODE = 1039;
        public const int RESULT_PARAM_ERROR_APPRAISAL_DATE = 1040;
        public const int RESULT_PARAM_ERROR_CALVE_DUE_DATE = 1041;
        public const int RESULT_PARAM_ERROR_CALVE_DATE = 1042;
        public const int RESULT_PARAM_ERROR_CALVE_FLAG = 1043;
        public const int RESULT_PARAM_ERROR_CALVE_CODE = 1044;
        public const int RESULT_PARAM_ERROR_LANG_CODE = 1045;
        public const int RESULT_PARAM_ERROR_DIAGNOSIS_SEQ = 1046;
        public const int RESULT_PARAM_ERROR_DIAGNOSIS_NAME = 1047;
        public const int RESULT_PARAM_ERROR_PRESCRIPTION_SEQ = 1048;
        public const int RESULT_PARAM_ERROR_PRESCRIPTION_NAME = 1049;
        public const int RESULT_PARAM_ERROR_DIAGNOSIS_DATE = 1050;
        public const int RESULT_PARAM_ERROR_CURE_SEQ = 1051;
        public const int RESULT_PARAM_ERROR_FLAG = 1052;
        public const int RESULT_PARAM_ERROR_DEPT_NO = 1053;
        public const int RESULT_PARAM_ERROR_TAG_VALUE = 1054;
        public const int RESULT_PARAM_ERROR_ENTITY_NO = 1055;
        public const int RESULT_PARAM_ERROR_TAG_SEQ = 1056;
        public const int RESULT_PARAM_ERROR_WORKER_SEQ = 1057;
        public const int RESULT_PARAM_ERROR_TAG_SERIAL = 1058;
        public const int RESULT_PARAM_ERROR_NOTICE_SEQ = 1059;
        public const int RESULT_PARAM_ERROR_CHECK_DATE = 1060;
        public const int RESULT_PARAM_ERROR_HISTORY_SEQ = 1061;
        public const int RESULT_PARAM_ERROR_INSEMINATE_DUE_DATE = 1062;
        public const int RESULT_PARAM_ERROR_APPRAISAL_DUE_DATE = 1063;
        public const int RESULT_PARAM_ERROR_SEQ = 1064;
        public const int RESULT_PARAM_ERROR_ID_FLAG = 1065;
        public const int RESULT_PARAM_ERROR_AD_FLAG = 1066;
        public const int RESULT_PARAM_ERROR_CD_FLAG = 1067;
        public const int RESULT_PARAM_ERROR_DD_FLAG = 1068;
        public const int RESULT_PARAM_ERROR_AA_FLAG = 1069;
        public const int RESULT_PARAM_ERROR_AN_FLAG = 1070;
        public const int RESULT_PARAM_ERROR_KN_FLAG = 1071;
        public const int RESULT_PARAM_ERROR_FROM_DATE = 1072;
        public const int RESULT_PARAM_ERROR_TO_DATE = 1073;
        public const int RESULT_PARAM_ERROR_DRYUP_DATE = 1074;
        public const int RESULT_PARAM_ERROR_BREED_SEQ = 1075;
        public const int RESULT_PARAM_ERROR_BREED_DATE = 1076;
        public const int RESULT_PARAM_ERROR_HA_FLAG = 1077;
        public const int RESULT_PARAM_ERROR_LA_FLAG = 1078;
        public const int RESULT_PARAM_ERROR_ESTRUS_CODE = 1079;
        public const int RESULT_PARAM_ERROR_ENTITY_PIN = 1080;
        public const int RESULT_PARAM_ERROR_ALARM_TITLE = 1081;
        public const int RESULT_PARAM_ERROR_BASE_DATE = 1082;
        public const int RESULT_PARAM_ERROR_BASE_TYPE = 1083;
        public const int RESULT_PARAM_ERROR_ALARM_SEQ = 1084;

        public const int RESULT_SEARCH_NOTEXIST_CODE = 2001;
        public const int RESULT_SEARCH_NOTEXIST_MEMBER = 2002;
        public const int RESULT_SEARCH_NOTEXIST_FARM = 2003;
        public const int RESULT_SEARCH_NOTEXIST_TAG = 2004;
        public const int RESULT_SEARCH_NOTEXIST_SEMEN = 2005;
        public const int RESULT_SEARCH_NOTEXIST_SCHEDULE = 2006;
        public const int RESULT_SEARCH_NOTEXIST_SETTING = 2007;
        public const int RESULT_SEARCH_NOTEXIST_ENTITY = 2008;
        public const int RESULT_SEARCH_NOTEXIST_HISTORY = 2009;
        public const int RESULT_SEARCH_NOTEXIST_ALARM = 2010;
        public const int RESULT_SEARCH_NOTEXIST_DIAGNOSIS = 2011;
        public const int RESULT_SEARCH_NOTEXIST_PRESCRIPTION = 2012;
        public const int RESULT_SEARCH_NOTEXIST_CURE = 2013;
        public const int RESULT_SEARCH_NOTEXIST_CHAT = 2014;
        public const int RESULT_SEARCH_NOTEXIST_WORKER = 2015;
        public const int RESULT_SEARCH_NOTEXIST_NOTICE = 2016;
        public const int RESULT_SEARCH_NOTEXIST_PUSH_SETTING = 2017;

        public const int RESULT_ERROR_DEVICE_NOTMATCHED = 3001;
        public const int RESULT_ERROR_MEMBER_NOTMATCHED = 3002;
        public const int RESULT_ERROR_PASSWORD_NOTMATCHED = 3003;
        public const int RESULT_ERROR_APPUPDATE_NEEDED = 3004;
        public const int RESULT_ERROR_APPINFO_FAILED = 3005;
        public const int RESULT_ERROR_TAG_ALREADY_USED = 3006;
        public const int RESULT_ERROR_HISTORY_EXISTED = 3007;
        public const int RESULT_ERROR_SEMEN_EXISTED = 3008;
        public const int RESULT_ERROR_TAG_ID_EXISTED = 3009;
        public const int RESULT_ERROR_ENTITY_NO_EXISTED = 3010;
        public const int RESULT_ERROR_ENTITY_PIN_EXISTED = 3011;
        public const int RESULT_ERROR_OSUPDATE_NEEDED = 3012;

        public const int RESULT_SYSTEM_ERROR_NETWORK = 9100;
        public const int RESULT_SYSTEM_ERROR_DATABASE = 9200;
        public const int RESULT_SYSTEM_ERROR_EXCEPTION = 9300;

        private readonly Dictionary<int, string> _mKRMessageList = new Dictionary<int, string>();
        private readonly Dictionary<int, string> _mJPMessageList = new Dictionary<int, string>();
        private readonly Dictionary<int, string> _mENMessageList = new Dictionary<int, string>();
        private readonly Dictionary<int, string> _mZHMessageList = new Dictionary<int, string>();
        private readonly Dictionary<int, string> _mPTMessageList = new Dictionary<int, string>();

        public ClassError()
        {
            AddKoreanMessageList();
            AddJapanMessageList();
            AddEnglishMessageList();
            AddChinaMessageList();
            AddPortugalMessageList();
        }

        private void AddKoreanMessageList()
        {
            _mKRMessageList.Add(RESULT_SUCCESS, string.Empty);

            _mKRMessageList.Add(RESULT_PARAM_ERROR_UID, "uid 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_PWD, "pwd 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_DEVICE_TYPE, "device_type 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_MODEL, "model 파라미터의 값이 맞지 않습니다");
            //_mKRMessageList.Add(RESULT_PARAM_ERROR_SERIAL, "serial 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_SERIAL, "전화걸기를 허용하셔야 됩니다" + Environment.NewLine + "앱의 권한에 전화권한을 허용해 주세요");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_USER_TOKEN, "user_token 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_APP_VERSION, "app_version 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_CODE_DIV, "code_div 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_SEMEN_NO, "semen_no 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_SEMEN_SEQ, "semen_seq 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_IMAGE_NAME, "image_name 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_IMAGE_TYPE, "image_type 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_IMAGE_DATA, "image_data 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_FILE_NAME, "file name 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_FARM_SEQ, "farm_seq 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_SEARCH_YEAR, "search_year 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_SEARCH_MONTH, "search_month 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_SEARCH_DATE, "search_date 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_PAGE_INDEX, "page_index 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_ENTITY_SEQ, "entity_seq 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_SEARCH_TYPE, "search_type 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_SEARCH_COUNT, "search_count 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_SEARCH_LEVEL, "search_level 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_PREGNANCY_FLAG, "pregnancy_flag 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_ORDER_FIELD, "order_field 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_ORDER_TYPE, "order_type 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_ENTITY_ID, "entity_id 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_ENTITY_SEX, "entity_sex 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_ENTITY_TYPE, "entity_type 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_DETAIL_TYPE, "detail_type 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_ENTITY_BIRTH, "entity_birth 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_CODE_NO, "code_no 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_CALVE_COUNT, "calve_count 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_ESTRUS_DATE, "estrus_date 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_INSEMINATE_CODE, "inseminate_code 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_DUE_DATE, "due_date 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_INSEMINATE_COUNT, "inseminate_count 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_INSEMINATE_DATE, "inseminate_date 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_APPRAISAL_CODE, "appraisal_code 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_APPRAISAL_DATE, "appraisal_date 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_CALVE_DUE_DATE, "calve_due_date 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_CALVE_DATE, "calve_date 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_CALVE_FLAG, "calve_flag 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_CALVE_CODE, "calve_code 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_LANG_CODE, "lang_code 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_DIAGNOSIS_SEQ, "diagnosis_seq 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_DIAGNOSIS_NAME, "diagnosis_name 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_PRESCRIPTION_SEQ, "prescription_seq 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_PRESCRIPTION_NAME, "prescription_name 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_DIAGNOSIS_DATE, "diagnosis_date 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_CURE_SEQ, "cure_seq 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_FLAG, "flag 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_DEPT_NO, "dept_no 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_TAG_VALUE, "tag_value 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_ENTITY_NO, "entity_no 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_TAG_SEQ, "tag_seq 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_WORKER_SEQ, "worker_seq 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_TAG_SERIAL, "tag_serial 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_NOTICE_SEQ, "notice_seq 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_CHECK_DATE, "check_date 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_HISTORY_SEQ, "history_seq 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_INSEMINATE_DUE_DATE, "inseminate_due_date 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_APPRAISAL_DUE_DATE, "appraisal_due_date 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_SEQ, "seq 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_ID_FLAG, "id_flag 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_AD_FLAG, "ad_flag 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_CD_FLAG, "cd_flag 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_DD_FLAG, "dd_flag 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_AA_FLAG, "aa_flag 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_AN_FLAG, "an_flag 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_KN_FLAG, "kn_flag 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_FROM_DATE, "from_date 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_TO_DATE, "to_date 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_DRYUP_DATE, "dryup_date 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_BREED_SEQ, "breed_seq 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_BREED_DATE, "breed_date 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_HA_FLAG, "ha_flag 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_LA_FLAG, "la_flag 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_ESTRUS_CODE, "estrus_code 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_ENTITY_PIN, "entity_pin 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_ALARM_TITLE, "alarm_title 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_BASE_DATE, "base_date 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_BASE_TYPE, "base_type 파라미터의 값이 맞지 않습니다");
            _mKRMessageList.Add(RESULT_PARAM_ERROR_ALARM_SEQ, "alarm_seq 파라미터의 값이 맞지 않습니다");

            _mKRMessageList.Add(RESULT_SEARCH_NOTEXIST_CODE, "검색된 코드정보가 없습니다");
            _mKRMessageList.Add(RESULT_SEARCH_NOTEXIST_MEMBER, "검색된 회원정보가 없습니다");
            _mKRMessageList.Add(RESULT_SEARCH_NOTEXIST_FARM, "검색된 농장정보가 없습니다");
            _mKRMessageList.Add(RESULT_SEARCH_NOTEXIST_TAG, "검색된 태그정보가 없습니다");
            _mKRMessageList.Add(RESULT_SEARCH_NOTEXIST_SEMEN, "검색된 정액번호가 없습니다");
            _mKRMessageList.Add(RESULT_SEARCH_NOTEXIST_SCHEDULE, "검색된 번식일정이 없습니다");
            _mKRMessageList.Add(RESULT_SEARCH_NOTEXIST_SETTING, "검색된 번식설정이 없습니다");
            _mKRMessageList.Add(RESULT_SEARCH_NOTEXIST_ENTITY, "검색된 개체정보가 없습니다");
            _mKRMessageList.Add(RESULT_SEARCH_NOTEXIST_HISTORY, "검색된 이력정보가 없습니다");
            _mKRMessageList.Add(RESULT_SEARCH_NOTEXIST_ALARM, "검색된 알람목록이 없습니다");
            _mKRMessageList.Add(RESULT_SEARCH_NOTEXIST_DIAGNOSIS, "검색된 진단명이 없습니다");
            _mKRMessageList.Add(RESULT_SEARCH_NOTEXIST_PRESCRIPTION, "검색된 처방제가 없습니다");
            _mKRMessageList.Add(RESULT_SEARCH_NOTEXIST_CURE, "검색된 치료이력이 없습니다");
            _mKRMessageList.Add(RESULT_SEARCH_NOTEXIST_CHAT, "검색된 채팅목록이 없습니다");
            _mKRMessageList.Add(RESULT_SEARCH_NOTEXIST_WORKER, "검색된 직원정보가 없습니다");
            _mKRMessageList.Add(RESULT_SEARCH_NOTEXIST_NOTICE, "검색된 공지사항이 없습니다");
            _mKRMessageList.Add(RESULT_SEARCH_NOTEXIST_PUSH_SETTING, "검색된 알람설정이 없습니다");

            _mKRMessageList.Add(RESULT_ERROR_DEVICE_NOTMATCHED, "등록된 단말기가 아닙니다");
            _mKRMessageList.Add(RESULT_ERROR_MEMBER_NOTMATCHED, "등록된 회원이 아닙니다");
            _mKRMessageList.Add(RESULT_ERROR_PASSWORD_NOTMATCHED, "비밀번호가 일치하지 않습니다");
            _mKRMessageList.Add(RESULT_ERROR_APPUPDATE_NEEDED, "새로운 버전의 업데이트가 필요합니다");
            _mKRMessageList.Add(RESULT_ERROR_APPINFO_FAILED, "앱 버젼정보 추출에 실패했습니다");
            _mKRMessageList.Add(RESULT_ERROR_TAG_ALREADY_USED, "이미 사용중인 태그입니다");
            _mKRMessageList.Add(RESULT_ERROR_HISTORY_EXISTED, "이미 수정내역이 등록된 개체 이력정보입니다");
            _mKRMessageList.Add(RESULT_ERROR_SEMEN_EXISTED, "이미 등록된 정액번호입니다");
            _mKRMessageList.Add(RESULT_ERROR_TAG_ID_EXISTED, "이미 태그가 등록된 개체입니다");
            _mKRMessageList.Add(RESULT_ERROR_ENTITY_NO_EXISTED, "이미 등록된 개체번호입니다");
            _mKRMessageList.Add(RESULT_ERROR_ENTITY_PIN_EXISTED, "이미 등록된 개체 식별번호입니다");
            _mKRMessageList.Add(RESULT_ERROR_OSUPDATE_NEEDED, "OS 업데이트가 필요합니다");

            _mKRMessageList.Add(RESULT_SYSTEM_ERROR_NETWORK, "네트워크 오류가 발생하였습니다");
            _mKRMessageList.Add(RESULT_SYSTEM_ERROR_DATABASE, "데이타베이스 오류가 발생하였습니다");
            _mKRMessageList.Add(RESULT_SYSTEM_ERROR_EXCEPTION, "예외적인 오류가 발생하였습니다");
        }

        private void AddJapanMessageList()
        {
            _mJPMessageList.Add(RESULT_SUCCESS, string.Empty);

            _mJPMessageList.Add(RESULT_PARAM_ERROR_UID, "uid パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_PWD, "pwd パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_DEVICE_TYPE, "device_type パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_MODEL, "model パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_SERIAL, "serial パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_USER_TOKEN, "user_token パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_APP_VERSION, "app_version パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_CODE_DIV, "code_div パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_SEMEN_NO, "semen_no パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_SEMEN_SEQ, "semen_seq パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_IMAGE_NAME, "image_name パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_IMAGE_TYPE, "image_type パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_IMAGE_DATA, "image_data パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_FILE_NAME, "file name パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_FARM_SEQ, "farm_seq パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_SEARCH_YEAR, "search_year パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_SEARCH_MONTH, "search_month パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_SEARCH_DATE, "search_date パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_PAGE_INDEX, "page_index パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_ENTITY_SEQ, "entity_seq パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_SEARCH_TYPE, "search_type パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_SEARCH_COUNT, "search_count パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_SEARCH_LEVEL, "search_level パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_PREGNANCY_FLAG, "pregnancy_flag パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_ORDER_FIELD, "order_field パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_ORDER_TYPE, "order_type パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_ENTITY_ID, "entity_id パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_ENTITY_SEX, "entity_sex パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_ENTITY_TYPE, "entity_type パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_DETAIL_TYPE, "detail_type パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_ENTITY_BIRTH, "entity_birth パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_CODE_NO, "code_no パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_CALVE_COUNT, "calve_count パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_ESTRUS_DATE, "estrus_date パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_INSEMINATE_CODE, "inseminate_code パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_DUE_DATE, "due_date パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_INSEMINATE_COUNT, "inseminate_count パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_INSEMINATE_DATE, "inseminate_date パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_APPRAISAL_CODE, "appraisal_code パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_APPRAISAL_DATE, "appraisal_date パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_CALVE_DUE_DATE, "calve_due_date パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_CALVE_DATE, "calve_date パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_CALVE_FLAG, "calve_flag パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_CALVE_CODE, "calve_code パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_LANG_CODE, "lang_code パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_DIAGNOSIS_SEQ, "diagnosis_seq パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_DIAGNOSIS_NAME, "diagnosis_name パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_PRESCRIPTION_SEQ, "prescription_seq パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_PRESCRIPTION_NAME, "prescription_name パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_DIAGNOSIS_DATE, "diagnosis_date パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_CURE_SEQ, "cure_seq パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_FLAG, "flag パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_DEPT_NO, "dept_no パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_TAG_VALUE, "tag_value パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_ENTITY_NO, "entity_no パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_TAG_SEQ, "tag_seq パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_WORKER_SEQ, "worker_seq パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_TAG_SERIAL, "tag_serial パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_NOTICE_SEQ, "notice_seq パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_CHECK_DATE, "check_date パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_HISTORY_SEQ, "history_seq パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_INSEMINATE_DUE_DATE, "inseminate_due_date パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_APPRAISAL_DUE_DATE, "appraisal_due_date パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_SEQ, "seq パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_ID_FLAG, "id_flag パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_AD_FLAG, "ad_flag パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_CD_FLAG, "cd_flag パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_DD_FLAG, "dd_flag パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_AA_FLAG, "aa_flag パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_AN_FLAG, "an_flag パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_KN_FLAG, "kn_flag パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_FROM_DATE, "from_date パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_TO_DATE, "to_date パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_DRYUP_DATE, "dryup_date パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_BREED_SEQ, "breed_seq パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_BREED_DATE, "breed_date パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_HA_FLAG, "ha_flag パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_LA_FLAG, "la_flag パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_ESTRUS_CODE, "estrus_code パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_ENTITY_PIN, "entity_pin パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_ALARM_TITLE, "alarm_title パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_BASE_DATE, "base_date パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_BASE_TYPE, "base_type パラメータの値が正しくありません");
            _mJPMessageList.Add(RESULT_PARAM_ERROR_ALARM_SEQ, "alarm_seq パラメータの値が正しくありません");

            _mJPMessageList.Add(RESULT_SEARCH_NOTEXIST_CODE, "検索されたコード情報がありません");
            _mJPMessageList.Add(RESULT_SEARCH_NOTEXIST_MEMBER, "検索された会員情報がありません");
            _mJPMessageList.Add(RESULT_SEARCH_NOTEXIST_FARM, "検索された農場情報がありません");
            _mJPMessageList.Add(RESULT_SEARCH_NOTEXIST_TAG, "検索されたタグ情報がありません");
            _mJPMessageList.Add(RESULT_SEARCH_NOTEXIST_SEMEN, "検索された精液番号がありません");
            _mJPMessageList.Add(RESULT_SEARCH_NOTEXIST_SCHEDULE, "検索された繁殖日程がありません");
            _mJPMessageList.Add(RESULT_SEARCH_NOTEXIST_SETTING, "検索された繁殖設定がありません");
            _mJPMessageList.Add(RESULT_SEARCH_NOTEXIST_ENTITY, "検索された個体情報がありません");
            _mJPMessageList.Add(RESULT_SEARCH_NOTEXIST_HISTORY, "検索された履歴情報がありません");
            _mJPMessageList.Add(RESULT_SEARCH_NOTEXIST_ALARM, "検索されたアラームリストがありません");
            _mJPMessageList.Add(RESULT_SEARCH_NOTEXIST_DIAGNOSIS, "検索された診断名がありません");
            _mJPMessageList.Add(RESULT_SEARCH_NOTEXIST_PRESCRIPTION, "検索された処方制がありません");
            _mJPMessageList.Add(RESULT_SEARCH_NOTEXIST_CURE, "検索された治療履歴がありません");
            _mJPMessageList.Add(RESULT_SEARCH_NOTEXIST_CHAT, "検索されたチャットのリストがありません");
            _mJPMessageList.Add(RESULT_SEARCH_NOTEXIST_WORKER, "検索された従業員の情報はありません");
            _mJPMessageList.Add(RESULT_SEARCH_NOTEXIST_NOTICE, "検索されたお知らせはありません");
            _mJPMessageList.Add(RESULT_SEARCH_NOTEXIST_PUSH_SETTING, "検索されたアラーム設定がありません");

            _mJPMessageList.Add(RESULT_ERROR_DEVICE_NOTMATCHED, "登録されている端末機ではありません");
            _mJPMessageList.Add(RESULT_ERROR_MEMBER_NOTMATCHED, "登録されている会員ではありません");
            _mJPMessageList.Add(RESULT_ERROR_PASSWORD_NOTMATCHED, "パスワードが一致しません");
            _mJPMessageList.Add(RESULT_ERROR_APPUPDATE_NEEDED, "新しいバージョンのアップデートが必要です");
            _mJPMessageList.Add(RESULT_ERROR_APPINFO_FAILED, "アプリバージョン情報の抽出することができません");
            _mJPMessageList.Add(RESULT_ERROR_TAG_ALREADY_USED, "既に使用されているタグです");
            _mJPMessageList.Add(RESULT_ERROR_HISTORY_EXISTED, "既に授精履歴が登録されている個体履歴情報です");
            _mJPMessageList.Add(RESULT_ERROR_SEMEN_EXISTED, "既に登録されている精液番号です");
            _mJPMessageList.Add(RESULT_ERROR_TAG_ID_EXISTED, "すでにタグが登録されているオブジェクトです");
            _mJPMessageList.Add(RESULT_ERROR_ENTITY_NO_EXISTED, "既に登録されているオブジェクトの番号です");
            _mJPMessageList.Add(RESULT_ERROR_ENTITY_PIN_EXISTED, "既に登録されている個体識別番号です");
            _mJPMessageList.Add(RESULT_ERROR_OSUPDATE_NEEDED, "OSのアップデートが必要になります");

            _mJPMessageList.Add(RESULT_SYSTEM_ERROR_NETWORK, "ネットワークエラーが発生しました");
            _mJPMessageList.Add(RESULT_SYSTEM_ERROR_DATABASE, "データベースエラーが発生しました");
            _mJPMessageList.Add(RESULT_SYSTEM_ERROR_EXCEPTION, "例外的なエラーが発生しました");
        }

        private void AddEnglishMessageList()
        {
            _mENMessageList.Add(RESULT_PARAM_ERROR_UID, "uid parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_PWD, "pwd parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_DEVICE_TYPE, "device_type parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_MODEL, "model parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_SERIAL, "serial parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_USER_TOKEN, "user_token parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_APP_VERSION, "app_version parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_CODE_DIV, "code_div parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_SEMEN_NO, "semen_no parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_SEMEN_SEQ, "semen_seq parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_IMAGE_NAME, "image_name parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_IMAGE_TYPE, "image_type parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_IMAGE_DATA, "image_data parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_FILE_NAME, "file name parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_FARM_SEQ, "farm_seq parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_SEARCH_YEAR, "search_year parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_SEARCH_MONTH, "search_month parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_SEARCH_DATE, "search_date parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_PAGE_INDEX, "page_index parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_ENTITY_SEQ, "entity_seq parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_SEARCH_TYPE, "search_type parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_SEARCH_COUNT, "search_count parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_SEARCH_LEVEL, "search_level parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_PREGNANCY_FLAG, "pregnancy_flag parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_ORDER_FIELD, "order_field parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_ORDER_TYPE, "order_type parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_ENTITY_ID, "entity_id parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_ENTITY_SEX, "entity_sex parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_ENTITY_TYPE, "entity_type parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_DETAIL_TYPE, "detail_type parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_ENTITY_BIRTH, "entity_birth parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_CODE_NO, "code_no parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_CALVE_COUNT, "calve_count parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_ESTRUS_DATE, "estrus_date parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_INSEMINATE_CODE, "inseminate_code parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_DUE_DATE, "due_date parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_INSEMINATE_COUNT, "inseminate_count parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_INSEMINATE_DATE, "inseminate_date parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_APPRAISAL_CODE, "appraisal_code parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_APPRAISAL_DATE, "appraisal_date parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_CALVE_DUE_DATE, "calve_due_date parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_CALVE_DATE, "calve_date parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_CALVE_FLAG, "calve_flag parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_CALVE_CODE, "calve_code parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_LANG_CODE, "lang_code parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_DIAGNOSIS_SEQ, "diagnosis_seq parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_DIAGNOSIS_NAME, "diagnosis_name parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_PRESCRIPTION_SEQ, "prescription_seq parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_PRESCRIPTION_NAME, "prescription_name parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_DIAGNOSIS_DATE, "diagnosis_date parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_CURE_SEQ, "cure_seq parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_FLAG, "flag parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_DEPT_NO, "dept_no parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_TAG_VALUE, "tag_value parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_ENTITY_NO, "entity_no parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_TAG_SEQ, "tag_seq parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_WORKER_SEQ, "worker_seq parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_TAG_SERIAL, "tag_serial parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_NOTICE_SEQ, "notice_seq parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_CHECK_DATE, "check_date parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_HISTORY_SEQ, "history_seq parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_INSEMINATE_DUE_DATE, "inseminate_due_date parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_APPRAISAL_DUE_DATE, "appraisal_due_date parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_SEQ, "seq parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_ID_FLAG, "id_flag parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_AD_FLAG, "ad_flag parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_CD_FLAG, "cd_flag parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_DD_FLAG, "dd_flag parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_AA_FLAG, "aa_flag parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_AN_FLAG, "an_flag parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_KN_FLAG, "kn_flag parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_FROM_DATE, "from_date parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_TO_DATE, "to_date parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_DRYUP_DATE, "dryup_date parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_BREED_SEQ, "breed_seq parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_BREED_DATE, "breed_date parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_HA_FLAG, "ha_flag parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_LA_FLAG, "la_flag parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_ESTRUS_CODE, "estrus_code parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_ENTITY_PIN, "entity_pin parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_ALARM_TITLE, "alarm_title parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_BASE_DATE, "base_date parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_BASE_TYPE, "base_type parameter data not available");
            _mENMessageList.Add(RESULT_PARAM_ERROR_ALARM_SEQ, "alarm_seq parameter data not available");

            _mENMessageList.Add(RESULT_SEARCH_NOTEXIST_CODE, "Code data not found");
            _mENMessageList.Add(RESULT_SEARCH_NOTEXIST_MEMBER, "User data not found");
            _mENMessageList.Add(RESULT_SEARCH_NOTEXIST_FARM, "Farm data not found");
            _mENMessageList.Add(RESULT_SEARCH_NOTEXIST_TAG, "Bio-tag data not found");
            _mENMessageList.Add(RESULT_SEARCH_NOTEXIST_SEMEN, "Semen number not found");
            _mENMessageList.Add(RESULT_SEARCH_NOTEXIST_SCHEDULE, "Scheduled data not found");
            _mENMessageList.Add(RESULT_SEARCH_NOTEXIST_SETTING, "Breeding Configuration data not found");
            _mENMessageList.Add(RESULT_SEARCH_NOTEXIST_ENTITY, "Cattle data not found");
            _mENMessageList.Add(RESULT_SEARCH_NOTEXIST_HISTORY, "Breeding history not found");
            _mENMessageList.Add(RESULT_SEARCH_NOTEXIST_ALARM, "Notification not found");
            _mENMessageList.Add(RESULT_SEARCH_NOTEXIST_DIAGNOSIS, "Diagnosis not found");
            _mENMessageList.Add(RESULT_SEARCH_NOTEXIST_PRESCRIPTION, "Prescription not found");
            _mENMessageList.Add(RESULT_SEARCH_NOTEXIST_CURE, "Medical history not found");
            _mENMessageList.Add(RESULT_SEARCH_NOTEXIST_CHAT, "Chatting message not found");
            _mENMessageList.Add(RESULT_SEARCH_NOTEXIST_WORKER, "Worker not found");
            _mENMessageList.Add(RESULT_SEARCH_NOTEXIST_NOTICE, "Notice not found");
            _mENMessageList.Add(RESULT_SEARCH_NOTEXIST_PUSH_SETTING, "push setting not found");

            _mENMessageList.Add(RESULT_ERROR_DEVICE_NOTMATCHED, "Not registered device");
            _mENMessageList.Add(RESULT_ERROR_MEMBER_NOTMATCHED, "Incorrect user id");
            _mENMessageList.Add(RESULT_ERROR_PASSWORD_NOTMATCHED, "Incorrect password");
            _mENMessageList.Add(RESULT_ERROR_APPUPDATE_NEEDED, "APP Update required");
            _mENMessageList.Add(RESULT_ERROR_APPINFO_FAILED, "Failed to extract APP version");
            _mENMessageList.Add(RESULT_ERROR_TAG_ALREADY_USED, "This bio-tag is being used");
            _mENMessageList.Add(RESULT_ERROR_HISTORY_EXISTED, "Already registered breeding history information");
            _mENMessageList.Add(RESULT_ERROR_SEMEN_EXISTED, "Already registered semen code");
            _mENMessageList.Add(RESULT_ERROR_TAG_ID_EXISTED, "Already registered Bio-tag id");
            _mENMessageList.Add(RESULT_ERROR_ENTITY_NO_EXISTED, "Already registered entity ID");
            _mENMessageList.Add(RESULT_ERROR_ENTITY_PIN_EXISTED, "Already registered entity PIN");
            _mENMessageList.Add(RESULT_ERROR_OSUPDATE_NEEDED, "OS Update required");

            _mENMessageList.Add(RESULT_SYSTEM_ERROR_NETWORK, "A network error occurred");
            _mENMessageList.Add(RESULT_SYSTEM_ERROR_DATABASE, "A database error occurred");
            _mENMessageList.Add(RESULT_SYSTEM_ERROR_EXCEPTION, "An unexpected error occurred");
        }

        private void AddChinaMessageList()
        {
            _mZHMessageList.Add(RESULT_PARAM_ERROR_UID, "uid 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_PWD, "pwd 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_DEVICE_TYPE, "device_type 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_MODEL, "model 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_SERIAL, "serial 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_USER_TOKEN, "user_token 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_APP_VERSION, "app_version 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_CODE_DIV, "code_div 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_SEMEN_NO, "semen_no 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_SEMEN_SEQ, "semen_seq 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_IMAGE_NAME, "image_name 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_IMAGE_TYPE, "image_type 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_IMAGE_DATA, "image_data 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_FILE_NAME, "file name 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_FARM_SEQ, "farm_seq 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_SEARCH_YEAR, "search_year 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_SEARCH_MONTH, "search_month 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_SEARCH_DATE, "search_date 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_PAGE_INDEX, "page_index 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_ENTITY_SEQ, "entity_seq 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_SEARCH_TYPE, "search_type 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_SEARCH_COUNT, "search_count 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_SEARCH_LEVEL, "search_level 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_PREGNANCY_FLAG, "pregnancy_flag 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_ORDER_FIELD, "order_field 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_ORDER_TYPE, "order_type 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_ENTITY_ID, "entity_id 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_ENTITY_SEX, "entity_sex 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_ENTITY_TYPE, "entity_type 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_DETAIL_TYPE, "detail_type 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_ENTITY_BIRTH, "entity_birth 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_CODE_NO, "code_no 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_CALVE_COUNT, "calve_count 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_ESTRUS_DATE, "estrus_date 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_INSEMINATE_CODE, "inseminate_code 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_DUE_DATE, "due_date 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_INSEMINATE_COUNT, "inseminate_count 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_INSEMINATE_DATE, "inseminate_date 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_APPRAISAL_CODE, "appraisal_code 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_APPRAISAL_DATE, "appraisal_date 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_CALVE_DUE_DATE, "calve_due_date 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_CALVE_DATE, "calve_date 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_CALVE_FLAG, "calve_flag 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_CALVE_CODE, "calve_code 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_LANG_CODE, "lang_code 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_DIAGNOSIS_SEQ, "diagnosis_seq 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_DIAGNOSIS_NAME, "diagnosis_name 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_PRESCRIPTION_SEQ, "prescription_seq 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_PRESCRIPTION_NAME, "prescription_name 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_DIAGNOSIS_DATE, "diagnosis_date 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_CURE_SEQ, "cure_seq 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_FLAG, "flag 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_DEPT_NO, "dept_no 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_TAG_VALUE, "tag_value 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_ENTITY_NO, "entity_no 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_TAG_SEQ, "tag_seq 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_WORKER_SEQ, "worker_seq 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_TAG_SERIAL, "tag_serial 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_NOTICE_SEQ, "notice_seq 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_CHECK_DATE, "check_date 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_HISTORY_SEQ, "history_seq 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_INSEMINATE_DUE_DATE, "inseminate_due_date 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_APPRAISAL_DUE_DATE, "appraisal_due_date 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_SEQ, "seq 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_ID_FLAG, "id_flag 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_AD_FLAG, "ad_flag 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_CD_FLAG, "cd_flag 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_DD_FLAG, "dd_flag 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_AA_FLAG, "aa_flag 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_AN_FLAG, "an_flag 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_KN_FLAG, "kn_flag 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_FROM_DATE, "from_date 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_TO_DATE, "to_date 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_DRYUP_DATE, "dryup_date 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_BREED_SEQ, "breed_seq 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_BREED_DATE, "breed_date 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_HA_FLAG, "ha_flag 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_LA_FLAG, "la_flag 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_ESTRUS_CODE, "estrus_code 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_ENTITY_PIN, "entity_pin 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_ALARM_TITLE, "alarm_title 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_BASE_DATE, "base_date 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_BASE_TYPE, "base_type 参数值不正确");
            _mZHMessageList.Add(RESULT_PARAM_ERROR_ALARM_SEQ, "alarm_seq 参数值不正确");

            _mZHMessageList.Add(RESULT_SEARCH_NOTEXIST_CODE, "检索的Cord信息不存在");
            _mZHMessageList.Add(RESULT_SEARCH_NOTEXIST_MEMBER, "检索的会员信息不存在");
            _mZHMessageList.Add(RESULT_SEARCH_NOTEXIST_FARM, "检索的农场信息不存在");
            _mZHMessageList.Add(RESULT_SEARCH_NOTEXIST_TAG, "检索的Tag信息不存在");
            _mZHMessageList.Add(RESULT_SEARCH_NOTEXIST_SEMEN, "检索的精液编号不存在");
            _mZHMessageList.Add(RESULT_SEARCH_NOTEXIST_SCHEDULE, "检索的繁殖日程不存在");
            _mZHMessageList.Add(RESULT_SEARCH_NOTEXIST_SETTING, "检索的繁殖设定不存在");
            _mZHMessageList.Add(RESULT_SEARCH_NOTEXIST_ENTITY, "检索的个体信息不存在");
            _mZHMessageList.Add(RESULT_SEARCH_NOTEXIST_HISTORY, "检索的历史记录信息不存在");
            _mZHMessageList.Add(RESULT_SEARCH_NOTEXIST_ALARM, "检索的提示目录不存在");
            _mZHMessageList.Add(RESULT_SEARCH_NOTEXIST_DIAGNOSIS, "검색된 진단명이 없습니다");
            _mZHMessageList.Add(RESULT_SEARCH_NOTEXIST_PRESCRIPTION, "검색된 처방제가 없습니다");
            _mZHMessageList.Add(RESULT_SEARCH_NOTEXIST_CURE, "검색된 치료이력이 없습니다");
            _mZHMessageList.Add(RESULT_SEARCH_NOTEXIST_CHAT, "검색된 채팅목록이 없습니다");
            _mZHMessageList.Add(RESULT_SEARCH_NOTEXIST_WORKER, "검색된 직원정보가 없습니다");
            _mZHMessageList.Add(RESULT_SEARCH_NOTEXIST_NOTICE, "검색된 공지사항이 없습니다");
            _mZHMessageList.Add(RESULT_SEARCH_NOTEXIST_PUSH_SETTING, "검색된 알람설정이 없습니다");

            _mZHMessageList.Add(RESULT_ERROR_DEVICE_NOTMATCHED, "非注册终端");
            _mZHMessageList.Add(RESULT_ERROR_MEMBER_NOTMATCHED, "非注册会员");
            _mZHMessageList.Add(RESULT_ERROR_PASSWORD_NOTMATCHED, "密码不一致");
            _mZHMessageList.Add(RESULT_ERROR_APPUPDATE_NEEDED, "需要升级新版本");
            _mZHMessageList.Add(RESULT_ERROR_APPINFO_FAILED, "软件版本信息提取失败");
            _mZHMessageList.Add(RESULT_ERROR_TAG_ALREADY_USED, "Tag已被使用");
            _mZHMessageList.Add(RESULT_ERROR_HISTORY_EXISTED, "该个体历史记录已注册授精历史记录");
            _mZHMessageList.Add(RESULT_ERROR_SEMEN_EXISTED, "精液编号已注册");
            _mZHMessageList.Add(RESULT_ERROR_TAG_ID_EXISTED, "이미 태그가 등록된 개체입니다");
            _mZHMessageList.Add(RESULT_ERROR_ENTITY_NO_EXISTED, "이미 둥록된 개체번호입니다");
            _mZHMessageList.Add(RESULT_ERROR_ENTITY_PIN_EXISTED, "이미 둥록된 개체 식별번호입니다");
            _mZHMessageList.Add(RESULT_ERROR_OSUPDATE_NEEDED, "OS Update required");

            _mZHMessageList.Add(RESULT_SYSTEM_ERROR_NETWORK, "网络出现错误");
            _mZHMessageList.Add(RESULT_SYSTEM_ERROR_DATABASE, "数据库出现错误");
            _mZHMessageList.Add(RESULT_SYSTEM_ERROR_EXCEPTION, "出现例外错误");
        }

        private void AddPortugalMessageList()
        {
            _mPTMessageList.Add(RESULT_SUCCESS, string.Empty);

            _mPTMessageList.Add(RESULT_PARAM_ERROR_UID, "uid dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_PWD, "pwd dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_DEVICE_TYPE, "device_type dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_MODEL, "model dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_SERIAL, "serial dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_USER_TOKEN, "user_token dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_APP_VERSION, "app_version dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_CODE_DIV, "code_div dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_SEMEN_NO, "semen_no dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_SEMEN_SEQ, "semen_seq dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_IMAGE_NAME, "image_name dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_IMAGE_TYPE, "image_type dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_IMAGE_DATA, "image_data dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_FILE_NAME, "file name dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_FARM_SEQ, "farm_seq dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_SEARCH_YEAR, "search_year dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_SEARCH_MONTH, "search_month dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_SEARCH_DATE, "search_date dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_PAGE_INDEX, "page_index dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_ENTITY_SEQ, "entity_seq dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_SEARCH_TYPE, "search_type dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_SEARCH_COUNT, "search_count dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_SEARCH_LEVEL, "search_level dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_PREGNANCY_FLAG, "pregnancy_flag dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_ORDER_FIELD, "order_field dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_ORDER_TYPE, "order_type dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_ENTITY_ID, "entity_id dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_ENTITY_SEX, "entity_sex dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_ENTITY_TYPE, "entity_type dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_DETAIL_TYPE, "detail_type dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_ENTITY_BIRTH, "entity_birth dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_CODE_NO, "code_no dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_CALVE_COUNT, "calve_count dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_ESTRUS_DATE, "estrus_date dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_INSEMINATE_CODE, "inseminate_code dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_DUE_DATE, "due_date dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_INSEMINATE_COUNT, "inseminate_count dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_INSEMINATE_DATE, "inseminate_date dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_APPRAISAL_CODE, "appraisal_code dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_APPRAISAL_DATE, "appraisal_date dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_CALVE_DUE_DATE, "calve_due_date dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_CALVE_DATE, "calve_date dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_CALVE_FLAG, "calve_flag dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_CALVE_CODE, "calve_code dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_LANG_CODE, "lang_code dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_DIAGNOSIS_SEQ, "diagnosis_seq dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_DIAGNOSIS_NAME, "diagnosis_name dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_PRESCRIPTION_SEQ, "prescription_seq dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_PRESCRIPTION_NAME, "prescription_name dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_DIAGNOSIS_DATE, "diagnosis_date dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_CURE_SEQ, "cure_seq dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_FLAG, "flag dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_DEPT_NO, "dept_no dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_TAG_VALUE, "tag_value dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_ENTITY_NO, "entity_no dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_TAG_SEQ, "tag_seq dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_WORKER_SEQ, "worker_seq dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_TAG_SERIAL, "tag_serial dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_NOTICE_SEQ, "notice_seq dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_CHECK_DATE, "check_date dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_HISTORY_SEQ, "history_seq dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_INSEMINATE_DUE_DATE, "inseminate_due_date dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_APPRAISAL_DUE_DATE, "appraisal_due_date dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_SEQ, "seq dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_ID_FLAG, "id_flag dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_AD_FLAG, "ad_flag dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_CD_FLAG, "cd_flag dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_DD_FLAG, "dd_flag dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_AA_FLAG, "aa_flag dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_AN_FLAG, "an_flag dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_KN_FLAG, "kn_flag dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_FROM_DATE, "from_date dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_TO_DATE, "to_date dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_DRYUP_DATE, "dryup_date dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_BREED_SEQ, "breed_seq dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_BREED_DATE, "breed_date dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_HA_FLAG, "ha_flag dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_LA_FLAG, "la_flag dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_ESTRUS_CODE, "estrus_code dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_ENTITY_PIN, "entity_pin dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_ALARM_TITLE, "alarm_title dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_BASE_DATE, "base_date dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_BASE_TYPE, "base_type dados de parâmetro não disponíveis");
            _mPTMessageList.Add(RESULT_PARAM_ERROR_ALARM_SEQ, "alarm_seq dados de parâmetro não disponíveis");

            _mPTMessageList.Add(RESULT_SEARCH_NOTEXIST_CODE, "Não existe o dado buscado de código");
            _mPTMessageList.Add(RESULT_SEARCH_NOTEXIST_MEMBER, "Não existe o dado buscado de usuário");
            _mPTMessageList.Add(RESULT_SEARCH_NOTEXIST_FARM, "Não existe o dado buscado de fazenda");
            _mPTMessageList.Add(RESULT_SEARCH_NOTEXIST_TAG, "Não existe o dado buscado de etiqueta");
            _mPTMessageList.Add(RESULT_SEARCH_NOTEXIST_SEMEN, "Não existe o dado buscado de cód.sêmen");
            _mPTMessageList.Add(RESULT_SEARCH_NOTEXIST_SCHEDULE, "Não existe o cronograma de procriação");
            _mPTMessageList.Add(RESULT_SEARCH_NOTEXIST_SETTING, "Não existe a config. buscada de procriação");
            _mPTMessageList.Add(RESULT_SEARCH_NOTEXIST_ENTITY, "Não existe o dado buscado de indivíduo");
            _mPTMessageList.Add(RESULT_SEARCH_NOTEXIST_HISTORY, "Não existe o dado buscado de histórico");
            _mPTMessageList.Add(RESULT_SEARCH_NOTEXIST_ALARM, "Não existe a lista buscada de alarmes");
            _mPTMessageList.Add(RESULT_SEARCH_NOTEXIST_DIAGNOSIS, "Não existe o nome buscado de diagnóstico");
            _mPTMessageList.Add(RESULT_SEARCH_NOTEXIST_PRESCRIPTION, "Não existe o nome buscado de prescrição");
            _mPTMessageList.Add(RESULT_SEARCH_NOTEXIST_CURE, "Não existe o histórico buscado de tratamento");
            _mPTMessageList.Add(RESULT_SEARCH_NOTEXIST_CHAT, "Não existe a lista buscada de bate-papo");
            _mPTMessageList.Add(RESULT_SEARCH_NOTEXIST_WORKER, "Não existe o dado buscado de funcionário(a)");
            _mPTMessageList.Add(RESULT_SEARCH_NOTEXIST_NOTICE, "Não existe o aviso buscado");
            _mPTMessageList.Add(RESULT_SEARCH_NOTEXIST_PUSH_SETTING, "Não existe o alarme configuração");

            _mPTMessageList.Add(RESULT_ERROR_DEVICE_NOTMATCHED, "Dispositivo não registrado");
            _mPTMessageList.Add(RESULT_ERROR_MEMBER_NOTMATCHED, "ID de usuário incorreta");
            _mPTMessageList.Add(RESULT_ERROR_PASSWORD_NOTMATCHED, "Senha incorreta");
            _mPTMessageList.Add(RESULT_ERROR_APPUPDATE_NEEDED, "Atualização de aplicativo necessária");
            _mPTMessageList.Add(RESULT_ERROR_APPINFO_FAILED, "Falha em extrair versão de APP");
            _mPTMessageList.Add(RESULT_ERROR_TAG_ALREADY_USED, "Esta bio-tag está sendo usada");
            _mPTMessageList.Add(RESULT_ERROR_HISTORY_EXISTED, "Históricos de Inseminação já registrados");
            _mPTMessageList.Add(RESULT_ERROR_SEMEN_EXISTED, "Cód. sêmen já registrado");
            _mPTMessageList.Add(RESULT_ERROR_TAG_ID_EXISTED, "O indivíduo já registrado com uma etiqueta");
            _mPTMessageList.Add(RESULT_ERROR_ENTITY_NO_EXISTED, "Entity ID já registrado");
            _mPTMessageList.Add(RESULT_ERROR_ENTITY_PIN_EXISTED, "Entity PIN já registrado");
            _mPTMessageList.Add(RESULT_ERROR_OSUPDATE_NEEDED, "Atualização do sistema operacional é necessária");

            _mPTMessageList.Add(RESULT_SYSTEM_ERROR_NETWORK, "Um erro de rede ocorreu");
            _mPTMessageList.Add(RESULT_SYSTEM_ERROR_DATABASE, "Um erro de banco de dados ocorreu");
            _mPTMessageList.Add(RESULT_SYSTEM_ERROR_EXCEPTION, "Um erro inesperado ocorreu");
        }

        public string GetErrorMessage(string pLangCode, int pCode)
        {
            string sMessage;

            // 언어코드가 없거나 틀린 코드인 경우에는 한글 메세지를 보낸다
            switch (pLangCode)
            {
                case "KR": sMessage = GetKoreanMessage(pCode); break;
                case "JP": sMessage = GetJapanMessage(pCode); break;
                case "US": sMessage = GetEnglishMessage(pCode); break;
                case "CN": sMessage = GetChinaMessage(pCode); break;
                case "PT": sMessage = GetPortugalMessage(pCode); break;
                case "BR": sMessage = GetPortugalMessage(pCode); break;
                default: sMessage = GetKoreanMessage(pCode); break;
            }

            return sMessage;
        }

        private string GetKoreanMessage(int pCode)
        {
            if (_mKRMessageList.ContainsKey(pCode))
                return _mKRMessageList[pCode];
            else
                return _mKRMessageList[RESULT_SYSTEM_ERROR_EXCEPTION];
        }

        private string GetJapanMessage(int pCode)
        {
            if (_mJPMessageList.ContainsKey(pCode))
                return _mJPMessageList[pCode];
            else
                return _mJPMessageList[RESULT_SYSTEM_ERROR_EXCEPTION];
        }

        private string GetEnglishMessage(int pCode)
        {
            if (_mENMessageList.ContainsKey(pCode))
                return _mENMessageList[pCode];
            else
                return _mENMessageList[RESULT_SYSTEM_ERROR_EXCEPTION];
        }

        private string GetChinaMessage(int pCode)
        {
            if (_mZHMessageList.ContainsKey(pCode))
                return _mZHMessageList[pCode];
            else
                return _mZHMessageList[RESULT_SYSTEM_ERROR_EXCEPTION];
        }

        private string GetPortugalMessage(int pCode)
        {
            if (_mPTMessageList.ContainsKey(pCode))
                return _mPTMessageList[pCode];
            else
                return _mPTMessageList[RESULT_SYSTEM_ERROR_EXCEPTION];
        }
    }
}