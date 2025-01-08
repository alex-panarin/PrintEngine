namespace PrintEngine.Templates.InputDataModels
{
	public class InputDataSample
	{
		public class Contract_
		{
			public string Number { get; set; }
			public DateTime Date { get; set; }
			public string Preamble { get; set; }
			public string LeaseAgreementNumber { get; set; }
			public string PledgeAgreementNumber { get; set; }
			public string Payer { get; set; }
			public string IsGAP { get; set; }
			public string InsuranceCond { get; set; }
			public string Additional { get; set; }
			public string ServiceCard { get; set; }
			public DateTime StartDate { get; set; }
			public DateTime EndDate { get; set; }
			public string IsInspection { get; set; }
			public string ActInspectionDate { get; set; }
			public string BottomString { get; set; }
		}
		public class Row_
		{
			public DateTime PeriodStartDate { get; set; }
			public DateTime PeriodEndDate { get; set; }
			public string SumInsured { get; set; }
			public string InsSummDSAGO { get; set; }
			public string InsSummPndExp { get; set; }
			public string InsSummPndTech { get; set; }
			public string InsSummNS { get; set; }
			public string InsSummDMS { get; set; }
			public string InsuransPremium { get; set; }
			public string InsPremiumDSAGO { get; set; }
			public string InsPremiumPndExp { get; set; }
			public string InsPremiumPndTech { get; set; }
			public string InsPremiumNS { get; set; }
			public string InsPremiumDMS { get; set; }
			public string InsPeriodPremium { get; set; }
		}
		public class RisksTable_
		{
			public Row_ Row { get; set; }
			public string UnConditionalFranchise { get; set; }
			public string ConditionalFranchise { get; set; }
			public string TotalPremium { get; set; }
			public string TotalPremiumString { get; set; }
			public string IsOutOfRoad { get; set; }
			public string IsDamageAtLoadUnload { get; set; }
		}
		public RisksTable_ RisksTable { get; set; }
		public class Signatory_
		{
			public string FullName { get; set; }
		}

		public class Insurant_
		{
			public string IsLessee { get; set; }
			public string Name { get; set; }
			public string INN { get; set; }
			public string KPP { get; set; }
			public string Address { get; set; }
			public string Phone { get; set; }
			public string BankDetails { get; set; }
			public Signatory_ Signatory { get; set; }
		}

		public class Proxy_
		{
			public string Number { get; set; }
			public string Date { get; set; }
		}

		public class Insurer_
		{
			public string UseFacsimile { get; set; }
			public string Post { get; set; }
			public string FullName { get; set; }
			public Proxy_ Proxy { get; set; }
		}

		public class LeasingParty_
		{
			public string Name { get; set; }
			public string INN { get; set; }
			public string KPP { get; set; }
			public string Address { get; set; }
			public string Phone { get; set; }
			public string BankDetails { get; set; }
		}

		public class Document_
		{
			public string Type { get; set; }
			public string Series { get; set; }
			public string Number { get; set; }
		}

		public class Vehicle_
		{
			public string Brand { get; set; }
			public string Model { get; set; }
			public string Category { get; set; }
			public string ManufactureYear { get; set; }
			public string VIN { get; set; }
			public string EnginePower { get; set; }
			public string LicensePlate { get; set; }
			public string AlarmSystem { get; set; }
			public string OutfitType { get; set; }
			public string InsCost { get; set; }
			public Document_ Document { get; set; }
		}

		public class Payment_
		{
			public string Value { get; set; }
			public DateTime EndDate { get; set; }
		}

		public string DocType { get; set; }
		public string LeasingCompanyId { get; set; }
		public string PolicyInsuransRules { get; set; }
		public Contract_ Contract { get; set; }
		public Insurant_ Insurant { get; set; }
		public Insurer_ Insurer { get; set; }
		public LeasingParty_ LeasingParty { get; set; }
		public Vehicle_ Vehicle { get; set; }
		public List<Payment_> Payments { get; set; }
	}
}
