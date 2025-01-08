using PrintEngine.Core.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

namespace PrintEngine.Examples
{
    /// <summary>
    /// Образец ответа метода Engage
    /// </summary>
    public class EngageResponseExampleOk
        : IMultipleExamplesProvider<PrintResult>
    {
        private readonly byte[] _data = Encoding.UTF8.GetBytes("JVBERi0xLjcKJeLjz9MKNyAwIG9iago8PC9GaWx0ZXIvRm");
        private readonly Guid _correlation = Guid.Parse("68B1F46F-3814-41CA-963C-22EC58C6A9EA");
        /// <summary>
        /// GetExamples
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SwaggerExample<PrintResult>> GetExamples()
        {
            yield return SwaggerExample.Create("Успешный ответ без ЭЦП",
                PrintResult.Success
                (
                  new PrintData
                  {
                      FileName = "NEWEOSAGO_POLICY_SAMPLE.out",
                      FileOutput = _data,
                      FileType = "pdf",
                      OutputLength = 100,
                      PageCount = 1,
                  },
                  correlationId: _correlation
                ));

            yield return SwaggerExample.Create("Успешный ответ с ЭЦП",
                PrintResult.Success
                (
                  new PrintData
                  {
                      FileName = "NEWEOSAGO_POLICY.pdf",
                      FileOutput = _data,
                      FileType = "pdf",
                      OutputLength = 100,
                      PageCount = 1,
                  },
                  new SignData
                  {
                      FileOutput = _data,
                      FileName = "NEWEOSAGO_POLICY.pdf.sgn"
                  },
                  _correlation
                ));
        }
    }
    /// <summary>
    /// Образец ответа метода Engage
    /// </summary>
    public class EngageResponseExampleBadRequest
        : IMultipleExamplesProvider<PrintResult>
    {
        /// <summary>
        /// GetExamples
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SwaggerExample<PrintResult>> GetExamples()
        {
           
            yield return SwaggerExample.Create("Ответ с ошибкой",
                PrintResult.Fail("Описание ошибки", Guid.Parse("68B1F46F-3814-41CA-963C-22EC58C6A9EA")));
        }
    }
}
