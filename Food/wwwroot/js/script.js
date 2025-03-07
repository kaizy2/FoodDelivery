// Lấy tất cả các mục menu bên trong sidebar
const allSideMenu = document.querySelectorAll('#sidebar .side-menu.top li a');

// Tự động thêm lớp 'active' dựa trên URL hiện tại khi tải trang
allSideMenu.forEach(item => {
    const li = item.parentElement;
    const pageUrl = window.location.pathname;

    // Kiểm tra URL hiện tại với href của từng item và thêm lớp 'active' nếu khớp
    if (item.getAttribute('href') === pageUrl) {
        li.classList.add('active');
    }

    // Thêm sự kiện click để đổi lớp 'active' khi nhấp vào các mục menu
    item.addEventListener('click', function () {
        allSideMenu.forEach(i => {
            i.parentElement.classList.remove('active');
        });
        li.classList.add('active');
    });
});

// TOGGLE SIDEBAR
const menuBar = document.querySelector('#content nav .bx.bx-menu');
const sidebar = document.getElementById('sidebar');

menuBar.addEventListener('click', function () {
    sidebar.classList.toggle('hide');
});

// XỬ LÝ NÚT TÌM KIẾM TRÊN THANH ĐIỀU HƯỚNG
const searchButton = document.querySelector('#content nav form .form-input button');
const searchButtonIcon = document.querySelector('#content nav form .form-input button .bx');
const searchForm = document.querySelector('#content nav form');

searchButton.addEventListener('click', function (e) {
    if (window.innerWidth < 576) {
        e.preventDefault();
        searchForm.classList.toggle('show');
        if (searchForm.classList.contains('show')) {
            searchButtonIcon.classList.replace('bx-search', 'bx-x');
        } else {
            searchButtonIcon.classList.replace('bx-x', 'bx-search');
        }
    }
});

// Ẩn sidebar khi màn hình nhỏ hơn 768px và điều chỉnh form tìm kiếm
if (window.innerWidth < 768) {
    sidebar.classList.add('hide');
} else if (window.innerWidth > 576) {
    searchButtonIcon.classList.replace('bx-x', 'bx-search');
    searchForm.classList.remove('show');
}

// Điều chỉnh form tìm kiếm khi thay đổi kích thước màn hình
window.addEventListener('resize', function () {
    if (this.innerWidth > 576) {
        searchButtonIcon.classList.replace('bx-x', 'bx-search');
        searchForm.classList.remove('show');
    }
});

// CHUYỂN ĐỔI CHẾ ĐỘ SÁNG/TỐI
const switchMode = document.getElementById('switch-mode');

switchMode.addEventListener('change', function () {
    if (this.checked) {
        document.body.classList.add('dark');
    } else {
        document.body.classList.remove('dark');
    }
});
