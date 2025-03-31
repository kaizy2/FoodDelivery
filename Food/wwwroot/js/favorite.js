const FAVORITE_KEY_PREFIX = "favoriteFoods_";

// Lấy ID của người dùng đang đăng nhập từ localStorage
function getLoggedInUserId() {
    return localStorage.getItem("loggedInUserId");
}

// Lấy danh sách món yêu thích
function getFavoriteFoods() {
    let userId = getLoggedInUserId();
    return userId ? JSON.parse(localStorage.getItem(FAVORITE_KEY_PREFIX + userId) || "[]") : [];
}

// Lưu danh sách yêu thích
function saveFavoriteFoods(favorites) {
    let userId = getLoggedInUserId();
    if (userId) {
        localStorage.setItem(FAVORITE_KEY_PREFIX + userId, JSON.stringify(favorites));
    }
}

// Kiểm tra sản phẩm có trong danh sách yêu thích không
function isFavorite(productId) {
    return getFavoriteFoods().some(food => food.id === productId);
}

function displayFavoriteFoods() {
    let userId = getLoggedInUserId();
    if (!userId) {
        console.log("Chưa đăng nhập, không hiển thị danh sách yêu thích.");
        document.getElementById("favoriteList").innerHTML = "<p>Please log in to see your favorite foods.</p>";
        return;
    }

    let favoriteFoods = getFavoriteFoods();
    let favoriteList = document.getElementById("favoriteList");

    // Xóa nội dung cũ trước khi cập nhật
    favoriteList.style.minHeight = "fit-content"; 
    favoriteList.innerHTML = "";
    

    if (favoriteFoods.length === 0) {
        favoriteList.innerHTML = "<p>You have no favorite foods yet.</p>";
        return;
    }

    favoriteFoods.forEach(product => {
        let productHTML = `
            <div class="col-sm-6 col-lg-4">
                <div class="box">
                    <div>
                        <div class="img-box">
                            <img src="${product.imageUrl}" alt="${product.name}" />
                        </div>
                        <div class="detail-box">
                            <h5>${product.name}</h5>
                            <p>${product.description}</p>
                            <div class="options">
                                <h6>$${product.price}</h6>
                                <button class="btn btn-danger" onclick="removeFromFavorites(${product.id})">
                                    Remove ❌
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        `;
        favoriteList.innerHTML += productHTML;
    });
}

function redirectToLogin() {
    window.location.href = "/Users/Login";
}

// Thêm / Xóa món ăn khỏi danh sách yêu thích
function toggleFavorite(button) {
    let isLoggedIn = button.getAttribute("data-logged-in") === "true";

    if (!isLoggedIn) {
        redirectToLogin();
        return;
    }

    let productId = parseInt(button.getAttribute("data-id"));
    let name = button.getAttribute("data-name");
    let imageUrl = button.getAttribute("data-image");
    let description = button.getAttribute("data-description");
    let price = parseFloat(button.getAttribute("data-price"));

    let favorites = getFavoriteFoods();
    let index = favorites.findIndex(food => food.id === productId);

    if (index !== -1) {
        // Nếu đã có trong danh sách, thì xóa
        favorites.splice(index, 1);
    } else {
        // Nếu chưa có, thì thêm vào
        favorites.push({ id: productId, name, imageUrl, description, price });
    }

    saveFavoriteFoods(favorites);
    updateFavoriteButtons(); // Cập nhật màu nút
}

// Xóa món ăn khỏi danh sách yêu thích
function removeFromFavorites(productId) {
    let favorites = getFavoriteFoods();

    // Lọc ra những sản phẩm không phải là productId để xóa sản phẩm đó
    favorites = favorites.filter(food => food.id !== productId);

    saveFavoriteFoods(favorites);  // Lưu lại danh sách mới
    displayFavoriteFoods();        // Cập nhật lại giao diện danh sách yêu thích
    updateFavoriteButtons();       // Cập nhật màu sắc của nút yêu thích trong Menu
}

// Cập nhật màu sắc nút yêu thích trên toàn bộ trang
function updateFavoriteButtons() {
    document.querySelectorAll(".favorite-button").forEach(button => {
        let productId = parseInt(button.getAttribute("data-id"));

        if (isFavorite(productId)) {
            button.classList.remove("btn-outline-danger");
            button.classList.add("btn-danger");
        } else {
            button.classList.remove("btn-danger");
            button.classList.add("btn-outline-danger");
        }
    });
}

function clearFavoriteFoodsOnLogout() {
    let userId = getLoggedInUserId();
    if (userId) {
        localStorage.removeItem(FAVORITE_KEY_PREFIX + userId);
    }
    localStorage.removeItem("loggedInUserId"); // Xóa ID người dùng khỏi localStorage
}


// Load trạng thái nút khi trang tải xong
document.addEventListener("DOMContentLoaded", updateFavoriteButtons);
