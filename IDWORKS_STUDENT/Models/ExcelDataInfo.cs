using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDWORKS_STUDENT.Models
{
    public enum ColumnType
    {
        Coverage,
        FirstName, 
        LastName, 
        School,
        Policy,
        EffectiveDate, 
        CoverageProvider,
        None
    }
    public class ExcelDataInfo
    {
       
        public class Section
        {
            private int _coverageIndex = -1;
            private int _firstNameIndex = -1;
            private int _lastNameIndex = -1;
            private int _schoolIndex = -1;
            private int _policyIndex = -1;
            private int _effectiveDateIndex = -1;
            private int _coverageProviderIndex = -1;

            public Section(int coverageIndex, int firstNameIndex, int lastNameIndex, int schoolIndex, int policyIndex, int effectiveDateIndex, int coverageProviderIndex)
            {
                this._coverageIndex = coverageIndex;
                this._firstNameIndex = firstNameIndex;
                this._lastNameIndex = lastNameIndex;
                this._schoolIndex = schoolIndex;
                this._policyIndex = policyIndex;
                this._effectiveDateIndex = effectiveDateIndex;
                this._coverageProviderIndex = coverageProviderIndex;
            }

            public List<DataInfoCell> dataInfoCells = new List<DataInfoCell>();
            public List<ErrorInfoCell> errorInfoCells = new List<ErrorInfoCell>();
            public bool ColumnsInCorrectlyFormatted = false;

            public int CoverageIndex { get { return _coverageIndex; } }
            public int FirstNameIndex {  get { return _firstNameIndex; } }
            public int LastNameIndex { get { return _lastNameIndex; } }
            public int SchoolIndex {  get { return _schoolIndex; } }
            public int PolicyIndex {  get { return _policyIndex; } }
            public int EffectiveDateIndex { get { return _effectiveDateIndex; } }
            public int CoverageProviderIndex {  get { return _coverageProviderIndex; } }
        }

        public class DataInfoCell
        {
            private string _coverage = "";
            private string _firstName = "";
            private string _lastName = "";
            private string _school = "";
            private string _policyNumber = "";
            private DateTime _effectiveDate;
            private string _coverageProvider = "";

            public DataInfoCell(string coverage, string firstName, string lastName, string school, string policyNumber, DateTime effectiveDate, string coverageProvider)
            {
                this._coverageProvider = coverageProvider;
                this._firstName = firstName;
                this._lastName = lastName;
                this._school = school;
                this._policyNumber = policyNumber;
                this._effectiveDate = effectiveDate;
                this._coverage = coverage;
            }

            public string Coverage { get { return _coverage;}}
            public string FirstName { get { return _firstName;}}
            public string LastName { get { return _lastName;}}
            public string School { get { return _school;}}
            public string PolicyNumber{get { return _policyNumber;}} 
            public DateTime EffectiveDate { get { return _effectiveDate;}}
            public string CoverageProvider {  get { return _coverageProvider;}}
        }



        public class ErrorInfoCell
        {
            private string _coverage = "";
            private string _firstName = "";
            private string _lastName = "";
            private string _school = "";
            private string _policyNumber = "";
            private DateTime _effectiveDate;
            private string _coverageProvider = "";

            public ErrorInfoCell(string coverage, string firstName, string lastName, string school, string policyNumber, DateTime effectiveDate, string coverageProvider)
            {
                this._coverageProvider = coverageProvider;
                this._firstName = firstName;
                this._lastName = lastName;
                this._school = school;
                this._policyNumber = policyNumber;
                this._effectiveDate = effectiveDate;
                this._coverage = coverage;
            }

            public string Coverage { get { return _coverage; } }
            public string FirstName { get { return _firstName; } }
            public string LastName { get { return _lastName; } }
            public string School { get { return _school; } }
            public string PolicyNumber { get { return _policyNumber; } }
            public DateTime EffectiveDate { get { return _effectiveDate; } }
            public string CoverageProvider { get { return _coverageProvider; } }
        }
    }
}
