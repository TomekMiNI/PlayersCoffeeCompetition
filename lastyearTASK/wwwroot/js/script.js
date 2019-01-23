$(function () {
    $(".addbutton").click(function () {
        var number = $("#player_number").val();
        var name = $("#player_firstname").val();
        var age = $("#player_age").val();
        var exp = $("#player_exp").val();
        $.ajax({
            type: 'POST',
            url: "/player/addplayer",
            data: JSON.stringify({
                "Number": number,
                "FirstName": name,
                "Age": age,
                "Experience": exp
            }),
            contentType: 'application/json',
            dataType: 'json'
        }).done(function () {
            alert("player added!");
            }).fail(function () {
                $("#messages").append("<li>Error adding player</li>");
            alert("player cannot be added!");
        });
    });

    $(".refresh").click(function () {
        $.get("/player/getall").done(function (data) {
            var listofall = $("#playerlist>ul");
            listofall.children().remove();
            for (let el in data) {
                var buttoninfo = $("<button>Info</button>");
                var buttondelete = $("<button>Delete</button>");
                var labelid = $("<p></p>");
                var li = $("<li>"+data[el]+"</li>");
                
                li.append(buttondelete);
                li.append(buttoninfo);
                listofall.append(li);

                $(buttoninfo).click(function () {
                    var number_info = $("#playerinfo #pi_number");
                    var name_info = $("#playerinfo #pi_firstname");
                    var age_info = $("#playerinfo #pi_age");
                    var exp_info = $("#playerinfo #pi_experience");
                    $.get("/player/getplayer?number=" + data[el]).done(function (data) {
                        number_info.html(data.number);
                        name_info.html(data.firstName);
                        age_info.html(data.age);
                        exp_info.html(data.experience);
                    }).fail(function () {
                        alert("ERROR GETTING PLAYER INFO");
                    })
                });

                $(buttondelete).click(function () {
                    $.ajax({
                        type: 'DELETE',
                        url: "/player/deleteplayer/" + data[el],
                        success: function () {
                            alert("player deleted!");
                        }
                    });
                })
            }
        });
    });

    $("#start_competition").click(function () {
        var d1 = $.get("/player/getresults");
        var d2 = $.get("/player/getresults");
        var d3 = $.get("/player/getresults");
        var compresults = $("#competition_results");
        compresults.html("Waiting...");
        $.when(d1, d2, d3).done(function (t1, t2, t3) {
            compresults.html("Players who have received a point:");
              var ul=$("<ul>");
            for (var el in t1[0])
                ul.append("<li>" + t1[0][el] + "</li>");
            for (var el in t2[0])
                ul.append("<li>" + t2[0][el] + "</li>");
            for (var el in t3[0])
                ul.append("<li>" + t3[0][el] + "</li>");
            compresults.append(ul);
        });
    });
});