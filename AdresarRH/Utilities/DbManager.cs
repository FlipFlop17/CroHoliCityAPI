using ClosedXML.Excel;
using CroHoliCityAPI.Data;
using CroHoliCityAPI.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace CroHoliCityAPI.Utilities
{
	//insert podataka iz tablice u bazu. first time only
	public static class DbManager 
	{
		/// <summary>
		/// Inserta nove podatke u bazu
		/// </summary>
		public static void InsertLokacijeDb( ApplicationDbContext db)
		{
			var filePath = "Assets/MjestaRH.xlsx";
			var sheetName = "Table 1";
			using (var wb = new XLWorkbook(filePath)) {
				var worksheet = wb.Worksheet(sheetName);
				int rowCount = worksheet.RowsUsed().Count();
				List<Lokacija> entities = new List<Lokacija>();

				for (int row = 3; row <= rowCount; row++) // Assuming data starts from the third row
				{
					Lokacija entity = new Lokacija
					{
						PostanskiBroj = (int)worksheet.Cell(row, 1).Value,
						Naziv= worksheet.Cell(row, 2).Value.ToString(),
						Naselje = worksheet.Cell(row, 3).Value.ToString(),
						Zupanija = worksheet.Cell(row, 4).Value.ToString(),
						VrijediOd=DateTime.Now.ToUniversalTime(),
					};

					db.Lokacije.Add(entity);
				}
				db.SaveChanges();
			}
		}

		public static void UpdateLokacijeDb(ApplicationDbContext db)
		{

		}
		public static void InsertNeradniDani( ApplicationDbContext db)
		{
			//delete the existing data
			db.Kalendar.ExecuteDelete();
            //read the NeradniDaniRH.json file from the Assets folder and add it to the database
            string daniJson = File.ReadAllText("Assets/NeradniDaniRH.json");

            JArray jArray = JArray.Parse(daniJson);
			//loop through the array and add to the dictionary
			foreach (JObject item in jArray) {
                JProperty property = item.Properties().First(); // Get the first property in the object

                Dan neradniDan = new Dan()
				{
					Datum = property.Name,
					Opis = property.Value.ToString(),
					NazivDan= DateTime.Parse(property.Name, CultureInfo.CreateSpecificCulture("hr-RH")).ToString("dddd", CultureInfo.CreateSpecificCulture("hr-RH")),
					NeradniDan=true
                };
				db.Kalendar.Add(neradniDan);
			}
			DateTime uskrsDatum= Helpers.CalculateEasterDate(DateTime.Now.Year);
			db.Kalendar.Add(new Dan() { Datum = uskrsDatum.ToString(), 
				Opis = "Uskrs", 
				NazivDan = uskrsDatum.ToString("dddd",CultureInfo.CreateSpecificCulture("hr-RH")),
				NeradniDan=true });
			DateTime tjelovoDatum= Helpers.CalculateTjelovoDate(DateTime.Now.Year);
			db.Kalendar.Add(new Dan() { 
				Datum = tjelovoDatum.ToString(), 
				Opis = "Tjelovo", 
				NazivDan = tjelovoDatum.ToString("dddd",CultureInfo.CreateSpecificCulture("hr-RH")),
				NeradniDan = true
			});

			db.SaveChanges();
		}
	}
}
