using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using IDWORKS_STUDENT.Models;
using System.Data;
using ExcelDataReader;
using Microsoft.Extensions.Configuration;
using TinyCsvParser;
using System.Text;
using System.Globalization;

namespace IDWORKS_STUDENT
{
    public class LocalFileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment envrionment;

        public LocalFileUploadService(IWebHostEnvironment envionrment)
        {
            this.envrionment = envionrment;
        }
        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var filePath = Path.Combine(envrionment.ContentRootPath, "Staging", file.FileName);
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return filePath;
        }

        public List<ExcelDataInfo.Section> ReadCSV(IFormFile file)
        {
            List<ExcelDataInfo.Section> sections = new List<ExcelDataInfo.Section>();
            ExcelDataInfo.Section section = new ExcelDataInfo.Section(0, 0, 0, 0, 0, 0, 0);
            sections.Add(section);

            CultureInfo culture = new CultureInfo("en-US");
            CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',');
            IDWORKS_STUDENT.CSV.CsvUserDetailsMapping csvMapper = new IDWORKS_STUDENT.CSV.CsvUserDetailsMapping();
            CsvParser<IDWORKS_STUDENT.CSV.StudentCSVDetails> csvParser = new CsvParser<IDWORKS_STUDENT.CSV.StudentCSVDetails>(csvParserOptions, csvMapper);

            var filePath = Path.Combine(envrionment.ContentRootPath, "Staging", file.FileName);

            var result = csvParser
                       .ReadFromFile(filePath, Encoding.ASCII)
                       .ToList();

            foreach (var details in result)
            {
                if (details.Result != null)
                {
                    DateTime parsedDate = new DateTime();
                    bool wasDateParsed = DateTime.TryParse(details.Result.EffectiveDate, out parsedDate);
                    bool blankCsv = isBlankCSVRecords(details);

                    if (wasDateParsed && !blankCsv)
                    {
                        DateTime effectiveDate = Convert.ToDateTime(details.Result.EffectiveDate, culture);
                        ExcelDataInfo.DataInfoCell dataInfoCell = new ExcelDataInfo.DataInfoCell(details.Result.Coverage, details.Result.FirstName, details.Result.LastName,
                                                                details.Result.School, details.Result.PolicyNumber, effectiveDate, details.Result.CoverageProvider);
                        section.dataInfoCells.Add(dataInfoCell);

                    } else
                    {
                        ExcelDataInfo.ErrorInfoCell errorInfoCell = new ExcelDataInfo.ErrorInfoCell(details.Result.Coverage, details.Result.FirstName, details.Result.LastName,
                              details.Result.School, details.Result.PolicyNumber, parsedDate, details.Result.CoverageProvider);

                        section.errorInfoCells.Add(errorInfoCell);
                    }
                }
            }
            // Delete file in staging folder
            if (File.Exists(filePath))
                File.Delete(filePath);

            return sections;
        }

        public List<ExcelDataInfo.Section> ReadExcel(IFormFile file)
        {
            List<ExcelDataInfo.Section> sections = new List<ExcelDataInfo.Section>();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var filePath = Path.Combine(envrionment.ContentRootPath, "Staging", file.FileName);
            var dt = new DataTable();

            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        string cellVal = (reader.GetValue(0) != null) ? reader.GetValue(0).ToString() : "";
                        ColumnType columnType = determineWhichColumnToUse(cellVal);
                        bool isHeader = wasHeaderFound(columnType);

                        if (isHeader)
                        {
                            int _coverageIndex = -1;
                            int _coverageProviderIndex = -1;
                            int _effectiveDateIndex = -1;
                            int _firstNameIndex = -1;
                            int _lastNameIndex = -1;
                            int _policyIndex = -1;
                            int _schoolIndex = -1;

                            for (int i = 0; i < IDWORKS_STUDENT.AppConfig.IDWorksAppConfig._numberOfColumns; i++)
                            {
                                string cellVal2 = (reader.GetValue(i) != null) ? reader.GetValue(i).ToString() : "";
                                ColumnType columnType2 = determineWhichColumnToUse(cellVal2);

                                if (_coverageIndex < 0) { _coverageIndex = columnType2 == ColumnType.Coverage ? i : -1; }
                                if (_coverageProviderIndex < 0) { _coverageProviderIndex = columnType2 == ColumnType.CoverageProvider ? i : -1; }
                                if (_effectiveDateIndex < 0) { _effectiveDateIndex = columnType2 == ColumnType.EffectiveDate ? i : -1; }
                                if (_firstNameIndex < 0) { _firstNameIndex = columnType2 == ColumnType.FirstName ? i : -1; }
                                if (_lastNameIndex < 0) { _lastNameIndex = columnType2 == ColumnType.LastName ? i : -1; }
                                if (_policyIndex < 0) { _policyIndex = columnType2 == ColumnType.Policy ? i : -1; }
                                if (_schoolIndex < 0) { _schoolIndex = columnType2 == ColumnType.School ? i : -1; }
                            }

                            ExcelDataInfo.Section section = new ExcelDataInfo.Section(_coverageIndex, _firstNameIndex, _lastNameIndex, _schoolIndex, _policyIndex, _effectiveDateIndex, _coverageProviderIndex);
                            sections.Add(section);
                        }
                        else
                        {
                            // Get the last section to populate the asscociated data info
                            if (sections.Count > 0 && sections.Last() != null)
                            {
                                ExcelDataInfo.Section lastSection = sections.Last();

                                bool incorrectColumnFormatting = doHaveIncorrectColumnFormatting(lastSection);

                                // Detect for any incorrect column formatting
                                // If none, proceed...
                                if (!incorrectColumnFormatting) {

                                    // Get Coverage 
                                    string data_coverage = (reader.GetValue(lastSection.CoverageIndex) != null) ? reader.GetValue(lastSection.CoverageIndex).ToString() : "";
                                    string data_coverageProvider = (reader.GetValue(lastSection.CoverageProviderIndex) != null) ? reader.GetValue(lastSection.CoverageProviderIndex).ToString() : "";
                                    string data_effectiveDate = (reader.GetValue(lastSection.EffectiveDateIndex) != null) ? reader.GetValue(lastSection.EffectiveDateIndex).ToString() : "";
                                    string data_firstName = (reader.GetValue(lastSection.FirstNameIndex) != null) ? reader.GetValue(lastSection.FirstNameIndex).ToString() : "";
                                    string data_lastName = (reader.GetValue(lastSection.LastNameIndex) != null) ? reader.GetValue(lastSection.LastNameIndex).ToString() : "";
                                    string data_policy = (reader.GetValue(lastSection.PolicyIndex) != null) ? reader.GetValue(lastSection.PolicyIndex).ToString() : "";
                                    string data_school = (reader.GetValue(lastSection.SchoolIndex) != null) ? reader.GetValue(lastSection.SchoolIndex).ToString() : "";

                                    DateTime parsedDate = new DateTime();
                                    bool wasDateParsed = DateTime.TryParse(data_effectiveDate, out parsedDate);

                                    if (wasDateParsed && isDataValid(data_coverage, data_coverageProvider, data_firstName, data_lastName, data_policy, data_school))
                                    {
                                        ExcelDataInfo.DataInfoCell dataInfoCell = new ExcelDataInfo.DataInfoCell(data_coverage, data_firstName, data_lastName, data_school, data_policy, parsedDate, data_coverageProvider);
                                        lastSection.dataInfoCells.Add(dataInfoCell);
                                    }
                                    else
                                    {
                                        bool ignoreLine = shouldIgnoreLineForErrors(data_effectiveDate, data_coverage, data_coverageProvider, data_firstName, data_lastName, data_policy, data_school);
                                        if (!ignoreLine)
                                        {
                                            ExcelDataInfo.ErrorInfoCell errorInfoCell = new ExcelDataInfo.ErrorInfoCell(data_coverage, data_firstName, data_lastName, data_school, data_policy, parsedDate, data_coverageProvider);
                                            lastSection.errorInfoCells.Add(errorInfoCell);
                                        }
                                    }
                                } else
                                {
                                    lastSection.ColumnsInCorrectlyFormatted = true;
                                }
                            }
                        }
                    }
                }
            }
            // Delete file in staging folder
            if (File.Exists(filePath))
                File.Delete(filePath);

            return sections;
        }

        private bool doHaveIncorrectColumnFormatting(ExcelDataInfo.Section section)
        {
            bool incorrectColumnFormatting = false;

            if (section.LastNameIndex < 0 || section.FirstNameIndex < 0 || section.EffectiveDateIndex < 0 ||
                section.CoverageIndex < 0 || section.CoverageProviderIndex < 0 || section.PolicyIndex < 0
                || section.SchoolIndex < 0)
                incorrectColumnFormatting = true;

            return incorrectColumnFormatting;
        }

        private bool isDataValid(string coverage, string coverageProvider, string firstName, string lastName, string policy, string school)
        {
            bool isValid = false;

            if (coverage.Replace(" ", "").Length > 0 && coverageProvider.Replace(" ", "").Length > 0 && firstName.Replace(" ", "").Length > 0 && lastName.Replace(" ", "").Length > 0 && policy.Replace(" ", "").Length > 0 && school.Replace(" ", "").Length > 0)
                isValid = true;

            return isValid;
        }

        private bool shouldIgnoreLineForErrors(string effectiveDate, string coverage, string coverageProvider, string firstName, string lastName, string policy, string school)
        {
            bool ignoreBlankLine = true;

            DateTime parsedDate = new DateTime();
            int countDataEntry = 0;

            effectiveDate = effectiveDate.Replace(" ", "");
            countDataEntry += effectiveDate.Length > 0 ? 1 : 0;

            coverage = coverage.Replace(" ", "");
            countDataEntry += coverage.Length > 0 ? 1 : 0;

            coverageProvider = coverageProvider.Replace(" ", "");
            countDataEntry += coverageProvider.Length > 0 ? 1 : 0;

            firstName = firstName.Replace(" ", "");
            countDataEntry += firstName.Length > 0 ? 1 : 0;

            lastName = lastName.Replace(" ", "");
            countDataEntry += lastName.Length > 0 ? 1 : 0;

            policy = policy.Replace(" ", "");
            countDataEntry += policy.Length > 0 ? 1 : 0;

            school = school.Replace(" ", "");
            countDataEntry += school.Length > 0 ? 1 : 0;

            bool wasDateParsed = DateTime.TryParse(effectiveDate, out parsedDate);

            if (countDataEntry > 1)
            {
                if (effectiveDate.Length <= 0 || !wasDateParsed || coverage.Length <= 0 || coverageProvider.Length <= 0 || firstName.Length <= 0
                    || lastName.Length <= 0 || policy.Length <= 0 || school.Length <= 0)
                    ignoreBlankLine = false;
            }
           
            return ignoreBlankLine;
        }

        private bool wasHeaderFound(ColumnType columnType)
        {
            bool isHeader = false;

            switch (columnType)
            {
                case ColumnType.Coverage:
                    isHeader = true;
                    break;

                case ColumnType.CoverageProvider:
                    isHeader = true;
                    break;

                case ColumnType.EffectiveDate:
                    isHeader = true;
                    break;

                case ColumnType.FirstName:
                    isHeader = true;
                    break;

                case ColumnType.LastName:
                    isHeader = true;
                    break;

                case ColumnType.Policy:
                    isHeader = true;
                    break;

                case ColumnType.School:
                    isHeader = true;
                    break;
            }

            return isHeader;
        }

        private ColumnType determineWhichColumnToUse(string s)
        {
            ColumnType colType = ColumnType.None;
            bool continueSearch = true;
            s = s.ToLower().Replace(" ", "").Trim();
            foreach (string col in IDWORKS_STUDENT.AppConfig.IDWorksAppConfig._coverageColumns)
            {
                string col2 = col.ToLower().Trim().Replace(" ", "");
                if (s == col2)
                {
                    colType = ColumnType.Coverage;
                    continueSearch = false;
                    break;
                }
            }

            if (continueSearch)
            {
                foreach (string col in IDWORKS_STUDENT.AppConfig.IDWorksAppConfig._coverageProviderColumns)
                {
                    string col2 = col.ToLower().Trim().Replace(" ", "");
                    if (s == col2)
                    {
                        colType = ColumnType.CoverageProvider;
                        continueSearch = false;
                        break;
                    }
                }
            }

            if (continueSearch)
            {
                foreach (string col in IDWORKS_STUDENT.AppConfig.IDWorksAppConfig._firstNameColumns)
                {
                    string col2 = col.ToLower().Trim().Replace(" ", "");
                    if (s == col2)
                    {
                        colType = ColumnType.FirstName;
                        continueSearch = false;
                        break;
                    }
                }
            }

            if (continueSearch)
            {
                foreach (string col in IDWORKS_STUDENT.AppConfig.IDWorksAppConfig._lastNameColumns)
                {
                    string col2 = col.ToLower().Trim().Replace(" ", "");
                    if (s == col2)
                    {
                        colType = ColumnType.LastName;
                        continueSearch = false;
                        break;
                    }
                }
            }

            if (continueSearch)
            {
                foreach (string col in IDWORKS_STUDENT.AppConfig.IDWorksAppConfig._schoolColumns)
                {
                    string col2 = col.ToLower().Trim().Replace(" ", "");
                    if (s == col2)
                    {
                        colType = ColumnType.School;
                        continueSearch = false;
                        break;
                    }
                }
            }

            if (continueSearch)
            {
                foreach (string col in IDWORKS_STUDENT.AppConfig.IDWorksAppConfig._effectiveDateColumns)
                {
                    string col2 = col.ToLower().Trim().Replace(" ", "");
                    if (s == col2)
                    {
                        colType = ColumnType.EffectiveDate;
                        break;
                    }
                }
            }


            if (continueSearch)
            {
                foreach (string col in IDWORKS_STUDENT.AppConfig.IDWorksAppConfig._policyColumns)
                {
                    string col2 = col.ToLower().Trim().Replace(" ", "");
                    if (s == col2)
                    {
                        colType = ColumnType.Policy;
                        break;
                    }
                }
            }

            return colType;
        }

        private bool isBlankCSVRecords(TinyCsvParser.Mapping.CsvMappingResult<CSV.StudentCSVDetails> student)
        {
            bool isBlank = false;

            string coverage = student.Result.Coverage.Trim().Replace(" ", "");
            string firstName = student.Result.FirstName.Trim().Replace(" ", "");
            string lastName = student.Result.LastName.Trim().Replace(" ", "");
            string school = student.Result.School.Trim().Replace(" ", "");
            string policy = student.Result.PolicyNumber.Trim().Replace(" ", "");
            string coverageProvider = student.Result.CoverageProvider.Trim().Replace(" ", "");

            if (coverage.Length <= 0 || firstName.Length <= 0 || lastName.Length <= 0 || school.Length <= 0 || policy.Length <= 0 || coverageProvider.Length <= 0)
                isBlank = true;

            return isBlank;
        }

    }
}

