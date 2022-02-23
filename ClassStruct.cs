using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LC_Service
{
    public class ClassStruct
    {
        public static string[] LANGUAGE_CODE = { "AR", "AU", "BR", "BG", "CA", "CH", "CN", "CZ", "DE", "DK", "ES", "FI", "FR", "GB", "GR", "HU", "ID", "IT", "JP", "KR", "NL", "PL", "PT", "RO", "RU", "SE", "SK", "SI", "TR", "TW", "US" };

        public struct ST_DATABASE
        {
            public string db_server;
            public string db_name;
            public string db_user;
            public string db_password;
        }

        public struct ST_USERINFO
        {
            public string user_name;
            public string user_type;
            public string user_kind;
            public int farm_seq;
            public string profile_url;
        }

        public struct ST_CODEINFO
        {
            public string code_div;
            public string div_name;
            public int code_no;
            public string code_name;
        }

        public struct ST_FARMINFO
        {
            public int farm_seq;
            public string farm_name;
            public string owner;
            public string tel_no;
            public string address;
            public int entity_type;
            public string entity_type_disp;
        }

        public struct ST_ENTITYINFO
        {
            public int entity_seq;
            public string entity_id;
            public int entity_sex;
            public string entity_sex_disp;
            public int entity_type;
            public string entity_type_disp;
            public int detail_type;
            public string detail_type_disp;
            public int entity_kind;
            public string entity_kind_disp;
            public string birth_date;
            public int birth_month;
            public string temp_min;
            public string temp_max;
            public string image_url;
            public string warning_level;
            public string warning_level_disp;
            public int drink_count;
            public string tag_id;
            public string pregnancy_flag;
            public string entity_pin;
            public string favorite_flag;
            public string shipment_date;
            public string shipment_reason;
            public string calve_flag;
            public int calve_count;
            public string sire_name;
            public string sire_info;
            public string dam_name;
            public string dam_info;
            public string gs_name;
            public string gs_info;
            public string ggs_name;
            public string ggs_info;
        }

        public struct ST_CHART_DONUT
        {
            public int high_count;
            public int normal_count;
            public int low_count;
        }

        public struct ST_CHART_LINE
        {
            public int seq;
            public string time_disp;
            public double time_value;
            public string normal_flag;
        }

        public struct ST_COLOR_INFO
        {
            public string time_disp;
            public string time_value;
            public string normal_flag;
        }

        public struct ST_CHART_COLOR
        {
            public string day_disp;
            public List<ST_COLOR_INFO> day_value;
        }

        public struct ST_CHART_ALARM
        {
            public int seq;
            public string warning_time;
            public int warning_level;
            public string warning_level_disp;
        }

        public struct ST_CHART_ACTIVITY
        {
            public string time_disp;
            public double activity_value;
            public double avg_value;
        }

        public struct ST_HISTORY_DETAIL
        {
            public string breed_code;
            public string breed_title;
            public int breed_day;
            public int calve_count;
            public string last_calve_date;
            public int pregnancy_code;
            public string pregnancy_code_disp;
            public int entity_kind;
            public string entity_kind_disp;
        }

        public struct ST_BREEDINFO
        {
            public int entity_seq;
            public string entity_id;
            public string image_url;
            public string breed_code;
            public string breed_title;
            public string breed_date;
            public int breed_day;
        }

        public struct ST_GROUPCOUNT
        {
            public string code_div;
            public string div_name;
            public int code_no;
            public string code_name;
            public int group_count;
        }

        public struct ST_HISTORYGROUP
        {
            public int today_count;
            public List<ST_GROUPCOUNT> group_info;
        }

        public struct ST_REQUESTCODE
        {
            public string code_div;
            public int code_no;
        }

        public struct ST_HISTORYMONTH
        {
            public string history_date;
            public string ed_flag;
            public string id_flag;
            public string ad_flag;
            public string dd_flag;
            public string cd_flag;
        }

        public struct ST_TAGINFO
        {
            public string farm_name;
            public string tag_id;
            public string make_date;
        }

        public struct ST_HISTORYDAYLIST
        {
            public ST_HISTORYDAYINFO estrus_info;
            public ST_HISTORYDAYINFO inseminate_info;
            public ST_HISTORYDAYINFO appraisal_info;
            public ST_HISTORYDAYINFO dryup_info;
            public ST_HISTORYDAYINFO calve_info;
        }

        public struct ST_HISTORYDAYINFO
        {
            public string breed_code;
            public string breed_title;
            public List<ST_HISTORYDAYENTITY> entity_list;
        }

        public struct ST_HISTORYDAYENTITY
        {
            public int entity_seq;
            public string entity_id;
            public string image_url;
            public string breed_date;
            public int breed_day;
            public string breed_method;
        }

        public struct ST_BREEDMONTH
        {
            public string history_date;
            public string ed_flag;
            public string id_flag;
            public string ad_flag;
            public string dd_flag;
            public string cd_flag;
        }

        public struct ST_BREEDDAYLIST
        {
            public ST_BREEDDAYINFO estrus_info;
            public ST_BREEDDAYINFO inseminate_info;
            public ST_BREEDDAYINFO appraisal_info;
            public ST_BREEDDAYINFO dryup_info;
            public ST_BREEDDAYINFO calve_info;
        }

        public struct ST_BREEDDAYINFO
        {
            public string breed_code;
            public string breed_title;
            public List<ST_BREEDDAYENTITY> entity_list;
        }

        public struct ST_BREEDDAYENTITY
        {
            public int entity_seq;
            public string entity_id;
            public string image_url;
            public string pregnancy_flag;
            public string inseminate_due_date;
            public string semen_no;
            public string calve_due_date;
            public string estrus_next_date;
            public string calve_flag;
            public string calve_code_disp;
        }

        public struct ST_SEMENINFO
        {
            public int semen_seq;
            public string semen_no;
        }

        public struct ST_BREEDDEFAULT
        {
            public int semen_seq;
            public string semen_no;
            public int inseminate_code;
            public string inseminate_code_disp;
            public int inseminate_day;
            public int appraisal_code;
            public string appraisal_code_disp;
            public int appraisal_day;
            public string dryup_method;
            public int dryup_month;
            public int dryup_day;
            public string calve_method;
            public int calve_month;
            public int calve_day;
            public int estrus_b_day;
            public int estrus_n_day;
        }

        public struct ST_INSEMINATEDUE
        {
            public int inseminate_code;
            public string inseminate_code_disp;
            public int inseminate_day;
            public int calve_m_month;
            public int calve_m_day;
            public int calve_d_day;
            public int dryup_m_month;
            public int dryup_m_day;
            public int dryup_d_day;
        }

        public struct ST_APPRAISALDUE
        {
            public int appraisal_code;
            public string appraisal_code_disp;
            public int appraisal_day;
        }

        public struct ST_DUEDAYINFO
        {
            public List<ST_INSEMINATEDUE> inseminate_dueday;
            public List<ST_APPRAISALDUE> appraisal_dueday;
        }

        public struct ST_PUSHINFO
        {
            public int entity_seq;
            public string entity_id;
            public string alarm_code;
            public string alarm_type;
            public string alarm_date;
            public string add_title;
            public string add_date;
        }

        public struct ST_PREGNANCY
        {
            public string pregnancy_flag;
        }

        public struct ST_ENTITYHISTORY_INFO
        {
            public int breed_seq;
            public string breed_date;
            public string breed_type;
            public string pregnancy_flag;
            public int calve_terms;
            public string calve_flag;
            public int calve_code;
            public string calve_code_disp;
            public string semen_no;
        }

        public struct ST_DIAGNOSIS_INFO
        {
            public int diagnosis_seq;
            public string diagnosis_name;
        }

        public struct ST_PRESCRIPTION_INFO
        {
            public int prescription_seq;
            public string prescription_type;
            public string prescription_name;
            public string ingredient;
        }

        public struct ST_CURE_PRESCRIPTION_PROC
        {
            public string flag;
            public int seq;
            public string prescription_date;
            public int prescription_seq;
            public string memo;
        }

        public struct ST_CURE_IMAGE_PROC
        {
            public string flag;
            public int seq;
            public string image_name;
        }

        public struct ST_CURE_LIST
        {
            public int seq;
            public int diagnosis_seq;
            public string diagnosis_name;
            public string diagnosis_date;
            public string prescription_name;
            public string prescription_date;
        }

        public struct ST_CURE_PRESCRIPTION
        {
            public int seq;
            public string prescription_date;
            public int prescription_seq;
            public string prescription_name;
            public string memo;
        }

        public struct ST_CURE_IMAGE
        {
            public int seq;
            public string image_url;
        }

        public struct ST_CURE_INFO
        {
            public int cure_seq;
            public int diagnosis_seq;
            public string diagnosis_name;
            public string diagnosis_date;
            public List<ST_CURE_PRESCRIPTION> prescription_list;
            public List<ST_CURE_IMAGE> image_list;
        }

        public struct ST_CHATINFO
        {
            public int message_no;
            public string user_id;
            public string user_name;
            public string user_profile;
            public string message_time;
            public string message_type;
            public string message_data;
        }

        public struct ST_LORA_TAGINFO
        {
            public int seq;
            public string app_eui;
            public string tag_id;
            public string tag_serial;
            public string dev_eui;
        }

        public struct ST_LORATAG_FULLINFO
        {
            public int seq;
            public int farm_seq;
            public int entity_seq;
            public string entity_no;
            public string tag_id;
            public string tag_type;
            public string tag_kind;
            public string tag_version;
            public string tag_serial;
            public string app_eui;
            public string dev_eui;
            public string make_date;
            public string user_id;
            public string server_url;
            public string auth_key;
            public string gateway;
        }

        public struct ST_WORKERINFO
        {
            public int worker_seq;
            public string worker_name;
        }

        public struct ST_MAPPINGENTITY
        {
            public int seq;
            public string entity_no;
        }

        public struct ST_LORA_ENTITY
        {
            public int entity_seq;
            public string entity_id;
            public int entity_sex;
            public string entity_sex_disp;
            public int entity_type;
            public string entity_type_disp;
            public int detail_type;
            public string detail_type_disp;
            public int entity_kind;
            public string entity_kind_disp;
            public string birth_date;
            public int birth_month;
            public string temp_min;
            public string temp_max;
            public string image_url;
            public string tag_id;
            public int tag_count;
            public int inject_count;
            public string entity_pin;
            public int drink_count;
            public string pregnancy_flag;
            public string calve_due_date;
            public int calve_due_remain;
        }

        public struct ST_NOTICE_INFO
        {
            public int notice_seq;
            public string notice_date;
            public string title;
            public string contents;
            public string new_flag;
        }

        public struct ST_NOTICE_DETAIL
        {
            public int notice_seq;
            public string notice_date;
            public string title;
            public string contents;
        }

        public struct ST_DASHBOARD
        {
            public int farm_seq;
            public string farm_name;
            public int entity_seq;
            public string entity_id;
            public int entity_sex;
            public string entity_sex_disp;
            public int entity_type;
            public string entity_type_disp;
            public int detail_type;
            public string detail_type_disp;
            public int entity_kind;
            public string entity_kind_disp;
            public string birth_date;
            public int birth_month;
            public string temp_min;
            public string temp_max;
            public string image_url;
            public string warning_level;
            public string warning_level_disp;
            public int drink_count;
            public string tag_id;
            public string pregnancy_flag;
            public string entity_pin;
            public string favorite_flag;
        }

        public struct ST_STATISTICS_INFO
        {
            public ST_STATISTICS_PREGNANCY pregnancy_info;
            public ST_STATISTICS_DISPLAY calve_info;
            public ST_STATISTICS_DISPLAY kind_info;
            public ST_STATISTICS_DISPLAY schedule_pre_info;
            public ST_STATISTICS_DISPLAY schedule_pos_info;
            public ST_STATISTICS_DISPLAY breed_info;
        }

        public struct ST_STATISTICS_DISPLAY
        {
            public string display_name;
            public string display_count;
        }

        public struct ST_STATISTICS_PREGNANCY
        {
            public string display_name;
            public string pregnancy_count;
            public string nopregnancy_count;
        }

        public struct ST_COUNT_INFO
        {
            public int ed_count;
            public int id_count;
            public int ad_count;
            public int dd_count;
            public int cd_count;
        }

        public struct GEMTEK_SENSOR_DATA
        {
            public string DATA_TYPE;
            public Tuple<int, int, int> SENSOR_TURPLE;
            public int INT_COUNTER;
            public double INT_TIME;
            public double SENSOR_VALUE;
        }

        public struct ST_USER_PUSH
        {
            public string ha_flag;
            public string la_flag;
            public string aa_flag;
            public string an_flag;
            public string kn_flag;
            public string id_flag;
            public string ad_flag;
            public string cd_flag;
            public string dd_flag;
        }

        public struct ST_ESTRUS_INFO
        {
            public string estrus_date;
            public string estrus_due_date;
            public string inseminate_due_date;
            public int inseminate_code;
            public string memo;
        }

        public struct ST_INSEMINATE_INFO
        {
            public string inseminate_date;
            public int inseminate_code;
            public int inseminate_count;
            public int semen_seq;
            public string semen_no;
            public string appraisal_due_date;
            public int appraisal_code;
            public string memo;
        }

        public struct ST_APPRAISAL_INFO
        {
            public string appraisal_date;
            public string pregnancy_flag;
            public int appraisal_code;
            public string calve_due_date;
            public string dryup_due_date;
            public string inseminate_date;
            public int inseminate_code;
            public string memo;
        }

        public struct ST_DRYUP_INFO
        {
            public string dryup_date;
            public string pregnancy_flag;
            public string calve_due_date;
            public string inseminate_date;
            public int inseminate_code;
            public string memo;
        }

        public struct ST_CALVE_INFO
        {
            public string calve_date;
            public string calve_flag;
            public int calve_code;
            public int calve_count;
            public string estrus_due_date;
            public string memo;
        }

        public struct ST_LAST_INSEMINATE
        {
            public string inseminate_date;
            public int inseminate_code;
        }

        public struct ST_LAST_APPRAISAL
        {
            public string pregnancy_flag;
            public string calve_due_date;
            public string inseminate_date;
            public int inseminate_code;
        }

        public struct ST_SENSOR_SETTING
        {
            public int RSSI;
            public int SNR;
            public int ATH;
            public int ADT;
            public int IDT;
            public int ITH;
            public int INTERVAL;
            public double BATTERY;
            public int HALL_SENSOR;
        }

        public struct ST_SENSOR_DATA
        {
            public string DATA_TYPE;
            public int INT_COUNTER;
            public double INT_TIME;
            public double TEMP_VALUE;
            public int SENSOR_VALUE;
            public Tuple<int, int, int> SENSOR_DATA;
            public Tuple<double, double, double> ANGLE_DATA;
        }

        public struct ST_CALVE_DUEDAY_INFO
        {
            public int inseminate_code;
            public string inseminate_code_disp;
            public int due_day;
        }

        public struct ST_ESTRUS_DUEDAY_INFO
        {
            public string estrus_code;
            public string estrus_code_disp;
            public int due_day;
        }

        public struct ST_DUEDAY_COMPUTE
        {
            public List<ST_CALVE_DUEDAY_INFO> calve_dueday;
            public List<ST_ESTRUS_DUEDAY_INFO> estrus_dueday;
        }

        public struct ST_PIN_INFO
        {
            public int entity_sex;
            public int entity_type;
            public string birth_day;
            public int birth_month;
            public int birth_code;
        }

        public struct ST_USER_ALARM
        {
            public int alarm_seq;
            public string alarm_title;
            public string base_date;
            public string base_type;
            public int alarm_day;
        }

        public struct ST_CALVE_DUE_DAY
        {
            public int inseminate_code;
            public int due_day;
        }

        public struct ST_ESTRUS_DUE_DAY
        {
            public string estrus_code;
            public int due_day;
        }

        public struct ST_CHART_INTERUPT
        {
            public int int_average;
            public List<ST_INTERUPT_INFO> int_list;
        }

        public struct ST_INTERUPT_INFO
        {
            public string time_disp;
            public int interupt_value;
        }
    }
}