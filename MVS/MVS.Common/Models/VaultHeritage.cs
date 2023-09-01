using System;
using System.Collections.Generic;

namespace MVS.Common.Models
{
    public partial class VaultHeritage
    {
        public string Id { get; set; }
        public string VaultId { get; set; }
        public bool? HeritageProtect { get; set; }
        public bool? MainResidence { get; set; }
        public string MainResidenceWording { get; set; }
        public string MainResidenceAccuracy { get; set; }
        public bool? SecondResidence { get; set; }
        public string SecondResidenceWording { get; set; }
        public string SecondResidenceAccuracy { get; set; }
        public bool? OtherRealEstate { get; set; }
        public string OtherRealEstateWording { get; set; }
        public string OtherRealEstateAccuracy { get; set; }
        public bool? BankAccount { get; set; }
        public string BankAccountWording { get; set; }
        public string BankAccountAccuracy { get; set; }
        public bool? LifeInsurance { get; set; }
        public string LifeInsuranceWording { get; set; }
        public string LifeInsuranceAccuracy { get; set; }
        public bool? UnitsSharesCompanies { get; set; }
        public string UnitsSharesCompaniesWording { get; set; }
        public string UnitsSharesCompaniesAccuracy { get; set; }
        public bool? Copyright { get; set; }
        public string CopyrightWording { get; set; }
        public string CopyrightAccuracy { get; set; }
        public bool? IndustrialProperty { get; set; }
        public string IndustrialPropertyWording { get; set; }
        public string IndustrialPropertyAccuracy { get; set; }
        public bool? MotorVehicle { get; set; }
        public string MotorVehicleWording { get; set; }
        public string MotorVehicleAccuracy { get; set; }
        public bool? ValuablePersonalProperty { get; set; }
        public string ValuablePersonalPropertyWording { get; set; }
        public string ValuablePersonalPropertyAccuracy { get; set; }
        public bool? PortfolioOfShares { get; set; }
        public string PortfolioOfSharesWording { get; set; }
        public string PortfolioOfSharesAccuracy { get; set; }
        public bool? Borrowing { get; set; }
        public string BorrowingWording { get; set; }
        public string BorrowingAccuracy { get; set; }
        public bool? ShareWithMyFutureAgents { get; set; }
        public bool CompletedForm { get; set; }

        public virtual Vault Vault { get; set; }
    }
}
