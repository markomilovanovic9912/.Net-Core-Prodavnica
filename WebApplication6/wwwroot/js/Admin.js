
//Functions
function GetModels(selected, target) {
    var sendJsonData = {
        ItemTypeName: $(selected).text()
    };

    $.ajax({
        url: "/Admin/GetModels",
        type: "POST",
        dataType: "JSON",
        contentType: "application/json",
        data: JSON.stringify(sendJsonData),
        success: function (data) {
            var Models = "";
            $.each(data.models, function (index, item) {

                Models += '<option value="' + item.name + '">' + item.name + '</option>'
            });

            $(target).html(Models);
        }
    });
};

function GetSpecs(modelName) {
    var sendJsonData = {
        ModelName: modelName
    };
    $('#updateForm')[0].reset();
    $.ajax({
        url: "/Admin/GetSpecs",
        type: "POST",
        dataType: "Json",
        contentType: "application/json",
        data: JSON.stringify(sendJsonData),
        success: function (data) {

            $('#SpecsUpdt').html(data.specs.specification);
            $('#DescriptUpdt').html(data.specs.description);
            $('#ColorUpdt').html(data.color);
            $('#AvailabilityUpdt').html(data.availibility);
            $('#PriceUpdt').html(data.price);
            $('#DiscountUpdt').html(data.discount);
            $('#IdUpdt').html(data.id);
            $('#SpecIdUpdt').html(data.specs.id);
        }
    });
};

function GetReviews(selected, target) {
    var sendJsonData = {
        ModelName: $(selected).text()
    };
    $.ajax({
        url: "/Admin/GetReviews",
        type: "POST",
        dataType: "json",
        contentType: "application/json",
        data: JSON.stringify(sendJsonData),
        success: function (data) {

            $(target).empty();
            $.each(data, function (index, reviews) {
                $(target).append("Review ID: " + reviews.id + "\n" + "Review Text: " + reviews.text + " " + "\n\n");
            });

        }
    });
}




function GetSubCategories(selected, target) {
    var sendJsonData = {
        ItemTypeName: $(selected).text()
    };
    $.ajax({
        url: "/Admin/GetSubTypes",
        type: "POST",
        dataType: "Json",
        contentType: "application/json",
        data: JSON.stringify(sendJsonData),
        success: function (data) {
            var subCategories = "";
            $.each(data.itemSubTypes, function (index, item) {

                subCategories = subCategories + '<option value="' + item.id + '">' + item.subTypeName + '</option>'
            })

            $(target).html(subCategories);
        }
    });
}


function RemoveReview() {

    reviewId = $('#revId').val();
 

    var revModel =
        {
            ReviewId: reviewId

        };

    $.ajax({
        url: "/Admin/RemoveReview",
        type: "POST",
        dataType: "JSON",
        contentType: "application/json",
        data: JSON.stringify(revModel),
        success: function (data) {

            alert(data);

        }
    });

    }

function AddManufacturer() {

    manuName = $('#ManuAdd').val();

    var manuf =
        {
            Manufacturer: { Name: manuName }
           

        };

    $.ajax({
        url: "/Admin/AddManufacturer",
        type: "POST",
        dataType: "Json",
        contentType: "application/json",
        data: JSON.stringify(manuf),
        success: function (data) {

            alert(data);

        }
    });

}

function RemoveManuf(manufac) {

    manufacturer = $(manufac).val();


    var RemManuf =
        {
            ManufacturerId: manufacturer,
        };

    $.ajax({
        url: "/Admin/RemoveManufacturer",
        type: "POST",
        dataType: "JSON",
        contentType: "application/json",
        data: JSON.stringify(RemManuf),
        success: function (data) {

            alert(data);

        }
    });

}

function AddItemType() {

    itmTypName = $('#ItemTypeAdd').val();

    var itemTypeAdd =
        {
            ItemType: { Name: itmTypName }


        };

    $.ajax({
        url: "/Admin/AddItemType",
        type: "POST",
        dataType: "Json",
        contentType: "application/json",
        data: JSON.stringify(itemTypeAdd),
        success: function (data) {

            alert(data);

        }
    });

}

