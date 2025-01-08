using PrintEngine.Core.Interfaces;

namespace PrintEngine.Templates.UserModels
{
    public class PrintModelSample : IPrintModel
	{
		public class Condition_
		{
			public bool IsSign { get; set; }
			public string Text { get; set; }
		}

		public class Period_
		{
			public double CurrencyRate { get; set; }
			public DateTime EndDate { get; set; }
			public string InsPayment { get; set; }
		}

		public class PurposeUse_
		{
			public int UsagePurposeId { get; set; }
		}

		public string ActualValue { get; set; }
		public string AdditionalExpencesPrem { get; set; }
		public string AdditionalExpencesSum { get; set; }
		public int AdmitType { get; set; }
		public DateTime AlarmAddAgreementDate { get; set; }
		public string AlarmSystem { get; set; }
		public int ApprovedAge { get; set; }
		public int ApprovedExperience { get; set; }
		public string BeneficiaryFullName { get; set; }
		public bool BlankIsUsed { get; set; }
		public string BottomString { get; set; }
		public DateTime ContractCalculationDate { get; set; }
		public DateTime ContractEndDate { get; set; }
		public int ContractNumber { get; set; }
		public double ContractSeries { get; set; }
		public DateTime ContractStartDate { get; set; }
		public int DocType { get; set; }
		public bool HasTrailer { get; set; }
		public int InsFranchiseMainRisk { get; set; }
		public string InsPremiumMainRisk { get; set; }
		public double InsPremiumNS { get; set; }
		public double InsPremiunDSAGO { get; set; }
		public double InsSummDSAGO { get; set; }
		public string InsSummMainRisk { get; set; }
		public string InsSummMainRiskDamage { get; set; }
		public string InsSummMainRiskDamageTotal { get; set; }
		public string InsSummMainRiskSteal { get; set; }
		public double InsSummNS { get; set; }
		public string InsurantAddress { get; set; }
		public DateTime InsurantBirthDate { get; set; }
		public string InsurantCitizen { get; set; }
		public string InsurantDocument { get; set; }
		public string InsurantEmail { get; set; }
		public string InsurantFullName { get; set; }
		public string InsurantINN { get; set; }
		public string InsurantKPP { get; set; }
		public string InsurantMobile { get; set; }
		public string InsurantSNILS { get; set; }
		public int InsurantType { get; set; }
		public bool IsAlarmAddAgreement { get; set; }
		public bool IsDynamicFranchise { get; set; }
		public bool IsNotInspection { get; set; }
		public bool IsPaymentHP { get; set; }
		public string IssueBy { get; set; }
		public DateTime IssueDate { get; set; }
		public string KascoInsSum { get; set; }
		public double KProportion { get; set; }
		public int MazdaGMPolis { get; set; }
		public string MedicalCarePrem { get; set; }
		public string MedicalCareSum { get; set; }
		public int Number { get; set; }
		public Condition_[] OtherConditions { get; set; }
		public DateTime OutfitAddAgreementDate { get; set; }
		public int OutfitType { get; set; }
		public Period_[] PeriodsOfPayment { get; set; }
		public string PlaceOfBirth { get; set; }
		public string PledgeholderFullName { get; set; }
		public string PreviousContractNumber { get; set; }
		public string PreviousContractSeries { get; set; }
		public int ProgramCode { get; set; }
		public string ProjectProgramm { get; set; }
		public PurposeUse_ PurposeUse { get; set; }
		public bool RiskKASKO { get; set; }
		public int Series { get; set; }
		public int SignAgreement { get; set; }
		public string SupplementToContract { get; set; }
		public string TechnicalAssistancePrem { get; set; }
		public string TechnicalAssistanceSum { get; set; }
		public string TopPolisProductName { get; set; }
		public string TotalPremium { get; set; }
		public int UsagePurposeId { get; set; }
		public int VariantOfPayment { get; set; }
		public string VehicleCategory { get; set; }
		public int VehicleDocNumber { get; set; }
		public string VehicleDocSeries { get; set; }
		public int VehicleDocType { get; set; }
		public int VehicleEngineCapacity { get; set; }
		public double VehicleEngineRating { get; set; }
		public int VehicleKeysCount { get; set; }
		public string VehicleLicensePlate { get; set; }
		public int VehicleManufactureYear { get; set; }
		public string VehicleMarka { get; set; }
		public int VehicleMaxLadenWeight { get; set; }
		public string VehicleModel { get; set; }
		public string VehicleSeating { get; set; }
		public string VehicleVIN { get; set; }
				
		void IPrintModel.SetValue(object value, string name)
		{
			throw new NotImplementedException();
		}

		T IPrintModel.GetValue<T>(string name)
		{
			throw new NotImplementedException();
		}

		void IPrintModel.Validate()
		{
		}
	}
	
}
