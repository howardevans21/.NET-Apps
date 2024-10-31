using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPipelinePStepWebService.API.ENUM
{
    public static  class APIENUM
    {
        public enum POL
        {
            ProcessClassID,
            MIR_USER_SESN_CTRY_CD, 
            MIR_CO_ID, 
            MIR_POL_ID, 
            MIR_POL_INS_PURP_CD, 
            MIR_POL_CLI_INSRD_CD, 
            MIR_POL_COUNTRY_CD, 
            MIR_POL_APP_RECV_DT, 
            MIR_POL_ISS_EFF_DT,
            MIR_POL_ISS_LOC_CD,
            MIR_POL_BILL_MODE_CD, 
            MIR_POL_BILL_TYP_CD,
            MIR_POL_DIVOPT_CD,
            MIR_POL_CONTINGENT_GIV_NM,
            MIR_POL_CONTINGENT_SUR_NM, 
            MIR_POL_CONTINGENT_BTH_DT,
            MIR_POL_CONTIN_CLI_INSRD_CD, 
         
        }

        public enum SERV
        {
            MIR_SERV_AGT_ID
        }

        public enum AGT
        {
            MIR_AGT_SHRE_PCT1,
            MIR_AG2_ID, 
            MIR_AGT2_SHRE_PCT
        }

        public enum CVG
        {
            MIR_CVG_TYP_CD_T, 
            MIR_CVG_PLAN_ID_T,
            MIR_CVG_FACE_AMT_T 
        }

        public enum CLIC
        {
            MIR_CLIC_CNTCT_EMAIL_T,
            MIR_CLIC_CNTCT_HOME_PH_T,
            MIR_CLIC_CNTCT_CELL_PH_T,
            MIR_CLIC_CNTCT_WORK_PH_T, 
        }

        public enum BNFY
        {
            MIR_BNFY_CVG_NUM_T, 
            MIR_BNFY_NM_T, 
            MIR_BNFY_TYP_CD_T, 
            MIR_BNFY_CLI_TYP_CD_T,
            MIR_BNFY_REL_INSRD_CD_T, 
            MIR_BNFY_PRCDS_PCT_T, 
            MIR_BNFY_TRUSTEE_NM_T, 
            MIR_BNFT_TRUSTEE_REL_CD_T

                
        }

        public enum CLI
        {
            MIR_CLI_TYP_CD_T, 
            MIR_CLI_INDV_GIV_NM_T, 
            MIR_CLI_INDV_MID_NM_T, 
            MIR_CLI_INDV_SUR_NM_T, 
            MIR_CLI_INDV_TITL_TXT_T, 
            MIR_CLI_INDV_SFX_NM_T, 
            MIR_CLI_COMPANY_NM_T, 
            MIR_CLI_BTH_DT_T, 
            MIR_CLI_SEX_CD_T, 
            MIR_CLI_SMKR_CD_T, 
            MIR_CLI_TAX_ID_T, 
            MIR_CLI_BTH_LOC_CD_T,
            MIR_CLI_EI_IND_T, 
            MIR_CLI_SHR_IND_T, 
            MIR_CLI_EMPL_YR_DUR_T, 
            MIR_CLI_EMPL_NM_T,
            MIR_CLI_SSN_ID_T, 
            MIR_CLI_TIN_ID_T, 
            MIR_CLI_NATLTY_CNTRY_CD_T, 
            MIR_CLI_PREM_RES_CNTRY_CD_T,
            MIR_CLI_COMNT_TXT_T, 
            MIR_CLI_INCM_EARN_AMT_T, 
            MIR_CLIA_TYP_CD_T,
            MIR_CLI_ADDR_TYP_CD_T,
            MIR_CLI_ADDR_LN_1_TXT_T, 
            MIR_CLI_ADDR_LN_2_TXT_T,
            MIR_CLI_ADDR_LN_3_TXT_T, 
            MIR_CLI_ADDR_ADDL_TXT_T,
            MIR_CLI_CRNT_LOC_CD_T, 
            MIR_CLI_ADDR_MUN_CD_T,
            MIR_CLI_CITY_NM_TXT_T, 
            MIR_CLI_COUNTRY_CD_T, 
            MIR_CLI_PSTL_CD_T, 
            MIR_CLI_ADDR_YR_DUR_T, 
            MIR_CLI_DOC_TYP_CD_T,
            MIR_CLI_DOC_ID_T, 
            MIR_CLI_DOC_CNTRY_CD_T, 
            MIR_CLI_DOC_XPRY_DT_T, 
        }
    }
}
