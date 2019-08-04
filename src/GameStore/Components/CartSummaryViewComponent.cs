using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GameStore.Models.Interface;

namespace GameStore.Components
{
    [ViewComponent(Name = "CartSummary")]
    public class CartSummaryViewComponent : ViewComponent
    {
        private readonly IShopping_Cart_Repository _shop_repo = null;
        private readonly IPaymentSettingRepository _pay_repo = null;

        public CartSummaryViewComponent(IShopping_Cart_Repository shop_repo,
            IPaymentSettingRepository pay_repo)
        {
            _shop_repo = shop_repo;
            _pay_repo = pay_repo;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var payment = await _pay_repo.Get();
            var result = await _shop_repo.Shopping_Cart_Tracking_Get_By_Id(id);
            if (result != null) { result.Credit_Ratio = payment.Credit_Ratio; }
            return View(result);
        }
    }
}
