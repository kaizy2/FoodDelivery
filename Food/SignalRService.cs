using Microsoft.AspNetCore.SignalR;

namespace Food
{
    public class SignalRService : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendNewCategory(object newCategory)
        {
            // Gửi dữ liệu danh mục mới tới tất cả các client qua sự kiện ReceiveNewCategory
            await Clients.All.SendAsync("ReceiveNewCategory", newCategory);
        }

        public async Task SendUpdateCategory(object newCategory)
        {
            // Gửi dữ liệu danh mục mới tới tất cả các client qua sự kiện ReceiveNewCategory
            await Clients.All.SendAsync("ReceiveUpdatedCategory", newCategory);
        }
    }
}
