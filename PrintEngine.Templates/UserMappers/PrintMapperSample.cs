using PrintEngine.Core.Mappers;
using PrintEngine.Templates.InputDataModels;
using PrintEngine.Templates.UserModels;

namespace PrintEngine.Templates.UserMappers
{
    public class PrintMapperSample : PrintMapperBase<InputDataSample, PrintModelSample>
	{
		protected override PrintModelSample MapInternal(InputDataSample inputData)
		{
			return new PrintModelSample
			{
				ContractCalculationDate = inputData.Contract.Date,
				ContractEndDate = inputData.Contract.EndDate,
				ContractStartDate = inputData.Contract.StartDate,
				InsurantFullName = inputData.Insurant.Name,
				InsurantAddress = inputData.Insurant.Address,
				InsurantMobile = inputData.Insurant.Phone,
				InsurantINN = inputData.Insurant.INN,
				InsurantDocument = "ПАСПОРТ - 4512 125354 выдан 14.08.2001 КЕМ ТО В ГОРОДЕ Н",
				PeriodsOfPayment = new PrintModelSample.Period_[]
				{
					new PrintModelSample.Period_
					{
						EndDate = inputData.Payments[0].EndDate,
						InsPayment = inputData.Payments[0].Value
					}
				},
				VehicleCategory = inputData.Vehicle.Category,
				VehicleMarka = inputData.Vehicle.Brand,
				VehicleModel = inputData.Vehicle.Model,	
				VehicleVIN= inputData.Vehicle.VIN,	
				/// ........ 
			};
		}
	}
}
