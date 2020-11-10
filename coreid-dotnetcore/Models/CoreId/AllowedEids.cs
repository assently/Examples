namespace Assently.Samples.DotnetCore.Models.CoreId
{
    public class AllowedEids : Provider
    {
        public static string All = "*"; // All
    }

    public class Aud : Provider
    {
        public static string All = "all"; // All
        public static string Se = "se"; // All Swedish eID-s
        public static string Fi = "fi"; // All Finnish eID-s
        public static string No = "no"; // All Norwegian eID-s
        public static string Dk = "dk"; // All Danish eID-s
    }

    public class Provider
    {
        public static string FiMv = "fi-mv"; // Finnish Mobiilivarmenne                                            
        public static string FiTupas = "fi-tupas"; // Finnish Tupas                                                
        public static string FiVrk = "fi-vrk"; // Finnish VRK                                                      
        public static string NoBankId = "no-bankid"; // Norwegian BankID                                           
        public static string NoBankIdMobile = "no-bankid-mobile"; // Norwegian BankID mobile                       
        public static string SeBankId = "se-bankid"; // Swedish BankID                                             
        public static string SeTelia = "se-telia-osif"; // Swedish Telia                                           
        public static string DkNemId = "dk-nemid"; // Danish NemID                                                      
    }
}