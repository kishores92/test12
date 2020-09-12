using GreenPOS.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenPOS.Common
{
    public static class Extensions
    {
        public static T GetJsonResponse<T>(StringBuilder jsonResult, DbDataReader reader, string parserString)
        {
            //Check reader has some rows
            try
            {
                //If reader has rows, then get the value of each row and add it in to the json builder object
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        //Append value row in string builder object
                        jsonResult.Append(reader.GetValue(0).ToString());
                    }

                    //Create object of JObject class and parse the json result
                    JObject jsonResponse = JObject.Parse(jsonResult.ToString());
                    var objResponse = jsonResponse[parserString];
                    if (objResponse != null)
                        return JsonConvert.DeserializeObject<T>(Convert.ToString(objResponse));

                    return (T)Activator.CreateInstance(typeof(T));
                }

                return Enumerable.Empty<T>().FirstOrDefault();
            }
            catch (Exception ex)
            {
                return (T)Activator.CreateInstance(typeof(T));
            }
        }

        public static IEnumerable<UserViewModel> WithoutPasswords(this IEnumerable<UserViewModel> users)
        {
            return users.Select(x => x.WithoutPassword());
        }

        public static UserViewModel WithoutPassword(this UserViewModel user)
        {
            user.Password = null;
            return user;
        }
    }
}
