using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DWPipelineWebService
{
    public class TERROR
    {
        private string error_Message = string.Empty;
        private string app_ID = string.Empty;
        private string pol_ID = string.Empty;
        private DateTime createDate = DateTime.Now;

        public TERROR()
        {

        }

        public TERROR(string app_ID, string pol_ID, string error_Message, DateTime createDate)
        {
            this.app_ID = app_ID;
            this.pol_ID = pol_ID;
            this.error_Message = error_Message;
            this.createDate = createDate;
           
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(10)]
        public string POL_ID { get { return pol_ID; } set { } }

        [Required]
        public DateTime CreateDate { get { return createDate; } set { } }

        [Required]
        [StringLength(50)]
        public string APP_ID { get { return app_ID; } set { } }

        [Required]
        [MaxLength]
        public string ERROR_MESSAGE { get { return error_Message; } set { } }

    }
}

