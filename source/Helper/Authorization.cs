using System;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;


namespace collaby_backend.Helper{

    class Authorization{

        public static int GetUserId(IHeaderDictionary headers)
        {

            string token = headers["Authorization"];
            int userId = Int16.Parse(Jwt.decryptJSONWebToken(token)["Id"].ToString());

            return userId;
        }
        public static Boolean IsAdmin(IHeaderDictionary headers){

            Boolean IsAdmin = false;
            string token = headers["Authorization"];

            if(Jwt.decryptJSONWebToken(token)["IsAdmin"].ToString()=="1"){
                IsAdmin = true;
            }
            return IsAdmin;
        }
        public static JwtPayload GetPayload(IHeaderDictionary headers){

            string token = headers["Authorization"];
            return  Jwt.decryptJSONWebToken(token);
        }
    }
}