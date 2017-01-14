$(document).ready(function () {

    var apiKey = $("#apiKeyPanel").attr("data-apikey");
    
    if (apiKey != null && apiKey !== "") {
        $("#apiKeyLabel").text(apiKey);
        $("#apiKeyInput").hide();
        $("#apiKeyDisplay").show();
    } else {
        $("#apiKeyDisplay").hide();
        $("#apiKeyInput").show();
    }

    $("#searchButton").click(function () {
        Search();
    });

    $('#searchButton').on('click', function () {
        $(this).button('loading');
    });

    $("#changeToken").click(function() {
        $("#apiKeyDisplay").hide();
        $("#apiKeyInput").show();
    });

    $(".searchOnEnter").keyup(function (event) {
        if (event.keyCode === 13) {
            $("#searchButton").click();
        }
    });

    $(".qm").click(function () {
        $(".qText").toggle();
    });
});

function Search() {

    $.ajax({
        url: rootDir + "Search/SearchFlows",
        type: "post",
        data: " { apiKey: \"" + $('#apiKey').val() + "\", searchText: \"" + $('#searchText').val() + "\" } ",
        dataType: "json",
        contentType: "application/json",
        success: function (results) {
            Display(results);
        },
        error: function (requestObject, error, errorThrown) {

            alert("That didn't work. Have you entered your Flowdock API token correctly?");

            $('#searchButton').button('reset');
        }
    });
}

function Display(searchResults) {

    $('#resultsTable tbody').html("");

    $("#resultsTable").trigger("update");

    for (i = 0; i < searchResults.length; i++) {

        var row = "<tr>" +
                  "<td class='resultRow col-1'>" + escapeHtml(searchResults[i].CreatedAtString) + "</td>" +
                  "<td class='resultRow col-2 no-overflow'>" +escapeHtml(searchResults[i].FlowName) + "</td>" +
                  "<td class='resultRow col-3 no-overflow'>" +escapeHtml(searchResults[i].UserName) + "</td>" +
                  "<td class='resultRow col-4'><a href=\"" + searchResults[i].Url + "\" target=\"_blank\">" +escapeHtml(searchResults[i].Content) + "</a></td>" +
                  "</tr>";

        $('#resultsTable tbody').append(row);
    }

    $("#resultsTable").tablesorter();

    $('#searchButton').button('reset');

    $("#resultsPanel").show();
}

var entityMap = {
    "&": "&amp;",
    "<": "&lt;",
    ">": "&gt;",
    '"': '&quot;',
    "'": '&#39;',
    "/": '&#x2F;'
};

function escapeHtml(string) {
    return String(string).replace(/[&<>"'\/]/g, function (s) {
        return entityMap[s];
    });
}