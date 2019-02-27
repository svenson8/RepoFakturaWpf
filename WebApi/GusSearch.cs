using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FakturaWpf.WebApi
{
    class GusSearch
    {
        private static readonly HttpClient client = new HttpClient();
        private const string sendUri = "https://wyszukiwarkaregontest.stat.gov.pl/wsBIR/UslugaBIRzewnPubl.svc";
        private const string actionUrl = "http://CIS/BIR/PUBL/2014/07/IUslugaBIRzewnPubl/";
        private const string userKey = "abcde12345abcde12345";
        private string logKey;
        private string fSessionId;
        private string fErrorMessage;
        private XmlDocument fSearchResult;
        private HttpContent fRequest;


        public GusSearch()
        {
            fErrorMessage = "";
            fSearchResult = new XmlDocument();

//        FRequest:= TStringStream.Create('', TEncoding.UTF8);
//        FResponse:= TStringStream.Create('', TEncoding.UTF8);
        }

        private Boolean Login()
        {
            string body;
            body = "<ns:pKluczUzytkownika>" + userKey + "</ns:pKluczUzytkownika>" + Environment.NewLine;
            //  FSessionId = GetResponse("Zaloguj", body);
            return (fSessionId != "");
        }

        private void Logout()
        {
            string body;
            body = "<ns:pIdentyfikatorSesji>" + fSessionId + "</ns:pIdentyfikatorSesji>" + Environment.NewLine;
            //GetResponse('Wyloguj', body);
        }


        private void GetErrorMessage()
        {
            string body;
            body = "<ns:DaneKomunikat/>" + Environment.NewLine;
            //  FErrorMessage:= GetResponse('DaneKomunikat', body);

        }

        private Boolean SearchGustByData(string nip, string regon, string krs)
        {
            string searchFor = "";
            string body = "";

            if (nip.Length > 3)
                searchFor = "  <dat:Nip>" + nip.Trim() + "</dat:Nip>" + Environment.NewLine;

            if (regon.Length > 3)
                searchFor = "  <dat:Regon>" + regon.Trim() + "</dat:Regon>" + Environment.NewLine;

            if (krs.Length > 3) 
              searchFor = "  <dat:Krs>" + krs.Trim() + "</dat:Krs>" + Environment.NewLine;

            body =  "<ns:pParametryWyszukiwania>" + Environment.NewLine;
            body += searchFor;
            body += "</ns:pParametryWyszukiwania>" + Environment.NewLine;
          //  fSearchResult.LoadXml(GetResponse("DaneSzukaj", body));

            if (fSearchResult.ChildNodes.Count == 0)
                GetErrorMessage();

            return (fSearchResult.ChildNodes.Count != 0);


        }

        private async string GetResponse(string action, string body)
        {
            fErrorMessage = "";
            //set FRequest
            var restr = @"<soap:Envelope xmlns:soap=""http://www.w3.org/2003/05/soap-envelope"" xmlns:ns=""http://CIS/BIR/PUBL/2014/07"" xmlns:dat=""http://CIS/BIR/PUBL/2014/07/DataContract""> " + Environment.NewLine+
                        @"<soap:Header xmlns:wsa=""http://www.w3.org/2005/08/addressing""> " + Environment.NewLine +
                           "<wsa:To>" + sendUri + "</wsa:To> " + Environment.NewLine +
                           "<wsa:Action>"+ actionUrl + action+"</wsa:Action> " + Environment.NewLine +
                           "</soap:Header> " + Environment.NewLine +
                           "    <soap:Body> " + Environment.NewLine +
                           "      <ns:" + action + ">" + Environment.NewLine +
                           body +
                           "      </ns:" + action + ">" + Environment.NewLine +
                           "    </soap:Body> " + Environment.NewLine +
                           "</soap:Envelope> ";

            fRequest = new StringContent(restr, Encoding.UTF8, "application/soap+xml");


            //set custom headers
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("action", actionUrl + action);
            if (fSessionId != "")
              client.DefaultRequestHeaders.Add("sid", fSessionId);

            var response = await client.PostAsync(sendUri, fRequest);

            var responseString = await response.Content.ReadAsStringAsync();
            //send Request
            FResponse.Clear;
                        FHttp.Post(WSDLUrl, FRequest, FResponse);

                    //get Response text from Response
                    Result:= FResponse.DataString;
                    p:= Pos('<' + Action + 'Result>', Result);
                        if p > 0 then
                        begin
                Delete(Result, 1, p + Length(Action) + 7);
                    p:= Pos('</' + Action + 'Result>', Result);
                        if p > 0 then
                        begin
                    Result:= Copy(Result, 1, p - 1);
                    Result:= StringReplace(Result, '&lt;', '<', [rfReplaceAll, rfIgnoreCase]);
                    Result:= StringReplace(Result, '&gt;', '>', [rfReplaceAll, rfIgnoreCase]);
                    Result:= StringReplace(Result, '&#xD;', '', [rfReplaceAll, rfIgnoreCase]);
                        end
                else
                    Result:= '';
                        end
                else
                Result:= '';
                        FRequest.Clear;
                        FResponse.Clear;
                        end;





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
        }
        }

       /* public async Task SendAsync()
        {



          //  TakeLogKey(responseString);
        }  */

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
