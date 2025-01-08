using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.Filters;

namespace PrintEngine.Examples
{
    /// <summary>
    /// Образец запроса метода Engage
    /// </summary>
    public class EngageRequestExample
        : IMultipleExamplesProvider<JObject>
    {
        const string request = "{\"CorrelationId\":\"53dd6112-cf41-42e9-8706-556a1203af12\",\"Data\":{\"OsagoPrintPolicyDto\": {\"AdmitDrivers\": {\"OsagoPrintPolicyAdmitDriverDto\": {\"BirthDate\": \"1997-08-03T00:00:00\",\"DocumentNumber\": 412563,\"DocumentSeries\": 6725,\"DrivingExperienceYear\": 8,\"DrivingStartExperienceDate\": \"2016-03-03T00:00:00\",\"FirstName\": \"Илья\",\"KbmClass\": 0,\"KbmValue\": \"———\",\"LastName\": \"Зыков\",\"SecondName\": \"Юрьевич\",\"StatusId\": 0}},\"Coefficients\": {\"Kbm\": 1.17,\"KbmRequestDate\": \"2024-10-06T00:00:00\",\"Km\": 1.4,\"Kn\": \"———\",\"Ko\": \"———\",\"Kp\": \"———\",\"Kpr\": \"———\",\"Ks\": 1,\"Kt\": 1.8,\"Kvs\": 1.926,\"Premium\": \"32 493.05\",\"Tb\": 5722,\"VehicleKbmRsaRequestId\": \"9d7504d1-0a9b-4faa-a284-5aad73abc3c4\"},\"ContractConclusionDate\": \"2024-10-06T16:13:30\",\"ContractDate\": \"2024-10-06T16:13:30\",\"ContractEnd\": \"2025-10-06T23:59:59\",\"ContractStart\": \"2024-10-07T00:00:00\",\"CurrentDocumentNumber\": 160724962,\"CurrentDocumentSeries\": \"ХХХ\",\"HasTrailer\": false,\"InsurancePremium\": \"32 493.05 (Тридцать две тысячи четыреста девяносто три рубля 05 копеек)\",\"Insurant\": \"ОБЩЕСТВО С ОГРАНИЧЕННОЙ ОТВЕТСТВЕННОСТЬЮ \\\\\\\"АВТОКРЕДИТБАНК\\\\\\\", ИНН 1626000087\",\"Insurer\": \"Пурсанов Д. В.\",\"IsAnyone\": false,\"IsEaRsa\": false,\"IsLKUL\": false,\"Periods\": {\"OsagoPrintPolicyPeriodOfUseDto\": [{\"EndDate\": \"2025-10-06T23:59:59\",\"PeriodTypeId\": 1,\"StartDate\": \"2024-10-07T00:00:00\"},{\"EndDate\": \"----------\",\"PeriodTypeId\": 0,\"StartDate\": \"----------\"},{\"EndDate\": \"----------\",\"PeriodTypeId\": 0,\"StartDate\": \"----------\"}]},\"Power\": 123,\"PowerKvt\": 90,\"Purpose\": {\"IsExtra\": false,\"IsOther\": false,\"IsPassanger\": false,\"IsRent\": false,\"IsSelf\": true,\"IsShippingDanger\": false,\"IsSpecial\": false,\"IsStudy\": false,\"IsTaxi\": false},\"QRCode\": \"https://dkbm-web.autoins.ru/dkbm-web-1.0/qr.htm?id=ХХХ0160724962\",\"SalesChannel\": 310,\"SpecialNotes\": \"ТС в качестве такси не используется. Номер заявки 97-ОС-178857. Распечатано из веб-ЕКИС 16:13 06.10.2024. Код: 2AAA ХХХ 0160724962. WEBEKIS\",\"VehicleName\": \"Hyundai Solaris\",\"VehicleOwner\": \"ОБЩЕСТВО С ОГРАНИЧЕННОЙ ОТВЕТСТВЕННОСТЬЮ \\\\\\\"АВТОКРЕДИТБАНК\\\\\\\", ИНН 1626000087\\\\r\\\\nРеспублика Татарстан, Город Казань, Проспект Альберта Камалеева, д. 16А, 420081\",\"VehiclePassportNumber\": 859825,\"VehiclePassportSerial\": 4430,\"VehiclePassportType\": \"Свидетельство о регистрации ТС\",\"VehicleRegistrationNumber\": \"Н401НН44\",\"VehicleVin\": \"Z94CT51DAFR132154\"}}}";

        /// <summary>
        /// GetExamples
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SwaggerExample<JObject>> GetExamples()
        {
            yield return SwaggerExample.Create("Запрос без ЭЦП", Get(false));
            yield return SwaggerExample.Create("Запрос c ЭЦП", Get(true));
        }

        private JObject Get(bool needSign)
        {
            var o = JObject.Parse(request);
            o.AddFirst(new JProperty("NeedSign", $"{needSign}"));
            o.AddFirst(new JProperty("TemplateId", $"{(needSign ? "EOSAGOPOLICY" : "EOSAGOPOLICYEXAMPLE")}"));
            return o;
        }
    }
}
