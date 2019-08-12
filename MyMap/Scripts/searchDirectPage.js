
let newMap = () => {
    var mapContainer = document.getElementById("containerMap");
    var mapProp = {
        center: new vbd.LatLng(14.102783, 109.649506),
        zoom: 6,
        minZoom: 2,
    };
    var map = new vbd.Map(mapContainer, mapProp);
    return map;
}



$(document).ready(function () {

    let map = newMap();
    let currenMarker = null;
    $("div.list-group-item").click(function () {

        if (currenMarker != null) {
            map.removeMarker(currenMarker);
        }

        let lng = $(this).find("#lat").text();
        let log = $(this).find("#lng").text();

        currenMarker = new vbd.Marker({
            position: new vbd.LatLng(lng, log),
        });
        map.addMarker(currenMarker);
        map.setZoom(9);
        map.panTo(new vbd.LatLng(lng, log));
    })

    //Check Point  Exist
    $("div.list-group-item").each((index, item) => {
        let Latitude = $(item).find("#lat").text();
        let Longitude = $(item).find("#lng").text();
        let UserName = $("#userName").text();
        let checkPoint = MyMap.Library.Ajax.MongoAjax.isPointExited(Latitude, Longitude, UserName)
        if (checkPoint.value) {
            $(item).children().find("#btn").attr("disabled", true);
            $(item).attr("title", "have saved")
        }
    })

    $("div.list-group-item").each((index, item) => {
        $(item).find("#saveBtn").click(() => {

            let PointSave = {
                UserName: $("#userName").text(),
                Latitude: $(item).find("#lat").text(),
                Longitude: $(item).find("#lng").text(),
                Name: $(item).find(".namePoint").text().trim(),
                Address: $(item).find(".addressPoint").text().trim(),
            }

            //Save Point 
            let respond = MyMap.Library.Ajax.MongoAjax.AddPoint(PointSave)
            respond = respond.value;
            console.log(PointSave, respond)

            if (respond.isSuccess) {
                $(item).children().find("#btn").attr("disabled", true);
                $('#success_inform').find("#success_text").text(respond.Message);
                $("#success_inform").slideToggle("slow");
                setTimeout(function () {
                    $("#success_inform").slideToggle("slow");
                }, 2000);

            }
            else {
                $('#fail_inform').find("#fail_text").text(respond.Message);
                $("#fail_inform").slideToggle("slow");
                setTimeout(function () {
                    $("#fail_inform").slideToggle("slow");
                }, 2000);
            }

        })
    })



});