function RemoveItemType(itTyp) {

    itemType = $(itTyp).val();


    var RemItTyp =
        {
            ItemTypeId: itemType,
        };

    $.ajax({
        url: "/Admin/RemoveItemType",
        type: "POST",
        dataType: "JSON",
        contentType: "application/json",
        data: JSON.stringify(RemItTyp),
        success: function (data) {

            alert(data);

        }
    });

}

function AddItemSubType() {

    itmSubTypName = $('#ItemSubTypeAdd').val();
    itmSubTypTyp = $('#AddSubItemType option:selected').val();

    var itemSubTypeAdd =
        {
            ItemSubType: { ItemType: { Id: itmSubTypTyp }, SubTypeName: itmSubTypName }

        };

    $.ajax({
        url: "/Admin/AddItemSubType",
        type: "POST",
        dataType: "text",
        contentType: "application/json",
        data: JSON.stringify(itemSubTypeAdd),
        success: function (data) {

            alert(data);

        }
    });

}


function RemoveItemSubType(itSubTyp) {

    itemSubType = $(itSubTyp).val();


    var RemItSubTyp =
        {
            ItemSubTypeId: itemSubType,
        };

    $.ajax({
        url: "/Admin/RemoveItemSubType",
        type: "POST",
        dataType: "JSON",
        contentType: "application/json",
        data: JSON.stringify(RemItSubTyp),
        success: function (data) {

            alert(data);

        }
    });

}

function AddItem() {


    specsAdd = $('#AddSpecSpec').val();
    descriptAdd = $('#AddSpecDescript').val();
    colorAdd = $('#AddModelColor').val();
    availabilityAdd = $('#AddModelAvailibility').val();
    priceAdd = $('#AddModelPrice').val();
    discountAdd = $('#AddModelDiscount').val();
    modelNameAdd = $('#AddModelName').val();
    modelManufacturAdd = document.getElementById("AddModelManufacturer").value;
    itemTypeAdd = document.getElementById("AddModelItemType").value;
    itemSubTypeAdd = document.getElementById("AddModelItemTypeSub").value;
    itemDeptAdd = document.getElementById("AddModelItemDepartment").value;
    imgUrlAdd = $('#AddModelAddImage').val();

    var AddItemModel =
        {


            Price: priceAdd,
            Discount: discountAdd,
            Availibility: availabilityAdd,
            Color: colorAdd,
            SpecSpec: specsAdd,
            SpecDesc: descriptAdd,
            ModelName: modelNameAdd,
            ImageUrls: imgUrlAdd,
            Manufacturer: { Id: modelManufacturAdd },
            Specs: { Specification: specsAdd, Description: descriptAdd },
            ItemDepartment: { Id: itemDeptAdd },
            ItemType: { Id: itemTypeAdd },
            ItemSubType: { Id: itemSubTypeAdd }
        };

    $.ajax({
        url: "/Admin/AddItem",
        type: "POST",
        dataType: "text",
        contentType: "application/json",
        data: JSON.stringify(AddItemModel),
        success: function (data) {

            alert(data);

        }
    });

}

function UpdateItems() {

    updateId = $('#IdUpdt').val();
    specsUpdateId = $('#SpecIdUpdt').val();
    specsUpdate = $('#SpecsUpdt').val();
    descriptUpdate = $('#DescriptUpdt').val();
    colorUpdate = $('#ColorUpdt').val();
    availabilityUpdate = $('#AvailabilityUpdt').val();
    priceUpdate = $('#PriceUpdt').val();
    discountUpdate = $('#DiscountUpdt').val();
    modelName = document.getElementById("seleModel").value;


    var model =
        {
            Price: priceUpdate,
            Discount: discountUpdate,
            Availibility: availabilityUpdate,
            Color: colorUpdate,
            SpecSpec: specsUpdate,
            SpecDesc: descriptUpdate,
            Id: updateId,
            SpecId: specsUpdateId,
            ModelName: modelName

        };

    $.ajax({
        url: "/Admin/UpdateItems",
        type: "POST",
        dataType: "JSON",
        contentType: "application/json",
        data: JSON.stringify(model),
        success: function (data) {

            alert(data);

        }
    });

}

