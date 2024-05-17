using System.ComponentModel.DataAnnotations;

namespace Mirabel.MM.Models
{

        public class CaptchaResult
        {
            [Required]
            [StringLength(6)]
            public string CaptchaCode { get; set; }
            public byte[] CaptchaByteData { get; set; }
            public string CaptchBase64Data => Convert.ToBase64String(CaptchaByteData);
            public DateTime Timestamp { get; set; }
        }

    public class ControllerResult 
    {
        public string ErrorCode { get; set; } 
        public string ErrorMsg { get; set; }  
        public ControllerResponse ResultSet { get; set; }    
    }
   
    public class ControllerResponse 
    {
        public int Response { get; set; } 
        public string ResultSet { get; set; }  
    }

    public class TableResult 
    {
        public string ErrorCode { get; set; }
        public string ErrorMsg { get; set; }
        public string ResultSet { get; set; }
    }

    public class LoginIP 
    {
        public string UserId { get; set; }
        public string Password { get; set; } 
    }

    public class DashboardIP 
    {
        public string DBName { get; set; }
        public string TableName { get; set; }
    }

}
