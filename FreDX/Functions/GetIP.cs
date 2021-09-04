using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace FreDX.Functions
{
    public class GetIP
    {

        // Функция проверки ip-адреса клиента еще юзабильно
          public string GetIPAddress()
          {

                string strHostName = Dns.GetHostName();
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());  // <----  Да устарел но работает лучше новых методов
            IPAddress ipAddress = ipHostInfo.AddressList[0];

                return ipAddress.ToString(); // возвращает строку с адресом
            }
    }
}