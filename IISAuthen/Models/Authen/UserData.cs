using System;
using System.Collections.Generic;

namespace IISAuthen.Models.Authen
{
    public class UserData
    {
        public UserDataEmploymentInfo? employmentInfo { get; set; }
        public UserDataPermissionInfo? permissionInfo { get; set; }
        public UserDataPersonInfo? personInfo { get; set; }
        public CompanyInfo? currentComp { get; set; }
        public string? sub { get; set; }
        public string? email { get; set; }
    }
    public class UserDataPermissionInfo
    {
        public string? LoginType { get; set; }
        public IEnumerable<Right>? Rights { get; set; }
        public Role? Role { get; set; }
        public IEnumerable<string>? Scopes { get; set; }
    }
    public class UserDataEmploymentInfo
    {
        public string? CompIdReference { get; set; }
        public string? Department { get; set; }
        public string? Email { get; set; }
        public string? MoblieNo { get; set; }
        public string? TelephoneNo { get; set; }
    }
    public class UserDataPersonInfo
    {
        public string? FullNameTh { get; set; }
        public string? FullNameEn { get; set; }
        public string? TitleNameTh { get; set; }
        public string? TitleNameEn { get; set; }
        public string? FirstNameTh { get; set; }
        public string? FirstNameEn { get; set; }
        public string? SurNameTh { get; set; }
        public string? SurNameEn { get; set; }
        public string? Department { get; set; }
        public string? Email { get; set; }
        public string? CountryCode { get; set; }
        public int AccountId { get; set; }
        public string? IalLevel { get; set; }
        public string? MobileNo { get; set; }
        public string? Source { get; set; }
        public string? TelephoneNo { get; set; }
        public string? idNo { get; set; }
        public DateTime? LastAuthen { get; set; }
        public CompanyInfo? Company { get; set; }
        public CompanyInfo? CompanyOutsourceInfo { get; set; }
    }
    public struct LoginTypeStruct
    {
        public const string INDIVIDUAL = "INDIVIDUAL";
        public const string CORPORATE = "CORPORATE";
        public const string NONE = "NONE";
    }
    public struct SenderTypeStruct
    {
        public const string CORPORATE = "C";
        public const string INDIVIDUAL = "P";
        public const string INDIVIDUAL_CompID = "000000000000000";
    }
}