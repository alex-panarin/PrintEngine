using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using PrintEngine.Core;
using PrintEngine.Templates.InputDataModels;
using PrintEngine.Templates.UserModels;

namespace PrintEngine.Templates.UserTemplates
{
    /// <summary>
    /// Порядок разработки:
    /// 1. Создается класс модели печати (пример PrintModelSample), наследуемый от интерыейса IPrintModel
    /// 2. Создается класс Шаблона, наследуемый от PrintTemplateBase<ИмяВашегоКлассаМоделиПечати>
    /// 3. Устанавливаете на класс TemplateAttribute, в котором в свойстве Names указываете имена шаблонов
    /// 4. Получаете или создаете на основе запроса модель входных данных. 
    ///		Для валидации модели входных данных можно посмотреть пример из теста (EnsureInputDataIsValid)
    /// 5. Создается класс мапппера (пример PrintMapperSample), наследуемый
    ///		от PrintMapperBase<ИмяМоделиВходныхДанных, ИмяВашегоКлассаМоделиПечати>
    /// 6. В тестовом проекте реализованы основные варианты тестирования ваших классов.
    /// </summary>
    [Template(Templates = new[] { TEMPLATE }, Files = new[] { FILE })]
	public class PrintTemplateSample : PrintTemplateBase<PrintModelSample>
	{
		const string TEMPLATE = "SAMPLE";
		const string FILE = "Sample";
        /// <summary>
        /// Один шаблон может обрабатывать несколько форматов входных данных
        /// </summary>
        /// <returns></returns>
        public override Type[] GetValidInputDataTypes()
		{
			return new[] {typeof(InputDataSample) };
		}
		protected override string GetTemplateFileName()
		{
			return FILE;
		}

