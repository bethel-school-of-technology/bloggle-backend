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
                int asciiVal = (int)character;

                if(asciiVal>=65 && asciiVal<=90){
                    continue;
                }else if(asciiVal>=97 && asciiVal<=122){
                    continue;
                }else{
                    return "Cannot use the character"+character+"in first or last name";
                }
            }
            return null;
        }
        /*public static string ValidateName(){
            
        }*/

    }
}