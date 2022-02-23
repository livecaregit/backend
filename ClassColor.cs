using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LC_Service
{
    public class ClassColor
    {
        private readonly string[] _mColorList = new string[121];

        public ClassColor()
        {
            SetColorList();
        }

        public int GetColorArrayLength()
        {
            return _mColorList.Length;
        }

        public string GetColorString(int pIndex)
        {
            return _mColorList[pIndex];
        }

        private void SetColorList()
        {
            _mColorList[0] = "#002060";
            _mColorList[1] = "#022261";
            _mColorList[2] = "#052463";
            _mColorList[3] = "#082765";
            _mColorList[4] = "#0B2967";
            _mColorList[5] = "#0E2C68";
            _mColorList[6] = "#112E6A";
            _mColorList[7] = "#13316C";
            _mColorList[8] = "#16336E";
            _mColorList[9] = "#19366F";
            _mColorList[10] = "#1C3871";
            _mColorList[11] = "#1F3B73";
            _mColorList[12] = "#213D75";
            _mColorList[13] = "#244076";
            _mColorList[14] = "#274278";
            _mColorList[15] = "#2A457A";
            _mColorList[16] = "#2D477C";
            _mColorList[17] = "#304A7E";
            _mColorList[18] = "#334C7F";
            _mColorList[19] = "#354F81";
            _mColorList[20] = "#385183";
            _mColorList[21] = "#3B5485";
            _mColorList[22] = "#3E5686";
            _mColorList[23] = "#415888";
            _mColorList[24] = "#435B8A";
            _mColorList[25] = "#465D8C";
            _mColorList[26] = "#49608D";
            _mColorList[27] = "#4C628F";
            _mColorList[28] = "#4F6591";
            _mColorList[29] = "#526793";
            _mColorList[30] = "#556A95";
            _mColorList[31] = "#576C96";
            _mColorList[32] = "#5A6F98";
            _mColorList[33] = "#5D719A";
            _mColorList[34] = "#60749C";
            _mColorList[35] = "#63769D";
            _mColorList[36] = "#66799F";
            _mColorList[37] = "#687BA1";
            _mColorList[38] = "#6B7EA3";
            _mColorList[39] = "#6E80A4";
            _mColorList[40] = "#7183A6";
            _mColorList[41] = "#7485A8";
            _mColorList[42] = "#7788AA";
            _mColorList[43] = "#798AAB";
            _mColorList[44] = "#7C8DAD";
            _mColorList[45] = "#7F8FAF";
            _mColorList[46] = "#8291B1";
            _mColorList[47] = "#8594B3";
            _mColorList[48] = "#8896B4";
            _mColorList[49] = "#8A99B6";
            _mColorList[50] = "#8D9BB8";
            _mColorList[51] = "#909EBA";
            _mColorList[52] = "#93A0BB";
            _mColorList[53] = "#96A3BD";
            _mColorList[54] = "#99A5BF";
            _mColorList[55] = "#9BA8C1";
            _mColorList[56] = "#9EAAC2";
            _mColorList[57] = "#A1ADC4";
            _mColorList[58] = "#A4AFC6";
            _mColorList[59] = "#A7B2C8";
            _mColorList[60] = "#AAB4CA";
            _mColorList[61] = "#ACB7CB";
            _mColorList[62] = "#AFB9CD";
            _mColorList[63] = "#B2BCCF";
            _mColorList[64] = "#B5BED1";
            _mColorList[65] = "#B8C1D2";
            _mColorList[66] = "#BBC3D4";
            _mColorList[67] = "#BDC6D6";
            _mColorList[68] = "#C0C8D8";
            _mColorList[69] = "#C3CAD9";
            _mColorList[70] = "#C6CDDB";
            _mColorList[71] = "#C9CFDD";
            _mColorList[72] = "#CCD2DF";
            _mColorList[73] = "#CED4E0";
            _mColorList[74] = "#D1D7E2";
            _mColorList[75] = "#D4D9E4";
            _mColorList[76] = "#D7DCE6";
            _mColorList[77] = "#DADEE8";
            _mColorList[78] = "#DDE1E9";
            _mColorList[79] = "#DFE3EB";
            _mColorList[80] = "#E2E6ED";
            _mColorList[81] = "#E5E8EF";
            _mColorList[82] = "#E8EBF0";
            _mColorList[83] = "#EBEDF2";
            _mColorList[84] = "#EEF0F4";
            _mColorList[85] = "#F0F2F6";
            _mColorList[86] = "#F3F5F7";
            _mColorList[87] = "#F6F7F9";
            _mColorList[88] = "#F9FAFB";
            _mColorList[89] = "#FCFCFD";
            _mColorList[90] = "#FFFFFF";
            _mColorList[91] = "#FFF7F7";
            _mColorList[92] = "#FFEEEE";
            _mColorList[93] = "#FFE6E6";
            _mColorList[94] = "#FFDDDD";
            _mColorList[95] = "#FFD5D5";
            _mColorList[96] = "#FFCCCC";
            _mColorList[97] = "#FFC4C4";
            _mColorList[98] = "#FFBBBB";
            _mColorList[99] = "#FFB3B3";
            _mColorList[100] = "#FFAAAA";
            _mColorList[101] = "#FFA2A2";
            _mColorList[102] = "#FF9999";
            _mColorList[103] = "#FF9191";
            _mColorList[104] = "#FF8888";
            _mColorList[105] = "#FF8080";
            _mColorList[106] = "#FF7777";
            _mColorList[107] = "#FF6F6F";
            _mColorList[108] = "#FF6666";
            _mColorList[109] = "#FF5E5E";
            _mColorList[110] = "#FF5555";
            _mColorList[111] = "#FF4D4D";
            _mColorList[112] = "#FF4444";
            _mColorList[113] = "#FF3C3C";
            _mColorList[114] = "#FF3333";
            _mColorList[115] = "#FF2B2B";
            _mColorList[116] = "#FF2222";
            _mColorList[117] = "#FF1A1A";
            _mColorList[118] = "#FF1111";
            _mColorList[119] = "#FF0909";
            _mColorList[120] = "#FF0000";
        }
    }
}