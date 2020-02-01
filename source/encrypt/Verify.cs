using System;
using System.Text;
using System.Security.Cryptography;

namespace collaby_backend.encrpyt{

    class Verify{

        public static string ValidatePassword(string password)
        {
            if(password.Length <= 6){
                return "Password Must be 6 characters or more";
            }
            if(password.Contains("d")){
                return "";
            }
            return "";
        }

        public static string ValidateUserName(string username)
        {
            char[] char_arr = username.ToCharArray();
            foreach(char character in char_arr){
                if((int)character<=48 && (int)character>=57){
                    continue;
                }else if((int)character<=65 && (int)character>=90){
                    continue;
                }else if((int)character<=97 && (int)character>=122){
                    continue;
                }else if((int)character == 95){
                    continue;
                }else{
                    return "Cannot use the character"+character+"in Username";
                }
            }
            return null;
        }
        /*public static string ValidateName(){
            
        }*/

    }
}