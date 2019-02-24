using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FakturaWpf.WebApi
{
    class GusSearch
    {
        private static readonly HttpClient client = new HttpClient();
        private const string sendUri = "https://wyszukiwarkaregontest.stat.gov.pl/wsBIR/UslugaBIRzewnPubl.svc";
        private string logKey;

        public async Task SendAsync()
        {
            var restr = @"<soap:Envelope xmlns:soap=""http://www.w3.org/2003/05/soap-envelope"" xmlns:ns=""http://CIS/BIR/PUBL/2014/07""> " +
                           @"<soap:Header xmlns:wsa=""http://www.w3.org/2005/08/addressing""> " +
                           "<wsa:To>https://wyszukiwarkaregontest.stat.gov.pl/wsBIR/UslugaBIRzewnPubl.svc</wsa:To> " +
                           "<wsa:Action>http://CIS/BIR/PUBL/2014/07/IUslugaBIRzewnPubl/Zaloguj</wsa:Action> " +
                           "</soap:Header> " +
                           "    <soap:Body> " +
                           "        <ns:Zaloguj> " +
                           "            <ns:pKluczUzytkownika>abcde12345abcde12345</ns:pKluczUzytkownika> " +
                           "        </ns:Zaloguj> " +
                           "    </soap:Body> " +
                           "</soap:Envelope> ";


            HttpContent hp = new StringContent(restr, Encoding.UTF8, "application/soap+xml");

            var response = await client.PostAsync(sendUri, hp);

            var responseString = await response.Content.ReadAsStringAsync();

            TakeLogKey(responseString);
        }

        private void TakeLogKey(string answer)
        {
            int firstStringPosition = answer.IndexOf("<ZalogujResult>");
            int secondStringPosition = answer.IndexOf("</ZalogujResult>");
            string stringBetweenTwoStrings = answer.Substring(firstStringPosition,
                secondStringPosition - firstStringPosition + 7);

            Various.InfoOk(stringBetweenTwoStrings);
        }

        
    }
}
