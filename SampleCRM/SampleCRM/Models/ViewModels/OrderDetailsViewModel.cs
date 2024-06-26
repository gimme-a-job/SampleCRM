using SampleCRM.Models.TableModels;
using System.ComponentModel.DataAnnotations;

namespace SampleCRM.Models.ViewModels
{
    public class OrderDetailsViewModel : OrderListItemViewModel
    {
        // T_Payments, M_PaymentMethods
        [Display( Name = "支払いコード" )]
        public int PaymentCode { get; set; }

        [Display( Name = "支払い方法" )]
        public string PaymentMethodName { get; set; }

        [Display( Name = "リース年数" )]
        public int? LeaseTerm { get; set; }

        [Display( Name = "分割回数" )]
        public int? NumberOfPayments { get; set; }

        [Display( Name = "リース・分割金額" )]
        public int? LeaseOrSplitPrice { get; set; }

        [Display( Name = "一括・頭金金額" )]
        public int? LumpSumPrice { get; set; }

        [Display( Name = "支払い備考" )]
        public string PaymentNote { get; set; }

        // T_Contracts
        public ContractListViewModel ContractList { get; set; }

        // T_MaintenanceFeeBills
        public IEnumerable<MaintenanceFeeBillListItemViewModel> MaintenanceFeeBillList { get; set; }

        public OrderDetailsViewModel( Order order, DeliveryStatus deliveryStatus, Employee clientEmployee, Employee salesEmployee, Payment payment, PaymentMethod paymentMethod )
        : base( order, deliveryStatus, clientEmployee, salesEmployee )
        {
            // かなり適当な実装なので、後でよく確認する
            if ( payment is { } )
            {
                this.PaymentCode = payment.PaymentCode;
                this.PaymentMethodName = paymentMethod?.PaymentMethodName;
                this.LeaseTerm = payment?.LeaseTerm;
                this.NumberOfPayments = payment?.NumberOfPayments;
                this.LeaseOrSplitPrice = payment?.LeaseOrSplitPrice;
                this.LumpSumPrice = payment?.LumpSumPrice;
                this.PaymentNote = payment?.PaymentNote;
            }
        }
    }
}
