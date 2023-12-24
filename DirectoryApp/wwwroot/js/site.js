    $(function () {

        $.ajax({
            async: true,
            url: "/Home/GetContacts",
            type: "GET",
            error: function (request, status, error) {
                console.log(request);
                window.location.reload();
            },
            success: function (response) {
                console.log(response);
                fillContact(response);
                pagination();
            }
        });
    $("#searchinput").on("keyup", function () {
            var value = $(this).val().toLowerCase();
    $("#table-contact tr").filter(function () {
        $(this).toggle($(this).find("td:eq(1)").text().toLowerCase().indexOf(value) > -1)
    });
        });
    });

    function fillContact(response) {
        $("#table-contact").empty();
    for (var i = 0; i < response.length; i++) {
            var data = response[i];
    console.log(data);
    var tr = document.createElement("tr");
    var td1 = document.createElement("td");
    var td2 = document.createElement("td");
    var td3 = document.createElement("td");
    var td4 = document.createElement("td");
    var checkbox = document.createElement("input");
    $(checkbox).attr("type", "checkbox").attr("name", "contact-checkbox").attr("id", "rd" + i).appendTo(td1);
    $(td1).addClass("contact-td1");
    $(td4).addClass("contact-td4");
    $(checkbox).on('change', function () {
                if (this.checked) {
        console.log("************************");
    var currentRow = $(this).closest("tr");

    var fullname = currentRow.find("td:eq(1)").html();
    var name = fullname.toString().split(' ')[0];
    var surname = fullname.toString().split(' ')[1];
    var phonecol = currentRow.find("td:eq(2)").html();
    var id = currentRow.find("td:eq(3)").html();
    // var phone = phonecol.substring(3, phonecol.length).replace('(', "").replace(')', "").replace('-', "").replace('-', "");
    var phone = phonecol.substring(3, phonecol.length);
    $('.phone-text').val(phone);
    $('.name').val(name);
    $('.surname').val(surname);
    $('.recordId').val(id);
    $('#updateModal').modal('show');
    console.log("************************");
                }
            });
    var content = data.name +" "+ data.surname;
    $(td2).html(content);
    $(td3).html(data.phone);
    $(td4).html(data.id);
    $(tr).append(td1).append(td2).append(td3).append(td4).appendTo($("#table-contact"));
        }
    }

    function pagination() {
        $(function () {
            jQuery(function ($) {
                var items = $("table tbody tr");
                var numItems = items.length;
                var perPage = 18;
                items.slice(perPage).hide();

                $(".pagination-page").pagination({
                    items: numItems,
                    itemsOnPage: perPage,
                    cssStyle: "light-theme",
                    onPageClick: function (pageNumber) {
                        // We need to show and hide `tr`s appropriately.
                        var showFrom = perPage * (pageNumber - 1);
                        var showTo = showFrom + perPage;

                        // We'll first hide everything...
                        items.hide()
                            // ... and then only show the appropriate rows.
                            .slice(showFrom, showTo).show();
                    }
                });

                function checkFragment() {
                    // If there's no hash, treat it like page 1.
                    var hash = window.location.hash || "#page-1";

                    // We'll use a regular expression to check the hash string.
                    hash = hash.match(/^#page-(\d+)$/);

                    if (hash) {
                        // The `selectPage` function is described in the documentation.
                        // We've captured the page number in a regex group: `(\d+)`.
                        $(".pagination-page").pagination("selectPage", parseInt(hash[1]));
                    }
                };

                // We'll call this function whenever back/forward is pressed...
                $(window).bind("popstate", checkFragment);

                // ... and we'll also call it when the page has loaded
                // (which is right now).
                checkFragment();



            });
        });
    }
    function save() {
        var formData = $("#addRecordForm").serialize();
    console.log(formData);
    $.ajax({
        url: "/Home/AddRecord",
    type: "POST",
    data:formData,
    error: function (request, status, error) {
        console.log(request);
    window.location.reload();
            },
    success: function (response) {
        console.log(response);
    window.location.reload();
            }
        });
    }

    function update() {
        var formData = $("#updateRecordForm").serialize();
    console.log(formData);
    $.ajax({
        url: "/Home/UpdateRecord",
    type: "POST",
    data: formData,
    error: function (request, status, error) {
        console.log(request);
    window.location.reload();
            },
    success: function (response) {
        console.log(response);
    window.location.reload();
            }
        });
    }

    function softdelete() {
        console.log("tıktıktık");
    var formData = $("#deleteRecordForm").serialize();
    console.log(formData);
    $.ajax({
        url: "/Home/DeleteRecord",
    type: "POST",
    data: formData,
    error: function (request, status, error) {
        console.log(request);
    window.location.reload();
            },
    success: function (response) {
        console.log(response);
    window.location.reload();
            }
        });
    }
