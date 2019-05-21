$(document).ready(function () {
    $("#CartItemQuantity").val(1);
    $("#qty-reduce").click(function () {
        if ($("#CartItemQuantity").val() > 1) {
            var quantity = $("#CartItemQuantity").val();
            $("#CartItemQuantity").val(quantity - 1);
        }
    });

    $("#qty-add").click(function () {
        var quantity = $("#CartItemQuantity").val();
        $("#CartItemQuantity").val(+quantity + +1);
    });
});
function AddCartItem(ProductPricingId, ProductName, ProductId, BuyPrice, SellPrice, profit) {
    $("#CartItemQuantity").val(1);
    $("#Cart_ProductName").html(ProductName + " - " + ProductId);
    $("#Cart_BuyPrice").html("Buy Price: Rs." + BuyPrice);
    $("#Cart_SellPrice").html("Sell Price: Rs." + SellPrice);
    $("#Cart_Profit").html("Profit: Rs." + profit);
    $("#Cart_ProductPricingId").val(ProductPricingId);
    $("#AddToCartModal").modal();
}

function UpdateCartItem() {
    var CartItemQuantity = $("#CartItemQuantity").val();
    var ProductPricingId = $("#Cart_ProductPricingId").val();
    $("#AddToCartModal").modal('toggle');
    $.ajax({
        url: '../Sales/UpdateCartItem',
        type: 'POST',
        data: { ProductPricingId: ProductPricingId, CartItemQuantity: CartItemQuantity },
        success: function (data) {
            $("#ChampionsPricingGrid").html(data);
        },
        error: function (e) {
            alert("Something wrong. Please check the internet connection!!!");
        }
    });
}