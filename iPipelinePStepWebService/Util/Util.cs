using iPipelinePStepWebService.API.ENUM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace iPipelinePStepWebService
{
    internal class Util
    {

        public string Format_JSON_BODY_VariableFormatWithComma(APIENUM.POL mirField, string val)
        {
            string field = ReplaceUnderScoresWithDashes(mirField.ToString());
            string formattedJson = string.Empty;
            string comma = ",";
            string newLine = "\r\n";
            formattedJson =  string.Format("\"{0}\":\"{1}\"{2}{3}", field, val, comma, newLine);
            return formattedJson;
        }


        public string Format_JSON_BODY_VariableFormatWithComma(APIENUM.SERV mirField, string val)
        {
            string field = ReplaceUnderScoresWithDashes(mirField.ToString());
            string formattedJson = string.Empty;
            string comma = ",";
            string newLine = "\r\n";
            formattedJson = string.Format("\"{0}\":\"{1}\"{2}{3}", field, val, comma, newLine);
            return formattedJson;
        }

        public string Format_JSON_BODY_VariableFormatWithComma(APIENUM.AGT mirField, string val)
        {
            string field = ReplaceUnderScoresWithDashes(mirField.ToString());
            string formattedJson = string.Empty;
            string comma = ",";
            string newLine = "\r\n";
            formattedJson = string.Format("\"{0}\":\"{1}\"{2}{3}", field, val, comma, newLine);
            return formattedJson;
        }


        public string Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLI mirField, string val, int seqNum)
        {
            string field = ReplaceUnderScoresWithDashes(mirField.ToString());
            string formattedJson = string.Empty;
            string comma = ",";
            string newLine = "\r\n";
            formattedJson = string.Format("\"{0}[{1}]\":\"{2}\"{3}{4}", field, seqNum, val, comma, newLine);
            return formattedJson;
        }

        public string Format_JSON_BODY_VariableFormatWithComma(APIENUM.CLIC mirField, string val, int seqNum)
        {
            string field = ReplaceUnderScoresWithDashes(mirField.ToString());
            string formattedJson = string.Empty;
            string comma = ",";
            string newLine = "\r\n";
            formattedJson = string.Format("\"{0}[{1}]\":\"{2}\"{3}{4}", field, seqNum, val, comma, newLine);
            return formattedJson;
        }


        public string Format_JSON_BODY_VariableFormatWithComma(APIENUM.BNFY mirField, string val, int seqNum)
        {
            string field = ReplaceUnderScoresWithDashes(mirField.ToString());
            string formattedJson = string.Empty;
            string comma = ",";
            string newLine = "\r\n";
            formattedJson = string.Format("\"{0}[{1}]\":\"{2}\"{3}{4}", field, seqNum, val, comma, newLine);
            return formattedJson;
        }
        public string Format_JSON_BODY_VariableFormatWithNoComma(APIENUM.BNFY mirField, string val, int seqNum)
        {
            string field = ReplaceUnderScoresWithDashes(mirField.ToString());
            string formattedJson = string.Empty;
            formattedJson = string.Format("\"{0}[{1}]\":\"{2}\"", field, seqNum, val);
            return formattedJson;
        }

        public string Format_JSON_BODY_VariableFormatWithComma(APIENUM.CVG mirField, string val, int seqNum)
        {
            string field = ReplaceUnderScoresWithDashes(mirField.ToString());
            string formattedJson = string.Empty;
            string comma = ",";
            string newLine = "\r\n";
            formattedJson = string.Format("\"{0}[{1}]\":\"{2}\"{3}{4}", field, seqNum, val, comma, newLine);
            return formattedJson;
        }



        public string ReplaceUnderScoresWithDashes(string field)
        {
            string formmattedJSON = field.Replace("_", "-");
            return formmattedJSON;
        }

        public string ReplaceUnderScoresWithDashes(string field, int seqNum)
        {
            string formmattedJSON = field.Replace("_", "-");
            formmattedJSON = string.Format("{0}[{1}]", formmattedJSON, seqNum);
            return formmattedJSON;
        }

        private string dateFormat = "yyyy-MM-dd";
        public string DateFormat
        {
            get { return dateFormat; }
        }
    }
}