function RemoveItem(mN) {

    modelName= $(mN).text();


    var itmRemModel =
        {
            ModelName: modelName
        };

    $.ajax({
        url: "/Admin/RemoveItem",
        type: "POST",
        dataType: "JSON",
        contentType: "application/json",
        data: JSON.stringify(itmRemModel),
        success: function (data) {

            alert(data);

        }
    });

}       
function AddUserClaim() {

    userClaimType = document.getElementById("AddClaimUsrClaimType").value;
    userClaimValue = document.getElementById("AddClaimsUsrClaimValue").value;
    userClaimUserId = document.getElementById("AddClaimUsrId").value;


    var AddUserClaimModel = {

        UserClaim: {
            UserId: userClaimUserId,
            ClaimValue: userClaimValue,
            ClaimType: userClaimType
        }

    }

    $.ajax({
        url: "/Admin/AddUserClaim",
        type: "POST",
        dataType: "text",
        contentType: "application/json",
        data: JSON.stringify(AddUserClaimModel),
        success: function (data) {

            alert(data);

        }
    });
}

function RemoveUserClaim() {


    remClaimId = document.getElementById("RemoveClaimId").value;


    var RemUserClaimModel = {

        UserClaim: { Id: remClaimId }

    }

    $.ajax({
        url: "/Admin/RemoveUserClaim",
        type: "POST",
        dataType: "text",
        contentType: "application/json",
        data: JSON.stringify(RemUserClaimModel),
        success: function (data) {

            alert(data);

        }
    });
}

function RemoveUser() {

    userNameToRem = $('#usrNameToRemove').val();

    var RemUserModel = {
        UserName: userNameToRem
    }

    $.ajax({
        url: "/Admin/UpdateRemoveUser",
        type: "POST",
        dataType: "json",
        contentType: "application/json",
        data: JSON.stringify(RemUserModel),
        success: function (data) {

            alert(data);

        }
    });
}

function SelUser() {
    var sendJsonData = {
        UserName: document.getElementById("seleUser").value
    };
    $.ajax({
        url: "/Admin/GetUser",
        type: "POST",
        dataType: "json",
        contentType: "application/json",
        data: JSON.stringify(sendJsonData),
        success: function (data) {

            $(".myCheck").prop("checked", false);
            $(".myCheck1").prop("checked", false);


            $('#usrId').html(data.id);
            $('#usrFirstName').html(data.firstName);
            $('#usrMiddleInitiale').html(data.middleInitial);
            $('#usrLastName').html(data.lastName);
            $('#usrEmail').html(data.email);
            $('#usrPhoneNumber').html(data.phoneNumber);
            $('#usrMiddleInitial').html(data.middleInitial);
            if (data.emailConfirmed === true) {
                $(".myCheck").prop("checked", true);
            };
            if (data.phoneNumberConfirmed === true) {
                $(".myCheck1").prop("checked", true);
            };


        }
    });
    $.ajax({
        url: "/Admin/GetClaims",
        type: "POST",
        dataType: "json",
        contentType: "application/json",
        data: JSON.stringify(sendJsonData),
        success: function (data) {

            $('#usrClaims').empty();

            $.each(data, function (index, obj) {

                $('#usrClaims').append('Claim ID: ' + obj.id + ' \n ' + ' \ ' + obj.claimType + ' : ' + ' \ ' +

                    obj.claimValue + ' \n ' + ' \n ');

            });

        },



    });
}

function SearchUsers() {
    var sendJsonData = {
        UserSearch: document.getElementById("search").value
    };
    $.ajax({
        url: "/Admin/GetUserNames",
        type: "POST",
        dataType: "Json",
        contentType: "application/json",
        data: JSON.stringify(sendJsonData),
        success: function (data) {
            var users = "";
            $.each(data.userNames, function (index, item) {

                users += '<option value="' + item + '">' + item + '</option>'
            })

            $("#seleUser").html(users);
        }
    });
}

