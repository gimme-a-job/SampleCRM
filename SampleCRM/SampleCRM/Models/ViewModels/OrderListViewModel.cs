using SampleCRM.Models.TableModels;
using System.ComponentModel.DataAnnotations;

namespace SampleCRM.Models.ViewModels
{
    public class OrderListViewModel
    {
        public int CompanyCode { get; }

        public IEnumerable<OrderListItemViewModel> Orders { get; }
        public OrderListViewModel( int companyCode, IEnumerable<OrderListItemViewModel> orders )
        {
            CompanyCode = companyCode;
            Orders = orders;
        }
    }

    public class OrderListItemViewModel
    {
        private const string DateFormat = "yyyy/MM/dd";

        [Display( Name = "発注コード" )]
        public int OrderCode { get; }

        [Display( Name = "発注日" )]
        public string OrderDate { get; }

        [Display( Name = "納品予定日" )]
        public string DeliveryDate { get; }

        [Display( Name = "納品状態" )]
        public string DeliveryStatusName { get; }

        [Display( Name = "新規発注の会社かどうか" )]
        public string IsNewOrderCompany { get; }

        [Display( Name = "発注書でまとめて支払うかどうか" )]
        public string IsPayTogether { get; }

        [Display( Name = "顧客担当者名" )]
        public string ClientEmployeeName { get; }

        [Display( Name = "営業担当者名" )]
        public string SalesEmployeeName { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleName"></param>
        public OrderListItemViewModel( Order order, DeliveryStatus deliveryStatus, Employee clientEmployee, Employee salesEmployee )
        {
            this.OrderCode = order.OrderCode;
            this.OrderDate = order.OrderDate!.Value.ToString( DateFormat );
            this.DeliveryDate = order.DeliveryDate?.ToString( DateFormat );
            this.DeliveryStatusName = deliveryStatus.DeliveryStatusName;
            this.IsNewOrderCompany = order.IsNewOrderCompany ? "新規ユーザー" : "既存ユーザー";
            this.IsPayTogether = order.IsPayTogether ? "まとめて支払う" : "まとめない";

            this.ClientEmployeeName = clientEmployee.EmployeeName;

            this.SalesEmployeeName = salesEmployee.EmployeeName;
        }
    }
}