		protected override Task ProcessTemplate(Document layout)
		{
			//var cyrEncoding = FontEncoding.CreateFontEncoding("cp1251").GetBaseEncoding();
			// Шрифты и картинки берутся из рессурсов, куда их можно добавить во время раработки
			// (проект RgsPrintEngine.Resources)
			var cyrFont = ResourceService.GetFont("arial");
			var cyrFontBOLD = ResourceService.GetFont("arialbd");
			var cyrFontItal = ResourceService.GetFont("ariali");

			// Специальный шрифт для добавления checkbox и прочего ...
			// http://www.alanwood.net/demos/wingdings.html
			var wingDing = ResourceService.GetFont("wingding");
			
			var image = ResourceService.GetImage("rgs");

			layout.SetFont(cyrFont);
			#region T1
			Table table1 = new Table(UnitValue.CreatePercentArray(2))
				.UseAllAvailableWidth()
				.SetFont(cyrFont)
				// Вот здесь добавляется картинка
				.AddCell(new Cell(1, 2).Add(new Image(image) 
					.SetAutoScale(true))
					.SetBorder(Border.NO_BORDER)
				)
				.AddCell(new Cell(1, 2)
					.Add(new Paragraph("ПОЛИС ДОБРОВОЛЬНОГО СТРАХОВАНИЯ ТРАНСПОРТНОГО СРЕДСТВА")
						.SetFixedLeading(6)
						.SetTextAlignment(TextAlignment.CENTER))
					.SetVerticalAlignment(VerticalAlignment.MIDDLE)
					.SetFont(cyrFontBOLD)
					.SetFontSize(12f)
					.SetHeight(10f)
				)
				.AddCell(new Cell(1, 2)
					.Add(new Paragraph("серия 44620003010000 № 000002191")
						.SetTextAlignment(TextAlignment.CENTER)
						.SetFixedLeading(6))
					.SetVerticalAlignment(VerticalAlignment.MIDDLE)
					.SetFont(cyrFontBOLD)
					.SetFontSize(12f)
					.SetHeight(10f)
				)
				.AddCell(new Cell(1, 1)
					.Add(new Paragraph("РОСГОССТРАХ АВТО «ЗАЩИТА»").SetFixedLeading(6))
					.SetTextAlignment(TextAlignment.CENTER)
					.SetVerticalAlignment(VerticalAlignment.MIDDLE)
					.SetHeight(10f)
					.SetFont(cyrFontItal)
				)
				.AddCell(new Cell(1, 1)
					.Add(new Paragraph("Круглосуточный контакт - центр тел.").SetFontSize(8f).SetFixedLeading(10))
					.SetTextAlignment(TextAlignment.CENTER)
					.SetVerticalAlignment(VerticalAlignment.MIDDLE)
					.SetHeight(10f)
				);
			#endregion
			#region T2	
			Table table2 = new Table(UnitValue.CreatePercentArray(4))
				.UseAllAvailableWidth()
				.SetFont(cyrFont)
				.SetFontSize(8f)
				.AddCell(new Cell(1, 1).Add(new Paragraph("ПАО СК «Росгосстрах»").SetItalic()).SetHeight(16f))
				.AddCell(new Cell(1, 1)
					.Add(new Paragraph("Лицензия СИ0001").SetTextAlignment(TextAlignment.RIGHT).SetFixedLeading(8f))
					.Add(new Paragraph("СЛ0001").SetFixedLeading(8f).SetTextAlignment(TextAlignment.RIGHT)))
				.AddCell(new Cell(1, 1).Add(new Paragraph("- для звонков из Москвы,").SetFixedLeading(8f)).Add(new Paragraph("- для звонков из др. регионов РФ").SetFixedLeading(8f)))
				.AddCell(new Cell(1, 1).Add(new Paragraph("+7 (495) 926-55-55").SetFixedLeading(8f)).Add(new Paragraph("8-800-200-0-900").SetFixedLeading(8f)))
				.AddCell(new Cell(1, 4).Add(new Paragraph("Настоящий полис удостоверяет факт заключения договора страхования на условиях Правил добровольного страхования транспортных средств и спецтехники (типовых (единых)) №171 в действующей редакции (далее - Правила страхования ТС), Приложения №1 к настоящему Полису «Сервисные услуги», разработанного на основании Правил страхования ТС, Правил добровольного страхования гражданской ответственности владельцев транспортных средств №150 в действующей редакции (далее - Правила ДСАГО), Правил добровольного медицинского страхования граждан (типовых (единых)) №152 в действующей редакции (далее - Правила ДМС) и Приложения №2 к настоящему Полису - Программы страхования «Медицинская помощь для водителя» (далее - Программа ДМС), разработанной на основании Правил ДМС.")
						.SetFixedLeading(8f)
						.SetMarginLeft(2)
						.SetMarginRight(2))
					.SetHeight(56f)
					.SetTextAlignment(TextAlignment.JUSTIFIED));
			#endregion
			#region T3
			var table3 = new Table(UnitValue.CreatePercentArray(new[] { 26f, 74f }))
				.UseAllAvailableWidth()
				.SetFontSize(8)
				.SetFont(cyrFont)
				.AddCell(new Cell()
					.Add(new Paragraph("Срок действия Договора").SetFontSize(10).SetFont(cyrFontBOLD).SetFixedLeading(8f))
					.SetHorizontalAlignment(HorizontalAlignment.CENTER))
				.AddCell(new Cell()
					.Add(new Paragraph("С 00ч. 00мин. 02/06/2022 г. по 23ч. 59мин. 01/06/2023 г., но не ранее даты уплаты страховой премии\r\n(первого страхового взноса) в полном объеме (акцепта Полиса - оферты)")
					.SetFixedLeading(8f)));
			#endregion
			#region T4
			// Здесь можно посмотреть как устанавливаются checkbox
			var table4 = new Table(UnitValue.CreatePercentArray(new[] { 10f, 1f, 16, 1, 16, 1, 16, 15, 25 }))
				.UseAllAvailableWidth()
				.SetFont(cyrFont)
				.SetFontSize(8)
				.SetHorizontalAlignment(HorizontalAlignment.CENTER)
				.AddCell(new Cell().Add(new Paragraph("Признак\r\nПолиса").SetFixedLeading(8f)))
				// Для использования глифов их указывают посредством юникода
				.AddCell(new Cell(2, 1).Add(new Paragraph("\u00FE").SetFont(wingDing).SetFontSize(10f)
					.SetHorizontalAlignment(HorizontalAlignment.LEFT))
					.SetBorderRight(Border.NO_BORDER)
				)
				.AddCell(new Cell(2, 1).Add(new Paragraph("первоначальный")
						.SetHorizontalAlignment(HorizontalAlignment.LEFT)
						.SetFirstLineIndent(0))
					.SetBorderLeft(Border.NO_BORDER)
					.SetBorderRight(Border.NO_BORDER)
					.SetVerticalAlignment(VerticalAlignment.MIDDLE)
					.SetMarginLeft(0))
				.AddCell(new Cell(2, 1).Add(new Paragraph("\u00A8").SetFont(wingDing).SetFontSize(10f))
					.SetBorderLeft(Border.NO_BORDER)
					.SetBorderRight(Border.NO_BORDER)
					.SetHorizontalAlignment(HorizontalAlignment.LEFT))
				.AddCell(new Cell(2, 1).Add(new Paragraph("возобновление"))
					.SetBorderLeft(Border.NO_BORDER)
					.SetBorderRight(Border.NO_BORDER)
					.SetVerticalAlignment(VerticalAlignment.MIDDLE)
					.SetMarginLeft(0))
				.AddCell(new Cell(2, 1).Add(new Paragraph("\u00A8").SetFont(wingDing).SetFontSize(10f))
					.SetBorderLeft(Border.NO_BORDER)
					.SetBorderRight(Border.NO_BORDER)
					.SetHorizontalAlignment(HorizontalAlignment.LEFT))
				.AddCell(new Cell(2, 1).Add(new Paragraph("выдан взамен"))
					.SetBorderLeft(Border.NO_BORDER)
					.SetVerticalAlignment(VerticalAlignment.MIDDLE)
					.SetMarginLeft(0))
				.AddCell(new Cell(2, 1).Add(new Paragraph("Предыдущий полис").SetFixedLeading(8f)))
				.AddCell(new Cell(2, 1).Add(new Paragraph("---")));
			#endregion
			#region T5
			var table5 = new Table(1)
				.AddCell(new Cell().Add(new Paragraph("1. Страховщик").SetFontSize(10).SetFont(cyrFontBOLD).SetFixedLeading(8)))
				.AddCell(new Cell().Add(new Paragraph("ПАО СК «Росгосстрах», адрес местонахождения: 140002, Московская область, г. Люберцы, ул. Парковая, д.3, почтовый адрес: 119991, РФ, Москва-59, ГСП-1, ул. Киевская, д.7. Официальный сайт https://www.rgs.ru/. Банковские реквизиты ИНН/КПП 7707067683/997950001, р/с 40701810900000000187 в ПАО «РГС Банк» в г. Москва, к/с 30101810945250000174, БИК 044525174")
					.SetFontSize(8)
					.SetFixedLeading(10))
					.SetTextAlignment(TextAlignment.JUSTIFIED));
			#endregion
			#region T6
			var table6 = new Table(UnitValue.CreatePercentArray(new[] { 20f, 1f, 19f, 1f, 29f, 1f, 29f }))
				.UseAllAvailableWidth()
				.SetFont(cyrFont)
				.SetFontSize(8)
				.AddCell(new Cell().Add(new Paragraph("2. Страхователь").SetFontSize(10).SetFont(cyrFontBOLD).SetFixedLeading(8)))
				.AddCell(new Cell().Add(new Paragraph("\u00FE").SetFont(wingDing).SetFontSize(10f).SetFixedLeading(8))
					.SetBorderRight(Border.NO_BORDER)
					.SetHorizontalAlignment(HorizontalAlignment.RIGHT))
				.AddCell(new Cell().Add(new Paragraph("физическое лицо").SetFixedLeading(8))
					.SetBorderLeft(Border.NO_BORDER)
					.SetBorderRight(Border.NO_BORDER)
					.SetVerticalAlignment(VerticalAlignment.MIDDLE)
					.SetMarginLeft(0)) 
				.AddCell(new Cell().Add(new Paragraph("\u00A8").SetFont(wingDing).SetFontSize(10f).SetFixedLeading(8))
					.SetBorderRight(Border.NO_BORDER)
					.SetBorderLeft(Border.NO_BORDER)
					.SetHorizontalAlignment(HorizontalAlignment.RIGHT))
				.AddCell(new Cell().Add(new Paragraph("индивидуальный предприниматель").SetFixedLeading(8))
					.SetBorderLeft(Border.NO_BORDER)
					.SetBorderRight(Border.NO_BORDER)
					.SetVerticalAlignment(VerticalAlignment.MIDDLE)
					.SetMarginLeft(0))
				.AddCell(new Cell().Add(new Paragraph("\u00A8").SetFont(wingDing).SetFontSize(10f).SetFixedLeading(8))
					.SetBorderLeft(Border.NO_BORDER)
					.SetBorderRight(Border.NO_BORDER)
					.SetHorizontalAlignment(HorizontalAlignment.RIGHT))
				.AddCell(new Cell().Add(new Paragraph("юридическое лицо").SetFixedLeading(8))
					.SetBorderLeft(Border.NO_BORDER)
					.SetVerticalAlignment(VerticalAlignment.MIDDLE)
					.SetMarginLeft(0));
			#endregion
			#region T7
			// Здесь показано как идет обращение к модели данных.
			// Каждый шаблон имеет встроенное свойство Model, которое соответсвует модели данных для печати
			var document = GetInsurantDocumentValues(Model.InsurantDocument);
			var table7 = new Table(UnitValue.CreatePercentArray(new[] { 26f, 14f, 30f, 20f, 10f }))
				.UseAllAvailableWidth()
				.SetFont(cyrFont)
				.SetFontSize(7)
				.AddCell(new Cell(1, 1).Add(new Paragraph("ФИО (отчество при наличии)/\r\nНаименование юр. лица").SetFixedLeading(7)
					.SetVerticalAlignment(VerticalAlignment.TOP)))
				//.Add(new Paragraph("Наименование юр. лица").SetFixedLeading(10))))
				.AddCell(new Cell(1, 2).Add(new Paragraph($"{Model.InsurantFullName}").SetFixedLeading(7).SetVerticalAlignment(VerticalAlignment.TOP)))
				.AddCell(new Cell(1, 1).Add(new Paragraph("Дата рождения").SetFixedLeading(7).SetVerticalAlignment(VerticalAlignment.TOP)))
				.AddCell(new Cell(1, 1).Add(new Paragraph($"{Model.InsurantBirthDate.ToShortDateString()}").SetFixedLeading(7).SetVerticalAlignment(VerticalAlignment.TOP)))
				.AddCell(new Cell(3, 1).Add(new Paragraph("Документ, удостоверяющий\r\nличность/Документ,\r\nподтверждающий регистрацию ЮЛ")
					.SetFixedLeading(8).SetVerticalAlignment(VerticalAlignment.TOP)).SetHeight(30))
				.AddCell(new Cell(1, 4).Add(new Paragraph($"{document.t}").SetFixedLeading(7)).SetHeight(8).SetVerticalAlignment(VerticalAlignment.TOP))
				.AddCell(new Cell(1, 1).Add(new Paragraph("Серия/номер").SetFixedLeading(7).SetVerticalAlignment(VerticalAlignment.TOP)))
				.AddCell(new Cell(1, 1).Add(new Paragraph($"{document.sn}").SetFixedLeading(7).SetVerticalAlignment(VerticalAlignment.TOP)))
				.AddCell(new Cell(1, 1).Add(new Paragraph("Дата выдачи").SetFixedLeading(7).SetVerticalAlignment(VerticalAlignment.TOP)))
				.AddCell(new Cell(1, 1).Add(new Paragraph($"{document.d}").SetFixedLeading(7).SetVerticalAlignment(VerticalAlignment.TOP)))
				.AddCell(new Cell(1, 1).Add(new Paragraph("Кем выдан").SetFixedLeading(7).SetVerticalAlignment(VerticalAlignment.TOP)))
				.AddCell(new Cell(1, 3).Add(new Paragraph($"{document.ib}").SetFixedLeading(7).SetVerticalAlignment(VerticalAlignment.TOP)))
				.AddCell(new Cell().Add(new Paragraph("Место рождения").SetFixedLeading(7).SetHeight(8)))
				.AddCell(new Cell(1, 4).Add(new Paragraph("")))
				.AddCell(new Cell(1, 1).Add(new Paragraph("Почтовый индекс и адрес по месту\r\nрегистрации").SetFixedLeading(7).SetVerticalAlignment(VerticalAlignment.TOP)))
				.AddCell(new Cell(1, 4).Add(new Paragraph($"{Model.InsurantAddress}").SetFixedLeading(7)).SetVerticalAlignment(VerticalAlignment.TOP)
				);
			#endregion
			#region T8
			var table8 = new Table(UnitValue.CreatePercentArray(new[] { 13f, 13f, 18f, 16f, 20f, 20f }))
				.UseAllAvailableWidth()
				.SetFont(cyrFont)
				.SetFontSize(7)
				.AddCell(new Cell().Add(new Paragraph("ИНН").SetFixedLeading(7)))
				.AddCell(new Cell().Add(new Paragraph("-").SetFixedLeading(7)))
				.AddCell(new Cell().Add(new Paragraph("ОГРНИП /ОГРН").SetFixedLeading(7)))
				.AddCell(new Cell().Add(new Paragraph("-").SetFixedLeading(7)))
				.AddCell(new Cell().Add(new Paragraph("Дата гос. регистрации").SetFixedLeading(7)))
				.AddCell(new Cell().Add(new Paragraph("").SetFixedLeading(7)));
			#endregion
			#region T9
			var table9 = new Table(UnitValue.CreatePercentArray(new[] { 13f, 13f, 18f, 16f, 10f, 30f }))
				.UseAllAvailableWidth()
				.SetFont(cyrFont)
				.SetFontSize(7)
				.AddCell(new Cell().Add(new Paragraph("Гражданство").SetFixedLeading(7)))
				.AddCell(new Cell().Add(new Paragraph("РОССИЯ").SetFixedLeading(7)))
				.AddCell(new Cell().Add(new Paragraph("Тел. (мобильный для ФЛ)").SetFixedLeading(7)))
				.AddCell(new Cell().Add(new Paragraph("+79621868584").SetFixedLeading(7)))
				.AddCell(new Cell().Add(new Paragraph("e-mail").SetFixedLeading(7)))
				.AddCell(new Cell().Add(new Paragraph("ioparnikova@mmtr.ru").SetFixedLeading(7)));
			#endregion
			#region T10
			var table10 = new Table(UnitValue.CreatePercentArray(new[] { 20f, 1f, 19f, 1f, 29f, 1f, 29f }))
				.UseAllAvailableWidth()
				.SetFont(cyrFont)
				.SetFontSize(8)
				.AddCell(new Cell().Add(new Paragraph("3. Собственник").SetFontSize(10).SetFont(cyrFontBOLD).SetFixedLeading(8)))
				.AddCell(new Cell().Add(new Paragraph("\u00FE").SetFont(wingDing).SetFontSize(10f).SetFixedLeading(8))
					.SetBorderRight(Border.NO_BORDER)
					.SetHorizontalAlignment(HorizontalAlignment.RIGHT))
				.AddCell(new Cell().Add(new Paragraph("физическое лицо").SetFixedLeading(8))
					.SetBorderLeft(Border.NO_BORDER)
					.SetBorderRight(Border.NO_BORDER)
					.SetVerticalAlignment(VerticalAlignment.MIDDLE)
					.SetMarginLeft(0))
				.AddCell(new Cell().Add(new Paragraph("\u00A8").SetFont(wingDing).SetFontSize(10f).SetFixedLeading(8))
					.SetBorderRight(Border.NO_BORDER)
					.SetBorderLeft(Border.NO_BORDER)
					.SetHorizontalAlignment(HorizontalAlignment.RIGHT))
				.AddCell(new Cell().Add(new Paragraph("индивидуальный предприниматель").SetFixedLeading(8))
					.SetBorderLeft(Border.NO_BORDER)
					.SetBorderRight(Border.NO_BORDER)
					.SetVerticalAlignment(VerticalAlignment.MIDDLE)
					.SetMarginLeft(0))
				.AddCell(new Cell().Add(new Paragraph("\u00A8").SetFont(wingDing).SetFontSize(10f).SetFixedLeading(8))
					.SetBorderLeft(Border.NO_BORDER)
					.SetBorderRight(Border.NO_BORDER)
					.SetHorizontalAlignment(HorizontalAlignment.RIGHT))
				.AddCell(new Cell().Add(new Paragraph("юридическое лицо").SetFixedLeading(8))
					.SetBorderLeft(Border.NO_BORDER)
					.SetVerticalAlignment(VerticalAlignment.MIDDLE)
					.SetMarginLeft(0));
			#endregion
			//AddWatermark(layout, cyrFont);

			layout.Add(table1);
			layout.Add(table2);
			layout.Add(table3);
			layout.Add(table4);
			layout.Add(table5);
			layout.Add(table6);
			layout.Add(table7);
			layout.Add(table8);
			layout.Add(table9);
			layout.Add(table10);
			layout.Add(table7);
			layout.Add(table8);
			layout.Add(table9);
			layout.Add(table10);
			layout.Add(table7);
			layout.Add(table8);
			layout.Add(table9);
			layout.Add(table10);
			layout.Add(table7);
			layout.Add(table8);
			layout.Add(table9);
			//layout.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

			layout.Add(table10);
			layout.Add(table7);
			layout.Add(table8);
			layout.Add(table9);
			return Task.CompletedTask;
		}

		private (string t, string sn, string d, string ib) GetInsurantDocumentValues(string doc)
		{
			var docs = doc.Split('-');
			var t = docs[0];
			docs = docs[1].Split("выдан", StringSplitOptions.RemoveEmptyEntries);
			var sn = docs[0];
			var d = docs[1].Trim().Split(' ')[0];
			var ib = docs[1].Replace(d, "").Trim();
			return (t, sn, d, ib);
		}
	}
}
