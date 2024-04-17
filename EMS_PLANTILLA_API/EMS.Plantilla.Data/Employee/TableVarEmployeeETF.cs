using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EMS.Plantilla.Data.Employee
{
    // Table and Column names are all lower case to be mapped with MySQL
    // objects which are case sensitive on Linux and case insensitive on Windows.
    [Table("tv_employee_etf")]
    public class TableVarEmployeeETF
    {

        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("total_num")]
        public int TotalNum { get; set; }

        [Column("row_num")]
        public int RowNum { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("old_employee_id")]
        public string OldEmployeeID { get; set; }

        [Column("lastname")]
        public string LastName { get; set; }

        [Column("firstname")]
        public string FirstName { get; set; }

        [Column("middlename")]
        public string MiddleName { get; set; }

        [Column("suffix")]
        public string Suffix { get; set; }

        [Column("nickname")]
        public string Nickname { get; set; }

        [Column("nationality")]
        public string Nationality { get; set; }

        [Column("citizenship")]
        public string Citizenship { get; set; }

        [Column("birthdate")]
        public string BirthDate { get; set; }

        [Column("birthplace")]
        public string BirthPlace { get; set; }

        [Column("gender")]
        public string Gender { get; set; }

        [Column("height")]
        public string Height { get; set; }

        [Column("weight")]
        public string Weight { get; set; }

        [Column("civil_status")]
        public string CivilStatus { get; set; }

        [Column("religion")]
        public string Religion { get; set; }

        [Column("sss_status")]
        public string SSSStatus { get; set; }

        [Column("exemption_status")]
        public string ExemptionStatus { get; set; }

        [Column("home_address")]
        public string HomeAddress { get; set; }

        [Column("cellphone_number")]
        public string CellphoneNumber { get; set; }

        [Column("home_city")]
        public string HomeCity { get; set; }

        [Column("home_region")]
        public string HomeRegion { get; set; }

        [Column("pagibig_number")]
        public string PagibigNumber { get; set; }

        [Column("sss_number")]
        public string SSSNumber { get; set; }

        [Column("tin")]
        public string TIN { get; set; }

        [Column("philhealth_number")]
        public string PhilhealthNumber { get; set; }

        [Column("contact_person_name")]
        public string ContactPersonName { get; set; }

        [Column("contact_person_relationship")]
        public string ContactPersonRelationship { get; set; }

        [Column("spouse_name")]
        public string SpouseName { get; set; }

        [Column("spouse_birthdate")]
        public string SpouseBirthdate { get; set; }

        [Column("spouse_employer")]
        public string SpouseEmployer { get; set; }

        [Column("spouse_occupation")]
        public string SpouseOccupation { get; set; }

        [Column("father_name")]
        public string FatherName { get; set; }

        [Column("father_birthdate")]
        public string FatherBirthdate { get; set; }

        [Column("father_occupation")]
        public string FatherOccupation { get; set; }

        [Column("mother_name")]
        public string MotherName { get; set; }

        [Column("mother_birthdate")]
        public string MotherBirthdate { get; set; }

        [Column("mother_occupation")]
        public string MotherOccupation { get; set; }

        [Column("school")]
        public string School { get; set; }

        [Column("school_address")]
        public string SchoolAddress { get; set; }

        [Column("school_level")]
        public string SchoolLevel { get; set; }

        [Column("course")]
        public string Course { get; set; }

        [Column("year_from")]
        public string YearFrom { get; set; }

        [Column("year_to")]
        public string YearTo { get; set; }

        [Column("attainment_degree")]
        public string AttainmentDegree { get; set; }

        [Column("attainment_status")]
        public string AttainmentStatus { get; set; }

        [Column("company_code")]
        public string CompanyCode { get; set; }

        [Column("home_branch")]
        public string HomeBranch { get; set; }

        [Column("branch_code")]
        public string BranchCode { get; set; }

        [Column("branch_name")]
        public string BranchName { get; set; }

        [Column("cluster")]
        public string Cluster { get; set; }

        [Column("area")]
        public string Area { get; set; }

        [Column("region")]
        public string Region { get; set; }

        [Column("zone")]
        public string Zone { get; set; }

        [Column("monthly_salary")]
        public decimal MonthlySalary { get; set; }

        [Column("daily_salary")]
        public decimal DailySalary { get; set; }

        [Column("hourly_salary")]
        public decimal HourlySalary { get; set; }

        [Column("dept_code")]
        public string DeptCode { get; set; }

        [Column("dept_name")]
        public string DeptName { get; set; }

        [Column("position_code")]
        public string PositionCode { get; set; }

        [Column("position_title")]
        public string PositionTitle { get; set; }

        [Column("position_level")]
        public string PositionLevel { get; set; }

        [Column("job_class")]
        public string JobClass { get; set; }

        [Column("date_hired")]
        public string DateHired { get; set; }

        [Column("previous_employer")]
        public string PreviousEmployer { get; set; }

        [Column("previous_designation")]
        public string PreviousDesignation { get; set; }

        [Column("year_started")]
        public string YearStarted { get; set; }

        [Column("year_ended")]
        public string YearEnded { get; set; }

        [Column("employment_status")]
        public string EmploymentStatus { get; set; }
    }
}
