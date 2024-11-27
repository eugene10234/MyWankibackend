using static System.Runtime.InteropServices.JavaScript.JSType;

namespace prjWankibackend.Models.Account.Jwt.DTO
{
    public class webMsg
    {

        public object Token { get; set; }

        public bool Success { get; set; } = true;

        public string Msg { get; set; } = "操作成功!";
        public JwtUserDTO Data { get; set; }

        public webMsg()
        {
            Success=false;
            Msg = "error?";
        }
        public webMsg(string errmsg)
        {
            Success = false;
            Msg = errmsg;
        }
        public webMsg(JwtUserDTO data)
        {
            Data = data;
        }
        public webMsg(object token)
        {
            Token = token;
        }

        public webMsg(object token, JwtUserDTO data)
        {
            Token = token;
            Data = data;
        }
    }

}
