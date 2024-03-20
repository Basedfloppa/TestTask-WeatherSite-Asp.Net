using db_context;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace DatabaseService
{
    public static class db_service
    {
        public static void Migrate()
        {
            WeatherContext db = new WeatherContext();
            db.Database.EnsureCreated();
            db.Database.Migrate();
            db.Dispose();
        }

        public static void XlsxTableImport()
        {
            try
            {
                DirectoryInfo directory = new DirectoryInfo(@".\uploads");
                FileInfo[] files = directory.GetFiles("*.xlsx");
                WeatherContext db = new WeatherContext();

                foreach (FileInfo file in files)
                {
                    IWorkbook workbook;
                    using (FileStream fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
                    {
                        workbook = new XSSFWorkbook(fileStream);
                    }

                    for (int i = 0; i < workbook.NumberOfSheets; i++)
                    {
                        ISheet sheet = workbook.GetSheetAt(i);

                        for (int j = 0; j <= sheet.LastRowNum; j++) // Fixed typo here from 'i' to 'j'
                        {
                            IRow row = sheet.GetRow(j);

                            try
                            {
                                Weather entry = new Weather{
                                    Date = DateOnly.FromDateTime(DateTime.Parse(row.GetCell(0).StringCellValue)),
                                    Time = DateTime.Parse(row.GetCell(1).StringCellValue),
                                    Temperature = Convert.ToSByte(row.GetCell(2).NumericCellValue),
                                    AirMoisture = Convert.ToByte(row.GetCell(3).NumericCellValue),
                                    DewPoint = Convert.ToSByte(row.GetCell(4).NumericCellValue),
                                    Pressure = Convert.ToUInt16(row.GetCell(5).NumericCellValue),
                                    AirDirection = row.GetCell(6).StringCellValue,
                                    AirSpeed = Convert.ToByte(row.GetCell(7).NumericCellValue),
                                    Cloudiness = Convert.ToByte(row.GetCell(8).NumericCellValue),
                                    LowerCloudinessTreshold = Convert.ToUInt16(row.GetCell(9).NumericCellValue),
                                    HorizontalVisibility = Convert.ToByte(row.GetCell(10).NumericCellValue),
                                    WeatherConditions = row.GetCell(11).StringCellValue
                                };
                                

                                db.Weathers.Add(entry);
                                db.SaveChanges();
                            }
                            catch (Exception e)
                            {
                                // Log the error instead of writing to console
                                Console.BackgroundColor = ConsoleColor.Red;
                                Console.WriteLine(e.Message);
                                Console.ResetColor();
                            }
                        }
                    }
                    file.Delete();
                }
                db.Dispose();
            }
            catch (Exception ex)
            {
                // Log the error instead of writing to console
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
        }
    }
}