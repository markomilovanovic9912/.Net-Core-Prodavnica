var NoI = parseInt($('#pref-perpage option:selected').val());




function AddOrder(id) {

    $.ajax({
        type: "POST",
        url: "/Order/AddOrderFromIndex",
        data: JSON.stringify(id),
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    });
};

function AddNoI(value) {
    NoI += value;
};


function GetDetail(id) {
    window.location.href = '/Product/Detail/' + id;
};

var property1 = 0;
var property2 = 0;
var property3 = 0;



function RefreshItems() {


    property1 = $('#pref-orderby').val();

    var brands = [];
    var subCategories = [];
    var range = [];

    range = slider.noUiSlider.get();

    $('#brandbox input:checked').each(function () {
        brands.push($(this).attr('value'));
    });

    if (brands.length < 1) {
        brands = null;
    };

    $('#subCatBox input:checked').each(function () {
        subCategories.push($(this).attr('value'));
    });

    if (subCategories.length < 1) {
        subCategories = null;
    };

    var mfff =
        {

            ProductId: ids,
            Sort: property1,
            Name: "@Model.Name",
            ManufacturerList: brands,
            SubCategories: subCategories,
            MinPrice: parseFloat(range[0]),
            MaxPrice: parseFloat(range[1]),
            NumberOfItems: NoI.toString()

        };

    $.ajax({
        url: "/Product/RefreshItems",
        type: "POST",
        dataType: "json",
        contentType: "application/json",
        data: JSON.stringify(mfff),
        success: function (data) {

            var newHtml = "";

            $.each(data.productIndex.slice(0, NoI), function (index, item) {
                newHtml += '<div class="col-lg-4 col-md-4 col-sm-10 col-xs-12" style="display:inline-block; float:none; ">';
                newHtml += '<div class="card">';
                newHtml += '<div class="img-card" href="">';
                newHtml += '<a onClick="GetDetail(' + item.itemId + ')">' + '<img src="' + item.firstImage + '"/> </a>' + '</div>';

                newHtml += '<div class="card-content">';
                newHtml += '<h4 class="card-title" style="font-size: 1em !important;">';
                newHtml += '<a onClick="GetDetail(' + item.itemId + ')">' + '<p>' + item.manufacturerName + '<br>' + item.modelName + '</p>' + '</a >';
                newHtml += '</h4>';

                newHtml += '<div class="card-read-more">';
                newHtml += 'PRICE: &nbsp;' + item.price + ' €';
                newHtml += '</div>' + '<br>';

                newHtml += '<button class="btn-primary" onClick="AddOrder(' + item.itemId + ')">Add to Cart</button>';

                newHtml += '</div>';
                newHtml += '</div>';
                newHtml += '<br>' + '</div>';

            });
            ids.length = 0;

            $.each(data.productIndex, function (index, item) {

                ids.push(item.itemId);

            });
            /*$("#dddd").html(ids);*/
            $("#cont").html(newHtml);


        }
    });
}