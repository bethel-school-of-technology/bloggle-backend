using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace collaby_backend.Helper{

    class Verify{
        public static string ValidatePassword(string password)
        {
            //(?=.*[a-z]) : Should have at least one lower case
            //(?=.*[A-Z]) : Should have at least one upper case
            //(?=.*\d) : Should have at least one number
            //(?=.*[#$^+=!*()@%&] ) : Should have at least one special character
            //.{8,20} : Minimum 8 characters and max of 20
            Regex rx = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,20}$");

            return rx.IsMatch(password) ? null: "Invalid Password; you need to include a special character";
        }
        
        public static string ValidateName(string[] fullName)
        {   
            foreach(string name in fullName){
                char[] char_arr = name.ToCharArray();

                foreach(char character in char_arr){
                    int asciiVal = (int)character;

                    if(asciiVal>=65 && asciiVal<=90){
                        continue;
                    }else if(asciiVal>=97 && asciiVal<=122){
                        continue;
                    }else{
                        return "Cannot use the character '"+character+"' in first or last name";
                    }
                }
            }
            return null;
        }

        public static string ValidateUserName(string username)
        {
            char[] char_arr = username.ToCharArray();
            foreach(char character in char_arr){
                int asciiVal = (int)character;
                if(asciiVal>=48 && asciiVal<=57){
                    continue;
                }else if(asciiVal>=65 && asciiVal<=90){
                    continue;
                }else if(asciiVal>=95 && asciiVal<=122){
                    continue;
                }
                return "Cannot use the character '"+character+"' in username";
            }
            return null;
        }

        public static string ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return "Email cannot contain a null value or a space";

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                    RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            
            catch (RegexMatchTimeoutException)
            {
                return "Email verify timeout";
            }

            catch (ArgumentException)
            {
                return "Invalid Email";
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)) ? null : "93 Invalid Email";
            }
            catch (RegexMatchTimeoutException)
            {
                return "Email verify timeout";
            }
        }
    }
}