//Validation
        $('#updateForm').validate({
            rules: {

                IdUpdt: {
                    required: true,

                },
                SpecIdUpdt: {
                    required: true,

                },

                DescriptUpdt: { required: true },

                ColorUpdt: { required: true },

                AvailabilityUpdt: {
                    required: true,
                    digits: true
                },

                PriceUpdt: { required: true, number: true },

                DiscountUpdt: { required: true, digits: true },

            },
            messages: {


                IdUpdt: {
                    requred: "Item Id field is required!"
                },
                SpecIdUpdt: {
                    required: "Specs Id field is required!",

                },

                ColorUpdt: { required: "Color field is required!" },

                DescriptUpdt: { required: "Descripton field is requred!" },

                AvailabilityUpdt: {
                    required: "Availability field is required!",
                    digits: "Enter Numbers only"

                },

                PriceUpdt: {
                    required: "Price field is required!",
                    number: "Enter Numbers only"
                },

                DiscountUpdt: {
                    required: "Discount field is required!",
                    digits: "Enter Numbers only"
                }
            },
            submitHandler: function (form) {
                UpdateItems();
            }
         });

        $('#AddItemForm').validate({
            rules: {

                AddSpecSpec: { required: true, },

                AddSpecDescript: { required: true, },

                AddModelColor: { required: true, },

                AddModelAvailibility: { required: true, digits: true },

                AddModelPrice: { required: true, digits: true },

                AddModelDiscount: { required: true, digits: true },

                AddModelName: { required: true, },

                AddModelManufacturer: { required: true, },

                AddModelItemType: { required: true, },

                AddModelItemTypeSub: { required: true, },

                AddModelItemDepartment: { required: true, },

                AddModelAddImage: { required: true, }



            },
            messages: {

                AddSpecSpec: { requred: "This field is required!" },

                AddSpecDescript: { requred: "This field is required!" },

                AddModelColor: { requred: "This field is required!" },

                AddModelAvailibility: { requred: "This field is required!", digits: "Enter Numbers only" },

                AddModelPrice: { requred: "This field is required!", digits: "Enter Numbers only" },

                AddModelDiscount: { requred: "This field is required!", digits: "Enter Numbers only" },

                AddModelName: { requred: "This field is required!" },

                AddModelManufacturer: { requred: "This field is required!" },

                AddModelItemType: { requred: "This field is required!" },

                AddModelItemTypeSub: { requred: "This field is required!" },

                AddModelItemDepartment: { requred: "This field is required!" },

                AddModelAddImage: { requred: "This field is required!" }




            },
            submitHandler: function (form) {
                AddItem();
            }
        });






       
        $('#AddManufacturerForm').validate({
            rules: {

                ManuAdd: {
                    required: true,
                }



            },
            messages: {


                ManuAdd: {
                    requred: "Manufacturer Name field is required!"
                }

            },
            submitHandler: function (form) {
                AddManufacturer();
            }
        });

        $('#AddItemType').validate({
            rules: {

                ItemTypeAdd: {
                    required: true,
                }



            },
            messages: {


                ItemTypeAdd: {
                    requred: "Item Type Name field is required!"
                }

            },
            submitHandler: function (form) {
                AddItemType();
            }
        });





        $('#AddItemSubTypeForm').validate({
            rules: {

                ItemSubTypeAdd: {
                    required: true,
                }



            },
            messages: {


                ItemSubTypeAdd: {
                    requred: "Item Type Name field is required!"
                }

            },
            submitHandler: function (form) {
                AddItemSubType();
            }
        });

        $('#RemoveItemSubTypeForm').validate({
            rules: {

                RemItemTypeSub: {
                    required: true,
                }



            },
            messages: {


                RemItemTypeSub: {
                    requred: "Item Type Name field is required!"
                }

            },
            submitHandler: function (form) {
                RemoveItemSubType('#RemItemTypeSub option:selected');
            }
        });

      
 