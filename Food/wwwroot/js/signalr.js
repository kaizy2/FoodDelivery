const connection = new signalR.HubConnectionBuilder()
    .withUrl("/SignalRHub") // Đường dẫn phải khớp với route trong Program.cs
    .build();

// Kết nối với SignalR
connection.start().catch(err => console.error(err.toString()));

// Lắng nghe sự kiện ReceiveMessage từ server
connection.on("ReceiveMessage", (user, message) => {
    const msg = document.createElement("div");
    msg.textContent = `${user}: ${message}`;
    document.getElementById("messagesList").appendChild(msg);
});
