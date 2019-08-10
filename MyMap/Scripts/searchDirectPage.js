
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

        if (currenMarker!= null) {
            map.removeMarker(currenMarker);
        }

        let lng = $(this).find(".lng").text();
        let log = $(this).find(".log").text();

        currenMarker = new vbd.Marker({
            position: new vbd.LatLng(lng, log),
        });
        map.addMarker(currenMarker);
        map.setZoom(9);
        map.panTo(new vbd.LatLng(lng, log));

    })
});