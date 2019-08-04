using GameStore.Models.Entity;
using System.Threading.Tasks;

namespace GameStore.Models.Interface
{
    public interface IPaymentSettingRepository
    {
        Task<PaymentSetting> Get();
        Task<int> Update(PaymentSetting obj);
    }
}
