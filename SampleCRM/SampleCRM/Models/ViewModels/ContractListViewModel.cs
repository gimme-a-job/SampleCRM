using SampleCRM.Models.TableModels;
using System.ComponentModel.DataAnnotations;

namespace SampleCRM.Models.ViewModels
{
    public class ContractListViewModel
    {
        // TODO: 使わない場合は消す
        [Display( Name = "会社コード" )]
        public int? CompanyCode { get; set; }

        [Display( Name = "発注コード" )]
        public int? OrderCode { get; set; }

        public IEnumerable<ContractListItemViewModel> Contracts { get; set; }

        public ContractListViewModel( IEnumerable<ContractListItemViewModel> contracts )
        {
            Contracts = contracts;
        }
    }

    public class ContractListItemViewModel
    {
        private const string DateFormat = "yyyy/MM/dd";

        [Display( Name = "契約コード" )]
        public int ContractCode { get; set; }

        [Display( Name = "契約状態" )]
        public string ContractStatusName { get; set; }

        [Display( Name = "更新方法" )]
        public string UpdateMethodName { get; set; }

        [Display( Name = "システム種別" )]
        public string SystemKindName { get; set; }

        [Display( Name = "システム種別略称" )]
        public string SystemKindShortName { get; set; }

        //[Display( Name = "CD発送日" )]
        //public string CDShippingDate { get; set; }

        //[Display( Name = "CD発送内容" )]
        //public string CDShippingContents { get; set; }

        [Display( Name = "CD発送先部署" )]
        public string CDShippingDepartmentName { get; set; }

        [Display( Name = "発送備考" )]
        public string ShippingNote { get; set; }

        [Display( Name = "ライセンスキーまたはHASPID" )]
        public string LicenseKeyOrOldID { get; set; }

        [Display( Name = "有効開始日" )]
        public string StartDate { get; set; }

        [Display( Name = "有効終了日" )]
        public string EndDate { get; set; }

        [Display( Name = "ライセンス地域区分" )]
        public string LicenceDistricts { get; set; }

        // ここでは使わない想定
        [Display( Name = "備考" )]
        public string Note { get; set; }

        public ContractListItemViewModel(
            Contract contract,
            IEnumerable<SystemKind> systemKinds )
        {
            this.ContractCode = contract.ContractCode;
            this.SystemKindName = systemKinds.Single( x => x.SystemKindCode == contract.SystemKindCode ).SystemKindName;
            this.ShippingNote = contract.ShippingNote;
        }

        public ContractListItemViewModel(
            Contract contract,
            SystemKind systemKind )
        {
            this.ContractCode = contract.ContractCode;
            this.SystemKindName = systemKind.SystemKindName;
            this.ShippingNote = contract.ShippingNote;
        }

        /// <summary>
        /// コンストラクタ(要らないかもしれないs)
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="contractStatus"></param>
        /// <param name="updateMethod"></param>
        /// <param name="systemKind"></param>
        /// <param name="cdShippingDepartment"></param>
        public ContractListItemViewModel(
            Contract contract,
            ContractStatus contractStatus,
            UpdateMethod updateMethod,
            SystemKind systemKind,
            Department cdShippingDepartment )
        {
            this.ContractCode = contract.ContractCode;
            this.ContractStatusName = contractStatus.ContractStatusName;
            this.UpdateMethodName = updateMethod.UpdateMethodName;
            this.SystemKindName = systemKind.SystemKindName;
            this.CDShippingDepartmentName = cdShippingDepartment?.DepartmentName;
            this.ShippingNote = contract.ShippingNote;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="contractStatus"></param>
        /// <param name="updateMethod"></param>
        /// <param name="systemKind"></param>
        /// <param name="cdShippingDepartment"></param>
        /// <param name="oldLicense"></param>
        /// <param name="systemLicense"></param>
        /// <param name="dataLicense"></param>
        public ContractListItemViewModel(
            Contract contract,
            ContractStatus contractStatus,
            UpdateMethod updateMethod,
            SystemKind systemKind,
            Department cdShippingDepartment,
            SystemLicense systemLicense,
            DataLicense dataLicense )
            : this( contract, contractStatus, updateMethod, systemKind, cdShippingDepartment )
        {

            var licenseType = ( systemLicense is not null ) ? LicenseTypes.System
                            : ( dataLicense is not null ) ? LicenseTypes.Data
                            : LicenseTypes.Other;

            // TODO : 適当な実装をしているため、後でよく検討する
            this.SystemKindName = licenseType switch
            {
                LicenseTypes.System => "システム",
                LicenseTypes.Data => "データ",
                _ => systemKind.SystemKindName,
            };

            this.LicenseKeyOrOldID = licenseType switch
            {
                LicenseTypes.System => systemLicense!.SystemLicenseKey,
                LicenseTypes.Data => dataLicense!.DataLicenseKey,
                _ => null,
            };

            this.StartDate = licenseType switch
            {
                LicenseTypes.System => systemLicense.StartDate!.ToString( DateFormat ),
                LicenseTypes.Data => dataLicense.StartDate!.ToString( DateFormat ),
                _ => null,
            };

            this.EndDate = licenseType switch
            {
                LicenseTypes.System => systemLicense.EndDate?.ToString( DateFormat ),
                LicenseTypes.Data => dataLicense.EndDate?.ToString( DateFormat ),
                _ => null,
            };

            this.LicenceDistricts = licenseType switch
            {
                LicenseTypes.Data => $"({dataLicense.LicenseDistrictCode}を元に引っ張ります)",
                _ => null,
            };

            this.Note = licenseType switch
            {
                LicenseTypes.System => systemLicense.Note,
                LicenseTypes.Data => dataLicense.Note,
                _ => null,
            };
        }

        private enum LicenseTypes
        {
            System,
            Data,
            Other
        }
    }
}
