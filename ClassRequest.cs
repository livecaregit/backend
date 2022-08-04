using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LC_Service
{
    public class ClassRequest
    {
        [DataContract]
        public class REQ_LOGIN
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public string uid { get; set; }
            [DataMember(Order = 2)]
            public string pwd { get; set; }
            [DataMember(Order = 3)]
            public string device_type { get; set; }
            [DataMember(Order = 4)]
            public string user_token { get; set; }
            [DataMember(Order = 5)]
            public string app_version { get; set; }
            [DataMember(Order = 6)]
            public string model { get; set; }
            [DataMember(Order = 7)]
            public string serial { get; set; }
            [DataMember(Order = 8)]
            public string os_version { get; set; }
        }

        [DataContract]
        public class REQ_USERUPDATE
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public string uid { get; set; }
            [DataMember(Order = 2)]
            public string device_type { get; set; }
            [DataMember(Order = 3)]
            public string user_token { get; set; }
        }

        [DataContract]
        public class REQ_CODELIST
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public string code_div { get; set; }
        }

        [DataContract]
        public class REQ_UID
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public string uid { get; set; }
        }

        [DataContract]
        public class REQ_FARMLIST
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public string uid { get; set; }
            [DataMember(Order = 2)]
            public int entity_type { get; set; }
            [DataMember(Order = 3)]
            public int nation_code{ get; set; }
            [DataMember(Order = 4)]
            public int region_code{ get; set; }
            [DataMember(Order = 5)]
            public int partner_code{ get; set; }
            [DataMember(Order = 6)]
            public int branch_code{ get; set; }
            [DataMember(Order = 7)]
            public string address{ get; set; }
            [DataMember(Order = 8)]
            public string farm_name { get; set; }
        }

        [DataContract]
        public class REQ_FARMSEQ
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
        }

        [DataContract]
        public class REQ_ENTITYLIST
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public string uid { get; set; }
            [DataMember(Order = 2)]
            public int farm_seq { get; set; }
            [DataMember(Order = 3)]
            public string search_type { get; set; }
            [DataMember(Order = 4)]
            public int entity_seq { get; set; }
            [DataMember(Order = 5)]
            public string entity_id { get; set; }
            [DataMember(Order = 6)]
            public string dryup_flag { get; set; }
            [DataMember(Order = 7)]
            public string calve_flag { get; set; }
            [DataMember(Order = 8)]
            public string pregnancy_flag { get; set; }
            [DataMember(Order = 9)]
            public string breed_status { get; set; }
            [DataMember(Order = 10)]
            public string drink_flag { get; set; }
            [DataMember(Order = 11)]
            public string search_level { get; set; }
            [DataMember(Order = 12)]
            public string order_field { get; set; }
            [DataMember(Order = 13)]
            public string order_type { get; set; }
            [DataMember(Order = 14)]
            public int page_index { get; set; }
        }

        [DataContract]
        public class REQ_FARMENTITY
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
            [DataMember(Order = 2)]
            public int entity_seq { get; set; }
        }

        [DataContract]
        public class REQ_FARMPAGESEQ
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
            [DataMember(Order = 2)]
            public int entity_seq { get; set; }
            [DataMember(Order = 3)]
            public int page_seq { get; set; }
        }

        [DataContract]
        public class REQ_HISTORYINFO
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
            [DataMember(Order = 2)]
            public int entity_seq { get; set; }
            [DataMember(Order = 3)]
            public int history_seq { get; set; }
        }

        [DataContract]
        public class REQ_HISTORYLIST
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public string uid { get; set; }
            [DataMember(Order = 2)]
            public int farm_seq { get; set; }
            [DataMember(Order = 3)]
            public string search_type { get; set; }
            [DataMember(Order = 4)]
            public string entity_id { get; set; }
            [DataMember(Order = 5)]
            public string dryup_flag { get; set; }
            [DataMember(Order = 6)]
            public string calve_flag { get; set; }
            [DataMember(Order = 7)]
            public string pregnancy_flag { get; set; }
            [DataMember(Order = 8)]
            public string breed_status { get; set; }
            [DataMember(Order = 9)]
            public int page_index { get; set; }
        }

        [DataContract]
        public class REQ_TODAYLIST
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
            [DataMember(Order = 2)]
            public int page_index { get; set; }
        }

        [DataContract]
        public class REQ_HISTORYGROUP
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
            [DataMember(Order = 2)]
            public List<ClassStruct.ST_REQUESTCODE> group_list { get; set; }
        }

        [DataContract]
        public class REQ_HISTORYMONTH
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
            [DataMember(Order = 2)]
            public int search_year { get; set; }
            [DataMember(Order = 3)]
            public int search_month { get; set; }
        }

        [DataContract]
        public class REQ_ENTITYINSERT
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
            [DataMember(Order = 2)]
            public string entity_pin { get; set; }
            [DataMember(Order = 3)]
            public string entity_id { get; set; }
            [DataMember(Order = 4)]
            public string tag_id { get; set; }
            [DataMember(Order = 5)]
            public int entity_sex { get; set; }
            [DataMember(Order = 6)]
            public int entity_type { get; set; }
            [DataMember(Order = 7)]
            public int detail_type { get; set; }
            [DataMember(Order = 8)]
            public int entity_kind { get; set; }
            [DataMember(Order = 9)]
            public int calve_count { get; set; }
            [DataMember(Order = 10)]
            public string entity_birth { get; set; }
            [DataMember(Order = 11)]
            public string image_name { get; set; }
            [DataMember(Order = 12)]
            public string shipment { get; set; }
            [DataMember(Order = 13)]
            public string reason { get; set; }
            [DataMember(Order = 14)]
            public string sire_name { get; set; }
            [DataMember(Order = 15)]
            public string sire_info { get; set; }
            [DataMember(Order = 16)]
            public string dam_name { get; set; }
            [DataMember(Order = 17)]
            public string dam_info { get; set; }
            [DataMember(Order = 18)]
            public string gs_name { get; set; }
            [DataMember(Order = 19)]
            public string gs_info { get; set; }
            [DataMember(Order = 20)]
            public string ggs_name { get; set; }
            [DataMember(Order = 21)]
            public string ggs_info { get; set; }
        }

        [DataContract]
        public class REQ_ENTITYUPDATE
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
            [DataMember(Order = 2)]
            public int entity_seq { get; set; }
            [DataMember(Order = 3)]
            public string entity_pin { get; set; }
            [DataMember(Order = 4)]
            public string entity_id { get; set; }
            [DataMember(Order = 5)]
            public string tag_id { get; set; }
            [DataMember(Order = 6)]
            public int entity_sex { get; set; }
            [DataMember(Order = 7)]
            public int entity_type { get; set; }
            [DataMember(Order = 8)]
            public int detail_type { get; set; }
            [DataMember(Order = 9)]
            public int entity_kind { get; set; }
            [DataMember(Order = 10)]
            public int calve_count { get; set; }
            [DataMember(Order = 11)]
            public string entity_birth { get; set; }
            [DataMember(Order = 12)]
            public string image_name { get; set; }
            [DataMember(Order = 13)]
            public string shipment { get; set; }
            [DataMember(Order = 14)]
            public string reason { get; set; }
            [DataMember(Order = 15)]
            public string sire_name { get; set; }
            [DataMember(Order = 16)]
            public string sire_info { get; set; }
            [DataMember(Order = 17)]
            public string dam_name { get; set; }
            [DataMember(Order = 18)]
            public string dam_info { get; set; }
            [DataMember(Order = 19)]
            public string gs_name { get; set; }
            [DataMember(Order = 20)]
            public string gs_info { get; set; }
            [DataMember(Order = 21)]
            public string ggs_name { get; set; }
            [DataMember(Order = 22)]
            public string ggs_info { get; set; }
        }

        [DataContract]
        public class REQ_GROUPLIST
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
            [DataMember(Order = 2)]
            public string code_div { get; set; }
            [DataMember(Order = 3)]
            public int code_no  { get; set; }
            [DataMember(Order = 4)]
            public int page_index { get; set; }
        }

        [DataContract]
        public class REQ_HISTORYDATE
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
            [DataMember(Order = 2)]
            public string search_date { get; set; }
        }

        [DataContract]
        public class REQ_SEMENNO
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
            [DataMember(Order = 2)]
            public string semen_no { get; set; }
        }

        [DataContract]
        public class REQ_SEMENSEQ
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
            [DataMember(Order = 2)]
            public int semen_seq { get; set; }
        }

        [DataContract]
        public class REQ_ESTRUS
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public string uid { get; set; }
            [DataMember(Order = 2)]
            public int farm_seq { get; set; }
            [DataMember(Order = 3)]
            public int entity_seq { get; set; }
            [DataMember(Order = 4)]
            public int breed_seq { get; set; }
            [DataMember(Order = 5)]
            public string estrus_date { get; set; }
            [DataMember(Order = 6)]
            public string estrus_due_date { get; set; }
            [DataMember(Order = 7)]
            public string due_date { get; set; }
            [DataMember(Order = 8)]
            public int inseminate_code { get; set; }
            [DataMember(Order = 9)]
            public string memo { get; set; }
        }

        [DataContract]
        public class REQ_INSEMINNATE
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public string uid { get; set; }
            [DataMember(Order = 2)]
            public int farm_seq { get; set; }
            [DataMember(Order = 3)]
            public int entity_seq { get; set; }
            [DataMember(Order = 4)]
            public int breed_seq { get; set; }
            [DataMember(Order = 5)]
            public string inseminate_date { get; set; }
            [DataMember(Order = 6)]
            public int inseminate_code { get; set; }
            [DataMember(Order = 7)]
            public int inseminate_count { get; set; }
            [DataMember(Order = 8)]
            public string semen_no { get; set; }
            [DataMember(Order = 9)]
            public string due_date { get; set; }
            [DataMember(Order = 10)]
            public int appraisal_code { get; set; }
            [DataMember(Order = 11)]
            public string memo { get; set; }
        }

        [DataContract]
        public class REQ_APPRAISAL
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public string uid { get; set; }
            [DataMember(Order = 2)]
            public int farm_seq { get; set; }
            [DataMember(Order = 3)]
            public int entity_seq { get; set; }
            [DataMember(Order = 4)]
            public int breed_seq { get; set; }
            [DataMember(Order = 5)]
            public string appraisal_date { get; set; }
            [DataMember(Order = 6)]
            public string pregnancy_flag { get; set; }
            [DataMember(Order = 7)]
            public int appraisal_code { get; set; }
            [DataMember(Order = 8)]
            public string calve_due_date { get; set; }
            [DataMember(Order = 9)]
            public string dryup_due_date { get; set; }
            [DataMember(Order = 10)]
            public string inseminate_date { get; set; }
            [DataMember(Order = 11)]
            public int inseminate_code { get; set; }
            [DataMember(Order = 12)]
            public string memo { get; set; }
        }

        [DataContract]
        public class REQ_DRYUP
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public string uid { get; set; }
            [DataMember(Order = 2)]
            public int farm_seq { get; set; }
            [DataMember(Order = 3)]
            public int entity_seq { get; set; }
            [DataMember(Order = 4)]
            public int breed_seq { get; set; }
            [DataMember(Order = 5)]
            public string dryup_date{ get; set; }
            [DataMember(Order = 6)]
            public string pregnancy_flag { get; set; }
            [DataMember(Order = 7)]
            public string calve_due_date { get; set; }
            [DataMember(Order = 8)]
            public string inseminate_date { get; set; }
            [DataMember(Order = 9)]
            public int inseminate_code { get; set; }
            [DataMember(Order = 10)]
            public string memo { get; set; }
        }

        [DataContract]
        public class REQ_CALVE
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public string uid { get; set; }
            [DataMember(Order = 2)]
            public int farm_seq { get; set; }
            [DataMember(Order = 3)]
            public int entity_seq { get; set; }
            [DataMember(Order = 4)]
            public int breed_seq { get; set; }
            [DataMember(Order = 5)]
            public string calve_date { get; set; }
            [DataMember(Order = 6)]
            public string calve_flag { get; set; }
            [DataMember(Order = 7)]
            public int calve_code { get; set; }
            [DataMember(Order = 8)]
            public int calve_count { get; set; }
            [DataMember(Order = 9)]
            public string due_date { get; set; }
            [DataMember(Order = 10)]
            public string memo { get; set; }
        }

        [DataContract]
        public class REQ_BREEDINFO
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
            [DataMember(Order = 2)]
            public int entity_seq { get; set; }
            [DataMember(Order = 3)]
            public int breed_seq { get; set; }
        }

        [DataContract]
        public class REQ_UPLOADIMAGE
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public string image_kind { get; set; }
            [DataMember(Order = 2)]
            public string image_name { get; set; }
            [DataMember(Order = 3)]
            public string image_type { get; set; }
            [DataMember(Order = 4)]
            public string image_data { get; set; }
        }

        [DataContract]
        public class REQ_LANGCODE
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
        }

        [DataContract]
        public class REQ_ENTITYHISTORY
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
            [DataMember(Order = 2)]
            public int entity_seq { get; set; }
            [DataMember(Order = 3)]
            public int search_year { get; set; }
            [DataMember(Order = 4)]
            public string search_flag { get; set; }
        }

        [DataContract]
        public class REQ_VERSION
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public string device_type { get; set; }
            [DataMember(Order = 2)]
            public string app_version { get; set; }
        }

        [DataContract]
        public class REQ_DIAGNOSIS_INFO
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
            [DataMember(Order = 2)]
            public int diagnosis_seq { get; set; }
            [DataMember(Order = 3)]
            public string diagnosis_name { get; set; }
        }

        [DataContract]
        public class REQ_PRESCRIPTION_INFO
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
            [DataMember(Order = 2)]
            public int prescription_seq { get; set; }
            [DataMember(Order = 3)]
            public string prescription_type { get; set; }
            [DataMember(Order = 4)]
            public string prescription_name { get; set; }
            [DataMember(Order = 5)]
            public string ingredient { get; set; }
        }

        [DataContract]
        public class REQ_CURE_INFO
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
            [DataMember(Order = 2)]
            public int entity_seq { get; set; }
            [DataMember(Order = 3)]
            public int cure_seq { get; set; }
        }

        [DataContract]
        public class REQ_CURE_INSERT
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
            [DataMember(Order = 2)]
            public int entity_seq { get; set; }
            [DataMember(Order = 3)]
            public string diagnosis_date { get; set; }
            [DataMember(Order = 4)]
            public int diagnosis_seq { get; set; }
            [DataMember(Order = 5)]
            public List<ClassStruct.ST_CURE_PRESCRIPTION_PROC> prescription_list { get; set; }
            [DataMember(Order = 6)]
            public List<ClassStruct.ST_CURE_IMAGE_PROC> image_list { get; set; }
        }

        [DataContract]
        public class REQ_CURE_UPDATE
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
            [DataMember(Order = 2)]
            public int entity_seq { get; set; }
            [DataMember(Order = 3)]
            public int cure_seq { get; set; }
            [DataMember(Order = 4)]
            public string diagnosis_date { get; set; }
            [DataMember(Order = 5)]
            public int diagnosis_seq { get; set; }
            [DataMember(Order = 6)]
            public List<ClassStruct.ST_CURE_PRESCRIPTION_PROC> prescription_list { get; set; }
            [DataMember(Order = 7)]
            public List<ClassStruct.ST_CURE_IMAGE_PROC> image_list { get; set; }
        }

        [DataContract]
        public class REQ_USER_ID
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public string uid { get; set; }
        }

        [DataContract]
        public class REQ_PROFILE
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public string uid { get; set; }
            [DataMember(Order = 2)]
            public string image_name { get; set; }
        }

        [DataContract]
        public class REQ_CHATLIST
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
            [DataMember(Order = 2)]
            public string uid { get; set; }
            [DataMember(Order = 3)]
            public int msg_no { get; set; }
        }

        [DataContract]
        public class REQ_TAGINSERT
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
            [DataMember(Order = 2)]
            public List<ClassStruct.ST_LORA_TAGINFO> tag_list { get; set; }
        }

        [DataContract]
        public class REQ_LORA_TAGDATA
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
            [DataMember(Order = 2)]
            public string tag_serial { get; set; }
        }

        [DataContract]
        public class REQ_LORA_ENTITYDATA
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
            [DataMember(Order = 2)]
            public string entity_no { get; set; }
            [DataMember(Order = 3)]
            public string tag_serial { get; set; }
        }

        [DataContract]
        public class REQ_WORKERLIST
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int dept_no { get; set; }
        }

        [DataContract]
        public class REQ_TAGINFO
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public string tag_value { get; set; }
        }

        [DataContract]
        public class REQ_ENTITYINFO
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
            [DataMember(Order = 2)]
            public string entity_no { get; set; }
        }

        [DataContract]
        public class REQ_TAG_MAPPING
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
            [DataMember(Order = 2)]
            public int entity_seq { get; set; }
            [DataMember(Order = 3)]
            public int tag_seq { get; set; }
            [DataMember(Order = 4)]
            public int worker_seq { get; set; }
            [DataMember(Order = 5)]
            public string image_name { get; set; }
        }

        [DataContract]
        public class REQ_LORA_LOGIN
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public string uid { get; set; }
            [DataMember(Order = 2)]
            public string pwd { get; set; }
        }

        [DataContract]
        public class REQ_TAG_CHANGE
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
            [DataMember(Order = 2)]
            public int entity_seq { get; set; }
            [DataMember(Order = 3)]
            public string tag_serial { get; set; }
            [DataMember(Order = 4)]
            public int worker_seq { get; set; }
            [DataMember(Order = 5)]
            public string image_name { get; set; }
        }

        [DataContract]
        public class REQ_NOTICE_INFO
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
            [DataMember(Order = 2)]
            public string uid { get; set; }
        }

        [DataContract]
        public class REQ_NOTICE_DETAIL
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int notice_seq { get; set; }
            [DataMember(Order = 2)]
            public int farm_seq { get; set; }
            [DataMember(Order = 3)]
            public string uid { get; set; }
        }

        [DataContract]
        public class REQ_CHART_LINE
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
            [DataMember(Order = 2)]
            public int entity_seq { get; set; }
            [DataMember(Order = 3)]
            public string check_date { get; set; }
        }

        [DataContract]
        public class REQ_CHART_LINE_RANGE
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
            [DataMember(Order = 2)]
            public int entity_seq { get; set; }
            [DataMember(Order = 3)]
            public string check_start_date { get; set; }
            [DataMember(Order = 4)]
            public string check_end_date { get; set; }
        }


        [DataContract]
        public class REQ_CHART_COLOR
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
            [DataMember(Order = 2)]
            public int entity_seq { get; set; }
            [DataMember(Order = 3)]
            public string from_date { get; set; }
            [DataMember(Order = 4)]
            public string to_date { get; set; }
        }

        [DataContract]
        public class REQ_FAVORITE
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
            [DataMember(Order = 2)]
            public int entity_seq { get; set; }
            [DataMember(Order = 3)]
            public string uid { get; set; }
        }

        [DataContract]
        public class REQ_PUSH_REGIST
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public string uid { get; set; }
            [DataMember(Order = 2)]
            public string ha_flag { get; set; }
            [DataMember(Order = 3)]
            public string la_flag { get; set; }
            [DataMember(Order = 4)]
            public string aa_flag { get; set; }
            [DataMember(Order = 5)]
            public string an_flag { get; set; }
            [DataMember(Order = 6)]
            public string kn_flag { get; set; }
            [DataMember(Order = 7)]
            public string id_flag { get; set; }
            [DataMember(Order = 8)]
            public string ad_flag { get; set; }
            [DataMember(Order = 9)]
            public string cd_flag { get; set; }
            [DataMember(Order = 10)]
            public string dd_flag { get; set; }
        }

        [DataContract]
        public class REQ_LAST_INSEMINATE
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
            [DataMember(Order = 2)]
            public int entity_seq { get; set; }
            [DataMember(Order = 3)]
            public string breed_date { get; set; }
        }

        [DataContract]
        public class REQ_CALVE_DUE_DAY
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
            [DataMember(Order = 2)]
            public int inseminate_code { get; set; }
            [DataMember(Order = 3)]
            public int due_day { get; set; }
        }

        [DataContract]
        public class REQ_ESTRUS_DUE_DAY
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
            [DataMember(Order = 2)]
            public string estrus_code { get; set; }
            [DataMember(Order = 3)]
            public int due_day { get; set; }
        }

        [DataContract]
        public class REQ_PIN_INFO
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public string entity_pin { get; set; }
        }

        [DataContract]
        public class REQ_USER_ALARM_INSERT
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
            [DataMember(Order = 2)]
            public string alarm_title { get; set; }
            [DataMember(Order = 3)]
            public string base_date { get; set; }
            [DataMember(Order = 4)]
            public string base_type { get; set; }
            [DataMember(Order = 5)]
            public int alarm_day { get; set; }
        }

        [DataContract]
        public class REQ_USER_ALARM_UPDATE
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
            [DataMember(Order = 2)]
            public int alarm_seq { get; set; }
            [DataMember(Order = 3)]
            public string alarm_title { get; set; }
            [DataMember(Order = 4)]
            public string base_date { get; set; }
            [DataMember(Order = 5)]
            public string base_type { get; set; }
            [DataMember(Order = 6)]
            public int alarm_day { get; set; }
        }

        [DataContract]
        public class REQ_USER_ALARM_DELETE
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
            [DataMember(Order = 2)]
            public int alarm_seq { get; set; }
        }

        [DataContract]
        public class REQ_DUEDAY_UPDATE
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
            [DataMember(Order = 2)]
            public List<ClassStruct.ST_CALVE_DUE_DAY> calve_info;
            [DataMember(Order = 3)]
            public List<ClassStruct.ST_ESTRUS_DUE_DAY> estrus_info;
        }

        [DataContract]
        public class REQ_ALARM_LIST
        {
            [DataMember(Order = 0)]
            public string lang_code { get; set; }
            [DataMember(Order = 1)]
            public int farm_seq { get; set; }
            [DataMember(Order = 2)]
            public string entity_id { get; set; }
            [DataMember(Order = 3)]
            public string alarm_code { get; set; }
            [DataMember(Order = 4)]
            public int page_index { get; set; }
        }
    }
}