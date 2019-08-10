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
    let currentStartMarker = null;
    let currentEndMarker = null;
    let flagStartPoint = false;
    let flagEndPoint = false;
    let polyline = null;


    polylineIsOnlyOne = function () {
        if (polyline != null) {
            polyline.setMap(null);
        }
    }

    turnOnResultTap = function () {
        $("#result_search_view").addClass("hidden");
        $("#result_search_view").removeClass("hidden");
    }
    turnOffResultTap = function () {

        $("#result_search_view").addClass("hidden");
    }

    onListItemClick = function (listItemElement, id) {
        let _id = $(id).attr('id');
        let namePoint = $(listItemElement).find(".namePoint").text();
        let lng = $(listItemElement).find(".lng").text();
        let log = $(listItemElement).find(".log").text();
        let LatLng = new vbd.LatLng(lng, log)

        if (_id == "search_btn_startPoitn") {
            $("#seachStartPoint").val(namePoint);
            $("#startPointCover").find(".lat").text(LatLng.lat())
            $("#startPointCover").find(".lng").text(LatLng.lng())
        }
        else {
            $("#seachEndPoint").val(namePoint);
            $("#endPointCover").find(".lat").text(LatLng.lat())
            $("#endPointCover").find(".lng").text(LatLng.lng())
        }
        $("#result_search_view").addClass("hidden");
    }


    onListItemMouseover = function (listItemElement, pointSearchType) {
        let lng = $(listItemElement).find(".lng").text();
        let log = $(listItemElement).find(".log").text();
        let _id = $(pointSearchType).attr('id');
        let LatLng = new vbd.LatLng(lng, log)
        if (_id == "search_btn_startPoitn") {
            currentStartMarker = initMarkerSearch(currentStartMarker, LatLng, "StartPoint")
            map.addMarker(currentStartMarker)
        } else {
            currentEndMarker = initMarkerSearch(currentEndMarker, LatLng, "EndPoint");
            map.addMarker(currentEndMarker)
        }
        map.setZoom(6);
        map.panTo(LatLng);
    }

    initMarkerSearch = function (currentMarker, LatLng, type) {
        if (currentMarker != null) {
            map.removeMarker(currentMarker);
        }
        if (type == "StartPoint") {
            currentStartMarker = new vbd.Marker({
                position: LatLng,
            });
            return currentStartMarker

        } else if (type == "EndPoint") {
            currentEndMarker = new vbd.Marker({
                position: LatLng,
            });
            return currentEndMarker

        }

    }




    ajaxSuccessSearchResult = function (searchDirects, pointSearch) {
        let id = $(pointSearch).attr('id');

        $("#result_search_view").removeClass("hidden");
        $("#result_search").empty();
        if (searchDirects.results.length == 0) {
            $("#result_search").append('<h4 class="text-danger" style="padding-left:15px">No results with this key!</h4>')
        }
        else {
            searchDirects.results.map((point) => {
                $("#result_search").append(
                    "<div id='list-item' class='list-group-item list-group-item-action'" +
                    "onmouseenter='onListItemMouseover(this," + id + ")' " +
                    "onClick = 'onListItemClick(this," + id + ")' > " +
                    "<i id='iconPointMarker' class='glyphicon glyphicon-map-marker' ></i >" +
                    "<div class='pointContent'>" +
                    "<div class='namePoint' title='" + point.Name + "'>" + point.Name + "</div>" +
                    "<div class='addressPoint' title='" + point.Number + " " + point.Street + " - " + point.District + " - " + point.Province + "'>" + point.Number + " " + point.Street + " - " + point.District + " - " + point.Province + "</div>" +
                    "</div>" +
                    "<div class='lng hidden'>" + point.Latitude + "</div>" +
                    "<div class='log hidden'>" + point.Longitude + "</div>" +
                    "</div> "
                )
            })
        }
    }

    searchDirects = function (pointSearch) {
        let keySearch = $(pointSearch).prev().val();

        if (!keySearch) {
            $("#result_search_view").removeClass("hidden");
            $("#result_search").empty();
            $("#result_search").append('<h4 class="text-danger" style="padding-left:15px">Search key not found!</h4>')
        }
        else {
            $.ajax({
                type: "GET",
                url: "/Home/SearchDirect",
                dataType: 'Json',
                data: { keySearch: keySearch },
                success: (data) => ajaxSuccessSearchResult(data, pointSearch),
                error: function () {
                    alert('error');
                }

            })
        }
    }

    $("#search_btn_startPoitn").click(function () {
        polylineIsOnlyOne();
        searchDirects(this);
    })
    $("#search_btn_endPoitn").click(function () {
        polylineIsOnlyOne();
        searchDirects(this);
    })

    // SCREEN SHOT
    $("#screenShotStart").click(function () {
        turnOffResultTap()
        polylineIsOnlyOne();
        flagStartPoint = true;
        $("#containerMap").css('cursor', 'crosshair');
        vbd.event.addListener(map, 'click', function (param) {
            if (flagStartPoint) {
                $("#seachStartPoint").val(  param.LatLng.toString());
                $("#startPointCover").find(".lat").text(param.LatLng.lat())
                $("#startPointCover").find(".lng").text(param.LatLng.lng())
                currentStartMarker = initMarkerSearch(currentStartMarker, param.LatLng, "StartPoint")
                map.addMarker(currentStartMarker)
                map.panTo(param.LatLng);
                flagStartPoint = false;
            }
            $("#containerMap").css('cursor', '');
        });
    })

    $("#screenShotEnd").click(function () {
        turnOffResultTap()
        polylineIsOnlyOne();
        flagEndPoint = true;
        $("#containerMap").css('cursor', 'crosshair');
        vbd.event.addListener(map, 'click', function (param) {
            if (flagEndPoint) {
                $("#seachEndPoint").val(param.LatLng.toString());
                $("#endPointCover").find(".lat").text(param.LatLng.lat())
                $("#endPointCover").find(".lng").text(param.LatLng.lng())
                currentEndMarker = initMarkerSearch(currentEndMarker, param.LatLng, "EndPoint")
                map.addMarker(currentEndMarker)
                map.panTo(param.LatLng);
                flagEndPoint = false;
            }

            $("#containerMap").css('cursor', '');
        });
    })



    //SUBMIT FINDIRECTS 

    addMarkersDirects = function (markersRaw) {
        
    }


    ajaxSuccessFindResult = function (data) {
        let resultDirects = data.FullPath[0];
        let listPoints = [];
        resultDirects = resultDirects[0]
        for (var i = 0; i < resultDirects.length; i = i + 2) {
            listPoints.push(new vbd.LatLng(resultDirects[i + 1], resultDirects[i]))
        };
        if (polyline != null) {
            polyline.setMap(null);
        }
        alert(123)
        polyline = new vbd.Polyline({
            path: listPoints,
            strokeColor: 'blue', strokeColorArrow: 'blue',
            fillColorArrow: 'blue', strokeOpacity: 1, strokeWidth: 3, drawArrows: true
        });
        polyline.setMap(map);
        map.zoomFit();

    }

    findDirectHandle = function (findDirects, points) {
        if (!points[0] || !points[1]) {
            $("#result_search_view").removeClass("hidden");
            $("#result_search").empty();
            $("#result_search").append('<h4 class="text-danger" style="padding-left:15px">Start point or end point not found!</h4>')
        }
        else {
            $.ajax({
                type: "POST",
                url: "/Home/FindDirects",
                dataType: 'Json',
                data: { findDirects, points },
                success: (data) => ajaxSuccessFindResult(data),
                error: function () {
                    alert('Sorry. Distance of two point so far');
                }
            })
        }
    }
    OnSubmitFindDirects = function (findDirect) {
        alert(12321)
        let findDirects = new Object;
        findDirects = {
            nameStartPoint: $("#seachStartPoint").val(),
            nameEndPoint: $("#seachEndPoint").val()
        }
        points = [{
            Latitude: $("#startPointCover").find(".lat").text(),
            Longitude: $("#startPointCover").find(".lng").text()
        }, {
            Latitude: $("#endPointCover").find(".lat").text(),
            Longitude: $("#endPointCover").find(".lng").text()
        }]
        findDirectHandle(findDirects, points)


    }

});